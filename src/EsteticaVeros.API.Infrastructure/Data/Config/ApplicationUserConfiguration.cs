using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
  private const string AdminPasswordHash = "AQAAAAIAAYagAAAAEPRH/WsLqeDA3jE6O9APgcM+nvh2n5iy7H8YfiG5NjacpapDzFJU8WB0tz3AusFTrQ==";

  public void Configure(EntityTypeBuilder<ApplicationUser> builder)
  {
    builder.Property(u => u.FirstName)
      .IsRequired()
      .HasMaxLength(50);

    builder.Property(u => u.LastName)
      .IsRequired()
      .HasMaxLength(50);

    builder.Property(u => u.Address)
      .HasMaxLength(200);

    builder.Property(u => u.ProfilePictureUrl)
      .HasMaxLength(500);

    builder.Property(u => u.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(u => u.CreatedAt)
      .IsRequired();

    builder.HasIndex(u => u.IsActive);
    builder.HasIndex(u => u.CreatedAt);

    // Seed del usuario Admin por defecto
    builder.HasData(
      new ApplicationUser
      {
        Id = 1,
        FirstName = "Admin",
        LastName = "Sistema",
        UserName = "admin@esteticaveros.com",
        NormalizedUserName = "ADMIN@ESTETICAVEROS.COM",
        Email = "admin@esteticaveros.com",
        NormalizedEmail = "ADMIN@ESTETICAVEROS.COM",
        EmailConfirmed = true,
        PasswordHash = AdminPasswordHash,
        SecurityStamp = "c1d2e3f4-a5b6-7890-cdef-123456789abc",
        ConcurrencyStamp = "d1e2f3a4-b5c6-7890-defg-234567890bcd",
        PhoneNumber = null,
        PhoneNumberConfirmed = false,
        TwoFactorEnabled = false,
        LockoutEnd = null,
        LockoutEnabled = true,
        AccessFailedCount = 0,
        Address = null,
        DateOfBirth = null,
        ProfilePictureUrl = null,
        IsActive = true,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        LastLoginAt = null
      }
    );
  }
}