using EsteticaVeros.API.Core.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.ToTable("Customers");

    // Primary Key - INT simple
    builder.HasKey(c => c.Id);
    builder.Property(c => c.Id).ValueGeneratedOnAdd();

    // CustomerNumber - Vogen Value Object
    builder.Property(c => c.CustomerNumber)
      .HasConversion(
        cn => cn.Value,
        value => CustomerNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(c => c.CustomerNumber).IsUnique();

    // Basic Info
    builder.Property(c => c.FirstName)
      .IsRequired()
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    builder.Property(c => c.LastName)
      .IsRequired()
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

    builder.Property(c => c.PhoneNumber)
      .IsRequired()
      .HasMaxLength(50);

    builder.HasIndex(c => c.PhoneNumber);

    // Email (Optional, Unique when not null)
    builder.Property(c => c.Email)
      .HasMaxLength(100);

    builder.HasIndex(c => c.Email)
      .IsUnique()
      .HasFilter("[Email] IS NOT NULL");

    // Gender (Smart Enum)
    builder.Property(c => c.Gender)
      .HasConversion(
        g => g.Value,
        value => Gender.FromValue(value))
      .IsRequired();

    // Contact Info
    builder.Property(c => c.Address).HasMaxLength(200);
    builder.Property(c => c.City).HasMaxLength(100);

    // Medical Info
    builder.Property(c => c.Allergies).HasMaxLength(500);
    builder.Property(c => c.MedicalConditions).HasMaxLength(500);
    builder.Property(c => c.Medications).HasMaxLength(500);

    // Relationships
    builder.Property(c => c.PreferredAssistantId).IsRequired(false);
    builder.Property(c => c.CreatedByUserId).IsRequired(false);

    // Profile
    builder.Property(c => c.ProfilePictureUrl).HasMaxLength(500);

    // Loyalty & Stats
    builder.Property(c => c.LoyaltyPoints)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(c => c.TotalSpent)
      .IsRequired()
      .HasPrecision(18, 2)
      .HasDefaultValue(0);

    builder.Property(c => c.TotalVisits)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(c => c.LastVisitDate).IsRequired(false);

    // System Fields
    builder.Property(c => c.IsActive)
      .IsRequired()
      .HasDefaultValue(true);

    builder.Property(c => c.CreatedAt).IsRequired();
    builder.Property(c => c.UpdatedAt).IsRequired(false);
    builder.Property(c => c.DateOfBirth).IsRequired(false);

    // Indexes for performance
    builder.HasIndex(c => c.IsActive);
    builder.HasIndex(c => c.CreatedAt);
    builder.HasIndex(c => c.LastVisitDate);
    builder.HasIndex(c => c.PreferredAssistantId);

    // Ignore computed properties
    builder.Ignore(c => c.FullName);
    builder.Ignore(c => c.Age);
  }
}