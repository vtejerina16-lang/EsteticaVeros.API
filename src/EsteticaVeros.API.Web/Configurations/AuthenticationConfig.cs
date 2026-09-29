using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EsteticaVeros.API.Web.Configurations;

public static class AuthenticationConfig
{
  public static IServiceCollection AddAuthenticationConfigs(
    this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger)
  {
    logger.LogInformation("Configuring JWT Authentication");

    var jwtSettings = configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] 
      ?? throw new InvalidOperationException("JWT SecretKey is not configured");

    // Configure Authentication with BOTH Identity cookies AND JWT Bearer
    services.AddAuthentication(options =>
    {
      // Default to JWT Bearer for API
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // Add JWT Bearer for API authentication
    .AddJwtBearer(options =>
    {
      options.SaveToken = true;
      options.RequireHttpsMetadata = false; // Set to true in production
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "EsteticaVeros.API",
        ValidAudience = jwtSettings["Audience"] ?? "EsteticaVeros.Client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
      };

      options.Events = new JwtBearerEvents
      {
        OnAuthenticationFailed = context =>
        {
          logger.LogWarning("JWT Authentication failed: {Exception}", context.Exception.Message);
          return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
          var userId = context.Principal?.FindFirst("userId")?.Value;
          logger.LogInformation("JWT Token validated for user: {UserId}", userId);
          return Task.CompletedTask;
        }
      };
    })
    // Add Identity cookies for SignInManager (used internally)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ExternalScheme)
    .AddCookie(IdentityConstants.TwoFactorUserIdScheme);

    services.AddAuthorization();

    logger.LogInformation("JWT Authentication configured");

    return services;
  }
}