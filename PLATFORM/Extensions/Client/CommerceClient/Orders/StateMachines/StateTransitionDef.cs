using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Client.Orders.StateMachines
{	
	public class StateTransitionDef<T>
	{
		public T CurrentStatus;
		private readonly List<StateTransitionDef<T>> _availableTransitions = new List<StateTransitionDef<T>>();
		
		public StateTransitionDef(T status)
		{
			CurrentStatus = status;
			_availableTransitions.Add(this);
		}

		public void RegisterTransitions(IEnumerable<StateTransitionDef<T>> availableTransitons)
		{
			_availableTransitions.AddRange(availableTransitons.Where(x=>!x.CurrentStatus.Equals(CurrentStatus)));
		}
		/// <summary>
		/// Gets all transitions.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<StateTransitionDef<T>> AllTransitions
		{
			get
			{
				return _availableTransitions.Concat(_availableTransitions).Distinct();
			}
		}
		
		public bool IsTransitionAvailable(StateTransitionDef<T> newState)
		{
			return IsTransitionAvailable(AllTransitions.ToList(), newState);
		}

		private static bool IsTransitionAvailable(List<StateTransitionDef<T>> transitions, StateTransitionDef<T> newState)
		{
			return transitions.Any(x => x.CurrentStatus.Equals(newState.CurrentStatus));
		}
	}

}
