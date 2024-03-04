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
        private readonly List<HandlerWrapper> _handlers = [];

        public InProcessBus(ILogger<InProcessBus> logger)
        {
            _logger = logger;
        }

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

        public async Task Publish<T>(T @event, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            if (EventSuppressor.EventsSuppressed)
            {
                return;
            }

            var eventType = typeof(T);

            var tasks = _handlers
                .Where(x => x.EventType.IsAssignableFrom(eventType))
                .Select(x => x.Handle(@event, cancellationToken))
                .ToList();

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
        }
    }
}
