namespace cakeDelivery.DTO.CustomerDTOs;

    public record CustomerDTO(
        string CustomerID,
        string FirstName,
        string LastName,
        string FullName,
        string Email,
        string PhoneNumber,
        string Address,
        string City,
        string PostalCode,
        string Country
    );