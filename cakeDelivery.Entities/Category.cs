using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("Categories")]
public class Category 
{
    [BsonId]
    [BsonElement("categoryId"), BsonRepresentation(BsonType.ObjectId)]
    public string? CategoryId { get; set; }
    
    [BsonElement("categoryName")]
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(255, ErrorMessage = "Category name cannot exceed 255 characters")]
    public string CategoryName { get; set; }

    [BsonElement("categoryImageUrl")]
    [Url(ErrorMessage = "Invalid URL format")]
    [StringLength(255, ErrorMessage = "Category image URL cannot exceed 255 characters")]
    public string CategoryImageUrl { get; set; }
}
