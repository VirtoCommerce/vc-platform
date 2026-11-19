using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Bus
{
    public class InProcessBus : IEventHandlerRegistrar, IEventPublisher
    {
        private readonly ILogger<InProcessBus> _logger;
        private readonly List<EventHandlerRegistration> _handlers = [];

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

        public void RegisterEventHandler<T>(IEventHandler<T> handler)
            where T : IEvent
        {
            // A registration handed in as a handler, such as the ScopedEventHandler that RegisterEventHandler<TEvent, THandler>()
            // subscribes, is stored as it is. Any other instance becomes a registration for T.
            _handlers.Add(handler as EventHandlerRegistration ?? new InstanceEventHandler<T>(handler));
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            _handlers.Add(handler as EventHandlerRegistration ?? new InstanceCancellableEventHandler<T>(handler));
        }

        public void UnregisterEventHandler<T>(Type handlerType = null)
            where T : IEvent
        {
            var eventType = typeof(T);

            var handlers = _handlers
                .Where(x => x.EventType.IsAssignableFrom(eventType))
                .ToList();

            if (handlerType != null)
            {
                var matches = handlers
                    .Where(x => x.HandlerType == handlerType)
                    .ToList();

                if (matches.Count == 0)
                {
                    // Before handlers were resolved per event the bus knew each one by its runtime type, so a handler overridden
                    // in DI by a derived type was unregistered by that derived type. Resolve once to keep that working.
                    matches = handlers
                        .Where(x => x.ResolveImplementationType() == handlerType)
                        .ToList();
                }

                if (matches.Count == 0)
                {
                    _logger.LogWarning("No event handler of type {HandlerType} is registered for {EventType}", handlerType, eventType);
                }

                handlers = matches;
            }

            handlers.ForEach(x => _handlers.Remove(x));
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
                await Task.WhenAll(handlers.Select(x => Handle(x, @event, cancellationToken)));
            }
        }

        // Every registered handler receives the event, even when another one throws before its first await.
        private static async Task Handle(EventHandlerRegistration handler, IEvent @event, CancellationToken cancellationToken)
        {
            await handler.Handle(@event, cancellationToken);
        }
    }
}
