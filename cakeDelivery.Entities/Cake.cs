using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

public class Cake 
{
    [BsonId]
    [BsonElement("cakeId"), BsonRepresentation(BsonType.ObjectId)]
    public string? CakeId { get; set; }
    
    [BsonElement("cakeName")]
    [Required(ErrorMessage = "Cake name is required")]
    [StringLength(100, ErrorMessage = "Cake name cannot exceed 100 characters")]
    public string CakeName { get; set; }

    [BsonElement("description")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; }

    [BsonElement("price")]
    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Price { get; set; }

    [BsonElement("stockQuantity")]
    [Required(ErrorMessage = "Stock quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int StockQuantity { get; set; }

    [BsonElement("categoryId")]
    [Required(ErrorMessage = "Category ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; }

    [BsonElement("imageUrl")]
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
    public string ImageUrl { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
