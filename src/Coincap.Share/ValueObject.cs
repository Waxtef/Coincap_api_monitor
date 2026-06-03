namespace Coincap.Share;

// Clase base para objetos que se comparan por sus valores, no por identidad
// Ejemplo de uso: AssetId("bitcoin") == AssetId("bitcoin") → true
public abstract class ValueObject
{
    // Cada subclase define qué propiedades determinan su igualdad
    protected abstract IEnumerable<object?> GetEqualityComponents();

    // Compara componente por componente en lugar de comparar la referencia en memoria
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    // HashCode combinado de todos los componentes para que funcione en colecciones
    public override int GetHashCode()
        => GetEqualityComponents()
            .Aggregate(1, (current, obj) => HashCode.Combine(current, obj));

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
