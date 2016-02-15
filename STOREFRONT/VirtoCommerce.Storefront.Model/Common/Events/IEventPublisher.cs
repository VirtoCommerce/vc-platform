using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common.Events
{
    /// <summary>
    /// Generic interface for all event publisher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventPublisher<T>
    {
        /// <summary>
        /// Publish event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        Task PublishAsync(T eventMessage);
    }
}
