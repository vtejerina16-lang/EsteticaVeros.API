using EsteticaVeros.API.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()    // This sets up OpenTelemetry logging
       .AddLoggerConfigs();     // This adds Serilog for console formatting

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Starting web host");

builder.Services.AddOptionConfigs(builder.Configuration, startupLogger, builder);
builder.Services.AddServiceConfigs(startupLogger, builder);
builder.Services.AddAuthenticationConfigs(builder.Configuration, startupLogger);
builder.Services.AddAuthorizationConfigs(startupLogger);
builder.Services.AddCorsConfigs(builder.Configuration, startupLogger);

builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                  o.ShortSchemaNames = true;
                  o.DocumentSettings = settings =>
                  {
                    settings.Title = "EsteticaVeros API";
                    settings.Version = "v1";
                  };
                  
                  o.EnableJWTBearerAuth = true;
                });

var app = builder.Build();

app.UseCors(CorsConfig.DefaultPolicyName);

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints(); // Aspire health checks and metrics

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
