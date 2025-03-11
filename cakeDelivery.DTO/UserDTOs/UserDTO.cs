namespace cakeDelivery.DTO.UserDTOs;

    public record UserDTO(
        string UserID,
        string Email,
        string PasswordHash,
        string Role
        );
