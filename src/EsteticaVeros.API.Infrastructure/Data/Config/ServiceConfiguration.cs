using EsteticaVeros.API.Core.ServiceAggregate;
using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
  public void Configure(EntityTypeBuilder<Service> builder)
  {
    builder.ToTable("Services");

    // Primary Key
    builder.HasKey(s => s.Id);
    builder.Property(s => s.Id).ValueGeneratedOnAdd();

    // ServiceNumber - Vogen Value Object
    builder.Property(s => s.ServiceNumber)
      .HasConversion(
        sn => sn.Value,
        value => ServiceNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(s => s.ServiceNumber).IsUnique();

    // ServiceType - SmartEnum
    builder.Property(s => s.ServiceType)
      .HasConversion(
        st => st.Value,
        value => ServiceType.FromValue(value))
      .IsRequired();

    builder.HasIndex(s => s.ServiceType);

    // CategoryId - NULLABLE para paquetes
    builder.Property(s => s.CategoryId)
      .IsRequired(false);

    builder.HasIndex(s => s.CategoryId);

    // Basic Info
    builder.Property(s => s.Name)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(s => s.Description)
      .HasMaxLength(1000);

    builder.Property(s => s.ShortDescription)
      .HasMaxLength(200);

    // Pricing
    builder.Property(s => s.Duration)
      .IsRequired();

    builder.Property(s => s.BasePrice)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(s => s.OriginalPrice)
      .HasPrecision(18, 2);

    builder.Property(s => s.DiscountPercentage)
      .HasPrecision(5, 2);

    // Loyalty & Deposit
    builder.Property(s => s.LoyaltyPointsEarned)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(s => s.RequiresDeposit)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(s => s.DepositAmount)
      .HasPrecision(18, 2);

    // Display Settings
    builder.Property(s => s.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(s => s.IsFeatured)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(s => s.DisplayOrder)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(s => s.ImageUrl)
      .HasMaxLength(500);

    // Instructions & Notes
    builder.Property(s => s.Notes)
      .HasMaxLength(1000);

    builder.Property(s => s.PreparationInstructions)
      .HasMaxLength(1000);

    builder.Property(s => s.Contraindications)
      .HasMaxLength(1000);

    // System Fields
    builder.Property(s => s.CreatedAt)
      .IsRequired();

    builder.Property(s => s.UpdatedAt)
      .IsRequired(false);

    builder.Property(s => s.CreatedByUserId)
      .IsRequired(false);

    // Indexes
    builder.HasIndex(s => s.IsActive);
    builder.HasIndex(s => s.IsFeatured);
    builder.HasIndex(s => s.DisplayOrder);
    builder.HasIndex(s => s.BasePrice);
    builder.HasIndex(s => s.Name);

    // Relationship: Service → ServiceItems (One-to-Many)
    builder.HasMany(s => s.ServiceItems)
      .WithOne(si => si.ParentService)
      .HasForeignKey(si => si.ParentServiceId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relationship: Service → ServiceCategory (Many-to-One)
    builder.HasOne<ServiceCategory>()
      .WithMany()
      .HasForeignKey(s => s.CategoryId)
      .OnDelete(DeleteBehavior.Restrict)
      .IsRequired(false);

    // Metadata para la propiedad de navegación
    builder.Metadata.FindNavigation(nameof(Service.ServiceItems))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}