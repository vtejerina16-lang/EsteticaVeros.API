using EsteticaVeros.API.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS ---

builder.AddServiceDefaults() 
       .AddLoggerConfigs(); 

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

startupLogger.LogInformation("Iniciando Web Host con Parche de CORS");

// 1. REGISTRO DEL PARCHE (Permite todo desde cualquier lugar)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ParcheCORS", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

// --- MIDDLEWARE ---

// 2. APLICACIÓN DEL PARCHE
// Usamos "ParcheCORS" en lugar de CorsConfig.DefaultPolicyName para forzar la apertura
app.UseCors("ParcheCORS"); 

await app.UseAppMiddlewareAndSeedDatabase();

app.MapDefaultEndpoints(); 

app.Run();

public partial class Program { }