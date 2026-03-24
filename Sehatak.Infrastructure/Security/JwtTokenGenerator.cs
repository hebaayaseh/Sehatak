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

    public string GenerateToken(int userId, string name, string email, string role, int centerId)
    {
        // المعلومات اللي بنحطها جوا الـ Token
        var claims = new List<Claim>
        {
            // Id المستخدم
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

            // الاسم
            new Claim(ClaimTypes.Name, name),

            // الإيميل
            new Claim(ClaimTypes.Email, email),

            // الدور — مهم للـ Authorization
            new Claim(ClaimTypes.Role, role),

            // Id المركز — مهم للـ Multi-Tenant
            new Claim("CenterId", centerId.ToString())
        };

        // نجيب الـ Secret Key من الـ appsettings.json
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)
        );

        // خوارزمية التوقيع
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // ننشئ الـ Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
