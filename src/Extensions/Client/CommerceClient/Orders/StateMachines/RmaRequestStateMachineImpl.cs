using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Client.Orders.StateMachines
{
	public class RmaRequestStateMachineImpl : StateMachineBase<string>
	{
		public RmaRequestStateMachineImpl()
		{
			var awaitingCompletionState = RegisterStatus(RmaRequestStatus.AwaitingCompletion.ToString());
			var awaitingStockReturnState = RegisterStatus(RmaRequestStatus.AwaitingStockReturn.ToString());
			var canceledState = RegisterStatus(RmaRequestStatus.Canceled.ToString());
			var completeState = RegisterStatus(RmaRequestStatus.Complete.ToString());

			//AwaitingCompletion -> Complete, Canceled
			awaitingCompletionState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, completeState });

			//AwaitingStockReturn -> Canceled, AwaitingCompletion
			awaitingStockReturnState.RegisterTransitions(new StateTransitionDef<string>[] { canceledState, awaitingCompletionState });
		}
	}
}
