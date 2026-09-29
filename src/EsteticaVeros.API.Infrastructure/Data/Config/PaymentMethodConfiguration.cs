using EsteticaVeros.API.Core.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
  public void Configure(EntityTypeBuilder<PaymentMethod> builder)
  {
    builder.ToTable("PaymentMethods");

    builder.HasKey(p => p.Id);
    builder.Property(p => p.Id).ValueGeneratedOnAdd();

    builder.Property(p => p.Name)
      .IsRequired()
      .HasMaxLength(100);

    builder.HasIndex(p => p.Name).IsUnique();

    // Type - SmartEnum
    builder.Property(p => p.Type)
      .HasConversion(
        t => t.Value,
        value => PaymentMethodType.FromValue(value))
      .IsRequired();

    builder.HasIndex(p => p.Type);

    builder.Property(p => p.RequiresCashRegister)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(p => p.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(p => p.CreatedAt).IsRequired();
    builder.Property(p => p.UpdatedAt).IsRequired(false);
  }
}