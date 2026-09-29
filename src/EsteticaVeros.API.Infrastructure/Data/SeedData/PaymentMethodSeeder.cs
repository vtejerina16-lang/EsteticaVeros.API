using EsteticaVeros.API.Core.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure.Data.SeedData;

public static class PaymentMethodSeeder
{
  public static async Task SeedAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.PaymentMethods.AnyAsync())
    {
      logger.LogInformation("PaymentMethods already seeded");
      return;
    }

    var paymentMethods = new[]
    {
      new PaymentMethod("Efectivo", PaymentMethodType.Cash, requiresCashRegister: true),
      new PaymentMethod("Tarjeta de Débito", PaymentMethodType.Card, requiresCashRegister: false),
      new PaymentMethod("Tarjeta de Crédito", PaymentMethodType.Card, requiresCashRegister: false),
      new PaymentMethod("Transferencia Bancaria", PaymentMethodType.Transfer, requiresCashRegister: false),
      new PaymentMethod("QR - Tigo Money", PaymentMethodType.QR, requiresCashRegister: false),
    };

    await dbContext.PaymentMethods.AddRangeAsync(paymentMethods);
    await dbContext.SaveChangesAsync();
    
    logger.LogInformation("PaymentMethods seeded: {Count} records", paymentMethods.Length);
  }
}