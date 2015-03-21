using System;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
	public class ShipmentSplitActivity : OrderActivityBase
	{
		protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);

			// Process Shipment now
			SplitShipments();
		}

		/// <summary>
		/// Splits the shipments according to address and shipping method specified.
		/// </summary>
		private void SplitShipments()
		{
			foreach (var form in CurrentOrderGroup.OrderForms)
			{
				SplitForm(form);
			}
		}

		private void SplitForm(OrderForm form)
		{
			foreach (var item in form.LineItems)
			{
				Shipment itemShipment = null;

				// Find appropriate shipment for item
				foreach (var shipment in form.Shipments)
				{
					if (shipment.ShippingMethodId == item.ShippingMethodId
						&& string.CompareOrdinal(shipment.ShippingAddressId, item.ShippingAddressId) == 0
						&& string.Compare(shipment.FulfillmentCenterId, item.FulfillmentCenterId, StringComparison.OrdinalIgnoreCase) == 0)
					{
						// we found out match, exit
						itemShipment = shipment;
						//break;
					}
					else
					{
						// if shipment contains current LineItem, remove it from the shipment
						RemoveLineItemFromShipment(shipment, item.LineItemId);
					}
				}

				// did we find any shipment?
				if (itemShipment == null)
				{
					itemShipment = new Shipment
						{
							ShippingAddressId = item.ShippingAddressId,
							ShippingMethodId = item.ShippingMethodId,
							ShippingMethodName = item.ShippingMethodName,
							FulfillmentCenterId = item.FulfillmentCenterId,
							OrderForm = form
						};
					form.Shipments.Add(itemShipment);
				}

				// Add item to the shipment
				//if (item.LineItemId == 0)
				//    throw new ArgumentNullException("LineItemId = 0");

				RemoveLineItemFromShipment(itemShipment, item.LineItemId);

				var link = new ShipmentItem
					{
						LineItemId = item.LineItemId,
						LineItem = item,
						Quantity = item.Quantity,
						ShipmentId = itemShipment.ShipmentId
					};
				itemShipment.ShipmentItems.Add(link);
			}

            //Clear unused shipments
		    foreach (var shipment in form.Shipments.ToArray())
		    {
		        if (shipment.ShipmentItems.Count == 0)
		        {
		            form.Shipments.Remove(shipment);
		        }
		        else
		        {
                    //Calculate shipment weight
                    shipment.Weight = shipment.ShipmentItems.Where(x => x.LineItem != null)
                        .Select(x => x.LineItem).Sum(x => x.Weight);
		        }
		    }

		}

		private void RemoveLineItemFromShipment(Shipment shipment, string lineItemId)
		{
			var links = shipment.ShipmentItems.Where(link => link.LineItemId == lineItemId).ToList();

			foreach (var link in links)
			{
				shipment.ShipmentItems.Remove(link);
			}
		}

	}
}
