namespace cakeDelivery.DTO.OrderItemDTOs;

    public record OrderItemDTO(
        string OrderItemID,
        string OrderID,
        string CakeID,
        string SizeID,
        int Quantity,
        decimal PricePerItem
    );

