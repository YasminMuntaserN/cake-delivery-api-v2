namespace cakeDelivery.DTO.CategoryDTOs;

public record CategoryCreateDto(
    string CategoryName,
    string? CategoryImageURL = null
);
