using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Sehatak.Infrastructure.Security;

public class JwtTokenGenerator
{
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(int userId, string name, string email, string role, int? centerId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

            
            new Claim(ClaimTypes.Name, name),

           
            new Claim(ClaimTypes.Email, email),

            
            new Claim(ClaimTypes.Role, role),

        };
        if (centerId.HasValue)
            claims.Add(new Claim("CenterId", centerId.Value.ToString()));
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)
        );

        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
