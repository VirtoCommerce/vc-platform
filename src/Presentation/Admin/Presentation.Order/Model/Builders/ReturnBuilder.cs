using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Services;

namespace VirtoCommerce.ManagementClient.Order.Model.Builders
{
	public sealed class ReturnBuilder : ValidationRule
	{
		public const string RecalculateWorkflowName = "CalculateReturnTotalsWorkflow";

		private readonly IOrderEntityFactory _entityFactory;
		private readonly IPricelistRepository _priceListRepository;
		private readonly OrderClient _client;
		private readonly IOrderService _orderService;

		private readonly List<ReturnLineItem> _deletedLineItemList = new List<ReturnLineItem>();

		private RmaRequest _rmaRequest;
		public RmaRequest RmaRequest
		{
			get
			{
				return _rmaRequest;
			}
			private set
			{
				_rmaRequest = value;

				// set return info
				if (CurrentOrder.RmaRequests.All(x => x.RmaRequestId != _rmaRequest.RmaRequestId))
				{
					_rmaRequest.OrderId = CurrentOrder.OrderGroupId;
					_rmaRequest.Status = RmaRequestStatus.AwaitingStockReturn.ToString();
					_rmaRequest.AuthorizationCode = _rmaRequest.RmaRequestId;

					CurrentOrder.RmaRequests.Add(_rmaRequest);
				}
			}
		}

		private ObservableCollection<ReturnLineItem> _availableRetunLineItems;
		public ObservableCollection<ReturnLineItem> AvailableForReturnLineItems
		{
			get
			{
				if (_availableRetunLineItems == null)
				{
					_availableRetunLineItems = new ObservableCollection<ReturnLineItem>();

					// the same item can be returned only once. Include only returnable items.
					foreach (var item in CurrentOrder.OrderForms[0].LineItems)
					{
						var returInProgressCount = CurrentOrder.RmaRequests.Where(x => x.Status != RmaRequestStatus.Canceled.ToString())
											 .SelectMany(x => x.RmaReturnItems)
											 .SelectMany(x => x.RmaLineItems)
											 .Where(x => x.LineItemId == item.LineItemId)
											 .Sum(x => x.ReturnQuantity);

						if (item.Quantity > returInProgressCount)
						{
							_availableRetunLineItems.Add(new ReturnLineItem
							{
								AvailableQuantity = item.Quantity - returInProgressCount,
								LineItem = item
							});
						}
					}
				}
				return _availableRetunLineItems;
			}
		}

		private ObservableCollection<RmaReturnItem> _returnItems;
		public ObservableCollection<RmaReturnItem> RmaReturnItems
		{
			get { return _returnItems ?? (_returnItems = new ObservableCollection<RmaReturnItem>()); }
		}

		private ObservableCollection<LineItem> _exchangeLineItems;
		public ObservableCollection<LineItem> ExchangeLineItems
		{
			get { return _exchangeLineItems ?? (_exchangeLineItems = new ObservableCollection<LineItem>()); }
		}

		public OrderModel CurrentOrder
		{
			get;
			private set;
		}

		public OrderModel ExchangeOrder
		{
			get;
			private set;
		}

		public ReturnBuilder(IOrderEntityFactory entityFactory, IPricelistRepository priceListRepository, OrderClient client, IOrderService service)
		{
			_entityFactory = entityFactory;
			_priceListRepository = priceListRepository;
			_client = client;
			_orderService = service;
		}

		public static ReturnBuilder GetReturnBuilder(IOrderEntityFactory entityFactory, IPricelistRepository priceListRepository, Foundation.Orders.Model.Order order, RmaRequest rmaRequest, OrderClient client, IOrderService service)
		{
			if (entityFactory == null)
			{
				throw new ArgumentNullException("entityFactory");
			}
			if (order == null)
			{
				throw new ArgumentNullException("order");
			}

			var retVal = new ReturnBuilder(entityFactory, priceListRepository, client, service);
			retVal.WithOrderOrRmaRequest(order, rmaRequest);

			return retVal;
		}

		public ReturnBuilder WithOrderOrRmaRequest(Foundation.Orders.Model.Order order, RmaRequest rmaRequest)
		{
			CurrentOrder = new OrderModel(order, _client, _orderService);
			RmaRequest = rmaRequest;
			return this;
		}

		public ReturnBuilder WithRefundAmount(decimal refundAmount, string returnReason)
		{
			var returnItem = CreateEntity<RmaReturnItem>();
			returnItem.ReturnReason = returnReason;
			returnItem.ReturnAmount = refundAmount;

			RmaRequest.RmaReturnItems.Add(returnItem);

			Recalculate();
			return this;
		}

