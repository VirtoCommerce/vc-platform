using System.Activities;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{

    public sealed class CalculateOrderStatusActivity : OrderActivityBase
    {
        protected override void Execute(CodeActivityContext context)
        {
            base.Execute(context);
            var item = CurrentOrderGroup as Order;
            if (item != null)
            {
                RecalculateRmaRequestStatuses(item);
                RecalculateShipmentStatuses(item);
                RecalculateOrderStatus(item);
            }
        }

        private void RecalculateRmaRequestStatuses(Order order)
        {
            foreach (var rmaRequest in order.RmaRequests
				.Where(r=>r.Status == RmaRequestStatus.AwaitingStockReturn.ToString()))
            {
                //if all lineItems physically returned to stock need change status 
                //to AwaitingCompletion
                var retVal = rmaRequest.RmaReturnItems.All(x0 => x0.RmaLineItems.All(x => x.ReturnQuantity == x.Quantity));
                if (retVal)
                {
                    rmaRequest.Status = RmaRequestStatus.AwaitingCompletion.ToString();
                }
            }
        }

        private void RecalculateShipmentStatuses(Order order)
        {
			foreach (var shipment in order.OrderForms.SelectMany(o => o.Shipments))
            {
                //End states
                if (shipment.Status == ShipmentStatus.Shipped.ToString() ||
                    shipment.Status == ShipmentStatus.Cancelled.ToString())
                {
                    continue;
                }

                // Initialize empty value
                if (string.IsNullOrEmpty(shipment.Status))
                {
                    shipment.Status = ShipmentStatus.InventoryAssigned.ToString();
                }

                //Inherit order state for shipment
                if (order.Status == OrderStatus.Cancelled.ToString())
                {
                    shipment.Status = ShipmentStatus.Cancelled.ToString();
                }
            }
        }

        private void RecalculateOrderStatus(Order order)
        {
            var newStatus = order.Status;
            var shipments = order.OrderForms[0].Shipments;
            var activeStates = new string[] { ShipmentStatus.InventoryAssigned.ToString(), ShipmentStatus.AwaitingInventory.ToString(), ShipmentStatus.Released.ToString(), ShipmentStatus.Packing.ToString() };

            bool canceledFound = shipments.Any(x => x.Status == ShipmentStatus.Cancelled.ToString());
            bool completedFound = shipments.Any(x => x.Status == ShipmentStatus.Shipped.ToString());
            bool activeFound = shipments.Any(x => activeStates.Contains(x.Status));

            if (!canceledFound && !completedFound && !activeFound)
            {
                //Not found canceled,active,completed

            }
            else if (canceledFound && !completedFound && !activeFound)
            {
                //All canceled
                //retVal = OrderStatus.Cancelled;
            }
            else if (!canceledFound && completedFound && !activeFound)
            {
                //All completed
                newStatus = OrderStatus.Completed.ToString();
            }
            else if (!canceledFound && !completedFound && activeFound)
            {
                //All active
            }
            else if (canceledFound && completedFound && !activeFound)
            {
                //Found  cancelled and completed
                newStatus = OrderStatus.Completed.ToString();
            }
            else if (canceledFound && !completedFound && activeFound)
            {
                //Found cancelled and active
            }
            else if (!canceledFound && completedFound && activeFound)
            {
                //Found completed and active
                newStatus = OrderStatus.PartiallyShipped.ToString();
            }
            else if (canceledFound && completedFound && activeFound)
            {
                //Found cancelled and active and completed
                newStatus = OrderStatus.PartiallyShipped.ToString();
            }

            order.Status = newStatus;
        }
    }
}
