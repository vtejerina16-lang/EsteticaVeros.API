using Ardalis.GuardClauses;
using EsteticaVeros.API.Core.Interfaces;
using EsteticaVeros.API.Core.Services;
using EsteticaVeros.API.Core.IdentityAggregate;
using EsteticaVeros.API.Infrastructure.Data;
using EsteticaVeros.API.Infrastructure.Data.Queries;
using EsteticaVeros.API.Infrastructure.Identity;
using EsteticaVeros.API.UseCases.Contributors.List;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    // Try to get connection strings in order of priority:
    // 1. "EsteticaVerosDB" - provided by Aspire when using .WithReference(cleanArchDb)
    // 2. "DefaultConnection" - traditional SQL Server connection
    // 3. "SqliteConnection" - fallback to SQLite
    string? connectionString = config.GetConnectionString("EsteticaVerosDB")
                               ?? config.GetConnectionString("DefaultConnection") 
                               ?? config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);

    services.AddScoped<EventDispatchInterceptor>();
    services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();

    services.AddDbContext<AppDbContext>((provider, options) =>
    {
      var eventDispatchInterceptor = provider.GetRequiredService<EventDispatchInterceptor>();
      
      // Use SQL Server if Aspire or DefaultConnection is available, otherwise use SQLite
      if (config.GetConnectionString("EsteticaVerosDB") != null || 
          config.GetConnectionString("DefaultConnection") != null)
      {
        options.UseSqlServer(connectionString);
      }
      else
      {
        options.UseSqlite(connectionString);
      }
      
      options.AddInterceptors(eventDispatchInterceptor);
    });

    // ========================================================================
    // Configure ASP.NET Core Identity - Manual Registration with all dependencies
    // ========================================================================
    // Register Identity stores
    services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, ApplicationRole, AppDbContext, int>>();
    services.AddScoped<IRoleStore<ApplicationRole>, RoleStore<ApplicationRole, AppDbContext, int>>();

    // Register Identity core services with their dependencies
    services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
    services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
    services.AddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
    services.AddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
    services.AddScoped<IRoleValidator<ApplicationRole>, RoleValidator<ApplicationRole>>();
    services.AddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
    services.AddScoped<IUserConfirmation<ApplicationUser>, DefaultUserConfirmation<ApplicationUser>>();

    // Register managers
    services.AddScoped<UserManager<ApplicationUser>>();
    services.AddScoped<RoleManager<ApplicationRole>>();
    services.AddScoped<SignInManager<ApplicationUser>>();

    // Register claims factory
    services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

    // Register error describer
    services.AddScoped<IdentityErrorDescriber>();

    // Configure Identity Options
    services.Configure<IdentityOptions>(options =>
    {
      // Password settings
      options.Password.RequireDigit = true;
      options.Password.RequireLowercase = true;
      options.Password.RequireUppercase = true;
      options.Password.RequireNonAlphanumeric = true;
      options.Password.RequiredLength = 8;

      // Lockout settings
      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
      options.Lockout.MaxFailedAccessAttempts = 5;
      options.Lockout.AllowedForNewUsers = true;

      // User settings
      options.User.RequireUniqueEmail = true;
      options.User.AllowedUserNameCharacters = 
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

      // SignIn settings
      options.SignIn.RequireConfirmedEmail = false;
      options.SignIn.RequireConfirmedPhoneNumber = false;
    });

    // Configure Data Protection (required for tokens)
    services.AddDataProtection();
    // ========================================================================

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    // Register JWT token generator
    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
