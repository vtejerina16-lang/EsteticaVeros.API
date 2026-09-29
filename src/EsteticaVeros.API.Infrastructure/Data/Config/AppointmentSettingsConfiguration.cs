using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class AppointmentSettingsConfiguration : IEntityTypeConfiguration<AppointmentSettings>
{
  public void Configure(EntityTypeBuilder<AppointmentSettings> builder)
  {
    builder.ToTable("AppointmentSettings");

    builder.HasKey(s => s.Id);
    builder.Property(s => s.Id).ValueGeneratedOnAdd();

    builder.Property(s => s.SlotDuration).IsRequired();
    builder.Property(s => s.BufferTime).IsRequired();
    builder.Property(s => s.CancellationWindowHours).IsRequired();

    builder.Property(s => s.NoShowPenaltyPercentage)
      .IsRequired()
      .HasPrecision(5, 2);

    builder.Property(s => s.RequireDepositForCustomServices).IsRequired();

    builder.Property(s => s.DepositPercentage)
      .HasPrecision(5, 2);

    builder.Property(s => s.MaxConcurrentAppointments).IsRequired();
    builder.Property(s => s.EnableRecurringAppointments).IsRequired();
    builder.Property(s => s.ReminderHoursBefore).IsRequired();
    builder.Property(s => s.MaxAdvanceBookingDays).IsRequired();
    builder.Property(s => s.CreatedAt).IsRequired();
    builder.Property(s => s.UpdatedAt).IsRequired(false);
  }
}