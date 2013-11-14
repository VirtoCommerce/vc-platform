using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
	/// <summary>
	/// Checks currently ordered quantity against inventory values which are saved in the line item properties.
	/// Adjust ordered quantities, removing items if they are no longer available. Adjusts shipping items as well.
	/// </summary>
	public class CheckInventoryActivity : OrderActivityBase
	{
        ICatalogRepository _catalogRepository;
        protected ICatalogRepository CatalogRepository
        {
            get { return _catalogRepository ?? (_catalogRepository = ServiceLocator.GetInstance<ICatalogRepository>()); }
            set
            {
                _catalogRepository = value;
            }
        }

        ICacheRepository _cacheRepository;
        protected ICacheRepository CacheRepository
        {
            get { return _cacheRepository ?? (_cacheRepository = ServiceLocator.GetInstance<ICacheRepository>()); }
            set
            {
                _cacheRepository = value;
            }
        }

		public CheckInventoryActivity()
		{
		}

        public CheckInventoryActivity(ICatalogRepository catalogRepository, ICacheRepository cacheRepository)
		{
			_catalogRepository = catalogRepository;
			_cacheRepository = cacheRepository;
		}

		protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);

			// Check inventory
			ValidateItems();
		}

		private void ValidateItems()
		{
			//We don't need to validate quantity in the wish list
			var orderForms = CurrentOrderGroup.OrderForms.ToArray();
			var lineItems = orderForms.SelectMany(x => x.LineItems.ToArray());
			var validLineItems = lineItems.Where(x => !String.IsNullOrEmpty(x.CatalogItemId) && !x.CatalogItemId.StartsWith("@"));

			foreach (var lineItem in validLineItems)
			{
				var changeQtyReason = new List<string>();
				bool isUsingBackordersAndPreorders;
				var newQty = GetNewLineItemQty(lineItem, changeQtyReason, out isUsingBackordersAndPreorders);
				if (newQty == 0)
				{
					// Remove item if it reached this stage
					RegisterWarning(WorkflowMessageCodes.ITEM_NOT_AVAILABLE, lineItem, String.Format("Item \"{0}\" has been removed from the cart because it is no longer available", lineItem.DisplayName));
					DeleteLineItemFromShipments(lineItem);
					lineItem.OrderForm.LineItems.Remove(lineItem);
				}
				else
				{
					var delta = lineItem.Quantity - newQty;
					if (delta != 0)
					{
						lineItem.Quantity -= delta;
						ChangeShipmentsLineItemQty(lineItem, delta);
						RegisterWarning(WorkflowMessageCodes.ITEM_QTY_CHANGED, lineItem, String.Format("Item \"{0}\" quantity has been changed", lineItem.DisplayName));
					}

					// update Shipment Status if LineItem InStockQuantity was insufficient and Backorders/Preorders were used.
					if (isUsingBackordersAndPreorders)
					{
						var catalogHelper = new CatalogClient(CatalogRepository, null, null, CacheRepository, null);
						var catalogItem = catalogHelper.GetItem(lineItem.CatalogItemId);
						if (catalogItem != null && catalogItem.TrackInventory)
						{
							var allShipmentContainingLineItem = orderForms
								.SelectMany(x => x.Shipments)
								.Where(x => x.ShipmentItems.Select(si => si.LineItem).Contains(lineItem))
								.ToList();
							allShipmentContainingLineItem.ForEach(x => x.Status = ShipmentStatus.AwaitingInventory.ToString());
						}
					}
				}
			}
		}

		private decimal GetNewLineItemQty(LineItem lineItem, List<string> changeQtyReason, out bool isUsingBackordersAndPreorders)
		{
			isUsingBackordersAndPreorders = false;
			var newLineItemQty = lineItem.Quantity;
			if (newLineItemQty < lineItem.MinQuantity || newLineItemQty > lineItem.MaxQuantity)
			{
				newLineItemQty = Math.Max(lineItem.MinQuantity, newLineItemQty);
				if (newLineItemQty != lineItem.Quantity)
				{
					changeQtyReason.Add("by Min Quantity setting");
				}
				newLineItemQty = Math.Min(lineItem.MaxQuantity, newLineItemQty);
				if (newLineItemQty != lineItem.Quantity)
				{
					changeQtyReason.Add("by Max Quantity setting");
				}
			}

			if (lineItem.InventoryStatus == InventoryStatus.Enabled.ToString())
			{
				// Check Inventory
				// item exists with appropriate quantity
				if (lineItem.InStockQuantity < newLineItemQty)
				{
					if (lineItem.InStockQuantity > 0) // there still exist items in stock
					{
						// check if we can backorder some items
						if (lineItem.AllowBackorders)
						{
							isUsingBackordersAndPreorders = true;
							//Increase stock qty by backorder qty if 
							var availStockAndBackorderQty = lineItem.InStockQuantity + lineItem.BackorderQuantity;
							if (availStockAndBackorderQty < newLineItemQty)
							{
								newLineItemQty = availStockAndBackorderQty;
								changeQtyReason.Add("by BackOrder quantity");
							}
						}
						else
						{
							newLineItemQty = lineItem.InStockQuantity;
						}
					}
					else if (lineItem.InStockQuantity == 0)
					{
						if (lineItem.AllowPreorders && lineItem.PreorderQuantity > 0)
						{
							isUsingBackordersAndPreorders = true;
							if (lineItem.PreorderQuantity < newLineItemQty)
							{
								newLineItemQty = lineItem.PreorderQuantity;
								changeQtyReason.Add("by Preorder quantity");
							}
						}
						else if (lineItem.AllowBackorders && lineItem.BackorderQuantity > 0)
						{
							isUsingBackordersAndPreorders = true;
							if (lineItem.BackorderQuantity < newLineItemQty)
							{
								newLineItemQty = lineItem.BackorderQuantity;
								changeQtyReason.Add("by BackOrder quantity");
							}
						}
						else
						{
							newLineItemQty = 0;
						}
					}
				}
			}
			return newLineItemQty;
		}

		private void DeleteLineItemFromShipments(LineItem lineItem)
		{
			var order = CurrentOrderGroup;
			foreach (var form in order.OrderForms)
			{
				var allShipmentItems = form.Shipments.SelectMany(s => s.ShipmentItems.ToArray()).Where(x => x.LineItem == lineItem);

				foreach (var shipmentLineItem in allShipmentItems)
				{
					shipmentLineItem.Shipment.ShipmentItems.Remove(shipmentLineItem);
				}
			}
		}

		private void ChangeShipmentsLineItemQty(LineItem lineItem, decimal delta)
		{
			var order = CurrentOrderGroup;
			foreach (var form in order.OrderForms)
			{
				var allShipmentContainsLineItem = form.Shipments
					.Where(x => x.ShipmentItems.Select(si => si.LineItem).Contains(lineItem))
					.SelectMany(s => s.ShipmentItems);

				foreach (var shipmentLineItems in allShipmentContainsLineItem)
				{
					//Decrease qty in all shipment contains line item
					var shipmentQty = shipmentLineItems.Quantity;
					var newShipmentQty = shipmentQty - delta;
					newShipmentQty = newShipmentQty > 0 ? newShipmentQty : 0;
					//Set new line item qty in shipment
					shipmentLineItems.Quantity = newShipmentQty;
					delta -= Math.Min(delta, shipmentQty);

					if (delta == 0)
						break;
				}
			}
		}

	}
}
