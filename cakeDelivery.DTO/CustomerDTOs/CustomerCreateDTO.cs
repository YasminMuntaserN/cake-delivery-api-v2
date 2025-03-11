namespace cakeDelivery.DTO.CustomerDTOs;

    public record CustomerCreateDTO(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    string City,
    string PostalCode,
    string Country
);
