using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;

namespace FunctionalTests.Orders.Helpers
{
	public class TestOrderBuilder
	{
		private readonly OrderGroup _order;

		private TestOrderBuilder(OrderGroup order)
		{
			_order = order;

			var orderForm = new OrderForm { Name = "default", OrderGroupId = _order.OrderGroupId, OrderGroup = _order };
			_order.OrderForms.Add(orderForm);
		}

		public static TestOrderBuilder BuildOrder()
		{
			var order = new Order
				{
					Name = "default",
					BillingCurrency = "USD",
					StoreId = "SampleStore",
					TrackingNumber = string.Format("PO{0}-0000{1}", DateTime.UtcNow.ToString("yyyy-MMdd"), new Random().Next(1,9)),
					Total = 123.43m
				}; // can't create OrderExt!

			return new TestOrderBuilder(order);
		}

		public static TestOrderBuilder BuildCart()
		{
			var order = new ShoppingCart
				{
					Name = "default",
					BillingCurrency = "USD",
					StoreId = "SampleStore",
					Total = 123.43m
				}; // can't create OrderExt!

			return new TestOrderBuilder(order);
		}


		public TestOrderBuilder WithStatus(string status)
		{
			_order.Status = status;
			return this;
		}

		public TestOrderBuilder WithOrderFormDiscounts(int count)
		{
			var rnd = new Random();
			return WithOrderFormDiscounts(count, rnd.Next(100));
		}

		public TestOrderBuilder WithOrderFormDiscounts(int count, decimal discountAmount)
		{
			for (var i = 0; i < count; i++)
			{
				_order.OrderForms[0].Discounts.Add(new OrderFormDiscount { DiscountAmount = discountAmount, DiscountName = "discount#" + i, DiscountCode = "some code", OrderFormId = _order.OrderForms[0].OrderFormId });
			}
			return this;
		}

		public TestOrderBuilder WithLineItemDiscounts(int count)
		{
			return WithLineItemDiscounts(count, 61.23m);
		}

		public TestOrderBuilder WithLineItemDiscounts(int count, decimal discount)
		{
			for (var i = 0; i < count; i++)
			{
				foreach (var lineItem in _order.OrderForms[0].LineItems)
				{
					lineItem.Discounts.Add(new LineItemDiscount { DiscountAmount = discount, DiscountName = "discount#" + i, DiscountCode = "some code", LineItemId = lineItem.LineItemId });
				}
			}
			return this;
		}

		public TestOrderBuilder WithReturns()
		{
			var returnLineItems = _order.OrderForms[0].LineItems.Take(2).ToArray();

			var returnItem1 = new RmaReturnItem
				{
					ItemState = RmaLineItemState.AwaitingReturn.ToString(),
					ReturnAmount = 21.32m,
					ReturnReason = "Corrupt"
				};

			var rmaLineItem = new RmaLineItem
				{
					RmaReturnItemId = returnItem1.RmaReturnItemId,
					LineItemId = returnLineItems[0].LineItemId,
					LineItem = returnLineItems[0],
					ReturnQuantity = 1,
					Quantity = 0
				};
			returnItem1.RmaLineItems.Add(rmaLineItem);

			var returnItem2 = new RmaReturnItem
				{
					ItemState = RmaLineItemState.AwaitingReturn.ToString(),
					ReturnAmount = 210.67m,
					ReturnReason = "Other"
				};

			rmaLineItem = new RmaLineItem
				{
					RmaReturnItemId = returnItem2.RmaReturnItemId,
					LineItemId = returnLineItems[1].LineItemId,
					LineItem = returnLineItems[1],
					ReturnQuantity = 2,
					Quantity = 0
				};
			returnItem2.RmaLineItems.Add(rmaLineItem);

			var rmaRequest = new RmaRequest
				{
                    Status = RmaRequestStatus.AwaitingStockReturn.ToString(),
					ReturnTotal = 323.21m,
					RefundAmount = 301.89m,
					OrderId = _order.OrderGroupId
				};

			foreach (var rmaItem in new[] { returnItem1, returnItem2 })
			{
				rmaRequest.RmaReturnItems.Add(rmaItem);
			}
			((Order)_order).RmaRequests.Add(rmaRequest);
			return this;
		}

