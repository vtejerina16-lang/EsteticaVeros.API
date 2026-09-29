using EsteticaVeros.API.Core.AppointmentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class AppointmentServiceConfiguration : IEntityTypeConfiguration<AppointmentService>
{
  public void Configure(EntityTypeBuilder<AppointmentService> builder)
  {
    builder.ToTable("AppointmentServices");

    // Primary Key
    builder.HasKey(s => s.Id);
    builder.Property(s => s.Id).ValueGeneratedOnAdd();

    // Foreign Keys
    builder.Property(s => s.AppointmentId).IsRequired();
    builder.HasIndex(s => s.AppointmentId);

    builder.Property(s => s.ServiceId).IsRequired();
    builder.HasIndex(s => s.ServiceId);

    // Service Snapshot (datos del servicio al momento de la cita)
    builder.Property(s => s.ServiceName)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(s => s.Duration).IsRequired();

    builder.Property(s => s.Price)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(s => s.Discount)
      .HasPrecision(18, 2);

    builder.Property(s => s.FinalPrice)
      .IsRequired()
      .HasPrecision(18, 2);

    builder.Property(s => s.DisplayOrder)
      .IsRequired()
      .HasDefaultValue(0);

    // Status - SmartEnum
    builder.Property(s => s.Status)
      .HasConversion(
        st => st.Value,
        value => ServiceStatus.FromValue(value))
      .IsRequired();

    builder.HasIndex(s => s.Status);

    // Timing
    builder.Property(s => s.StartedAt).IsRequired(false);
    builder.Property(s => s.CompletedAt).IsRequired(false);

    // Notes
    builder.Property(s => s.Notes).HasMaxLength(500);

    // System Fields
    builder.Property(s => s.CreatedAt).IsRequired();
  }
}