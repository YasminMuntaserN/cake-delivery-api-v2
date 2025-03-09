using cakeDelivery.DTO.UserDTOs;

namespace cakeDelivery.DTO.AuthDTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public UserDTO User { get; set; }
}
