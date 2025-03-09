using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("OrderItems")]
public class OrderItem
{
    [BsonId]
    [BsonElement("orderItemId"), BsonRepresentation(BsonType.ObjectId)]
    public string? OrderItemId { get; set; }
    
    [BsonElement("cakeId")]
    [Required(ErrorMessage = "Cake ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CakeId { get; set; }

    [BsonElement("sizeId")]
    [Required(ErrorMessage = "Size ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SizeId { get; set; }

    [BsonElement("orderId")]
    [Required(ErrorMessage = "Order ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrderId { get; set; }
    
    [BsonElement("quantity")]
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
    public int Quantity { get; set; }

    [BsonElement("pricePerItem")]
    [Required(ErrorMessage = "Price per item is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price per item must be greater than zero")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PricePerItem { get; set; }
}
