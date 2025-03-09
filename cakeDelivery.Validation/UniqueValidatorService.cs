using cakeDelivery.DataAccess.Data;
using cakeDelivery.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace cakeDelivery.Validation;

public class UniqueValidatorService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UniqueValidatorService(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<User>("Users");
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var count = await _usersCollection.CountDocumentsAsync(u => u.Email == email);
        return count == 0; 
    }
}
