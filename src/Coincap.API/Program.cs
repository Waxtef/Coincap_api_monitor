using Coincap.API.Behaviors;
using Coincap.API.Data;
using Coincap.API.Middleware;
using Coincap.Alerts.Application;
using Coincap.Alerts.Application.Services;
using Coincap.Alerts.Domain;
using Coincap.API.Repositories.Alerts;
using Coincap.API.Repositories.Assets;
using Coincap.API.Repositories.PriceHistory;
using Coincap.Assets.Domain;
using Coincap.PriceHistory.Domain;
using Coincap.ExternalAPI.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR — registra Handlers y Pipeline Behaviors
// Escanea todos los assemblies del proyecto para encontrar Handlers automaticamente
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IPriceSnapshotRepository, PriceSnapshotRepository>();
builder.Services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();

// Servicios del módulo Alerts
builder.Services.AddScoped<PriceVariationService>();
builder.Services.Configure<AlertOptions>(
    builder.Configuration.GetSection(AlertOptions.Section));

// CoinCap — configuración tipada desde appsettings.json
builder.Services.Configure<CoinCapOptions>(
    builder.Configuration.GetSection(CoinCapOptions.Section));

// CoinCapClient con Resilience 
// AddStandardResilienceHandler agrega las 3 políticas de resiliencia
builder.Services.AddHttpClient<CoinCapClient>()
    .AddStandardResilienceHandler();

// sincronización automática cada # minutos
builder.Services.AddHostedService<AutoSyncJob>();

var app = builder.Build();

// Middleware global de excepciones
app.UseMiddleware<GlobalExceptionMiddleware>();

// Swagger DEV
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
