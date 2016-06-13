using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface IEventPublisher<T>
    {
        /// <summary>
        /// Publish event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        void Publish(T eventMessage);
    }
}
