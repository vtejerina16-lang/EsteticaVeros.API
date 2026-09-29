using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class CashRegisterSessionConfiguration : IEntityTypeConfiguration<CashRegisterSession>
{
  public void Configure(EntityTypeBuilder<CashRegisterSession> builder)
  {
    builder.ToTable("CashRegisterSessions");

    builder.HasKey(s => s.Id);
    builder.Property(s => s.Id).ValueGeneratedOnAdd();

    // SessionNumber - Vogen Value Object
    builder.Property(s => s.SessionNumber)
      .HasConversion(
        sn => sn.Value,
        value => SessionNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(s => s.SessionNumber).IsUnique();

    // Foreign Keys
    builder.Property(s => s.CashRegisterId).IsRequired();
    builder.HasIndex(s => s.CashRegisterId);

    builder.Property(s => s.OpenedByUserId).IsRequired();
    builder.Property(s => s.ClosedByUserId).IsRequired(false);

    // Dates
    builder.Property(s => s.OpenedAt).IsRequired();
    builder.Property(s => s.ClosedAt).IsRequired(false);

    builder.HasIndex(s => s.OpenedAt);

    // Amounts
    builder.Property(s => s.InitialAmount)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(s => s.ExpectedAmount)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(s => s.ActualAmount)
      .HasPrecision(18, 2);

    builder.Property(s => s.Difference)
      .HasPrecision(18, 2);

    // Status - SmartEnum
    builder.Property(s => s.Status)
      .HasConversion(
        st => st.Value,
        value => SessionStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(s => s.Status);

    // Notes
    builder.Property(s => s.Notes).HasMaxLength(1000);

    // Relationship: Session → CashRegister (Many-to-One)
    builder.HasOne(s => s.CashRegister)
      .WithMany()
      .HasForeignKey(s => s.CashRegisterId)
      .OnDelete(DeleteBehavior.Restrict);

    // Composite indexes
    builder.HasIndex(s => new { s.CashRegisterId, s.OpenedAt });
    builder.HasIndex(s => new { s.CashRegisterId, s.Status });

    // Ignore computed properties
    builder.Ignore(s => s.IsOpen);
    builder.Ignore(s => s.IsClosed);
  }
}