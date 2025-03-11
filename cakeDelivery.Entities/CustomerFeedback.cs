using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace cakeDelivery.Entities;

[Collection("CustomerFeedbacks")]
public class CustomerFeedback
{
    [BsonId]
    [BsonElement("feedbackId"), BsonRepresentation(BsonType.ObjectId)]
    public string? FeedbackId { get; set; }
    
    [BsonElement("customerId")]
    [Required(ErrorMessage = "Customer ID is required")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonElement("feedback")]
    [Required(ErrorMessage = "Feedback content is required")]
    public string Feedback { get; set; }

    [BsonElement("rating")]
    [Required(ErrorMessage = "Rating is required")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }

    [BsonElement("feedbackDate")]
    [Required(ErrorMessage = "Feedback date is required")]
    public DateTime FeedbackDate { get; set; } = DateTime.UtcNow;
}