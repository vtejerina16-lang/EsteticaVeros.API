using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
  public void Configure(EntityTypeBuilder<ApplicationRole> builder)
  {
    builder.Property(r => r.Description)
      .HasMaxLength(200);

    builder.Property(r => r.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(r => r.CreatedAt)
      .IsRequired();

    builder.HasIndex(r => r.IsActive);

    // Seed de roles por defecto
    builder.HasData(
      new ApplicationRole
      {
        Id = 1,
        Name = "Administrator",
        NormalizedName = "ADMINISTRATOR",
        Description = "Administrador - Acceso completo al sistema",
        IsActive = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        ConcurrencyStamp = "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
      },
      new ApplicationRole
      {
        Id = 2,
        Name = "Receptionist",
        NormalizedName = "RECEPTIONIST",
        Description = "Recepcionista - Acceso limitado a funciones de recepcion",
        IsActive = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        ConcurrencyStamp = "b2c3d4e5-f6a7-8901-bcde-f12345678901"
      }
    );
  }
}