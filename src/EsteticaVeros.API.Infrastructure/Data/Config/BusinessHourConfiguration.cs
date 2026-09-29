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
  }
}