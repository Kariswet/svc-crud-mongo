namespace svc_crud_mongo.Services;

using MongoDB.Driver;
using svc_crud_mongo.Models;
using svc_crud_mongo.Configs;
using svc_crud_mongo.Utils;

public class LoginService
{
    public readonly IMongoCollection<User> _users;
    private readonly JWTService _jwtHelper;
    public LoginService(MongoConfig mongo, JWTService jwtHelper)
    {
        _users = mongo.Database.GetCollection<User>("users");
        _jwtHelper = jwtHelper;
    }

    public async Task<string?> Login(LoginRequest request)
    {
        var user = await _users.Find(x => x.Username == request.Username).FirstOrDefaultAsync();
        if (user == null)
            return null;
        
        if (!user.Status)
            return null;
        
        bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!valid)
            return null;
        
        return _jwtHelper.CreateAccessToken(user);
    }
}