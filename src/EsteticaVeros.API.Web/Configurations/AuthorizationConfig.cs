using Microsoft.AspNetCore.Authorization;

namespace EsteticaVeros.API.Web.Configurations;

public static class AuthorizationConfig
{
  // Policy names
  public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";
  public const string RequireAdminRole = "RequireAdminRole";
  public const string RequireManagerRole = "RequireManagerRole";
  public const string RequireAdminOrManager = "RequireAdminOrManager";

  // Role names (should match Identity seed data)
  public const string AdminRole = "Administrator";
  public const string ManagerRole = "Manager";
  public const string SpecialistRole = "Specialist";
  public const string ReceptionistRole = "Receptionist";

  public static IServiceCollection AddAuthorizationConfigs(
    this IServiceCollection services,
    Microsoft.Extensions.Logging.ILogger logger)
  {
    logger.LogInformation("Configuring Authorization Policies");

    services.AddAuthorizationBuilder()
      // Política base: Usuario autenticado
      .AddPolicy(RequireAuthenticatedUser, policy =>
      {
        policy.RequireAuthenticatedUser();
      })
      
      // Política: Solo Admin
      .AddPolicy(RequireAdminRole, policy =>
      {
        policy.RequireRole(AdminRole);
      })
      
      // Política: Solo Manager
      .AddPolicy(RequireManagerRole, policy =>
      {
        policy.RequireRole(ManagerRole);
      })
      
      // Política: Admin o Manager
      .AddPolicy(RequireAdminOrManager, policy =>
      {
        policy.RequireRole(AdminRole, ManagerRole);
      });

    logger.LogInformation("Authorization Policies configured");

    return services;
  }
}