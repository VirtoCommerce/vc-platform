using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.StateMachines;

namespace VirtoCommerce.Client.Orders.StateMachines
{
    public class OrderStateMachineImpl : StateMachineBase<string>
    {
        public OrderStateMachineImpl()
        {
            var pendingState = RegisterStatus(OrderStatus.Pending.ToString());
            var canceledState = RegisterStatus(OrderStatus.Cancelled.ToString());
            var completedState = RegisterStatus(OrderStatus.Completed.ToString());
            var inProgressState = RegisterStatus(OrderStatus.InProgress.ToString());
            var holdState = RegisterStatus(OrderStatus.OnHold.ToString());
            var partiallyShippedState = RegisterStatus(OrderStatus.PartiallyShipped.ToString());
            var awaitingExchangeState = RegisterStatus(OrderStatus.AwaitingExchange.ToString());

            //Pending -> cancel, InProgress
            pendingState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, inProgressState });

            //InProgress -> cancel, completed, hold, partiallyShipped, awaitingExchange
            // order can become onHold only when it's inProgress
            inProgressState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, completedState, holdState, partiallyShippedState, awaitingExchangeState });

            //OnHold -> cancel, inProgress
            holdState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, inProgressState });

            //PartiallyShipped -> cancel, completed, inProgress 
            partiallyShippedState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, completedState, inProgressState });

            //AwaitingExchange -> cancel, partiallyShipped, inProgress
            awaitingExchangeState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, partiallyShippedState, inProgressState });
        }
    }
}
