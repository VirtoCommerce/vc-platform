using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common.Events
{
    /// <summary>
    /// Common domain event publisher implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class EventPublisher<T> : IEventPublisher<T>
    {
        private readonly IAsyncObserver<T>[] _observers;
        public EventPublisher(IAsyncObserver<T>[] observers)
        {
            _observers = observers;
        }

        #region IEventPublisher Members

        public async Task PublishAsync(T eventMessage)
        {
            foreach (var observer in _observers)
            {
                await observer.OnNextAsync(eventMessage);
            }
        }

        #endregion

    }
}
