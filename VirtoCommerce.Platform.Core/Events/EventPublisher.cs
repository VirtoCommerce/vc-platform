using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events
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
			var observers = _observers.OrderBy(x => x is IPriorityObserver ? ((IPriorityObserver)x).Priority : 0);
			var errors = new List<Exception>();
			foreach (var observer in observers)
			{
				try
				{
					observer.OnNext(eventMessage);
				}
				catch(Exception ex)
				{
					errors.Add(ex);
				}
			}

			if (errors.Any())
			{
				foreach (var observer in observers)
				{
					foreach(var error in errors)
					{
						observer.OnError(error);
					}
				}
			}
			else
			{
				foreach (var observer in observers)
				{
					observer.OnCompleted();
				}
			}
		}

		#endregion
	
	}
}
