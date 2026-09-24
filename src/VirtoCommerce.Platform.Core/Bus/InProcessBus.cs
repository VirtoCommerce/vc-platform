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
    public class InProcessBus : IEventHandlerRegistrar, IEventPublisher
    {
        private readonly ILogger<InProcessBus> _logger;
        private readonly List<EventHandlerRegistration> _registrations = [];

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

        public void RegisterEventHandler<T>(IEventHandler<T> handler)
            where T : IEvent
        {
            AddHandler<T>(handler, (message, _) => handler.Handle((T)message));
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            AddHandler<T>(handler, (message, cancellationToken) => handler.Handle((T)message, cancellationToken));
        }

        public void UnregisterEventHandler<T>(Type handlerType = null)
            where T : IEvent
        {
            var eventType = typeof(T);

            var registrations = _registrations
                .Where(x => x.EventType.IsAssignableFrom(eventType))
                .ToList();

            if (handlerType != null)
            {
                var matches = registrations
                    .Where(x => x.HandlerType == handlerType)
                    .ToList();

                if (matches.Count == 0)
                {
                    // Before handlers were resolved per event the bus knew each one by its runtime type, so a handler overridden
                    // in DI by a derived type was unregistered by that derived type. Resolve once to keep that working.
                    matches = registrations
                        .Where(x => x.Handler is IScopedEventHandler scoped && scoped.ResolveImplementationType() == handlerType)
                        .ToList();
                }

                if (matches.Count == 0)
                {
                    _logger.LogWarning("No event handler of type {HandlerType} is registered for {EventType}", handlerType, eventType);
                }

                registrations = matches;
            }

            registrations.ForEach(x => _registrations.Remove(x));
        }

        public void UnregisterAllEventHandlers()
        {
            _registrations.Clear();
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            if (EventSuppressor.EventsSuppressed)
            {
                return;
            }

            var eventType = @event.GetType();

            var registrations = _registrations
                .Where(x => x.EventType.IsAssignableFrom(eventType))
                .ToList();

            if (registrations.Count > 0)
            {
                await Task.WhenAll(registrations.Select(x => Handle(x, @event, cancellationToken)));
            }
        }

        // Every registered handler receives the event, even when another one throws before its first await.
        private static async Task Handle(EventHandlerRegistration registration, IEvent @event, CancellationToken cancellationToken)
        {
            await registration.Invoke(@event, cancellationToken);
        }

        private void AddHandler<T>(object handler, Func<IMessage, CancellationToken, Task> invoke)
            where T : IEvent
        {
            // A scoped stand-in is known by the type it resolves, so Unregister works with the type the module registered.
            var handlerType = handler is IScopedEventHandler scoped ? scoped.HandlerType : handler.GetType();

            _registrations.Add(new EventHandlerRegistration(typeof(T), handlerType, handler, invoke));
        }
    }
}
