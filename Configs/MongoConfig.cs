namespace svc_crud_mongo.Configs;

using MongoDB.Driver;

public class MongoConfig
{
    public IMongoDatabase Database { get; }
    public MongoConfig(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["Mongo:ConnectionString"]);
        Database = client.GetDatabase(configuration["Mongo:Database"]);
    }
}