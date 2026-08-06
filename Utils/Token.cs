namespace svc_crud_mongo.Utils;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using svc_crud_mongo.Models;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    private readonly IConfiguration _configuration;
    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string CreateAccessToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        
         var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role, user.Role),

            new Claim("userId", user.Id),
            new Claim("username", user.Username),
            new Claim("role", user.Role)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}