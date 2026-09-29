using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class CashReconciliationConfiguration : IEntityTypeConfiguration<CashReconciliation>
{
  public void Configure(EntityTypeBuilder<CashReconciliation> builder)
  {
    builder.ToTable("CashReconciliations");

    builder.HasKey(r => r.Id);
    builder.Property(r => r.Id).ValueGeneratedOnAdd();

    // ReconciliationNumber - Vogen Value Object
    builder.Property(r => r.ReconciliationNumber)
      .HasConversion(
        rn => rn.Value,
        value => ReconciliationNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(r => r.ReconciliationNumber).IsUnique();

    // Foreign Keys
    builder.Property(r => r.CashRegisterSessionId).IsRequired();
    builder.HasIndex(r => r.CashRegisterSessionId);

    builder.Property(r => r.ReconciledByUserId).IsRequired();
    builder.Property(r => r.ApprovedByUserId).IsRequired(false);

    // Dates
    builder.Property(r => r.ReconciliationDate).IsRequired();
    builder.HasIndex(r => r.ReconciliationDate);

    builder.Property(r => r.CreatedAt).IsRequired();
    builder.Property(r => r.ApprovedAt).IsRequired(false);

    // Amounts
    builder.Property(r => r.SystemAmount)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(r => r.CountedAmount)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(r => r.Difference)
      .IsRequired()
      .HasPrecision(18, 2);

    // Denomination Details (JSON)
    builder.Property(r => r.DenominationDetails)
      .HasMaxLength(2000)
      .IsRequired(false);

    // Notes
    builder.Property(r => r.Notes).HasMaxLength(1000);

    // Status - SmartEnum
    builder.Property(r => r.Status)
      .HasConversion(
        s => s.Value,
        value => ReconciliationStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(r => r.Status);

    // Composite indexes
    builder.HasIndex(r => new { r.CashRegisterSessionId, r.ReconciliationDate });
    builder.HasIndex(r => new { r.Status, r.ReconciliationDate });

    // Relationship: Reconciliation → Session (Many-to-One)
    builder.HasOne(r => r.Session)
      .WithMany()
      .HasForeignKey(r => r.CashRegisterSessionId)
      .OnDelete(DeleteBehavior.Restrict);

    // Ignore computed properties
    builder.Ignore(r => r.HasDifference);
    builder.Ignore(r => r.IsShort);
    builder.Ignore(r => r.IsOver);
  }
}