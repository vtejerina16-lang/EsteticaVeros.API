using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister>
{
  public void Configure(EntityTypeBuilder<CashRegister> builder)
  {
    builder.ToTable("CashRegisters");

    builder.HasKey(c => c.Id);
    builder.Property(c => c.Id).ValueGeneratedOnAdd();

    builder.Property(c => c.Name)
      .IsRequired()
      .HasMaxLength(100);

    builder.HasIndex(c => c.Name).IsUnique();

    builder.Property(c => c.Location)
      .HasMaxLength(200);

    builder.Property(c => c.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(c => c.CreatedAt).IsRequired();
    builder.Property(c => c.UpdatedAt).IsRequired(false);
  }
}