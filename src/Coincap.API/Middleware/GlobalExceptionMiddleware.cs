using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Coincap.API.Middleware;

// Captura cualquier excepción no manejada en toda la aplicación
// Convierte los errores en respuestas JSON consistentes con el estándar Problem Details (RFC 7807)
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Intenta ejecutar el siguiente middleware o el endpoint
            await _next(context);
        }
        catch (ValidationException ex)
        {
            // Errores de FluentValidation → HTTP 400 Bad Request
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Validation Error",
                string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (KeyNotFoundException ex)
        {
            // Recurso no encontrado → HTTP 404 Not Found
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (Exception ex)
        {
            // Cualquier otro error inesperado → HTTP 500 Internal Server Error
            _logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError,
                "Server Error", "Ocurrió un error interno. Intenta más tarde.");
        }
    }

    // Construye la respuesta JSON en formato Problem Details y la escribe en el response
    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
