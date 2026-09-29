using EsteticaVeros.API.Core.CashRegisterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure.Data.SeedData;

public static class CashRegisterSeeder
{
  public static async Task SeedAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.CashRegisters.AnyAsync())
    {
      logger.LogInformation("CashRegisters already seeded");
      return;
    }

    var cashRegister = new CashRegister("Caja Principal", "Recepción");

    await dbContext.CashRegisters.AddAsync(cashRegister);
    await dbContext.SaveChangesAsync();
    
    logger.LogInformation("CashRegister seeded: Caja Principal");
  }
}