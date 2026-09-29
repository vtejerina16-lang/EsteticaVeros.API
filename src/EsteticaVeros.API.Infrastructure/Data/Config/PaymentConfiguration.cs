using EsteticaVeros.API.Core.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
  public void Configure(EntityTypeBuilder<Payment> builder)
  {
    builder.ToTable("Payments");

    builder.HasKey(p => p.Id);
    builder.Property(p => p.Id).ValueGeneratedOnAdd();

    // PaymentNumber - Vogen Value Object
    builder.Property(p => p.PaymentNumber)
      .HasConversion(
        pn => pn.Value,
        value => PaymentNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(p => p.PaymentNumber).IsUnique();

    // Foreign Keys
    builder.Property(p => p.AppointmentId).IsRequired(false);
    builder.HasIndex(p => p.AppointmentId);

    builder.Property(p => p.CustomerId).IsRequired();
    builder.HasIndex(p => p.CustomerId);

    builder.Property(p => p.PaymentMethodId).IsRequired();
    builder.HasIndex(p => p.PaymentMethodId);

    builder.Property(p => p.CashRegisterSessionId).IsRequired(false);
    builder.HasIndex(p => p.CashRegisterSessionId);

    // Amount
    builder.Property(p => p.Amount)
      .IsRequired()
      .HasPrecision(18, 2);

    // Dates
    builder.Property(p => p.PaymentDate).IsRequired();
    builder.HasIndex(p => p.PaymentDate);

    // Notes and Reference
    builder.Property(p => p.Notes).HasMaxLength(500);
    builder.Property(p => p.Reference).HasMaxLength(100);

    // Status - SmartEnum
    builder.Property(p => p.Status)
      .HasConversion(
        s => s.Value,
        value => PaymentStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(p => p.Status);

    // System fields
    builder.Property(p => p.CreatedAt).IsRequired();
    builder.Property(p => p.UpdatedAt).IsRequired(false);
    builder.Property(p => p.CreatedByUserId).IsRequired(false);

    // Composite indexes
    builder.HasIndex(p => new { p.CustomerId, p.PaymentDate });
    builder.HasIndex(p => new { p.PaymentMethodId, p.PaymentDate });
    builder.HasIndex(p => new { p.CashRegisterSessionId, p.PaymentDate });

    // Relationship: Payment → PaymentMethod (Many-to-One)
    builder.HasOne(p => p.PaymentMethod)
      .WithMany()
      .HasForeignKey(p => p.PaymentMethodId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}