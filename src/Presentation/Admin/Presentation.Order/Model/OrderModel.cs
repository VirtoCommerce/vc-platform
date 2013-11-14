using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using coreModel = VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Model
{
	public class OrderModel : NotifyPropertyChanged
	{
		public const string RecalculateWorkflowName = "OrderRecalculateWorkflow";

		private readonly IOrderEntityFactory _entityFactory;
		private readonly OrderClient _OrderClient;
		private readonly IOrderService _orderService;

		public OrderModel(coreModel.Order innerItem, OrderClient client, IOrderService orderService)
			: this(new OrderEntityFactory(), innerItem, client, orderService)
		{
		}

		public OrderModel(IOrderEntityFactory entityFactory, coreModel.Order innerItem, OrderClient client, IOrderService orderService)
		{
			_entityFactory = entityFactory;
			_innerItem = innerItem;
			_OrderClient = client;
			_orderService = orderService;
		}

		coreModel.Order _innerItem;
		public coreModel.Order InnerItem
		{
			get { return _innerItem; }
			set { _innerItem = value; OnPropertyChanged(); }
		}

		public int CurrentStatus
		{
			get
			{
				var retVal = OrderStatus.InProgress;
				OrderStatus parsedVal;
				if (Enum.TryParse(_innerItem.Status, out parsedVal))
				{
					retVal = parsedVal;
				}
				return (int)retVal;
			}
			set
			{
				var newStatus = (OrderStatus)value;
				if (_OrderClient.CanChangeStatus(_innerItem, newStatus.ToString()))
				{
					_innerItem.Status = newStatus.ToString();
				}
				else
					throw new OperationCanceledException("[Order] Unable to transition: " + _innerItem.Status + " -> " + newStatus.ToString());

				OnPropertyChanged();
				OnPropertyChanged("CurrentStatusText");
			}
		}

		public string CurrentStatusText
		{
			get { return ((OrderStatus)CurrentStatus).ToString(); }
		}

		/// <summary>
		/// Determines whether if order can be cancelled.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if order can be cancelled; otherwise, <c>false</c>.
		/// </returns>
		public bool IsOrderCancellable
		{
			get
			{
				// Check if all shipments are cancellable
				var retVal = _OrderClient.CanChangeStatus(InnerItem, OrderStatus.Cancelled.ToString())
							&& _innerItem.OrderForms.Count > 0
							&& _innerItem.OrderForms[0].Shipments.All(x => x.IsCancellable(_innerItem, _OrderClient) || x.GetCurrentStatus(InnerItem) == ShipmentStatus.Cancelled.ToString());
				return retVal;
			}
		}

		/// <summary>
		/// Determines if order can be put on hold.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if can be put on hold; otherwise, <c>false</c>.
		/// </returns>
		public bool IsOrderHoldable
		{
			get
			{
				return _OrderClient.CanChangeStatus(InnerItem, OrderStatus.OnHold.ToString());
			}
		}

		/// <summary>
		/// Determines if order can be put on hold.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if can be put on hold; otherwise, <c>false</c>.
		/// </returns>
		public bool IsOrderHoldReleaseable
		{
			get
			{
				return InnerItem.Status == OrderStatus.OnHold.ToString() && _OrderClient.CanChangeStatus(InnerItem, OrderStatus.InProgress.ToString());
			}
		}

		/// <summary>
		/// Determines whether order have any waiting stock return return form
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is order awaiting stock returns] [the specified purchase order]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsOrderHaveAwaitingStockReturns
		{
			get
			{
				var retVal = CurrentStatus == (int)OrderStatus.Completed;
				if (retVal)
				{
					retVal = _innerItem.RmaRequests.Any(x => x.GetCurrentStatus() == (int)RmaRequestStatus.AwaitingStockReturn);
				}
				return retVal;
			}
		}

		/// <summary>
		/// Determines whether order have any awaiting completion return form
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is order awaiting return completable] [the specified purchase order]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsOrderHaveAwaitingReturnCompletable
		{
			get
			{
				var retVal = CurrentStatus == (int)OrderStatus.Completed;
				if (retVal)
				{
					retVal = _innerItem.RmaRequests.Any(x => x.GetCurrentStatus() == (int)RmaRequestStatus.AwaitingStockReturn);
				}
				return retVal;
			}
		}

		public void CancelOrder()
		{
			CurrentStatus = (int)OrderStatus.Cancelled;
		}

		public void HoldOrder()
		{
			CurrentStatus = (int)OrderStatus.OnHold;
		}

		public void ReleaseHold()
		{
			CurrentStatus = (int)OrderStatus.InProgress;
		}

		public void Process()
		{
			CurrentStatus = (int)OrderStatus.InProgress;
		}


		public void MoveShippingItem(ShipmentItem item, decimal quantity, Shipment targetShipment, Shipment sourceShipment)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (targetShipment == null)
			{
				throw new ArgumentNullException("targetShipment");
			}

			// prevent removing last Shipment
			if (item.Quantity == quantity
				&& sourceShipment.ShipmentItems.Count == 1
				&& _innerItem.OrderForms[0].Shipments.Count == 1)
			{
				throw new OperationCanceledException("Can't remove last item. Update Shipment instead");
			}

			item.Quantity -= quantity;
			if (item.Quantity <= 0)
			{
				// remove empty ShipmentItem, Shipment
				sourceShipment.ShipmentItems.Remove(item);

				if (sourceShipment.ShipmentItems.Count == 0)
				{
					_innerItem.OrderForms[0].Shipments.Remove(sourceShipment);
				}
			}

			var targetShipmentItem = targetShipment.ShipmentItems.FirstOrDefault(x => x.LineItemId == item.LineItemId);
			if (targetShipmentItem == null)
			{
				targetShipmentItem = _entityFactory.CreateEntityForType(typeof(ShipmentItem)) as ShipmentItem;
				targetShipmentItem.LineItemId = item.LineItemId;
				targetShipmentItem.LineItem = item.LineItem;
				targetShipmentItem.ShipmentId = targetShipment.ShipmentId;
				//targetShipmentItem.Shipment = targetShipment;
				targetShipment.ShipmentItems.Add(targetShipmentItem);
			}

			targetShipmentItem.Quantity += quantity;
		}

		public virtual void GenerateTrackingNumber()
		{
			var random = new Random((int)DateTime.Now.Ticks);
			TrackingNumber = "PO" + random.Next(10000, 99999);
		}

		public void Recalculate()
		{
			var result = _orderService.ExecuteWorkflow(RecalculateWorkflowName, InnerItem);
			var warnings = result.WorkflowResult.Warnings;
			if (warnings != null)
			{
				foreach (var warning in warnings)
				{
					//TODO: generate error message
				}
			}

			InnerItem.InjectFrom<CloneInjection>(result.OrderGroup);
		}

		#region IOrder Members

		public DateTime? ExpirationDate
		{
			get { return _innerItem.ExpirationDate; }
			set { _innerItem.ExpirationDate = value; }
		}

		public System.Collections.ObjectModel.ObservableCollection<RmaRequest> RmaRequests
		{
			get { return _innerItem.RmaRequests; }
		}

		public string TrackingNumber
		{
			get { return _innerItem.TrackingNumber; }
			set { _innerItem.TrackingNumber = value; }
		}

		public string AddressId
		{
			get { return _innerItem.AddressId; }
			set { _innerItem.AddressId = value; }
		}

		public string BillingCurrency
		{
			get { return _innerItem.BillingCurrency; }
			set { _innerItem.BillingCurrency = value; }
		}

		public string CustomerId
		{
			get { return _innerItem.CustomerId; }
			set { _innerItem.CustomerId = value; }
		}

		public string CustomerName
		{
			get { return _innerItem.CustomerName; }
			set { _innerItem.CustomerName = value; }
		}

		public decimal HandlingTotal
		{
			get { return _innerItem.HandlingTotal; }
			set { _innerItem.HandlingTotal = value; }
		}

		public string Name
		{
			get { return _innerItem.Name; }
			set { _innerItem.Name = value; }
		}

		public System.Collections.ObjectModel.ObservableCollection<OrderAddress> OrderAddresses
		{
			get { return _innerItem.OrderAddresses; }
		}

		public System.Collections.ObjectModel.ObservableCollection<OrderForm> OrderForms
		{
			get { return _innerItem.OrderForms; }
		}

		public string OrderGroupId
		{
			get { return _innerItem.OrderGroupId; }
			set { _innerItem.OrderGroupId = value; }
		}

		public string OrganizationId
		{
			get { return _innerItem.OrganizationId; }
			set { _innerItem.OrganizationId = value; }
		}

		public decimal ShippingTotal
		{
			get { return _innerItem.ShippingTotal; }
			set { _innerItem.ShippingTotal = value; }
		}

		public string Status
		{
			get { return _innerItem.Status; }
			set { _innerItem.Status = value; }
		}

		public string StoreId
		{
			get { return _innerItem.StoreId; }
			set { _innerItem.StoreId = value; }
		}

		public decimal Subtotal
		{
			get { return _innerItem.Subtotal; }
			set { _innerItem.Subtotal = value; }
		}

		public decimal TaxTotal
		{
			get { return _innerItem.TaxTotal; }
			set { _innerItem.TaxTotal = value; }
		}

		public decimal Total
		{
			get { return _innerItem.Total; }
			set { _innerItem.Total = value; }
		}

		public DateTime? Created
		{
			get { return _innerItem.Created; }
		}

		#endregion
	}
}
