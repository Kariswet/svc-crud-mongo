namespace svc_crud_mongo.Services;

using MongoDB.Driver;
using svc_crud_mongo.Configs;
using svc_crud_mongo.Models;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    public UserService(MongoConfig mongo)
    {
        _users = mongo.Database.GetCollection<User>("users");
    }


    public async Task<List<User>> GetAll()
    {
        return await _users.Find(_ => true).ToListAsync();
    }


    public async Task<User?> GetById(string id)
    {
        return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> Create(User user)
    {
        user.Id = Guid.NewGuid().ToString("N");
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Status = true;
        // user.Role = "user";

        await _users.InsertOneAsync(user);

        return user;
    }

    public async Task<bool> Update(string id, User user)
    {
        var update = Builders<User>.Update
            .Set(x => x.Username, user.Username)
            .Set(x => x.Age, user.Age);
        
        var result = await _users.UpdateOneAsync(x => x.Id == id, update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> Delete(string id)
    {
        var update = Builders<User>.Update
            .Set(x => x.Status, false);

        var result = await _users.UpdateOneAsync(x => x.Id == id, update);

        return result.ModifiedCount > 0;
    }

    public async Task<bool> ChangePassword(string id, string oldPassword, string newPassword)
    {
        var user = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (user == null)
            return false;
        
        bool valid = BCrypt.Net.BCrypt.Verify(oldPassword, user.Password);
        if (!valid)
            return false;

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        
        var update = Builders<User>.Update
            .Set(x => x.Password, hashedPassword);
        
        var result = await _users.UpdateOneAsync(x => x.Id == id, update);

        return result.ModifiedCount > 0;
    }

}