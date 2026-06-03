using FluentValidation;
using MediatR;

namespace Coincap.API.Behaviors;

// Ejecuta las validaciones de FluentValidation antes de que el request llegue al Handler
// Si hay errores de validación retorna 400 automáticamente sin llegar al Handler
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    // IEnumerable porque puede haber varios validadores para el mismo request
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Si no hay validadores registrados para este request, pasa directo al Handler
        if (!_validators.Any())
            return await next();

        // Ejecuta todos los validadores y recopila los errores
        var errors = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        // Si hay errores lanza una excepción que el GlobalExceptionMiddleware convierte en 400
        if (errors.Count != 0)
            throw new ValidationException(errors);

        return await next();
    }
}
