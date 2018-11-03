using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    public class InProcessBus : IEventPublisher, IHandlerRegistrar
    {

        private readonly Dictionary<Type, List<Func<IMessage, CancellationToken, Task>>> _routes = new Dictionary<Type, List<Func<IMessage, CancellationToken, Task>>>();

        public void RegisterHandler<T>(Func<T, CancellationToken, Task> handler) where T : class, IMessage
        {
            if (!_routes.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new List<Func<IMessage, CancellationToken, Task>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add((message, token) => handler((T)message, token));
        }

        public Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent
        {
            //Do not emit events if this flag is set
            if (!EventSupressor.EventsSuppressed)
            {
                if (_routes.TryGetValue(@event.GetType(), out var handlers))
                {
                    Task.Factory.StartNew(() => Task.WhenAll(handlers.Select(handler => handler(@event, cancellationToken))), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
                }
            }
            return Task.CompletedTask;
        }
    }
}
