using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("Orders")]
public class Order
{
    [BsonId]
    [BsonElement("OrderId"), BsonRepresentation(BsonType.ObjectId)]
    public string? OrderId { get; set; }
    
    [BsonElement("customerId")]
    [Required(ErrorMessage = "Customer ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonElement("totalAmount")]
    [Required(ErrorMessage = "Total amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotalAmount { get; set; }

    [BsonElement("orderDate")]
    [Required(ErrorMessage = "Order date is required")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [BsonElement("paymentStatus")]
    [Required(ErrorMessage = "Payment status is required")]
    [RegularExpression("^(Pending|Completed|Failed)$", ErrorMessage = "Invalid payment status")]
    public string PaymentStatus { get; set; }

    [BsonElement("deliveryStatus")]
    [Required(ErrorMessage = "Delivery status is required")]
    [RegularExpression("^(Pending|In Transit|Delivered|Cancelled)$", ErrorMessage = "Invalid delivery status")]
    public string DeliveryStatus { get; set; }

    [BsonElement("orderItems")]
    [Required(ErrorMessage = "Order must contain at least one item")]
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}