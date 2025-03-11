namespace cakeDelivery.DTO.OrderItemDTOs;

    public record OrderItemCreateDTO(
    string OrderID,
    string CakeID,
    int Quantity,
    string SizeID,
    decimal PricePerItem
);