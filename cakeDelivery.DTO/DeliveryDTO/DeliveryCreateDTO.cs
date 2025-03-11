namespace cakeDelivery.DTO.DeliveryDTO;

    public record DeliveryCreateDTO(
     int OrderID,
     string DeliveryAddress,
     string DeliveryCity,
     string DeliveryPostalCode,
     string DeliveryCountry,
     DateTime DeliveryDate,
     string DeliveryStatus
 );
