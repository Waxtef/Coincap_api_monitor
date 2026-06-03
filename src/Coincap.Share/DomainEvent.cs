using MediatR;

namespace Coincap.Share;

// Clase base para todos los eventos del dominio
// Hereda de INotification para que MediatR pueda publicarlos y enrutarlos a sus handlers
public abstract class DomainEvent : INotification
{
    // Registra exactamente cuándo ocurrió el evento, siempre en UTC
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
