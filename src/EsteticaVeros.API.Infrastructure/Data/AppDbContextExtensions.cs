namespace EsteticaVeros.API.Infrastructure.Data;

public static class AppDbContextExtensions
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString)
  {
    // Determinar el proveedor basándose en el connection string
    if (connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase) ||
        connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase) && 
        !connectionString.Contains(".db", StringComparison.OrdinalIgnoreCase) &&
        !connectionString.Contains(".sqlite", StringComparison.OrdinalIgnoreCase))
    {
      // SQL Server
      services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
    }
    else
    {
      // SQLite (fallback)
      services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString));
    }
  }
}
