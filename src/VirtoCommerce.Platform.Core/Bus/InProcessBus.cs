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
    public class InProcessBus : IEventPublisher, IHandlerRegistrar
    {
        private readonly ILogger<InProcessBus> _logger;
        private readonly Dictionary<Type, List<HandlerWrapper>> _handlersByType = new Dictionary<Type, List<HandlerWrapper>>();

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

        public void RegisterHandler<T>(Func<T, CancellationToken, Task> handler) where T : class, IMessage
        {
            if (!_handlersByType.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<HandlerWrapper>();
                _handlersByType.Add(typeof(T), handlers);
            }

            var handlerWrapper = new HandlerWrapper
            {
                EventName = typeof(T).Name,
                HandlerModuleName = handler.Target.GetType().Module.Assembly.GetName().Name,
                Handler = (message, token) => handler((T)message, token),
                Logger = _logger
            };

            handlers.Add(handlerWrapper);
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent
        {
            if (!EventSuppressor.EventsSuppressed && _handlersByType.TryGetValue(@event.GetType(), out var handlers))
            {
                await Task.WhenAll(handlers.Select(handler => handler.Handle(@event, cancellationToken)));
            }
        }
    }
}
