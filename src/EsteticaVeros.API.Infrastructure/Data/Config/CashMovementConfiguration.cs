using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class CashMovementConfiguration : IEntityTypeConfiguration<CashMovement>
{
  public void Configure(EntityTypeBuilder<CashMovement> builder)
  {
    builder.ToTable("CashMovements");

    builder.HasKey(m => m.Id);
    builder.Property(m => m.Id).ValueGeneratedOnAdd();

    // MovementNumber - Vogen Value Object
    builder.Property(m => m.MovementNumber)
      .HasConversion(
        mn => mn.Value,
        value => MovementNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(m => m.MovementNumber).IsUnique();

    // Foreign Keys
    builder.Property(m => m.CashRegisterSessionId).IsRequired();
    builder.HasIndex(m => m.CashRegisterSessionId);

    builder.Property(m => m.PaymentId).IsRequired(false);
    builder.HasIndex(m => m.PaymentId);

    // Type - SmartEnum
    builder.Property(m => m.Type)
      .HasConversion(
        t => t.Value,
        value => MovementType.FromValue(value))
      .IsRequired();

    builder.HasIndex(m => m.Type);

    // Amount
    builder.Property(m => m.Amount)
      .IsRequired()
      .HasPrecision(18, 2);

    // Description and Reference
    builder.Property(m => m.Description)
      .IsRequired()
      .HasMaxLength(500);

    builder.Property(m => m.Reference).HasMaxLength(100);

    // Dates
    builder.Property(m => m.MovementDate).IsRequired();
    builder.HasIndex(m => m.MovementDate);

    builder.Property(m => m.CreatedAt).IsRequired();
    builder.Property(m => m.CreatedByUserId).IsRequired(false);

    // Composite indexes
    builder.HasIndex(m => new { m.CashRegisterSessionId, m.Type });
    builder.HasIndex(m => new { m.CashRegisterSessionId, m.MovementDate });

    // Relationship: Movement → Session (Many-to-One)
    builder.HasOne(m => m.Session)
      .WithMany()
      .HasForeignKey(m => m.CashRegisterSessionId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}