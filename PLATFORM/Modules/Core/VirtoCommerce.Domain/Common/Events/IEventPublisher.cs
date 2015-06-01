using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common.Events
{
	public interface IEventPublisher<T>
	{
		/// <summary>
		/// Publish event
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="eventMessage">Event message</param>
		void Publish(T eventMessage);
	}
}
