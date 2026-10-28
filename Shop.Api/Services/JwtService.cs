using Microsoft.IdentityModel.Tokens;
using Shop.Api.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Shop.Api.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(User user)
    {
        var jwtSecret = configuration.GetSection("Settings:JwtSecret").Value!;
        var issuer = configuration.GetSection("Settings:Issuer").Value!;
        var audience = configuration.GetSection("Settings:Audience").Value!;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
              issuer: issuer,
              audience: audience,
              notBefore: DateTime.UtcNow,
              expires: DateTime.UtcNow.AddHours(4),
              signingCredentials: credentials
            //ToDo: Add Claims
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;

    }
}
