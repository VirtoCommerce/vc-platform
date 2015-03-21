using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.StateMachines;

namespace VirtoCommerce.Client.Orders.StateMachines
{
	public abstract class StateMachineBase<T> : IStateMachine<T>
	{
		protected List<StateTransitionDef<T>> Transitions = new List<StateTransitionDef<T>>();
		protected StateTransitionDef<T> RegisterStatus(T state)
		{
			var retVal = Transitions.FirstOrDefault(x => x.CurrentStatus.Equals(state));
			if (retVal == null)
			{
				retVal = new StateTransitionDef<T>(state);
				Transitions.Add(retVal);
			}

			return retVal;
		}

		#region IStateMachine<T> Members
		public IEnumerable<T> AllRegisteredStatuses
		{
			get
			{
				return Transitions.Select(x => x.CurrentStatus);
			}
		}

		public IEnumerable<T> GetAvailableTransitions(T status)
		{
			IEnumerable<T> result;

			var currentState = Transitions.FirstOrDefault(x => x.CurrentStatus.Equals(status));
			if (currentState == null)
			{
				result = Enumerable.Empty<T>();
			}
			else
			{
				result = currentState.AllTransitions.Select(x => x.CurrentStatus);
			}

			return result;
		}

		public bool IsTransitionAvailable(T curentStatus, T newStatus)
		{
			return GetAvailableTransitions(curentStatus).Contains(newStatus);
		}
		#endregion
	}
}
