namespace cakeDelivery.DTO.CakeDTOs;

    public record CakeCreateDto(
        string CakeName,
        string Description,
        decimal Price,
        int StockQuantity,
        string CategoryID,
        string? ImageUrl  = null
        );
