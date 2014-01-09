using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Client.Orders.StateMachines
{
	public class ShipmentStateMachineImpl : StateMachineBase<string>
	{
		public ShipmentStateMachineImpl()
		{
			var awaitingInventoryState = RegisterStatus(ShipmentStatus.AwaitingInventory.ToString());
			var canceledState = RegisterStatus(ShipmentStatus.Cancelled.ToString());
			var inventoryAssignedState = RegisterStatus(ShipmentStatus.InventoryAssigned.ToString());
			var onHoldState = RegisterStatus(ShipmentStatus.OnHold.ToString());
			var packingState = RegisterStatus(ShipmentStatus.Packing.ToString());
			var releasedState = RegisterStatus(ShipmentStatus.Released.ToString());
			var shippedState = RegisterStatus(ShipmentStatus.Shipped.ToString());

			//AwaitingInventory -> Cancelled, InventoryAssigned, OnHold
			awaitingInventoryState.RegisterTransitions(new[] { canceledState, inventoryAssignedState, onHoldState });

			//InventoryAssigned -> Cancelled,  OnHold, Released
			inventoryAssignedState.RegisterTransitions(new[] { canceledState, releasedState, onHoldState });

			//OnHold -> AwaitingInventory, InventoryAssigned, Packing, Released
			onHoldState.RegisterTransitions(new[] { awaitingInventoryState, inventoryAssignedState, packingState, releasedState });

			//Packing -> Cancelled, OnHold, Shipped
			packingState.RegisterTransitions(new[] { canceledState, onHoldState, shippedState });

			//Released -> Cancelled, OnHold, Packing
			releasedState.RegisterTransitions(new[] { canceledState, onHoldState, packingState, });
		}
	}
}
