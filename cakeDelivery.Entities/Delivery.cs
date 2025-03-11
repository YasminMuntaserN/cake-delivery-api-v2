using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("Deliveries")]
public class Delivery 
{
    [BsonId]
    [BsonElement("deliveryId"), BsonRepresentation(BsonType.ObjectId)]
    public string? DeliveryId { get; set; }
    
    [BsonElement("orderId")]
    [Required(ErrorMessage = "Order ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; }

    [BsonElement("deliveryAddress")]
    [Required(ErrorMessage = "Delivery address is required")]
    [StringLength(50, ErrorMessage = "Delivery address cannot exceed 50 characters")]
    public string DeliveryAddress { get; set; }

    [BsonElement("deliveryCity")]
    [Required(ErrorMessage = "Delivery city is required")]
    [StringLength(50, ErrorMessage = "Delivery city cannot exceed 50 characters")]
    public string DeliveryCity { get; set; }

    [BsonElement("deliveryPostalCode")]
    [Required(ErrorMessage = "Delivery postal code is required")]
    [StringLength(10, ErrorMessage = "Delivery postal code cannot exceed 10 characters")]
    public string DeliveryPostalCode { get; set; }

    [BsonElement("deliveryCountry")]
    [Required(ErrorMessage = "Delivery country is required")]
    [StringLength(50, ErrorMessage = "Delivery country cannot exceed 50 characters")]
    public string DeliveryCountry { get; set; }

    [BsonElement("deliveryDate")]
    [Required(ErrorMessage = "Delivery date is required")]
    public DateTime DeliveryDate { get; set; }

    [BsonElement("deliveryStatus")]
    [Required(ErrorMessage = "Delivery status is required")]
    [RegularExpression("^(Scheduled|In Transit|Delivered|Cancelled)$", ErrorMessage = "Invalid delivery status")]
    public string DeliveryStatus { get; set; }
}