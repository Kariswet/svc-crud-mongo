namespace svc_crud_mongo.Controllers;

using Microsoft.AspNetCore.Mvc;
using svc_crud_mongo.Services;
using svc_crud_mongo.Models;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly UserService _service;
    public UserController(UserService service)
    {
        _service = service;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var start = DateTime.UtcNow;
        var users = await _service.GetAll();
        
        return SetMetadataResponse.Success(this, start, users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var start = DateTime.UtcNow;
        var user = await _service.GetById(id);   

        if (user == null)
        {
            return SetMetadataResponse.Failed(this, start, "user not found", 404);
        }
    
        return SetMetadataResponse.Success(this, start, user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var start = DateTime.UtcNow;
        var createUser = await _service.Create(user);
        
        return SetMetadataResponse.Success(this,start,createUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, User user)
    {
        var start = DateTime.UtcNow;
        var update = await _service.Update(id, user);

        if (!update)
        {
            return SetMetadataResponse.Failed(this, start, "user not found", 404);
        }

        return SetMetadataResponse.Success(this, start, update);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var start = DateTime.UnixEpoch;
        var deleted = await _service.Delete(id);

        if (!deleted)
        {
            return SetMetadataResponse.Failed(this, start, "user not found", 404);
        }
        
        return SetMetadataResponse.Success(this, start, "user deleted successfully");
    }
    
    [Authorize]
    [HttpPut("{id}/password")]
    public async Task<IActionResult> ChangePassword(string id, ChangePasswordRequest request)
    {
        var start = DateTime.UtcNow;
        var success = await _service.ChangePassword(id, request.OldPassword, request.NewPassword);

        if (!success)
        {
            return SetMetadataResponse.Failed(this, start, "wrong old password", 400);    
        }

        return SetMetadataResponse.Success(this, start, "password updated successfully");
    }
}