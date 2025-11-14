using System;

namespace VirtoCommerce.Platform.Core.Events;

public interface IEventHandlerRegistrar
{
    void RegisterEventHandler<T>(IEventHandler<T> handler) where T : IEvent;
    void RegisterEventHandler<T>(ICancellableEventHandler<T> handler) where T : IEvent;

    void UnregisterEventHandler<T>(Type handlerType = null) where T : IEvent;
    void UnregisterAllEventHandlers();
}
