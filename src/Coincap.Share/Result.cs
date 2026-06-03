namespace Coincap.Share;

// Envuelve el resultado de una operación: éxito con datos o fallo con mensaje de error
// Evita usar excepciones para flujos esperados como "activo no encontrado"
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    // Crea un resultado exitoso con los datos
    public static Result<T> Success(T value) => new(true, value, null);

    // Crea un resultado fallido con el mensaje de error
    public static Result<T> Failure(string error) => new(false, default, error);
}

// Versión sin datos para operaciones que no retornan nada (solo éxito o fallo)
public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}
