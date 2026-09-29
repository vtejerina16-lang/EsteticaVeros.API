using EsteticaVeros.API.Core.BusinessHoursAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure.Data.SeedData;

public static class BusinessHoursSeeder
{
  public static async Task SeedAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.BusinessHours.AnyAsync())
    {
      logger.LogInformation("BusinessHours already seeded");
      return;
    }

    var businessHours = new[]
    {
      // Lunes a Viernes: 9:00-12:00, 15:00-19:00
      new BusinessHour(DayOfWeek.Monday, true, 
        new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0),
        new TimeSpan(15, 0, 0), new TimeSpan(19, 0, 0)),
      
      new BusinessHour(DayOfWeek.Tuesday, true,
        new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0),
        new TimeSpan(15, 0, 0), new TimeSpan(19, 0, 0)),
      
      new BusinessHour(DayOfWeek.Wednesday, true,
        new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0),
        new TimeSpan(15, 0, 0), new TimeSpan(19, 0, 0)),
      
      new BusinessHour(DayOfWeek.Thursday, true,
        new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0),
        new TimeSpan(15, 0, 0), new TimeSpan(19, 0, 0)),
      
      new BusinessHour(DayOfWeek.Friday, true,
        new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0),
        new TimeSpan(15, 0, 0), new TimeSpan(19, 0, 0)),
      
      // Sábado: 9:00-19:00 (corrido)
      new BusinessHour(DayOfWeek.Saturday, true,
        new TimeSpan(9, 0, 0), new TimeSpan(19, 0, 0)),
      
      // Domingo: Cerrado
      new BusinessHour(DayOfWeek.Sunday, false)
    };

    await dbContext.BusinessHours.AddRangeAsync(businessHours);
    await dbContext.SaveChangesAsync();
    
    logger.LogInformation("BusinessHours seeded: {Count} records", businessHours.Length);
  }
}