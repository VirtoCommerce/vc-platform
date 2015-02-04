using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Workflow;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;

namespace VirtoCommerce.OrderModule.Data.Workflow
{
	public class ObservableOrderWorkflowService : IWorkflowService, IObservable<CustomerOrder>
	{
		private readonly List<IObserver<CustomerOrder>> _observers;
		public ObservableOrderWorkflowService()
		{
			_observers = new List<IObserver<CustomerOrder>>();
		}

		#region IWorkflowService Members

		public WorkflowResult RunWorkflow(string workflowName, Dictionary<string, object> parameters, object[] extensions)
		{
			WorkflowResult retVal = new WorkflowResult();
			var customerOrder = parameters.Values.OfType<CustomerOrder>().FirstOrDefault();
			if (customerOrder != null)
			{
				foreach (var observer in _observers)
				{
					observer.OnNext(customerOrder);
				}

				foreach (var observer in _observers)
				{
					observer.OnCompleted();
				}
			}
			return retVal;
		}

		#endregion

		#region IObservable<CustomerOrder> Members

		public IDisposable Subscribe(IObserver<CustomerOrder> observer)
		{
			_observers.Add(observer);
			return new Unsubscriber(_observers, observer);
		}

		#endregion

		private class Unsubscriber : IDisposable
		{
			private List<IObserver<CustomerOrder>> _observers;
			private IObserver<CustomerOrder> _observer;

			public Unsubscriber(List<IObserver<CustomerOrder>> observers, IObserver<CustomerOrder> observer)
			{
				this._observers = observers;
				this._observer = observer;
			}

			public void Dispose()
			{
				if (_observer != null && _observers.Contains(_observer))
					_observers.Remove(_observer);
			}
		}
	}
}
