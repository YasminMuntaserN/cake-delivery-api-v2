namespace cakeDelivery.DTO.AuthDTOs;

public record AuthRequestDto(
    string Email,
    string Password
);