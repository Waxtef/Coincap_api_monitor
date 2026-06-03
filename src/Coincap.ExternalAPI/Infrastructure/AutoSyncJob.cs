using Coincap.ExternalAPI.Application.Commands.SyncAssets;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Coincap.ExternalAPI.Infrastructure;

// Servicio que corre en segundo plano y sincroniza con CoinCap automáticamente cada N minutos
// IHostedService lo inicia .NET al arrancar la aplicación y lo detiene al apagarla
public class AutoSyncJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AutoSyncJob> _logger;
    private readonly CoinCapOptions _options;

    public AutoSyncJob(
        IServiceScopeFactory scopeFactory,
        ILogger<AutoSyncJob> logger,
        IOptions<CoinCapOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AutoSyncJob iniciado. Intervalo: {Minutes} minutos", _options.SyncIntervalMinutes);

        // Sync inmediato al arrancar — la BD no queda vacía mientras llega el primer tick
        await RunSyncAsync(stoppingToken);

        // PeriodicTimer es más eficiente que Task.Delay — no bloquea hilos mientras espera
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(_options.SyncIntervalMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunSyncAsync(stoppingToken);
        }
    }

    // Método extraído para no repetir el mismo bloque try/catch dos veces
    private async Task RunSyncAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Crea un scope nuevo por cada ejecución porque ISender es Scoped, no Singleton
            using var scope = _scopeFactory.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            await sender.Send(new SyncAssetsCommand(), stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la sincronización automática");
        }
    }
}
