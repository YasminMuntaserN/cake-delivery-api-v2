namespace cakeDelivery.DTO.CategoryDTOs;

public record CategoryDTO(
    string CategoryID,
    string CategoryName,
    string? CategoryImageUrl = null
);
