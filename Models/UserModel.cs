namespace svc_crud_mongo.Models;

using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

public class User
{
    [BsonId]
    // [JsonIgnore]
    public string Id {get; set;} = string.Empty;
    public string Username {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public int Age {get; set;}
    public bool Status {get; set;} = true;
    public string Role {get; set;} = "User";
} 


public class ChangePasswordRequest
{
    public string OldPassword {get; set;} = string.Empty;
    public string NewPassword {get; set;} = string.Empty;
}