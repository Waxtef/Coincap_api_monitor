using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Coincap.API.Behaviors;

// Mide el tiempo de ejecución de cada Handler y emite una advertencia si supera 500ms
// Ayuda a detectar consultas lentas o problemas de rendimiento sin modificar el Handler
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    // Umbral en milisegundos — si el Handler tarda más de esto se emite un warning
    private const int WarningThresholdMs = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        // Solo registra si tardó más del umbral — no genera ruido en condiciones normales
        if (stopwatch.ElapsedMilliseconds > WarningThresholdMs)
        {
            _logger.LogWarning("Handler lento detectado: {RequestName} tardó {ElapsedMs}ms",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
