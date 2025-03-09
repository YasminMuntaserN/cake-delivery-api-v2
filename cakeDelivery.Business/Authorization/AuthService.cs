using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using cakeDelivery.DTO.AuthDTOs;
using cakeDelivery.DTO.UserDTOs;
using cakeDelivery.Entities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;


namespace cakeDelivery.Business.Authorization
{
    public class AuthService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<User> _usersCollection;

        public AuthService(JwtOptions jwtOptions, IMongoDatabase database, IMapper mapper)
        {
            _jwtOptions = jwtOptions;
            _usersCollection = database.GetCollection<User>("users");
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request)
        {
            var user = await _usersCollection
                .Find(x => x.Email == request.Email && x.PasswordHash == request.Password)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays);


            if (user.UserId == null) 
            {
                await _usersCollection.InsertOneAsync(user);
            }
            else
            {
                var updateDefinition = Builders<User>.Update
                    .Set(u => u.RefreshToken, refreshToken)
                    .Set(u => u.RefreshTokenExpiryTime, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays));

                await _usersCollection.UpdateOneAsync(u => u.UserId == user.UserId, updateDefinition);
            }

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDTO>(user)
            };
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var authUser = await _usersCollection
                .Find(u => u.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();

            if (authUser == null || authUser.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return (null, null);

            var newAccessToken = GenerateJwtToken(authUser);
            var newRefreshToken = GenerateRefreshToken();
            
            authUser.RefreshToken = newRefreshToken;
            authUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays);
            
            var updateDefinition = Builders<User>.Update
                .Set(u => u.RefreshToken, newRefreshToken)
                .Set(u => u.RefreshTokenExpiryTime, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays));

            await _usersCollection.UpdateOneAsync(u => u.UserId == authUser.UserId, updateDefinition);

            return (newAccessToken, newRefreshToken);
        }

        private string GenerateJwtToken(User user)
        {
            var permissions = GetPermissionsForRole(user.Role);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role ?? "User"),
                new("Permissions", permissions.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SignKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeMinutes)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task RevokeRefreshTokenAsync(string email)
        {
            var updateDefinition = Builders<User>.Update
                .Set(u => u.RefreshToken, null)
                .Set(u => u.RefreshTokenExpiryTime, null);

            await _usersCollection.UpdateOneAsync(u => u.Email == email, updateDefinition);
        }

        private int GetPermissionsForRole(string role) => role switch
        {
            "Admin" => (int)Permissions.View |
                      (int)Permissions.ManageCakes |
                      (int)Permissions.ManageUsers |
                      (int)Permissions.ManageOrders |
                      (int)Permissions.ManageDeliveries |
                      (int)Permissions.ManageCustomers |
                      (int)Permissions.ManagePayments |
                      (int)Permissions.ManageCategories,

            "Manager" => (int)Permissions.View |
                         (int)Permissions.ManageCakes |
                         (int)Permissions.ManageOrders |
                         (int)Permissions.ManageCustomers |
                         (int)Permissions.ManageDeliveries,

            "User" => (int)Permissions.View,

            _ => 0
        };
    }
}
