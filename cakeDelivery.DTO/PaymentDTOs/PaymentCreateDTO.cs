namespace cakeDelivery.DTO.PaymentDTOs;
    public record PaymentCreateDTO(
        string OrderID,
    string PaymentMethod,
    decimal AmountPaid,
    string PaymentStatus
);