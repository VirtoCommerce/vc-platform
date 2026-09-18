using System;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

public interface IEventHandlerRegistrar
{
    void RegisterEventHandler<T>(IEventHandler<T> handler) where T : IEvent;
    void RegisterEventHandler<T>(ICancellableEventHandler<T> handler) where T : IEvent;

    // Type-based registration: the handler is resolved from serviceProvider when an event is published, so it gets the lifetime it was registered with.
    // The default implementations keep the legacy behavior (one root-resolved instance) for registrars that predate these members.
    void RegisterEventHandler<TEvent, THandler>(IServiceProvider serviceProvider)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
        => RegisterEventHandler<TEvent>(serviceProvider.GetRequiredService<THandler>());

    void RegisterCancellableEventHandler<TEvent, THandler>(IServiceProvider serviceProvider)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
        => RegisterEventHandler<TEvent>(serviceProvider.GetRequiredService<THandler>());

    void UnregisterEventHandler<T>(Type handlerType = null) where T : IEvent;
    void UnregisterAllEventHandlers();
}
