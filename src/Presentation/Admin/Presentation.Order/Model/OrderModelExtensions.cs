using System;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using coreModel = VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Model
{
	public static class OrderModelExtensions
	{
		#region order ext

		public static int GetCurrentStatus(this OrderGroup item)
		{
			var retVal = OrderStatus.InProgress;
			OrderStatus parsedVal;
			if (Enum.TryParse(item.Status, out parsedVal))
			{
				retVal = parsedVal;
			}
			return (int)retVal;
		}

		#endregion

		#region Shipment ext

		public static string GetCurrentStatus(this Shipment item, OrderGroup order)
		{
			// shipment inherits onHold status from the order
			if (order.GetCurrentStatus() == (int)OrderStatus.OnHold)
			{
				return ShipmentStatus.OnHold.ToString();
			}

		    return string.IsNullOrEmpty(item.Status) ? ShipmentStatus.InventoryAssigned.ToString() : item.Status;
		}

		public static void SetCurrentStatus(this Shipment item, int value, OrderClient client)
		{
			var newStatus = (ShipmentStatus)value;

			if (client.CanChangeStatus(item, newStatus.ToString()))
			{
				item.Status = newStatus.ToString();
			}
			else
				throw new OperationCanceledException("[Shipment] Unable to transition: " + item.Status + " -> " + newStatus.ToString());

		}

		public static bool IsModifyable(this Shipment item, OrderGroup order)
		{
			var shipmentStatus = item.GetCurrentStatus(order);
			var orderStatus = order.GetCurrentStatus();
			var retVal = orderStatus == (orderStatus & ((int)OrderStatus.AwaitingExchange | (int)OrderStatus.InProgress | (int)OrderStatus.PartiallyShipped | (int)OrderStatus.Pending))
						&& (shipmentStatus == ShipmentStatus.AwaitingInventory.ToString() || shipmentStatus == ShipmentStatus.InventoryAssigned.ToString());
			return retVal;
		}

		public static bool IsCancellable(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			var retVal = ParentOrder.GetCurrentStatus() != (int)OrderStatus.AwaitingExchange
						&& client.CanChangeStatus(item, ShipmentStatus.Cancelled.ToString());
			return retVal;
		}

		public static void Cancel(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			item.SetCurrentStatus((int)ShipmentStatus.Cancelled, client);
		}

		public static bool IsReleaseable(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			var retVal = !string.IsNullOrEmpty(item.ShippingAddressId)
					&& !Guid.Empty.Equals(item.ShippingMethodId)
					&& ParentOrder.GetCurrentStatus() != (int)OrderStatus.Pending
					&& ParentOrder.GetCurrentStatus() != (int)OrderStatus.AwaitingExchange
					&& client.CanChangeStatus(item, ShipmentStatus.Released.ToString());
			return retVal;
		}

		public static void Release(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			item.SetCurrentStatus((int)ShipmentStatus.Released, client);
		}

		public static bool IsCompletable(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			var retVal = ParentOrder.GetCurrentStatus() != (int)OrderStatus.AwaitingExchange
						&& client.CanChangeStatus(item, ShipmentStatus.Shipped.ToString());

			return retVal;
		}

		public static void Complete(this Shipment item, coreModel.Order ParentOrder, OrderClient client)
		{
			item.SetCurrentStatus((int)ShipmentStatus.Shipped, client);
		}

		public static void PickForPack(this Shipment item, OrderClient client)
		{
			item.SetCurrentStatus((int)ShipmentStatus.Packing, client);
		}

		#endregion
		
		#region RmaRequest ext

		public static int GetCurrentStatus(this RmaRequest item)
		{
			var retVal = RmaRequestStatus.AwaitingStockReturn;
			RmaRequestStatus parsedVal;
			if (Enum.TryParse(item.Status, out parsedVal))
			{
				retVal = parsedVal;
			}
			return (int)retVal;
		}

		public static void SetCurrentStatus(this RmaRequest item, int value, OrderClient client)
		{
			var newStatus = (RmaRequestStatus)value;
			if (client.CanChangeStatus(item, newStatus.ToString()))
			{
				item.Status = newStatus.ToString();
			}
			else
				throw new OperationCanceledException("[RmaRequest] Unable to transition: " + item.Status + " -> " + newStatus.ToString());

		}

		public static bool IsCancellable(this RmaRequest item, OrderClient client)
		{
			return client.CanChangeStatus(item, RmaRequestStatus.Canceled.ToString());
		}

		public static bool IsCompletable(this RmaRequest item, OrderClient client)
		{
			return client.CanChangeStatus(item, RmaRequestStatus.Complete.ToString());
		}

		public static bool IsAllowCreateExchangeOrder(this RmaRequest item)
		{
			var retVal = ((RmaRequestStatus)item.GetCurrentStatus() & (RmaRequestStatus.AwaitingStockReturn | RmaRequestStatus.AwaitingCompletion)) == (RmaRequestStatus)item.GetCurrentStatus();
			if (retVal)
			{
				retVal = string.IsNullOrEmpty(item.ExchangeOrderId);
			}
			return retVal;
		}

		public static void Cancel(this RmaRequest item, OrderClient client)
		{
			item.SetCurrentStatus((int)RmaRequestStatus.Canceled, client);
		}

		public static void Complete(this RmaRequest item, OrderClient client)
		{
			item.SetCurrentStatus((int)RmaRequestStatus.Complete, client);
		}

		#endregion
	}
}