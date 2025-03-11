﻿
namespace cakeDelivery.DTO.CakeDTOs;

    public record CakeDTO(
        string CakeID,
        string CakeName,
        string Description,
        decimal Price,
        int StockQuantity,
        string CategoryID,
       string? ImageUrl = null
    );
