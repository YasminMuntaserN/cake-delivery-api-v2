namespace cakeDelivery.DTO.DeliveryDTO;

    public record DeliveryDTO(
        string DeliveryID,
        string OrderID,
        string DeliveryAddress,
        string DeliveryCity,
        string DeliveryPostalCode,
        string DeliveryCountry,
        DateTime DeliveryDate,
        string DeliveryStatus
    );
