namespace cakeDelivery.DTO.OrderDTOs;

    public record OrderCreateDTO(
     string CustomerID,
     decimal TotalAmount,
     string PaymentStatus,
     string DeliveryStatus
 );