using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.StateMachines;

namespace VirtoCommerce.Client.Orders.StateMachines
{
    public abstract class StateMachineBase<T> : IStateMachine<T>
    {
        protected List<StateTransitionDef<T>> _transistions = new List<StateTransitionDef<T>>();
        protected StateTransitionDef<T> RegisterStatus(T state)
        {
            var retVal = _transistions.FirstOrDefault(x => x.CurrentStatus.Equals(state));
            if (retVal == null)
            {
                retVal = new StateTransitionDef<T>(state);
                _transistions.Add(retVal);
            }

            return retVal;
        }

        #region IStateMachine<T> Members
        public IEnumerable<T> AllRegisteredStatuses
        {
            get
            {
                return _transistions.Select(x => x.CurrentStatus);
            }
        }

        public IEnumerable<T> GetAvailableTransitions(T status)
        {
            IEnumerable<T> result;

            var currentState = _transistions.FirstOrDefault(x => x.CurrentStatus.Equals(status));
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
