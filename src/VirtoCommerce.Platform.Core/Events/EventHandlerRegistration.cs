using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events;

// One entry in the event bus: a handler subscribed to one event type. ScopedEventHandler resolves the handler from DI
// per event; the bus wraps a handler instance registered directly in an entry of its own.
public abstract class EventHandlerRegistration
{
    public abstract Type EventType { get; }

    // The type the handler was registered as.
    public abstract Type HandlerType { get; }

    public abstract Task Handle(IEvent @event, CancellationToken cancellationToken);

    // The runtime type of the handler, which differs from HandlerType when a derived type is registered in DI for it.
    public virtual Type ResolveImplementationType()
    {
        return HandlerType;
    }
}
