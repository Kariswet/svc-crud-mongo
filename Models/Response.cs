using Microsoft.AspNetCore.Mvc;

namespace svc_crud_mongo.Models;

public class MetadataResponse
{
    public bool Status {get; set;}
    public string Message {get; set;} = "";
    public string TimeExecution {get; set;} = "";
    
}

public class Response
{
    public MetadataResponse? Metadata {get; set;} = new();
    public object? Data {get; set;}    

}

public static class SetMetadataResponse
{
    public static IActionResult Success(ControllerBase controller, DateTime startTime, object? data, string message = "OK")
    {
        var response = new Response
        {
            Metadata = new MetadataResponse
            {
                Status = true,
                Message = message,
                TimeExecution = (DateTime.UtcNow - startTime).ToString()
            },
            Data = data
        };
        return controller.Ok(response);
    }

    public static IActionResult Failed(ControllerBase controller, DateTime startTime, string message, int statusCode=400)
    {
        var response = new Response
        {
            Metadata = new MetadataResponse
            {
                Status = false,
                Message = message,
                TimeExecution = (DateTime.UtcNow - startTime).ToString()
            },
            Data = null
        };
        return controller.StatusCode(statusCode, response);
    }
}