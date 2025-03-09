using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cakeDelivery.Entities;

public class User 
{
    [BsonId]
    [BsonElement("UserId"), BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }
    
    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; }

    [BsonElement("role")]
    public string Role { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("lastLogin")]
    public DateTime? LastLogin { get; set; }

    [BsonElement("refreshToken")]
    public string RefreshToken { get; set; }

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
}