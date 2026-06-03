using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Coincap.API.Behaviors;

// Intercepta cada Command y Query antes de llegar al Handler
// MediatR lo ejecuta automáticamente — no hay que llamarlo manualmente
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        // Registra el nombre del Command o Query que está entrando
        _logger.LogInformation("Iniciando: {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        // Llama al siguiente paso de la cadena (otro Behavior o el Handler final)
        var response = await next();

        stopwatch.Stop();

        // Registra cuánto tardó en completarse el Handler
        _logger.LogInformation("Completado: {RequestName} en {ElapsedMs}ms", requestName, stopwatch.ElapsedMilliseconds);

        return response;
    }
}
