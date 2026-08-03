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
    public async Task Create(User user)
    {
        user.Id = Guid.NewGuid().ToString();

        await _users.InsertOneAsync(user);
    }
    public async Task Update(string id, User user)
    {
        user.Id = id;

        await _users.ReplaceOneAsync(
            x => x.Id == id,
            user
        );
    }

    public async Task Delete(string id)
    {
        await _users.DeleteOneAsync(x => x.Id == id);
    }

}