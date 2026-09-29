namespace EsteticaVeros.API.Web.Configurations;

public static class CorsConfig
{
  public const string DefaultPolicyName = "DefaultCorsPolicy";

  public static IServiceCollection AddCorsConfigs(
    this IServiceCollection services,
    IConfiguration configuration,
    Microsoft.Extensions.Logging.ILogger logger)
  {
    logger.LogInformation("Configuring CORS");

    var corsSettings = configuration.GetSection("CorsSettings");
    var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() 
                         ?? new[] { "http://localhost:3000" }; // Default para desarrollo

    services.AddCors(options =>
    {
      options.AddPolicy(DefaultPolicyName, policy =>
      {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials() // Importante para cookies/auth
              .WithExposedHeaders("Link", "X-Total-Count"); // Exponer headers personalizados
      });

      // Política permisiva para desarrollo (opcional)
      options.AddPolicy("DevelopmentPolicy", policy =>
      {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
      });
    });

    logger.LogInformation("CORS configured with allowed origins: {Origins}", 
                          string.Join(", ", allowedOrigins));

    return services;
  }
}