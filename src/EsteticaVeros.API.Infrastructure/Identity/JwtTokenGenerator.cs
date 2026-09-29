using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EsteticaVeros.API.Core.IdentityAggregate;
using EsteticaVeros.API.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EsteticaVeros.API.Infrastructure.Identity;

public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly IConfiguration _configuration;

  public JwtTokenGenerator(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string GenerateToken(ApplicationUser user, string role)
  {
    var jwtSettings = _configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
    var issuer = jwtSettings["Issuer"] ?? "EsteticaVeros.API";
    var audience = jwtSettings["Audience"] ?? "EsteticaVeros.Client";
    var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "8");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email!),
      new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
      new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
      new Claim(ClaimTypes.Role, role),
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim("userId", user.Id.ToString()),
      new Claim("role", role),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
      issuer: issuer,
      audience: audience,
      claims: claims,
      expires: DateTime.UtcNow.AddHours(expirationHours),
      signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}