		public TestOrderBuilder WithAddresess()
		{
			var orderAddresses = new[]
			{ 
				new OrderAddress { FirstName ="New", LastName ="York",  Name="Billing", City = "New York", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="(323) 212-2321", Email="user@mail.com", Line1="12 Main St", Line2="", PostalCode="323232", StateProvince="NY" },
				new OrderAddress { FirstName ="Los", LastName ="Angeles", Name="Billing", City = "Los Angeles", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="(323) 212-2321", Email="user2@mail.com", Line1="8347 Wilshire Blvd", Line2="", PostalCode="432142", StateProvince="CA" },
				new OrderAddress { FirstName ="York", LastName ="York", Name="Shipping", City = "New York", CountryName="USA", CountryCode="USA", DaytimePhoneNumber="(323) 212-2321", Email="user@mail.com", Line1="123 Broadway Ave", Line2="", PostalCode="12100" },
				new OrderAddress { FirstName ="Vilnius", LastName ="Lithuania", Name="Shipping", City = "Vilnius", CountryName="Lithuania", CountryCode="LTU", DaytimePhoneNumber="+370 5 2744-444", Email="user@mail.com", Line1="Laisves pr. 125", PostalCode="54821" }
			};
			foreach (var address in orderAddresses)
			{
				_order.OrderAddresses.Add(address);
			}
			_order.OrderForms[0].BillingAddressId = orderAddresses[0].OrderAddressId;
			return this;
		}

		public TestOrderBuilder WithLineItemsCount(int lineItemsCount)
		{
			foreach (var lineItem in GenerateLineItems(lineItemsCount))
			{
				lineItem.OrderForm = _order.OrderForms[0];
				_order.OrderForms[0].LineItems.Add(lineItem);
				var shipmentsCount = _order.OrderForms[0].Shipments.Count();
				if (shipmentsCount > 0)
				{
					var shipment = _order.OrderForms[0].Shipments[lineItemsCount % shipmentsCount];
					shipment.ShipmentItems.Add(new ShipmentItem { LineItem = lineItem, LineItemId = lineItem.LineItemId, Quantity = lineItem.Quantity, ShipmentId = shipment.ShipmentId, Shipment = shipment });
				}
			}

			return this;
		}

		public TestOrderBuilder WithLineItemsConstant()
		{
			foreach (var lineItem in GenerateLineItemsConstant())
			{
				lineItem.OrderForm = _order.OrderForms[0];
				_order.OrderForms[0].LineItems.Add(lineItem);
				var shipmentsCount = _order.OrderForms[0].Shipments.Count();
				if (shipmentsCount > 0)
				{
					var shipment = _order.OrderForms[0].Shipments[0];
					shipment.ShipmentItems.Add(new ShipmentItem { LineItem = lineItem, LineItemId = lineItem.LineItemId, Quantity = lineItem.Quantity, ShipmentId = shipment.ShipmentId, Shipment = shipment });
				}
			}

			return this;
		}

		public TestOrderBuilder WithShipment()
		{
			return WithShipmentCount(1);
		}

		public TestOrderBuilder WithShipmentDiscount(decimal discountAmount)
		{
			foreach (var shipment in _order.OrderForms[0].Shipments)
			{
				shipment.Discounts.Add(new ShipmentDiscount { DiscountAmount = discountAmount, DiscountName = "discount#" + Guid.NewGuid().ToString(), DiscountCode = "some code", ShipmentId = shipment.ShipmentId });
			}
			return this;
		}

		public TestOrderBuilder WithShipmentCount(int count)
		{
			return WithShipmentCount(count, 213.12m);
		}

