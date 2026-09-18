using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Events;

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
            var handlerType = handler.GetType();

            _handlers.Add(new HandlerWrapper
            {
                EventType = typeof(T),
                HandlerType = handlerType,
                ImplementationType = handlerType,
                HandlerModuleName = handlerType.Module.Assembly.GetName().Name,
                Handler = (message, _) => handler.Handle((T)message),
                Logger = _logger,
            });
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            var handlerType = handler.GetType();

            _handlers.Add(new HandlerWrapper
            {
                EventType = typeof(T),
                HandlerType = handlerType,
                ImplementationType = handlerType,
                HandlerModuleName = handlerType.Module.Assembly.GetName().Name,
                Handler = (message, cancellationToken) => handler.Handle((T)message, cancellationToken),
                Logger = _logger,
            });
        }

        public void RegisterEventHandler<TEvent, THandler>(IServiceProvider serviceProvider)
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            if (IsSingleton<THandler>(serviceProvider))
            {
                RegisterEventHandler<TEvent>(serviceProvider.GetRequiredService<THandler>());
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
                RegisterEventHandler<TEvent>(serviceProvider.GetRequiredService<THandler>());
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

            // Same fail-fast as instance registration: a handler missing from DI is a startup error, not a first-event error.
            Type implementationType;
            using (var probe = scopeFactory.CreateScope())
            {
                implementationType = probe.ServiceProvider.GetRequiredService<THandler>().GetType();
            }

            _handlers.Add(new HandlerWrapper
            {
                EventType = typeof(TEvent),
                HandlerType = typeof(THandler),
                ImplementationType = implementationType,
                HandlerModuleName = implementationType.Module.Assembly.GetName().Name,
                Handler = async (message, cancellationToken) =>
                {
                    // One scope per invocation: handlers of the same event run concurrently and must not share scoped services.
                    await using var scope = scopeFactory.CreateAsyncScope();
                    await invoke(scope.ServiceProvider.GetRequiredService<THandler>(), (TEvent)message, cancellationToken);
                },
                Logger = _logger,
            });
        }

        // Startup registers the IServiceCollection itself as a singleton once all modules are initialized.
        // A singleton handler is registered as an instance, exactly as before; without the snapshot every handler is resolved per invocation.
        private static bool IsSingleton<THandler>(IServiceProvider serviceProvider)
        {
            var descriptor = serviceProvider.GetService<IServiceCollection>()?.LastOrDefault(x => x.ServiceType == typeof(THandler));

            return descriptor?.Lifetime == ServiceLifetime.Singleton;
        }
    }
}
