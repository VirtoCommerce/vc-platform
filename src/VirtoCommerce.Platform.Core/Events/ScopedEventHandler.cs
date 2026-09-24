using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

// Stands in for THandler: resolves it from a DI scope of its own for every event, so the handler gets the lifetime it was registered with.
public sealed class ScopedEventHandler<TEvent, THandler> : IEventHandler<TEvent>, IScopedEventHandler
    where TEvent : IEvent
    where THandler : IEventHandler<TEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ScopedEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Type HandlerType => typeof(THandler);

    public async Task Handle(TEvent message)
    {
        // One scope per invocation: handlers of the same event run concurrently and must not share scoped services.
        await using var scope = _scopeFactory.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<THandler>().Handle(message);
    }

    public Type ResolveImplementationType()
    {
        using var scope = _scopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<THandler>().GetType();
    }
}
