using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common.Events
{
	public class EventPublisher<T>: IEventPublisher<T>
	{
		private readonly IObserver<T>[] _observers;
		public EventPublisher(IObserver<T>[] observers)
		{
			_observers = observers;
		}
		#region IEventPublisher Members

		public void Publish(T eventMessage)
		{
			foreach (var observer in _observers)
			{
				observer.OnNext(eventMessage);
			}

			foreach (var observer in _observers)
			{
				observer.OnCompleted();
			}
		}

		#endregion
	
	}
}
