using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public class InProcessBus : IEventHandlerRegistrar, IEventPublisher, IHandlerRegistrar
    {
        private readonly ILogger<InProcessBus> _logger;
        private readonly List<HandlerWrapper> _handlers = [];

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

        [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public void RegisterEventHandler<T>(Func<T, Task> handler)
            where T : IEvent
        {
            var eventType = typeof(T);

            var handlerWrapper = new HandlerWrapper
            {
                EventType = eventType,
                HandlerModuleName = handler.Target?.GetType().Module.Assembly.GetName().Name,
                Handler = (message, _) => handler((T)message),
                Logger = _logger
            };

            _handlers.Add(handlerWrapper);
        }

        [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public void RegisterEventHandler<T>(Func<T, CancellationToken, Task> handler)
            where T : IEvent
        {
            RegisterHandler(handler);
        }

        [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public void RegisterHandler<T>(Func<T, CancellationToken, Task> handler)
            where T : IMessage
        {
            var eventType = typeof(T);

            var handlerWrapper = new HandlerWrapper
            {
                EventType = eventType,
                HandlerModuleName = handler.Target?.GetType().Module.Assembly.GetName().Name,
                Handler = (message, token) => handler((T)message, token),
                Logger = _logger
            };

            _handlers.Add(handlerWrapper);
        }

        public void RegisterEventHandler<T>(IEventHandler<T> handler)
            where T : IEvent
        {
            var eventType = typeof(T);
            var handlerType = handler.GetType();

            var handlerWrapper = new HandlerWrapper
            {
                EventType = eventType,
                HandlerType = handlerType,
                HandlerModuleName = handlerType.Module.Assembly.GetName().Name,
                Handler = (message, _) => handler.Handle((T)message),
                Logger = _logger
            };

            _handlers.Add(handlerWrapper);
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            var eventType = typeof(T);
            var handlerType = handler.GetType();

            var handlerWrapper = new HandlerWrapper
            {
                EventType = eventType,
                HandlerType = handlerType,
                HandlerModuleName = handlerType.Module.Assembly.GetName().Name,
                Handler = (message, cancellationToken) => handler.Handle((T)message, cancellationToken),
                Logger = _logger
            };

            _handlers.Add(handlerWrapper);
        }

        public void UnregisterEventHandler<T>(Type handlerType = null)
            where T : IEvent
        {
            var eventType = typeof(T);

            var handlersToRemove = _handlers
                .Where(x =>
                    x.EventType.IsAssignableFrom(eventType) &&
                    (handlerType is null || x.HandlerType == handlerType))
                .ToList();

            handlersToRemove.ForEach(x => _handlers.Remove(x));
        }

        public void UnregisterAllEventHandlers()
        {
            _handlers.Clear();
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            if (EventSuppressor.EventsSuppressed)
            {
                return;
            }

            var eventType = @event.GetType();

            var handlers = _handlers
                .Where(x => x.EventType.IsAssignableFrom(eventType))
                .ToList();

            if (handlers.Count > 0)
            {
                await Task.WhenAll(handlers.Select(x => x.Handle(@event, cancellationToken)));
            }
        }
    }
}
