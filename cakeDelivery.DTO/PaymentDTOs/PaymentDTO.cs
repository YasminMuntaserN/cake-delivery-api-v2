namespace cakeDelivery.DTO.PaymentDTOs;

    public record PaymentDTO(
        string  PaymentID,
        string OrderID,
    string PaymentMethod,
    DateTime PaymentDate,
    decimal AmountPaid,
    string PaymentStatus
);