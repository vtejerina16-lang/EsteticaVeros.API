using EsteticaVeros.API.Core.ServiceCategoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
  public void Configure(EntityTypeBuilder<ServiceCategory> builder)
  {
    builder.ToTable("ServiceCategories");

    // Primary Key
    builder.HasKey(c => c.Id);
    builder.Property(c => c.Id).ValueGeneratedOnAdd();

    // CategoryNumber - Vogen Value Object
    builder.Property(c => c.CategoryNumber)
      .HasConversion(
        cn => cn.Value,
        value => CategoryNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(c => c.CategoryNumber).IsUnique();

    // Basic Info
    builder.Property(c => c.Name)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(c => c.Description)
      .HasMaxLength(500);

    // Display Settings
    builder.Property(c => c.DisplayOrder)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(c => c.IconName)
      .HasMaxLength(50);

    builder.Property(c => c.ColorCode)
      .HasMaxLength(20);

    // System Fields
    builder.Property(c => c.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(c => c.CreatedAt)
      .IsRequired();

    builder.Property(c => c.UpdatedAt)
      .IsRequired(false);

    builder.Property(c => c.CreatedByUserId)
      .IsRequired(false);

    // Indexes
    builder.HasIndex(c => c.IsActive);
    builder.HasIndex(c => c.DisplayOrder);
    builder.HasIndex(c => c.Name);
  }
}