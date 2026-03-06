using EsteticaVeros.API.Core.BusinessHoursAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class BusinessHourConfiguration : IEntityTypeConfiguration<BusinessHour>
{
  public void Configure(EntityTypeBuilder<BusinessHour> builder)
  {
    builder.ToTable("BusinessHours");

    builder.HasKey(b => b.Id);
    builder.Property(b => b.Id).ValueGeneratedOnAdd();

    builder.Property(b => b.DayOfWeek)
      .IsRequired()
      .HasConversion<int>();

    builder.HasIndex(b => b.DayOfWeek).IsUnique();

    builder.Property(b => b.IsWorkingDay).IsRequired();
    builder.Property(b => b.IsActive).IsRequired().HasDefaultValue(true);
    builder.Property(b => b.CreatedAt).IsRequired();
    builder.Property(b => b.UpdatedAt).IsRequired(false);

    // Seed data
    builder.HasData(
      // Monday to Friday
      new
      {
        Id = 1,
        DayOfWeek = DayOfWeek.Monday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(12, 0, 0),
        AfternoonStartTime = new TimeSpan(15, 0, 0),
        AfternoonEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      new
      {
        Id = 2,
        DayOfWeek = DayOfWeek.Tuesday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(12, 0, 0),
        AfternoonStartTime = new TimeSpan(15, 0, 0),
        AfternoonEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      new
      {
        Id = 3,
        DayOfWeek = DayOfWeek.Wednesday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(12, 0, 0),
        AfternoonStartTime = new TimeSpan(15, 0, 0),
        AfternoonEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      new
      {
        Id = 4,
        DayOfWeek = DayOfWeek.Thursday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(12, 0, 0),
        AfternoonStartTime = new TimeSpan(15, 0, 0),
        AfternoonEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      new
      {
        Id = 5,
        DayOfWeek = DayOfWeek.Friday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(12, 0, 0),
        AfternoonStartTime = new TimeSpan(15, 0, 0),
        AfternoonEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      // Saturday (continuous shift)
      new
      {
        Id = 6,
        DayOfWeek = DayOfWeek.Saturday,
        IsWorkingDay = true,
        MorningStartTime = new TimeSpan(9, 0, 0),
        MorningEndTime = new TimeSpan(19, 0, 0),
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      },
      // Sunday (closed)
      new
      {
        Id = 7,
        DayOfWeek = DayOfWeek.Sunday,
        IsWorkingDay = false,
        IsActive = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
      }
    );
  }
}