		public TestOrderBuilder WithShipmentCount(int count, decimal cost)
		{
			for (var i = 0; i < count; i++)
			{
				var shipment = new Shipment
					{
						ShippingMethodName = "Any localized test text",
						ShippingAddressId = _order.OrderAddresses[0].OrderAddressId,
						ShippingCost = 0m,
						Subtotal = cost,
						ShippingMethodId = "FreeShipping"
					};

				_order.OrderForms[0].Shipments.Add(shipment);
			}

			return this;
		}

		public TestOrderBuilder WithPayments()
		{
			var payments = new Payment[] {
											 new CreditCardPayment {PaymentMethodId = "CreditCard", PaymentType = PaymentType.CreditCard.GetHashCode(), Status = PaymentStatus.Pending.ToString(), CreditCardCustomerName="John Doe", PaymentMethodName="MasterCard", ValidationCode="RE21321-21", Amount=32.53m, CreditCardExpirationMonth = 12, CreditCardExpirationYear = 2014, CreditCardNumber = "4007000000027", CreditCardType = "VISA", CreditCardSecurityCode = "123", BillingAddressId = _order.OrderForms[0].BillingAddressId},
											 new CashCardPayment { PaymentMethodId = "Phone", PaymentType = PaymentType.CashCard.GetHashCode(), Status = PaymentStatus.Pending.ToString(), PaymentMethodName="Visa", ValidationCode="RE6211-44", Amount=55.73m },
											 new InvoicePayment { PaymentMethodId = "Phone", PaymentType = PaymentType.Invoice.GetHashCode(), Status = PaymentStatus.Pending.ToString(), PaymentMethodName="Bank transaction", ValidationCode="BE3-21", Amount=774.53m }
										   };
			foreach (var payment in payments)
			{
				_order.OrderForms[0].Payments.Add(payment);
			}

			return this;
		}


		public TestOrderBuilder WithCustomer(string customerId)
		{
			_order.CustomerId = customerId;
			return this;
		}

		public Order GetOrder()
		{
			return _order as Order;
		}

		public ShoppingCart GetCart()
		{
			return _order as ShoppingCart;
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
				var lineItem = new LineItem();
				var index = rnd.Next(names.Length);

				lineItem.DisplayName = names[index];
				lineItem.Quantity = 1 + rnd.Next(19);
				lineItem.ListPrice = rnd.Next(200);
				lineItem.PlacedPrice = lineItem.ListPrice;
				lineItem.CatalogItemCode = codes[index];
				lineItem.CatalogItemId = codes[index];

				retVal.Add(lineItem);
			}
			return retVal.ToArray();
		}

		public LineItem[] GenerateLineItemsConstant()
		{
			var retVal = new List<LineItem>();

			var items = new[]{ 
                new { Name = "Apple 30 GB iPod with Video Playback Black (5th Generation)", Price = 100m, Quantity = 1 },
                new { Name = "Sony MDR-IF240RK Wireless Headphone System", Price = 10.11m, Quantity = 8 },
                new { Name = "Samsung DVD-HD841 Up-Converting DVD Player", Price = 1000m, Quantity = 3 },
                new { Name = "Apple QuickTake 200 - Digital camera - compact - 0.35 Mpix - supported memory: SM", Price = 250m, Quantity = 2 },
                new { Name = "Samsung YP-T9JAB 4 GB Digital Multimedia Player (Black)", Price = 340m, Quantity = 1 },
                new { Name = "EFC-1B1NBECXAR Carrying Case for 10.1", Price = 222m, Quantity = 6 },
                new { Name = "Sony SGPFLS1 Tablet S LCD Protection Sheet", Price = 3422m, Quantity = 2 }
            };

			for (var i = 0; i < items.Count(); i++)
			{
				var lineItem = new LineItem { DisplayName = items[i].Name, Quantity = items[i].Quantity, ListPrice = items[i].Price };
				lineItem.PlacedPrice = lineItem.ListPrice;
				lineItem.ExtendedPrice = lineItem.PlacedPrice * items[i].Quantity;
				lineItem.CatalogItemId = "v-b0000u9h40";
				lineItem.CatalogItemCode = "v-b0000u9h40";

				retVal.Add(lineItem);
			}
			return retVal.ToArray();
		}

	}
}
