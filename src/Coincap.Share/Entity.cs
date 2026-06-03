namespace Coincap.Share;

// Clase base de la que heredan todas las entidades del dominio (Asset, PriceAlert, PriceSnapshot)
// TId es genérico porque cada entidad usa un tipo de Id diferente: string, Guid, int, etc.
public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;

    protected Entity(TId id) => Id = id;

    // Requerido por EF Core para poder reconstruir entidades cuando las trae de SQLite
    protected Entity() { }

    // Compara por Id y tipo — sin esto C# compararía si son el mismo objeto en memoria
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return Id!.Equals(other.Id);
    }

    public override int GetHashCode() => Id!.GetHashCode();

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}
