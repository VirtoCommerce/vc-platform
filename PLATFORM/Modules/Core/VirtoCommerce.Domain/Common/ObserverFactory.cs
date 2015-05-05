using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common
{
	public class ObserverFactory<T> : IObserver<T>
	{
		private readonly Func<IObserver<T>> _factory;
		public ObserverFactory(Func<IObserver<T>> factory)
		{
			_factory = factory;
		}

		#region IObserver<T> Members

		public void OnCompleted()
		{
			_factory().OnCompleted();
		}

		public void OnError(Exception error)
		{
			_factory().OnError(error);
		}

		public void OnNext(T value)
		{
			_factory().OnNext(value);
		}

		#endregion
	}
}