		public ReturnBuilder RemoveReturnItem(RmaReturnItem returnItem, decimal returnQuantity)
		{
			if (returnItem == null)
			{
				throw new ArgumentNullException("returnItem");
			}

			returnItem.RmaLineItems[0].ReturnQuantity -= returnQuantity;
			if (returnItem.RmaLineItems[0].ReturnQuantity <= 0)
			{
				RmaRequest.RmaReturnItems.Remove(returnItem);
				RmaReturnItems.Remove(returnItem);
			}

			var availableLineItem = AvailableForReturnLineItems.FirstOrDefault(x => x.LineItemId == returnItem.RmaLineItems[0].LineItemId);
			if (availableLineItem == null)
			{
				availableLineItem = _deletedLineItemList.FirstOrDefault(x => x.LineItemId == returnItem.RmaLineItems[0].LineItemId);
				// undelete
				AvailableForReturnLineItems.Add(availableLineItem);
			}

			availableLineItem.AvailableQuantity += returnQuantity;

			Recalculate();
			return this;
		}

		public ReturnBuilder AddReturnItem(ReturnLineItem lineItem, decimal returnQuantity, string returnReason)
		{
			if (lineItem == null)
			{
				throw new ArgumentNullException("lineItem");
			}

			var returnItem = RmaReturnItems.FirstOrDefault(x => x.RmaLineItems[0].LineItemId == lineItem.LineItemId && x.ReturnReason == returnReason);

			if (returnItem == null)
			{
				var rmaLineItem = CreateEntity<RmaLineItem>();
				rmaLineItem.LineItemId = lineItem.LineItemId;
				rmaLineItem.LineItem = lineItem.LineItem;

				returnItem = CreateEntity<RmaReturnItem>();
				returnItem.RmaLineItems.Add(rmaLineItem);
				returnItem.ItemState = RmaLineItemState.AwaitingReturn.ToString();
				returnItem.ReturnReason = returnReason;

				RmaRequest.RmaReturnItems.Add(returnItem);
				RmaReturnItems.Add(returnItem);
			}

			returnItem.RmaLineItems[0].ReturnQuantity += returnQuantity;
			lineItem.AvailableQuantity -= returnQuantity;

			if (lineItem.AvailableQuantity == 0)
			{
				_deletedLineItemList.Add(lineItem);
				AvailableForReturnLineItems.Remove(lineItem);
			}

			Recalculate();

			return this;
		}

		public ReturnBuilder RemoveExchangeItem(LineItem exchangeLineItem)
		{
			ExchangeOrder.OrderForms[0].LineItems.Remove(exchangeLineItem);
			ExchangeLineItems.Remove(exchangeLineItem);
			var exchangeShipmentItems = ExchangeOrder.OrderForms[0].Shipments[0].ShipmentItems;
			exchangeShipmentItems.Remove(exchangeShipmentItems.First(x => x.LineItemId == exchangeLineItem.LineItemId));

			RecalculateExchange();

			return this;
		}

		public ReturnBuilder AddExchangeItem(Item catalogItem, decimal quantity)
		{
			EnsureExchangeOrderCreated();

			var lineItem = ExchangeOrder.OrderForms[0].LineItems.FirstOrDefault(x => x.LineItemId == catalogItem.ItemId);
			if (lineItem == null)
			{
				lineItem = CatalogItem2LineItem(catalogItem, quantity);
				ExchangeOrder.OrderForms[0].LineItems.Add(lineItem);
				var shipmentItem = CreateEntity<ShipmentItem>();
				shipmentItem.LineItem = lineItem;
				shipmentItem.LineItemId = lineItem.LineItemId;
				shipmentItem.Quantity = quantity;
				shipmentItem.Shipment = ExchangeOrder.OrderForms[0].Shipments[0];
				ExchangeOrder.OrderForms[0].Shipments[0].ShipmentItems.Add(shipmentItem);
				ExchangeLineItems.Add(lineItem);
			}
			else
			{
				lineItem.Quantity += quantity;
				var shipmentItem = ExchangeOrder.OrderForms[0].Shipments[0].ShipmentItems.First(x => x.LineItemId == lineItem.LineItemId);
				shipmentItem.Quantity += quantity;
			}

			RecalculateExchange();

			return this;
		}


		public ReturnBuilder WithExchangeShippingMethod(ShippingMethod shippingMethod)
		{
			EnsureExchangeOrderCreated();

			var shipment = ExchangeOrder.OrderForms[0].Shipments.First();

			shipment.ShippingMethodId = shippingMethod.ShippingMethodId;
			shipment.ShippingMethodName = shippingMethod.Name;

			RecalculateExchange();

			return this;
		}

