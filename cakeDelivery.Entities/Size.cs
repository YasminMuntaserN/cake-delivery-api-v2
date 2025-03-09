using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cakeDelivery.Entities;

public class Size
{
    [BsonId]
    [BsonElement("sizeId"), BsonRepresentation(BsonType.ObjectId)]
    public string? SizeId { get; set; }
    
    [BsonElement("sizeName")]
    [Required(ErrorMessage = "Size name is required")]
    [StringLength(50, ErrorMessage = "size name cannot exceed 50 characters")]
    public string SizeName { get; set; }

    [BsonElement("priceMultiplier")]
    [Required(ErrorMessage = "PriceMultiplier is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "PriceMultiplier must be greater than zero")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PriceMultiplier { get; set; }
}