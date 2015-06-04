using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Orders.StateMachines
{
	public interface IStateMachine<T>
	{
		IEnumerable<T> AllRegisteredStatuses { get; }
		IEnumerable<T> GetAvailableTransitions(T status);
		bool IsTransitionAvailable(T curentStatus, T newStatus);
	}
}
