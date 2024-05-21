using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events;

public interface IEventHandlerRegistrar
{
    [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    void RegisterEventHandler<T>(Func<T, Task> handler) where T : IEvent;

    [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    void RegisterEventHandler<T>(Func<T, CancellationToken, Task> handler) where T : IEvent;

    void RegisterEventHandler<T>(IEventHandler<T> handler) where T : IEvent;
    void RegisterEventHandler<T>(ICancellableEventHandler<T> handler) where T : IEvent;

    void UnregisterEventHandler<T>(Type handlerType = null) where T : IEvent;
    void UnregisterAllEventHandlers();
}
