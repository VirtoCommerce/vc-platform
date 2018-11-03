using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events
{
    [Obsolete("Use non generic IEventPublisher instead")]
    public interface IEventPublisher<T>
    {
        /// <summary>
        /// Publish event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        void Publish(T eventMessage);
    }
    public interface IEventPublisher
    {
        Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent;
    }
}
