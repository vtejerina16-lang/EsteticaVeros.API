using EsteticaVeros.API.Core.ContributorAggregate;
using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EsteticaVeros.API.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<
  ApplicationUser,
  ApplicationRole,
  int,
  IdentityUserClaim<int>,
  IdentityUserRole<int>,
  IdentityUserLogin<int>,
  IdentityRoleClaim<int>,
  IdentityUserToken<int>>
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  // DbSets existentes
  public DbSet<Contributor> Contributors => Set<Contributor>();

  // Los DbSets de Identity ya están incluidos en IdentityDbContext:
  // - Users (ApplicationUser)
  // - Roles (ApplicationRole)
  // - UserRoles
  // - UserClaims
  // - etc.

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    
    // Personalizar nombres de tablas de Identity (opcional pero recomendado)
    modelBuilder.Entity<ApplicationUser>().ToTable("Users");
    modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
    modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
    modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
    modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
    modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
    modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

    modelBuilder.Entity<IdentityUserRole<int>>().HasData(
      new IdentityUserRole<int>
      {
        UserId = 1,  // Admin user (seed en ApplicationUserConfiguration)
        RoleId = 1   // Admin role (seed en ApplicationRoleConfiguration)
      }
    );
  }

  public override int SaveChanges() =>
    SaveChangesAsync().GetAwaiter().GetResult();
}
