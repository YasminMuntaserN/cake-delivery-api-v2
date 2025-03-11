namespace cakeDelivery.DTO.OrderDTOs;

    public record OrderDTO(
        string OrderID,
        string CustomerID,
        DateTime OrderDate,
        decimal TotalAmount,
        string PaymentStatus,
        string DeliveryStatus
    );