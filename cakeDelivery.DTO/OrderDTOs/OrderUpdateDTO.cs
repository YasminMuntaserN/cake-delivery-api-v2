namespace cakeDelivery.DTO.OrderDTOs;

    public record OrderUpdateDTO(
        string OrderID,
        string CustomerID,
        decimal TotalAmount,
        string PaymentStatus,
        string DeliveryStatus
    );