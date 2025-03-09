using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("Payments")]
public class Payment 
{
    [BsonId]
    [BsonElement("paymentId"), BsonRepresentation(BsonType.ObjectId)]
    public string? PaymentId { get; set; }
    
    [BsonElement("orderId")]
    [Required(ErrorMessage = "Order ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; }

    [BsonElement("paymentMethod")]
    [Required(ErrorMessage = "Payment method is required")]
    [StringLength(20, ErrorMessage = "Payment method cannot exceed 20 characters")]
    public string PaymentMethod { get; set; }

    [BsonElement("paymentDate")]
    [Required(ErrorMessage = "Payment date is required")]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [BsonElement("amountPaid")]
    [Required(ErrorMessage = "Amount paid is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than zero")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AmountPaid { get; set; }

    [BsonElement("paymentStatus")]
    [Required(ErrorMessage = "Payment status is required")]
    [StringLength(10, ErrorMessage = "Payment status cannot exceed 10 characters")]
    [RegularExpression("^(Pending|Completed|Failed)$", ErrorMessage = "Invalid payment status")]
    public string PaymentStatus { get; set; }
}