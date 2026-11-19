using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

// Stands in for THandler: resolves it from a DI scope of its own for every event, so the handler gets the lifetime it was registered with.
// Registered through IEventHandlerRegistrar like any handler; the bus stores it as it is, subscribed to TEvent.
public sealed class ScopedCancellableEventHandler<TEvent, THandler> : EventHandlerRegistration, ICancellableEventHandler<TEvent>
    where TEvent : IEvent
    where THandler : ICancellableEventHandler<TEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScopedCancellableEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public override Type EventType => typeof(TEvent);

    public override Type HandlerType => typeof(THandler);

    public async Task Handle(TEvent message, CancellationToken cancellationToken)
    {
        // One scope per invocation: handlers of the same event run concurrently and must not share scoped services.
        await using var scope = _scopeFactory.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<THandler>().Handle(message, cancellationToken);
    }

    public override Task Handle(IEvent @event, CancellationToken cancellationToken)
    {
        return Handle((TEvent)@event, cancellationToken);
    }

    // Resolves the handler once.
    public override Type ResolveImplementationType()
    {
        using var scope = _scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<THandler>().GetType();
    }
}
