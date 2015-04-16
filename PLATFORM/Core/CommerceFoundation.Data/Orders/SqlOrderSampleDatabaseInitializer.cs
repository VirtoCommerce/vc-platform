using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Data.Orders
{
	public class SqlOrderSampleDatabaseInitializer : SqlOrderDatabaseInitializer
	{
		private readonly string[] _files =
		{ 
			"Jurisdiction.sql",
			"JurisdictionGroup.sql",
			"JurisdictionRelation.sql",
			"Tax.sql",
			"TaxValue.sql"
		};

        private readonly string[] _orderFiles =
		{ 
			"Account.sql",
            "Member.sql",
            "AspNetUsers.sql",
            "Contact.sql",
            "Address.sql",
            "order_CustomerOrder.sql",
            "order_PaymentIn.sql",
            "order_Shipment.sql",
            "order_Address.sql",
            "order_LineItem.sql"
		};

		private readonly string[] _customers =
		{
			"Bauermeister, Denise",
			"Blackwell, Cynthia",
			"Bressler, Linda",
			"Caroompas, John",
			"Clark, Patti",
			"Dangl, Sherrilynne",
			"Desguin, Joel",
			"Dickinson, Kate",
			"Dugan, Kathy",
			"Galloway, Linda",
			"Granger, Deborah",
			"Heeschen, Kristin",
			"Hiber, Miss",
			"Hickson, Dorothy",
			"Hoevatanakul, Narisa",
			"Kwan, Shirley",
			"Lamb, Tricia",
			"LeCours, Kathie",
			"Molstad, Amy",
			"Murdock, Monica",
			"Ntzouras, Andrew"           
		};

		protected override void Seed(EFOrderRepository context)
		{
			//base.Seed(context);
			//CreateOrders(context);
			//FillOrdersScripts(context);
            FillCustomerOrdersScripts(context);
		}

		private void CreateOrders(EFOrderRepository repository)
		{
			var paymentMethods = repository.PaymentMethods.ToList();

			var customerId = 1;
			var rnd = new Random();
			for (var i = 0; i < _customers.Length - 1; i++) // 20 customers, with 2 orders each
			{
				for (var index = 0; index < 2; index++)
				{
					var order = MockOrderBuilder.BuildOrder()
												.WithAddresses()
												.WithPayments(paymentMethods)
												.WithShipment()
												.WithLineItemsCount(2 + rnd.Next(5))
												.WithReturns()
												.WithStatus("InProgress")
												.WithCustomer(customerId.ToString(CultureInfo.InvariantCulture), _customers[i])
						// .WithOrderFormPropertyValues()
												.GetOrder();
					order.StoreId = "SampleStore";
					order.OrderForms[0].Shipments[0].ShippingAddressId = order.OrderAddresses[1].OrderAddressId;

					repository.Add(order);
				}

				repository.UnitOfWork.Commit();

				customerId++;
			}
		}

		private void FillOrdersScripts(EFOrderRepository repository)
		{
			foreach (string file in _files)
			{
				ExecuteSqlScriptFile(repository, file, "Orders");
			}
		}

        private void FillCustomerOrdersScripts(EFOrderRepository repository)
        {
            foreach (string file in _orderFiles)
            {
                ExecuteSqlScriptFile(repository, file, "Orders");
            }
        }

	}

	public class MockOrderBuilder
	{
		private readonly Order _order;
		private readonly IOrderEntityFactory _entityFactory;

		private MockOrderBuilder(Order order, IOrderEntityFactory entityFactory)
		{

			_order = order;
			_entityFactory = entityFactory;
			var orderForm = entityFactory.CreateEntityForType(typeof(OrderForm)) as OrderForm;
			orderForm.Name = "default";
			orderForm.OrderGroupId = _order.OrderGroupId;
			_order.OrderForms.Add(orderForm);
		}


		public static MockOrderBuilder BuildOrder()
		{
			return BuildOrder(new OrderEntityFactory());
		}

		public static MockOrderBuilder BuildOrder(IOrderEntityFactory entityFactory)
		{

			var order = (Order)entityFactory.CreateEntityForType(typeof(Order));
			order.Name = "default";
			order.BillingCurrency = "USD";
			order.StoreId = "UK Store";
			order.TrackingNumber = "PO32" + DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(10, 5);
			order.Total = 123.43m;
			order.HandlingTotal = 1.2m;
			order.TaxTotal = 4.44m;
			order.Subtotal = 124.63m;
			order.ShippingTotal = 10.12m;

			return new MockOrderBuilder(order, entityFactory);
		}


		public MockOrderBuilder WithStatus(string status)
		{
			_order.Status = status;
			return this;
		}

		public MockOrderBuilder WithReturns()
		{
			var rnd = new Random();

			// generate returns for 30% of orders
			if (rnd.Next(10) < 3)
			{
				var rmaRequest = _entityFactory.CreateEntityForType(typeof(RmaRequest)) as RmaRequest;
				rmaRequest.ReturnTotal = 323.21m;
				rmaRequest.RefundAmount = 301.89m;
				rmaRequest.OrderId = _order.OrderGroupId;
				rmaRequest.AuthorizationCode = rmaRequest.RmaRequestId;

				var returnLineItems = _order.OrderForms[0].LineItems.Select(x => x.LineItemId).ToArray();
				var itemStates = ((RmaLineItemState[])Enum.GetValues(typeof(RmaLineItemState))).Select(x => x.ToString()).ToArray();
				//var rmaStatuses = ((RmaRequestStatus[])Enum.GetValues(typeof(RmaRequestStatus))).Select(x => x.ToString()).ToArray();

				var rmaReturnItem = CreateRmaReturnItem(returnLineItems[0], itemStates[rnd.Next(2)], rnd);
				rmaRequest.RmaReturnItems.Add(rmaReturnItem);
				rmaRequest.Status = rmaReturnItem.ItemState == RmaLineItemState.AwaitingReturn.ToString() ? RmaRequestStatus.AwaitingStockReturn.ToString() : RmaRequestStatus.AwaitingCompletion.ToString();

				// 50 %
				if (rnd.Next(2) > 0)
				{
					rmaReturnItem = CreateRmaReturnItem(returnLineItems[1], rmaReturnItem.ItemState, rnd);
					rmaRequest.RmaReturnItems.Add(rmaReturnItem);
				}

				_order.RmaRequests.Add(rmaRequest);
			}

			return this;
		}

		private RmaReturnItem CreateRmaReturnItem(string lineItemId, string itemState, Random rnd)
		{
			var item = _entityFactory.CreateEntityForType(typeof(RmaReturnItem)) as RmaReturnItem;
			item.ItemState = itemState;
			item.ReturnAmount = 10.05m + (decimal)(300 * rnd.NextDouble());
			item.ReturnReason = "Corrupt";

			var rmaLineItem = _entityFactory.CreateEntityForType(typeof(RmaLineItem)) as RmaLineItem;
			rmaLineItem.RmaReturnItemId = item.RmaReturnItemId;
			rmaLineItem.LineItemId = lineItemId;
			rmaLineItem.ReturnQuantity = 1 + rnd.Next(2);
			rmaLineItem.Quantity = 0;
			item.RmaLineItems.Add(rmaLineItem);
			return item;
		}

		public MockOrderBuilder WithAddresses()
		{
			var orderAddresses = new[] { 
							new OrderAddress { FirstName ="New", LastName ="Yourk",  Name="Billing", City = "New Yourk", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user@mail.com", Line1="str. 113", Line2="bld. 21", PostalCode="323232", StateProvince="WC" },
							new OrderAddress { FirstName ="Los", LastName ="Angeles", Name="Billing", City = "Los Angeles", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 4444-444", Email="user2@mail.com", Line1="av. 32", Line2="bld. 1", PostalCode="432142", StateProvince="LA" },
							new OrderAddress { FirstName ="Yourk", LastName ="Yourk", Name="Shipping", City = "Yourk", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user@mail.com", Line1="str. 113", Line2="Pas Juozapa", PostalCode="12100" },
							new OrderAddress { FirstName ="Vilnius", LastName ="Lithuania", Name="Shipping", City = "Vilnius", CountryName="Lithuania", CountryCode="LTU", DaytimePhoneNumber="+370 5 2744-444", Email="user@mail.com", Line1="Laisves pr. 125", PostalCode="54821" }
							};
			foreach (var address in orderAddresses)
			{
				address.OrderGroupId = _order.OrderGroupId;
				_order.OrderAddresses.Add(address);
			}
			return this;
		}

		public MockOrderBuilder WithLineItemsCount(int lineItemsCount)
		{
			foreach (var lineItem in GenerateLineItems(lineItemsCount))
			{
				_order.OrderForms[0].LineItems.Add(lineItem);
				var shipmentsCount = _order.OrderForms[0].Shipments.Count();
				var shipment = _order.OrderForms[0].Shipments[lineItemsCount % shipmentsCount];
				shipment.ShipmentItems.Add(new ShipmentItem { LineItem = lineItem, LineItemId = lineItem.LineItemId, Quantity = lineItem.Quantity, ShipmentId = shipment.ShipmentId });
			}
			return this;
		}

		public MockOrderBuilder WithShipment()
		{
			return WithShipment(_order.OrderForms[0].LineItems.Select(x => x.LineItemId).ToArray());
		}

		public MockOrderBuilder WithShipmentCount(int count)
		{
			var lineItems = _order.OrderForms[0].LineItems.ToArray();
			var lineItemsPerShipmentCount = lineItems.Count() / count;

			for (var i = 0; i < Math.Min(count, lineItems.Length); i++)
			{
				WithShipment(lineItems.Skip(i * lineItemsPerShipmentCount).Take(lineItemsPerShipmentCount).Select(x => x.LineItemId).ToArray());
			}
			return this;
		}

		public MockOrderBuilder WithShipment(string[] lineItemIds)
		{
			var shipment = _entityFactory.CreateEntityForType(typeof(Shipment)) as Shipment;
			shipment.ShippingMethodId = "FreeShipping";
			shipment.ShippingMethodName = "FreeShipping";
			shipment.ShippingCost = 0m;
			shipment.ShippingAddressId = "1";
			shipment.ShipmentTotal = 213.12m;
			shipment.Subtotal = 119;
			shipment.ShippingDiscountAmount = 5.99m;

			foreach (var lineItemId in lineItemIds)
			{
				var lineItem = _order.OrderForms[0].LineItems.FirstOrDefault(x => x.LineItemId == lineItemId);
				var shipmentItem = _entityFactory.CreateEntityForType(typeof(ShipmentItem)) as ShipmentItem;
				shipmentItem.LineItemId = lineItem.LineItemId;
				shipmentItem.Quantity = lineItem.Quantity;

				shipment.ShipmentItems.Add(shipmentItem);
			}
			shipment.ItemSubtotal = 200.12m;
			shipment.ItemTaxTotal = 5.01m;
			shipment.TotalBeforeTax = shipment.ItemSubtotal - 5m;
			shipment.ShippingTaxTotal = 0.35m;

			shipment.OrderFormId = _order.OrderForms[0].OrderFormId;
			_order.OrderForms[0].Shipments.Add(shipment);

			return this;
		}

		public MockOrderBuilder WithPayments(List<PaymentMethod> paymentMethods)
		{
			var pmCreditCard = paymentMethods.First(x => x.Name == "CreditCard");

			var payments = new Payment[] {
											 new CreditCardPayment
												 { 
												 PaymentType = PaymentType.CreditCard.GetHashCode(),
												 CreditCardCustomerName="John Doe", 
												 CreditCardExpirationMonth = 1, 
												 CreditCardExpirationYear = 2016, 
												 CreditCardNumber = "4007000000027",
												 CreditCardType = "VISA",
												 CreditCardSecurityCode = "123",
												 AuthorizationCode = "0",
												 PaymentMethodId  = pmCreditCard.PaymentMethodId, 
												 PaymentMethodName = pmCreditCard.Description, 
												 ValidationCode="000000", 
												 Amount=32.53m,
												 TransactionType = TransactionType.Sale.ToString(),
												 Status = PaymentStatus.Completed.ToString()
											 },
											new CashCardPayment
												{
													PaymentType = PaymentType.CashCard.GetHashCode(), 
													PaymentMethodName="Visa", 
													ValidationCode="RE6211-44", 
													Amount=55.73m,
													TransactionType = TransactionType.Sale.ToString(),
													Status = PaymentStatus.Failed.ToString()
												},
											 new InvoicePayment
												 {
													 PaymentType = PaymentType.Invoice.GetHashCode(), 
													 PaymentMethodName="Credit", 
													 ValidationCode="BE3-21", 
													 Amount=4.53m,
													 TransactionType = TransactionType.Credit.ToString(),
													 Status = PaymentStatus.Completed.ToString()
												 }
										   };
			foreach (var payment in payments)
			{
				payment.OrderFormId = _order.OrderForms[0].OrderFormId;
				_order.OrderForms[0].Payments.Add(payment);
			}

			return this;
		}


		public MockOrderBuilder WithCustomer(string customerId, string name)
		{
			_order.CustomerId = customerId;
			_order.CustomerName = name;
			return this;
		}

		public MockOrderBuilder WithOrderFormPropertyValues()
		{
			var rnd = new Random();

			var item = _entityFactory.CreateEntityForType(typeof(OrderFormPropertyValue)) as OrderFormPropertyValue;
			item.Name = "referrerId";
			item.ShortTextValue = "partner-1" + rnd.Next(100);
			item.ValueType = OrderFormValueType.ShortString.GetHashCode();
			_order.OrderForms[0].OrderFormPropertyValues.Add(item);

			return this;
		}

		public Order GetOrder()
		{
			return _order;
		}


		public LineItem[] GenerateLineItems(int count)
		{
			var retVal = new List<LineItem>();

			var names = new[] { "Apple 30 GB iPod with Video Playback Black (5th Generation)",
									   "Sony MDR-IF240RK Wireless Headphone System",
									   "Samsung DVD-HD841 Up-Converting DVD Player", 
									   "Apple QuickTake 200 - Digital camera - compact - 0.35 Mpix - supported memory: SM", 
									   "Samsung YP-T9JAB 4 GB Digital Multimedia Player (Black)", 
									   "EFC-1B1NBECXAR Carrying Case for 10.1", 
										"Sony SGPFLS1 Tablet S LCD Protection Sheet", 
										"Galaxy Tab 8.9 3G Android Honeycomb Tablet (16GB, 850/1900 3G)",
										"Samsung Galaxy Tab Gt-p7500 16gb, Wi-fi + 3g Unlocked"};

			var codes = new[] { "v-0239432c",
									   "MDR-IF240RK",
									   "DVD-HD841", 
									   "v-b12223cc2", 
									   "YP-T9JAB0012", 
									   "EFC-1B1NBECXAR", 
										"v-bc22234088d", 
										"v-2112393vd0",
										"v-b85699233c"};

			var rnd = new Random();
			for (var i = 0; i < count; i++)
			{
				var lineItem = _entityFactory.CreateEntityForType(typeof(LineItem)) as LineItem;
				var index = rnd.Next(names.Length);

				lineItem.DisplayName = names[index];
				lineItem.Quantity = 1 + rnd.Next(19);
				lineItem.ListPrice = rnd.Next(200);
				lineItem.PlacedPrice = lineItem.ListPrice;
				lineItem.CatalogItemId = codes[index];
				lineItem.CatalogItemCode = codes[index];
				lineItem.OrderFormId = _order.OrderForms[0].OrderFormId;

				retVal.Add(lineItem);
			}
			return retVal.ToArray();
		}
	}
}
