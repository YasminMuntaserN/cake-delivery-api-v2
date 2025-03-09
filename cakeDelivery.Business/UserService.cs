using System.Linq.Expressions;
using AutoMapper;
using cakeDelivery.Business.Base;
using cakeDelivery.DTO;
using cakeDelivery.DTO.UserDTOs;
using cakeDelivery.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace UserDelivery.Business;

public class UserService : BaseService<User, UserDTO>
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<User> _validator;

    public UserService(
        IMongoDatabase database,
        ILogger<UserService> logger,
        IMapper mapper,
        IValidator<User> validator)
        : base(database, "users", logger, mapper, validator)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UserDTO> AddUserAsync(UserCreateDTO userCreateDto)
        => await AddAsync(userCreateDto, "User");

    public async Task<UserDTO?> UpdateUserAsync(string id, UserDTO userDetailsDto)
        => await UpdateAsync(id, userDetailsDto, "User");

    public async Task<UserDTO?> GetUserByIdAsync(string id)
        => await FindBy(u => u.UserId == id);

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        => await GetAllAsync();

    public async Task<(IEnumerable<UserDTO> Data, long TotalCount, int TotalPages)> GetAllAsync(
        PaginationDto paginationDto)
    {
        Expression<Func<User, object>> orderBy = paginationDto.OrderBy switch
        {
            "Email" => user => user.Email,
            _ => user => user.UserId
        };

        return await GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize, orderBy, paginationDto.Ascending);
    }

    public async Task<bool> DeleteUserAsync(string id)
        => await HardDeleteAsync(id);

    public async Task<bool> DeleteUserByAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<User, bool>> predicate = criteria.Field.ToLower() switch
        {
            "email" => u => u.Email == criteria.Value,
            "id" => u => u.UserId == criteria.Value,
            _ => null
        };

        return predicate != null && await HardDeleteByAsync(predicate);
    }

    public async Task<bool> ExistsUserAsync(string id)
        => await ExistsAsync(id);

    public async Task<bool> ExistsUserByAsync(SearchCriteriaDto criteria)
        => criteria.Field.ToLower() switch
        {
            "email" => await ExistsByAsync(u => u.Email == criteria.Value),
            "id" => await ExistsByAsync(u => u.UserId == criteria.Value),
            _ => false
        };

    public async Task<IEnumerable<UserDTO>> SearchUserAsync(SearchCriteriaDto criteria)
    {
        Expression<Func<User, bool>> predicate = criteria.Field.ToLower() switch
        {
            "email" => u => u.Email.Contains(criteria.Value),
            "id" => u => u.UserId == criteria.Value,
            _ => null
        };

        return predicate != null 
            ? await SearchAsync(predicate)
            : Enumerable.Empty<UserDTO>();
    }
}
