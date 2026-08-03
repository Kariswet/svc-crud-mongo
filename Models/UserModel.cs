namespace svc_crud_mongo.Models;

using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [JsonIgnore]
    public string Id {get; set;} = string.Empty;
    public string Username {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public int Age {get; set;}
} 