using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public class InProcessBus : IEventHandlerRegistrar, IEventPublisher
    {
        private readonly ILogger<InProcessBus> _logger;
        private readonly List<HandlerWrapper> _handlers = [];

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

        public void RegisterEventHandler<T>(IEventHandler<T> handler)
            where T : IEvent
        {
            AddHandler<T>(handler.GetType(), handler.GetType(), (message, _) => handler.Handle((T)message));
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            AddHandler<T>(handler.GetType(), handler.GetType(), (message, cancellationToken) => handler.Handle((T)message, cancellationToken));
        }

        public void RegisterEventHandler<TEvent, THandler>(IServiceProvider serviceProvider)
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            if (IsSingleton<THandler>(serviceProvider))
            {
                var handler = serviceProvider.GetRequiredService<THandler>();
                AddHandler<TEvent>(typeof(THandler), handler.GetType(), (message, _) => handler.Handle((TEvent)message));
            }
            else
            {
                RegisterScopedEventHandler<TEvent, THandler>(serviceProvider, (handler, message, _) => handler.Handle(message));
            }
        }

        public void RegisterCancellableEventHandler<TEvent, THandler>(IServiceProvider serviceProvider)
            where TEvent : IEvent
            where THandler : ICancellableEventHandler<TEvent>
        {
            if (IsSingleton<THandler>(serviceProvider))
            {
                var handler = serviceProvider.GetRequiredService<THandler>();
                AddHandler<TEvent>(typeof(THandler), handler.GetType(), (message, cancellationToken) => handler.Handle((TEvent)message, cancellationToken));
            }
            else
            {
                RegisterScopedEventHandler<TEvent, THandler>(serviceProvider, (handler, message, cancellationToken) => handler.Handle(message, cancellationToken));
            }
        }

        public void UnregisterEventHandler<T>(Type handlerType = null)
            where T : IEvent
        {
            var eventType = typeof(T);

            var handlersToRemove = _handlers
                .Where(x =>
                    x.EventType.IsAssignableFrom(eventType) &&
                    (handlerType is null || x.HandlerType == handlerType || x.ImplementationType == handlerType))
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

        private void RegisterScopedEventHandler<TEvent, THandler>(IServiceProvider serviceProvider, Func<THandler, TEvent, CancellationToken, Task> invoke)
            where TEvent : IEvent
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            // The handler is constructed once here, as it was before type-based registration: a handler missing from DI stays
            // a startup error rather than a first-event error, and the probe yields the runtime type for Unregister and logging.
            Type implementationType;
            using (var probe = scopeFactory.CreateScope())
            {
                implementationType = probe.ServiceProvider.GetRequiredService<THandler>().GetType();
            }

            AddHandler<TEvent>(typeof(THandler), implementationType, async (message, cancellationToken) =>
            {
                // One scope per invocation: handlers of the same event run concurrently and must not share scoped services.
                await using var scope = scopeFactory.CreateAsyncScope();
                await invoke(scope.ServiceProvider.GetRequiredService<THandler>(), (TEvent)message, cancellationToken);
            });
        }

        private void AddHandler<TEvent>(Type handlerType, Type implementationType, Func<IMessage, CancellationToken, Task> handler)
            where TEvent : IEvent
        {
            _handlers.Add(new HandlerWrapper
            {
                EventType = typeof(TEvent),
                HandlerType = handlerType,
                ImplementationType = implementationType,
                HandlerModuleName = implementationType.Module.Assembly.GetName().Name,
                Handler = handler,
                Logger = _logger,
            });
        }

        // Startup registers the IServiceCollection itself as a singleton once all modules are initialized. With it, a singleton
        // handler is registered as an instance, exactly as before. Without it (custom hosts, test containers) every handler takes
        // the per-invocation path, which still yields the root singleton instance but creates and disposes a scope per event.
        private static bool IsSingleton<THandler>(IServiceProvider serviceProvider)
        {
            var descriptor = serviceProvider.GetService<IServiceCollection>()?.LastOrDefault(x => x.ServiceType == typeof(THandler));

            return descriptor?.Lifetime == ServiceLifetime.Singleton;
        }
    }
}
