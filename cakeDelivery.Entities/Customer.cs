using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("Customers")]
public class Customer 
{
    [BsonId]
    [BsonElement("customerId"), BsonRepresentation(BsonType.ObjectId)]
    public string? CustomerId { get; set; }
    
    [BsonElement("firstName")]
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "First name can only contain letters and spaces")]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Last name can only contain letters and spaces")]
    public string LastName { get; set; }

    [BsonElement("email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; }

    [BsonElement("phoneNumber")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    public string PhoneNumber { get; set; }

    [BsonElement("address")]
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [BsonElement("city")]
    [Required(ErrorMessage = "City is required")]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
    public string City { get; set; }

    [BsonElement("postalCode")]
    [Required(ErrorMessage = "Postal code is required")]
    [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
    public string PostalCode { get; set; }

    [BsonElement("country")]
    [Required(ErrorMessage = "Country is required")]
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string Country { get; set; }
}