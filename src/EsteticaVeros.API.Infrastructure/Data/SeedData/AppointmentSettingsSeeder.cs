using EsteticaVeros.API.Core.AppointmentSettingsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure.Data.SeedData;

public static class AppointmentSettingsSeeder
{
  public static async Task SeedAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.AppointmentSettings.AnyAsync())
    {
      logger.LogInformation("AppointmentSettings already seeded");
      return;
    }

    var settings = new AppointmentSettings(
      slotDuration: 30,                      // Slots de 30 minutos
      bufferTime: 15,                         // 15 min de buffer
      cancellationWindowHours: 1,             // Cancelar 1 hora antes
      noShowPenaltyPercentage: 10m,           // Penalización 10%
      requireDepositForCustomServices: true,  // Depósito para servicios personalizados
      depositPercentage: 20m,                 // Depósito 20%
      maxConcurrentAppointments: 2,           // 2 citas simultáneas
      enableRecurringAppointments: true,      // Permitir citas recurrentes
      reminderHoursBefore: 24,                // Recordatorio 24h antes
      maxAdvanceBookingDays: 60               // Agendar hasta 60 días adelante
    );

    await dbContext.AppointmentSettings.AddAsync(settings);
    await dbContext.SaveChangesAsync();
    
    logger.LogInformation("AppointmentSettings seeded");
  }
}