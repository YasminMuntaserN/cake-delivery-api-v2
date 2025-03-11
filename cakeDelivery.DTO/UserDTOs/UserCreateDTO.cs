namespace cakeDelivery.DTO.UserDTOs;

    public record UserCreateDTO(
        string Email,
        string PasswordHash,
        string Role
    );