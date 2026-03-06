using EsteticaVeros.API.Core.AppointmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
  public void Configure(EntityTypeBuilder<Appointment> builder)
  {
    builder.ToTable("Appointments");

    // Primary Key
    builder.HasKey(a => a.Id);
    builder.Property(a => a.Id).ValueGeneratedOnAdd();

    // AppointmentNumber - Vogen Value Object
    builder.Property(a => a.AppointmentNumber)
      .HasConversion(
        an => an.Value,
        value => AppointmentNumber.From(value))
      .IsRequired()
      .HasMaxLength(20);

    builder.HasIndex(a => a.AppointmentNumber).IsUnique();

    // Foreign Keys
    builder.Property(a => a.CustomerId).IsRequired();
    builder.HasIndex(a => a.CustomerId);

    builder.Property(a => a.AssignedAssistantId).IsRequired(false);
    builder.HasIndex(a => a.AssignedAssistantId);

    // Appointment Details
    builder.Property(a => a.AppointmentDate).IsRequired();
    builder.HasIndex(a => a.AppointmentDate);

    builder.Property(a => a.EstimatedDuration).IsRequired();
    builder.Property(a => a.ActualStartTime).IsRequired(false);
    builder.Property(a => a.ActualEndTime).IsRequired(false);
    builder.Property(a => a.ActualDuration).IsRequired(false);

    // Status and Source - SmartEnums
    builder.Property(a => a.Status)
      .HasConversion(
        s => s.Value,
        value => AppointmentStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(a => a.Status);

    builder.Property(a => a.Source)
      .HasConversion(
        s => s.Value,
        value => AppointmentSource.FromValue(value))
      .IsRequired();

    builder.HasIndex(a => a.Source);

    // Payment
    builder.Property(a => a.TotalAmount)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(a => a.DepositAmount)
      .HasPrecision(18, 2);

    builder.Property(a => a.DepositPaid)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(a => a.PaymentStatus)
      .HasConversion(
        ps => ps.Value,
        value => PaymentStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(a => a.PaymentStatus);

    // Notes
    builder.Property(a => a.Notes).HasMaxLength(1000);
    builder.Property(a => a.CustomerNotes).HasMaxLength(500);
    builder.Property(a => a.InternalNotes).HasMaxLength(1000);

    // Cancellation
    builder.Property(a => a.CancellationReason).HasMaxLength(500);
    builder.Property(a => a.CancelledAt).IsRequired(false);
    builder.Property(a => a.CancelledBy).IsRequired(false);

    // NoShow
    builder.Property(a => a.NoShowReason).HasMaxLength(500);

    // Reminders and Confirmation
    builder.Property(a => a.ReminderSent)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(a => a.ReminderSentAt).IsRequired(false);

    builder.Property(a => a.ConfirmedByCustomer)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(a => a.ConfirmedAt).IsRequired(false);

    // Recurrence
    builder.Property(a => a.IsRecurring)
      .IsRequired()
      .HasDefaultValue(false);

    builder.Property(a => a.RecurrencePattern).HasMaxLength(100);
    builder.Property(a => a.RecurrenceEndDate).IsRequired(false);
    builder.Property(a => a.ParentAppointmentId).IsRequired(false);

    builder.HasIndex(a => a.ParentAppointmentId);

    // System Fields
    builder.Property(a => a.CreatedAt).IsRequired();
    builder.Property(a => a.UpdatedAt).IsRequired(false);
    builder.Property(a => a.CreatedByUserId).IsRequired(false);

    // Composite Indexes for performance
    builder.HasIndex(a => new { a.AppointmentDate, a.Status });
    builder.HasIndex(a => new { a.CustomerId, a.AppointmentDate });
    builder.HasIndex(a => new { a.AssignedAssistantId, a.AppointmentDate });

    // Relationship: Appointment → AppointmentServices (One-to-Many)
    builder.HasMany(a => a.AppointmentServices)
      .WithOne(s => s.Appointment)
      .HasForeignKey(s => s.AppointmentId)
      .OnDelete(DeleteBehavior.Cascade);

    // Ignore computed properties
    builder.Ignore(a => a.EstimatedEndTime);
    builder.Ignore(a => a.IsUpcoming);
    builder.Ignore(a => a.CanBeCancelled);
    builder.Ignore(a => a.RequiresDeposit);
  }
}