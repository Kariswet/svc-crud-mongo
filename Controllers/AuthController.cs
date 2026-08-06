namespace svc_crud_mongo.Controllers;

using Microsoft.AspNetCore.Mvc;
using svc_crud_mongo.Models;
using svc_crud_mongo.Services;
using svc_crud_mongo.Utils;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    public readonly LoginService _loginService;
    public AuthController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var start = DateTime.UtcNow;
        var token = await _loginService.Login(request);

        if (token == null)
        {
            return SetMetadataResponse.Failed(this, start, "invalid username or password", 401);
        }

        return SetMetadataResponse.Success(this, start, new
        {
            accessToken = token
        });
    }
}