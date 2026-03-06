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

    // Seed data - configuración inicial
    builder.HasData(new
    {
      Id = 1,
      SlotDuration = 30,
      BufferTime = 15,
      CancellationWindowHours = 1,
      NoShowPenaltyPercentage = 10m,
      RequireDepositForCustomServices = true,
      DepositPercentage = (decimal?)20m,
      MaxConcurrentAppointments = 2,
      EnableRecurringAppointments = true,
      ReminderHoursBefore = 24,
      MaxAdvanceBookingDays = 60,
      CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    });
  }
}