using EsteticaVeros.API.Core.ContributorAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.Infrastructure.Data.SeedData;

public static class SeedData
{
  public const int NUMBER_OF_CONTRIBUTORS = 27;
  public static readonly Contributor Contributor1 = new(ContributorName.From("Ardalis"));
  public static readonly Contributor Contributor2 = new(ContributorName.From("Ilyana"));

  /// <summary>
  /// Inicializa todos los datos semilla de la base de datos
  /// </summary>
  public static async Task InitializeAsync(AppDbContext dbContext, ILogger logger)
  {
    try
    {
      logger.LogInformation("Starting database seeding...");

      // Orden de ejecución (importante por dependencias)
      await SeedContributorsAsync(dbContext, logger);
      await BusinessHoursSeeder.SeedAsync(dbContext, logger);
      await AppointmentSettingsSeeder.SeedAsync(dbContext, logger);
      await CashRegisterSeeder.SeedAsync(dbContext, logger);
      await PaymentMethodSeeder.SeedAsync(dbContext, logger);

      logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while seeding the database");
      throw;
    }
  }

  /// <summary>
  /// Seed de Contributors (datos de ejemplo del template)
  /// </summary>
  private static async Task SeedContributorsAsync(AppDbContext dbContext, ILogger logger)
  {
    if (await dbContext.Contributors.AnyAsync())
    {
      logger.LogInformation("Contributors already seeded");
      return;
    }

    dbContext.Contributors.AddRange([Contributor1, Contributor2]);
    await dbContext.SaveChangesAsync();

    // Add more contributors for paging demonstration
    for (int i = 1; i <= NUMBER_OF_CONTRIBUTORS - 2; i++)
    {
      dbContext.Contributors.Add(new Contributor(ContributorName.From($"Contributor {i}")));
    }
    
    await dbContext.SaveChangesAsync();
    logger.LogInformation("Contributors seeded: {Count} records", NUMBER_OF_CONTRIBUTORS);
  }

  /// <summary>
  /// Método de compatibilidad para tests (solo Contributors)
  /// </summary>
  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Contributors.AddRange([Contributor1, Contributor2]);
    await dbContext.SaveChangesAsync();

    // Add more contributors for paging demonstration
    for (int i = 1; i <= NUMBER_OF_CONTRIBUTORS - 2; i++)
    {
      dbContext.Contributors.Add(new Contributor(ContributorName.From($"Contributor {i}")));
    }
    
    await dbContext.SaveChangesAsync();
  }
}