		public ReturnBuilder WithExchangeShippingAddress(OrderAddress address)
		{
			EnsureExchangeOrderCreated();

			var shipment = ExchangeOrder.OrderForms[0].Shipments.First();
			shipment.ShippingAddressId = address.OrderAddressId;

			RecalculateExchange();

			return this;
		}

		public RmaRequest CompleteReturnBuild()
		{
			// Recalculate();

			return RmaRequest;
		}

		#region Private  methods

		private void EnsureExchangeOrderCreated()
		{
			if (ExchangeOrder == null)
			{
				ExchangeOrder = new OrderModel(CreateEntity<Foundation.Orders.Model.Order>(), _client, _orderService)
					{
						Name = CurrentOrder.Name,
						StoreId = CurrentOrder.StoreId,
						BillingCurrency = CurrentOrder.BillingCurrency,
						CurrentStatus = (int)OrderStatus.AwaitingExchange,
						CustomerId = CurrentOrder.CustomerId,
						CustomerName = CurrentOrder.CustomerName
					};
				ExchangeOrder.GenerateTrackingNumber();
				ExchangeOrder.InnerItem.ParentOrderId = CurrentOrder.OrderGroupId;
				RmaRequest.ExchangeOrder = ExchangeOrder.InnerItem;
				RmaRequest.ExchangeOrderId = ExchangeOrder.OrderGroupId;

				//Copy order addresses to exchange order
				foreach (var orderAddress in CurrentOrder.OrderAddresses)
				{
					var newAddress = CreateEntity<OrderAddress>();
					newAddress.InjectFrom(orderAddress);
					newAddress.OrderAddressId = newAddress.GenerateNewKey();
					newAddress.OrderGroup = ExchangeOrder.InnerItem;
					newAddress.OrderGroupId = ExchangeOrder.OrderGroupId;
					ExchangeOrder.OrderAddresses.Add(newAddress);
				}

				//Create order form
				var orderForm = CreateEntity<OrderForm>();
				orderForm.Name = CurrentOrder.OrderForms[0].Name;
				orderForm.OrderGroup = ExchangeOrder.InnerItem;
				ExchangeOrder.OrderForms.Add(orderForm);

				//Create shipment
				var shipment = CreateEntity<Shipment>();
				orderForm.Shipments.Add(shipment);
				shipment.OrderForm = orderForm;
			}
		}

		private LineItem CatalogItem2LineItem(Item item, decimal quantity)
		{
			var retVal = CreateEntity<LineItem>();
			retVal.DisplayName = item.Name;
			retVal.OrderFormId = ExchangeOrder.OrderForms[0].OrderFormId;
			retVal.ParentCatalogItemId = String.Empty;
			retVal.CatalogItemId = item.ItemId;
			retVal.CatalogItemCode = item.Code;
			var price = _priceListRepository.FindLowestPrice(null, item.ItemId, quantity);
			if (price != null)
			{
				retVal.ListPrice = price.Sale ?? price.List;
				retVal.PlacedPrice = price.Sale ?? price.List;
			}
			retVal.MaxQuantity = item.MaxQuantity;
			retVal.MinQuantity = item.MinQuantity;
			retVal.Quantity = quantity;

			return retVal;
		}

		/// <summary>
		/// Recalculates current Return's amounts: fees, subtotals, totals, etc.
		/// </summary>
		private void Recalculate()
		{
			var result = _orderService.ExecuteWorkflow(RecalculateWorkflowName, CurrentOrder.InnerItem);
			CurrentOrder.InnerItem.InjectFrom<CloneInjection>(result.OrderGroup);
		}

		private void RecalculateExchange()
		{
			var result = _orderService.ExecuteWorkflow("ShoppingCartPrepareWorkflow", ExchangeOrder.InnerItem);
			result = _orderService.ExecuteWorkflow("ShoppingCartValidateWorkflow", result.OrderGroup);
			ExchangeOrder.InnerItem.InjectFrom<CloneInjection>(result.OrderGroup);
		}

		private T CreateEntity<T>()
		{
			return (T)_entityFactory.CreateEntityForType(_entityFactory.GetEntityTypeStringName(typeof(T)));
		}

		#endregion

		#region ValidationRule overrides

		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			var retVal = true;
			var errorMsg = String.Empty;

			return new ValidationResult(retVal, errorMsg);
		}
		#endregion

		public class ReturnLineItem
		{
			public decimal AvailableQuantity { get; set; }
			public LineItem LineItem { get; set; }

			public string LineItemId
			{
				get { return LineItem.LineItemId; }
			}
		}
	}
}
