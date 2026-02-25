using Microsoft.AspNetCore.Authorization;

namespace EsteticaVeros.API.Web.Configurations;

public static class AuthorizationConfig
{
  // Policy names
  public const string RequireAdminRole = "RequireAdministratorRole";
  public const string RequireManagerRole = "RequireManagerRole";
  public const string RequireAssistantRole = "RequireAssistantRole";
  public const string RequireAdminOrManager = "RequireAdminOrManagerRole";
  public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";

  // Role names (should match Identity seed data)
  public const string AdminRole = "Administrator";
  public const string ManagerRole = "Manager";
  public const string AssistantRole  = "Assistant";

  public static IServiceCollection AddAuthorizationConfigs(
    this IServiceCollection services,
    Microsoft.Extensions.Logging.ILogger logger)
  {
    logger.LogInformation("Configuring Authorization Policies");

    services.AddAuthorizationBuilder()
      // Política: Solo Admin
      .AddPolicy(RequireAdminRole, policy => policy.RequireRole(AdminRole))

      // Política: Solo Manager
      .AddPolicy(RequireManagerRole, policy => policy.RequireRole(ManagerRole))

      // Política: Solo Assistant
      .AddPolicy(RequireAssistantRole, policy => policy.RequireRole(AssistantRole))

      // Política: Admin o Manager
      .AddPolicy(RequireAdminOrManager, policy => policy.RequireRole(AdminRole, ManagerRole))

      // Política base: Usuario autenticado
      .AddPolicy(RequireAuthenticatedUser, policy => policy.RequireAuthenticatedUser());
      
    logger.LogInformation("Authorization Policies configured");

    return services;
  }
}