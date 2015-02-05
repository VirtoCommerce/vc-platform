using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FunctionalTests.Orders.Helpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Moq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Currencies;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Gateways;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.OrderWorkflow;
using VirtoCommerce.Scheduling;
using VirtoCommerce.Scheduling.Jobs;
using VirtoCommerce.Scheduling.LogicalCall;
using Xunit;

namespace FunctionalTests.Orders
{
	public class CheckoutScenarios : OrderTestBase
	{
		[Fact]
		public void Can_run_activity_adjustinventory()
		{
			var invAvailable = new Inventory
				{
					AllowBackorder = true,
					AllowPreorder = true,
					BackorderAvailabilityDate = DateTime.Now,
					PreorderAvailabilityDate = DateTime.Now,
					FulfillmentCenterId = "default",
					InStockQuantity = 10,
					Sku = "v-9948444183",
					ReservedQuantity = 1,
					Status = InventoryStatus.Enabled.GetHashCode()
				};

			var invNotAvailable = new Inventory
				{
					AllowBackorder = true,
					AllowPreorder = true,
					BackorderAvailabilityDate = DateTime.Now,
					PreorderAvailabilityDate = DateTime.Now,
					FulfillmentCenterId = "default",
					InStockQuantity = 14,
					Sku = "v-b000068ilf",
					ReservedQuantity = 10,
					BackorderQuantity = 10,
					Status = InventoryStatus.Enabled.GetHashCode()
				};

			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var repository = new Mock<IInventoryRepository>();
			repository.Setup(x => x.Inventories).Returns(() => new[] { invAvailable, invNotAvailable }.AsQueryable());
			repository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			repository.SetupAllProperties();

			var item1 = new Product
				{
					ItemId = "v-9948444183",
					TrackInventory = true
				};

			var item2 = new Product
				{
					ItemId = "v-b000068ilf",
					TrackInventory = true
				};
			var catrepository = new Mock<ICatalogRepository>();
			catrepository.Setup(x => x.Items).Returns(() => new Item[] { item1, item2 }.AsQueryable());
			catrepository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			catrepository.SetupAllProperties();

			var customerSession = new Mock<ICustomerSessionService>();

			var cacheRepository = new Mock<ICacheRepository>();
			cacheRepository.SetupAllProperties();

			var orderGroup = CreateCart();
			var activity = new AdjustInventoryActivity(catrepository.Object, repository.Object, customerSession.Object, cacheRepository.Object);
			//activity.InventoryRepository = _repository.Object;
			//activity.CatalogRepository = _catrepository.Object;

			orderGroup.OrderForms[0].LineItems[0].CatalogItemId = "v-9948444183";
			orderGroup.OrderForms[0].LineItems[0].FulfillmentCenterId = "default";
			orderGroup.OrderForms[0].LineItems[0].Quantity = 6;
			orderGroup.OrderForms[0].LineItems[1].CatalogItemId = "v-b000068ilf";
			orderGroup.OrderForms[0].LineItems[1].FulfillmentCenterId = "default";
			orderGroup.OrderForms[0].LineItems[1].Quantity = 10;

			InvokeActivity(activity, orderGroup);

			// validate adjusted inventory
			var inventory = repository.Object.Inventories.SingleOrDefault(i => i.FulfillmentCenterId.Equals("default", StringComparison.OrdinalIgnoreCase)
																			   && i.Sku.Equals("v-9948444183", StringComparison.OrdinalIgnoreCase));

			Assert.True(inventory != null && inventory.InStockQuantity == 4);

			inventory = repository.Object.Inventories.SingleOrDefault(i => i.FulfillmentCenterId.Equals("default", StringComparison.OrdinalIgnoreCase)
																		   && i.Sku.Equals("v-b000068ilf", StringComparison.OrdinalIgnoreCase));

			Assert.True(inventory != null && inventory.InStockQuantity == 10);
			Assert.True(inventory != null && inventory.BackorderQuantity == 4);
		}

		[Fact]
		public void Can_run_activity_removediscount()
		{
			var orderGroup = CreateCart();

			var promotionId = Guid.NewGuid().ToString();
			var lineItemSkuReward = orderGroup.OrderForms[0].LineItems[0];

			lineItemSkuReward.Discounts.Add(new LineItemDiscount()
				{
					DiscountAmount = 100, 
					PromotionId = promotionId, 
					DiscountCode = "code", 
					LineItemId = lineItemSkuReward.LineItemId
				});

			var reward = new CatalogItemReward
				{
					PromotionId = promotionId,
					Amount = 100,
					AmountTypeId = 2,
					SkuId = lineItemSkuReward.CatalogItemId,
					QuantityLimit = 1
				};

			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var repository = new Mock<IMarketingRepository>();
			repository.Setup(x => x.PromotionRewards).Returns(() => new[] { reward }.AsQueryable());
			repository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			repository.SetupAllProperties();

			var activity = new RemoveDiscountActivity(repository.Object);

			InvokeActivity(activity, orderGroup);

			// now check if order contains any discounts
			var forms = from f in orderGroup.OrderForms select f;
			var orderForms = forms as OrderForm[] ?? forms.ToArray();
			Assert.True((from f in orderForms select f.Discounts.Count()).Sum() == 0, "still contains form discounts");

			Assert.False(forms.SelectMany(f=>f.LineItems).Any(x=>x.LineItemId == lineItemSkuReward.LineItemId), "still contains line item added as promotion reward");

			var shipments = from s in orderForms select s.Shipments;
			foreach (var shipmentCollection in shipments)
			{
				Assert.True((from s in shipmentCollection select s.Discounts.Count()).Sum() == 0, "still contains shipment discounts");
			}

			var lineitems = from s in orderForms select s.LineItems;
			foreach (var lineItemCollection in lineitems)
			{
				Assert.True((from s in lineItemCollection select s.Discounts.Count()).Sum() == 0, "still contains lineitem discounts");
			}
		}

		[Fact]
		public void Can_run_activity_calculatetotals()
		{
			// total of all line items is 12196.88
			const decimal subtotal = 12196.88m;
			const decimal orderFormDiscount = 50m;
			const decimal shippingTotal = 0m;
			const decimal shipmentDiscount = 20.11m;
			const decimal lineItemDiscount = 5.65m;

			var orderGroup = CreateCartConstant();
			var total = subtotal - orderFormDiscount - shipmentDiscount - (orderGroup.OrderForms[0].LineItems.Count * lineItemDiscount);

			var activity = new CalculateTotalsActivity();
			var result = InvokeActivity(activity, orderGroup);

			var order = result.OrderGroup;
			// now check totals            

			// Order totals
			Assert.True(order.Total == total, "Order.Total is incorrect");
			Assert.True(order.Subtotal == subtotal, "Order.Subtotal is incorrect");
			Assert.Equal(0m, order.HandlingTotal);
			Assert.Equal(shippingTotal, order.ShippingTotal);
			Assert.Equal(0m, order.TaxTotal); // taxes were not calculated

			// Order form totals
			var form = order.OrderForms[0];

			Assert.Equal(total, form.Total);
			Assert.Equal(subtotal, form.Subtotal);
			Assert.Equal(orderFormDiscount, form.DiscountAmount);
			Assert.Equal(0m, form.HandlingTotal);
			Assert.Equal(shippingTotal, form.ShippingTotal);
			Assert.Equal(0m, form.TaxTotal);
		}

		[Fact]
		public void Can_run_activity_CalculateReturnTotalsActivity()
		{
			// total of all line items is 120.22
			const decimal subtotal = 120.22m;
			const decimal shipmentSubtotal = 0m;

			var orderGroup = CreateOrderConstant();
			const decimal total = subtotal + shipmentSubtotal;

			var activity = new CalculateReturnTotalsActivity();
			var result = InvokeActivity(activity, orderGroup);

			var order = (Order)result.OrderGroup;
			var rmaRequest = order.RmaRequests.First(x =>
					string.IsNullOrEmpty(x.Status)
					|| x.Status == RmaRequestStatus.AwaitingStockReturn.ToString());
			// now check totals            

			// rmaRequest totals
			Assert.True(rmaRequest.ItemSubtotal == subtotal, "Subtotal is incorrect");
			Assert.True(rmaRequest.ReturnTotal == total, "Total is incorrect");
			Assert.True(rmaRequest.RefundAmount == 0, "Nothing should be refunded yet");
		}

		[Fact]
		public void Can_run_activity_checkinventory()
		{
			var orderGroup = CreateCart();

			var catrepository = new Mock<ICatalogRepository>();

			catrepository.Setup(x => x.Items).Returns(() =>
				{
					return
						orderGroup.OrderForms[0].LineItems.Select(
							item => new Sku() { TrackInventory = true, ItemId = item.CatalogItemId })
												.Cast<Item>()
												.ToArray()
												.AsQueryable();
				});
			catrepository.SetupAllProperties();

			var activity = new CheckInventoryActivity(catrepository.Object, CacheRepository);
			//var activity = new CheckInventoryActivity();

			orderGroup.OrderForms[0].LineItems[0].MinQuantity = 1;
			orderGroup.OrderForms[0].LineItems[0].MaxQuantity = 10;
			orderGroup.OrderForms[0].LineItems[0].InStockQuantity = 1;
			orderGroup.OrderForms[0].LineItems[0].BackorderQuantity = 1;
			orderGroup.OrderForms[0].LineItems[0].Quantity = 6;
			orderGroup.OrderForms[0].LineItems[0].AllowBackorders = true;
			orderGroup.OrderForms[0].LineItems[0].AllowPreorders = true;
			orderGroup.OrderForms[0].LineItems[0].InventoryStatus = InventoryStatus.Enabled.ToString();
			orderGroup.OrderForms[0].LineItems[0].CatalogItemId = "638397a5-9bda-49cf-b77e-ce3d3ee96483";

			orderGroup.OrderForms[0].LineItems[1].InStockQuantity = 0;
			orderGroup.OrderForms[0].LineItems[1].Quantity = 10;
			orderGroup.OrderForms[0].LineItems[1].MinQuantity = 1;
			orderGroup.OrderForms[0].LineItems[1].MaxQuantity = 10;
			orderGroup.OrderForms[0].LineItems[1].InventoryStatus = InventoryStatus.Enabled.ToString();

			InvokeActivity(activity, orderGroup);

			Assert.True(orderGroup.OrderForms[0].LineItems.Count == 1); // one item is out of stock and should be removed
			Assert.True(orderGroup.OrderForms[0].LineItems[0].Quantity == 2); // only 2 items are available backorder + instock
			Assert.True(orderGroup.OrderForms[0].Shipments[0].ShipmentItems.Count == 1); // only 1 item is available
			Assert.True(orderGroup.OrderForms[0].Shipments[0].Status == ShipmentStatus.AwaitingInventory.ToString()); // instock is insufficient, must await for Inventory
		}

		[Fact]
		public void Can_run_activity_processpayment()
		{
			var orderGroup = CreateCart();

			var paymentMethodsRepository = new Mock<IPaymentMethodRepository>();

            var gateway = new PaymentGateway
                {
                    GatewayId = "gwAuthorizeNet",
                    ClassType = "VirtoCommerce.PaymentGateways.AuthorizeNetPaymentGateway, VirtoCommerce.PaymentGateways",
                    Name = "Authorize.Net",
                    SupportsRecurring = false,
                    SupportedTransactionTypes = 0x1F
                };

			paymentMethodsRepository.Setup(x => x.PaymentGateways).Returns(() => new[] { gateway }.AsQueryable());

			var pm = new PaymentMethod
				{
					Description = "Credit Card",
					Name = "CreditCard",
					PaymentGateway = gateway,
					PaymentGatewayId = gateway.GatewayId,

				};
			var methodLanguage = new PaymentMethodLanguage
				{
					DisplayName = pm.Description,
					LanguageCode = "en-US",
					PaymentMethodId = pm.PaymentMethodId,
				};
			pm.PaymentMethodLanguages.Add(methodLanguage);

			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				Name = "MerchantLogin",
				ShortTextValue = "87WmkB7W"
			});
			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				Name = "MerchantPassword",
				ShortTextValue = "8hAuD275892cBFcb"
			});
			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode(),
				Name = "TestMode",
				BooleanValue = true
			});
            //pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            //{
            //    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
            //    Name = "GatewayURL",
            //    ShortTextValue = "https://test.authorize.net/gateway/transact.dll"
            //});

			paymentMethodsRepository.Setup(x => x.PaymentMethods).Returns(() => new[] { pm }.AsQueryable());

			paymentMethodsRepository.SetupAllProperties();

            var storesrep = new Mock<IStoreRepository>();
            storesrep.Setup(x => x.Stores).Returns(() => new List<Store>(){new Store
			{
			    CreditCardSavePolicy = CreditCardSavePolicy.LastFourDigits.GetHashCode(),
                StoreId = "SampleStore"
			}}.AsQueryable());
			storesrep.SetupAllProperties();

			orderGroup.OrderForms[0].Payments.Clear();
			orderGroup.OrderForms[0].Payments.Add(new CreditCardPayment
			{
				PaymentType = PaymentType.CreditCard.GetHashCode(),
				CreditCardCustomerName = "John Doe",
				CreditCardExpirationMonth = 1,
				CreditCardExpirationYear = 2016,
				CreditCardNumber = "4007000000027",
				CreditCardType = "VISA",
				CreditCardSecurityCode = "123",
				PaymentMethodId = pm.PaymentMethodId,
				PaymentMethodName = pm.Description,
				Amount = 32.53m,
				TransactionType = TransactionType.Sale.ToString(),
				Status = PaymentStatus.Pending.ToString(),
				OrderForm = orderGroup.OrderForms[0],
				BillingAddressId = orderGroup.OrderAddresses.First().OrderAddressId
			});

			orderGroup.Total = orderGroup.OrderForms.SelectMany(orderForm => orderForm.Payments).Sum(payment => payment.Amount);

			var activity = new ProcessPaymentActivity(paymentMethodsRepository.Object, storesrep.Object);

			var result = InvokeActivity(activity, orderGroup);

			var order = result.OrderGroup;

			foreach (var payment in order.OrderForms[0].Payments)
			{
				Assert.False(payment.Status == PaymentStatus.Pending.ToString());
			}
		}

		[Fact]
		public void Can_run_activity_calculatetaxtotals()
		{
			var orderGroup = CreateOrderConstant();

			var catrepository = new Mock<ICatalogRepository>();
			var items = new List<Item>();
			orderGroup.OrderForms[0].Shipments[0].ShipmentItems.ToList().ForEach(item => items.Add(new Product
				{
					ItemId = item.LineItem.CatalogItemId,
					TaxCategory = "472b9809-3530-4d95-9886-cbddcaa0b72e"
				}));

			orderGroup.OrderForms[0].Shipments[0].ShippingCost = 10;

			catrepository.Setup(x => x.Items).Returns(() => items.ToArray().AsQueryable());
			catrepository.SetupAllProperties();

			var taxrepository = new Mock<ITaxRepository>();
			var taxes = new[] 
			{ 
				new Tax { Name = "US Shipment tax", TaxType = (int)TaxTypes.ShippingTax, SortOrder = 1},
				new Tax { Name = "US Sales tax", TaxType = (int)TaxTypes.SalesTax, SortOrder = 1}
			};

			var jurisdiction = new Jurisdiction
			{
				Code = "VAT",
				CountryCode = "USA",
				JurisdictionType = (int)JurisdictionTypes.All
			};

			var jurisdictionGroup = new JurisdictionGroup
			{
				DisplayName = "USA VAT",
				Code = "USAVAT",
				JurisdictionType = (int)JurisdictionTypes.All
			};
			var jurisdictionRel = new JurisdictionRelation
			{
				JurisdictionGroup = jurisdictionGroup,
				JurisdictionGroupId = jurisdictionGroup.JurisdictionGroupId,
				Jurisdiction = jurisdiction,
				JurisdictionId = jurisdiction.JurisdictionId
			};

			jurisdictionGroup.JurisdictionRelations.Add(jurisdictionRel);

			taxes[0].TaxValues.Add(new TaxValue()
			{
				AffectiveDate = new DateTime(2013, 6, 1),
				Percentage = 18,
				TaxCategory = "472b9809-3530-4d95-9886-cbddcaa0b72e",
				TaxId = taxes[0].TaxId,
				Tax = taxes[0],
				JurisdictionGroup = jurisdictionGroup,
				JurisdictionGroupId = jurisdictionGroup.JurisdictionGroupId
			});

			taxes[1].TaxValues.Add(new TaxValue()
			{
				AffectiveDate = new DateTime(2013, 6, 1),
				Percentage = 5,
				TaxCategory = "472b9809-3530-4d95-9886-cbddcaa0b72e",
				TaxId = taxes[1].TaxId,
				Tax = taxes[1],
				JurisdictionGroup = jurisdictionGroup,
				JurisdictionGroupId = jurisdictionGroup.JurisdictionGroupId
			});

			taxrepository.Setup(x => x.Taxes).Returns(taxes.AsQueryable);
			taxrepository.SetupAllProperties();

			var activity = new CalculateTaxActivity(catrepository.Object, taxrepository.Object, CacheRepository);

			var result = InvokeActivity(activity, orderGroup);

			var order = result.OrderGroup;
			// now check totals            

			// Order totals
			Assert.Equal(order.TaxTotal, 660.24M);
			Assert.Equal(order.OrderForms[0].Shipments[0].ItemTaxTotal, 609.84M);
			Assert.Equal(order.OrderForms[0].Shipments[0].ShippingTaxTotal, 12.6M);
		}

		[Fact]
		public void Can_run_activity_processshipment()
		{
			var orderGroup = CreateCart();
			orderGroup.OrderForms[0].Shipments[0].ShippingMethodId = "FlatRate";

			var gateway0 = new ShippingGateway
						{
							ClassType = "VirtoCommerce.Shipping.SimpleShippingGateway, VirtoCommerce.SimpleShippingGateway",
							Name = "SimpleShippingGateway"
						};
			var option = new ShippingOption { Name = "default", Description = "Default", ShippingGateway = gateway0 };
			var option2 = new ShippingOption { Name = "default2", Description = "Default2", ShippingGateway = gateway0 };
			var shippingMethods = new List<ShippingMethod>
				{
					new ShippingMethod
						{
							ShippingMethodId = "FreeShipping",
							Name = "FreeShipping",
							DisplayName = "Free Shipping",
							Description = "Free Shipping",
							Currency = "USD",
							BasePrice = 0,
							IsActive = true,
							ShippingOption = option
						},
					new ShippingMethod
						{
							ShippingMethodId = "FlatRate",
							Name = "FlatRate",
							DisplayName = "Flat Rate",
							Description = "Flat Rate",
							Currency = "USD",
							BasePrice = 10,
							IsActive = true,
							ShippingOption = option2
						}
				};
			option.ShippingMethods.Add(shippingMethods[0]);
			option2.ShippingMethods.Add(shippingMethods[1]);

			var repository = new Mock<IShippingRepository>();
			repository.Setup(x => x.ShippingOptions).Returns(() => new[] { option, option2 }.AsQueryable());
			// initializing UnityContainer
			var initializedLocator = Locator;
			// mocking the IShippingRepository into UnityContainer as SimpleShippingGateway resolves it.
			_container.RegisterInstance(repository.Object);

			var activity = new ProcessShipmentActivity();
			var result = InvokeActivity(activity, orderGroup);
			var order = result.OrderGroup;

			foreach (var shipment in order.OrderForms[0].Shipments)
			{
				Assert.True(shipment.ShippingCost == 10m);
			}
		}

		[Fact]
		public void Can_run_activity_recordpromotionusage()
		{
			var orderGroup = CreateCart();

            var promotionId = Guid.NewGuid().ToString();
            var memberId = Guid.NewGuid().ToString();
		    const string couponCode = "Test123";
            const PromotionUsageStatus testStatus = PromotionUsageStatus.Reserved;
            var lineItemSkuReward = orderGroup.OrderForms[0].LineItems[0];

            lineItemSkuReward.Discounts.Add(new LineItemDiscount()
            {
                DiscountAmount = 100,
                PromotionId = promotionId,
                DiscountCode = couponCode,
                LineItemId = lineItemSkuReward.LineItemId
            });

            var customerSession = new Mock<ICustomerSessionService>();
            customerSession.Setup(x => x.CustomerSession).Returns(() => new CustomerSession { CustomerId = memberId });
		    customerSession.SetupAllProperties();


		    var usages = new List<PromotionUsage>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var repository = new Mock<IMarketingRepository>();
            repository.Setup(x => x.PromotionUsages).Returns(usages.AsQueryable);
            repository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
		    repository.Setup(x => x.Add(It.IsAny<PromotionUsage>())).Callback<PromotionUsage>(usages.Add);
            repository.SetupAllProperties();

            var activity = new RecordPromotionUsageActivity(customerSession.Object, repository.Object)
            {
                UsageStatus = testStatus
            };

			InvokeActivity(activity, orderGroup);

		    var addedUsage = repository.Object.PromotionUsages.First(x => x.PromotionId == promotionId);

            Assert.True(addedUsage.Status == (int)testStatus, "Wrong promotion usage status");
            Assert.True(addedUsage.CouponCode.Equals(couponCode), "Wrong promotion usage status");
		}

		[Fact]
		public void Can_run_activity_shipmentsplit()
		{
			var orderGroup = CreateCart();
			var activity = new ShipmentSplitActivity();

			// remove shipments
			orderGroup.OrderForms[0].Shipments.RemoveAt(0);

			var result = InvokeActivity(activity, orderGroup);

			Assert.True(result.OrderGroup.OrderForms[0].Shipments.Count == 1);

			// now reset the cart and do split shipments again
			orderGroup.OrderForms[0].Shipments.Clear();
			result = InvokeActivity(activity, orderGroup);

			Assert.True(result.OrderGroup.OrderForms[0].Shipments.Count == 1);

		}

		[Fact]
		public void Can_run_activity_validatelineitems()
		{
			var orderGroup = CreateCart();

			orderGroup.OrderForms[0].LineItems[0].Catalog = "default";
			orderGroup.OrderForms[0].LineItems[0].CatalogItemId = "v-9948444183";
			orderGroup.OrderForms[0].LineItems[0].FulfillmentCenterId = "default";
			orderGroup.OrderForms[0].LineItems[0].Quantity = 4;
			orderGroup.OrderForms[0].LineItems[1].Catalog = "default";
			orderGroup.OrderForms[0].LineItems[1].CatalogItemId = "v-b000068ilf";
			orderGroup.OrderForms[0].LineItems[1].FulfillmentCenterId = "default";
			orderGroup.OrderForms[0].LineItems[1].Quantity = 10;


			var invAvailable = new Inventory
				{
					AllowBackorder = true,
					AllowPreorder = true,
					FulfillmentCenterId = "default",
					InStockQuantity = 10,
					Sku = "v-9948444183",
					ReservedQuantity = 1,
					Status = InventoryStatus.Enabled.GetHashCode(),
					BackorderQuantity = 5,
					PreorderQuantity = 3
				};

			var invNotAvailable = new Inventory
				{
					AllowBackorder = true,
					AllowPreorder = true,
					FulfillmentCenterId = "default",
					InStockQuantity = 14,
					Sku = "v-b000068ilf",
					ReservedQuantity = 10,
					BackorderQuantity = 10,
					Status = InventoryStatus.Enabled.GetHashCode(),
					PreorderQuantity = 4
				};

			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var repository = new Mock<IInventoryRepository>();
			repository.Setup(x => x.Inventories).Returns(() => new[] { invAvailable, invNotAvailable }.AsQueryable());
			repository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			repository.SetupAllProperties();

			var item1 = new Product
				{
					ItemId = "v-9948444183",
					TrackInventory = true,
					IsActive = true,
					IsBuyable = true,
					StartDate = DateTime.UtcNow.AddDays(-1),
					EndDate = DateTime.UtcNow.AddDays(1)
				};

			var item2 = new Product
				{
					ItemId = "v-b000068ilf",
					IsActive = true,
					IsBuyable = true,
					TrackInventory = true,
					StartDate = DateTime.UtcNow.AddDays(-1),
					EndDate = DateTime.UtcNow.AddDays(1)
				};

			var catrepository = new Mock<ICatalogRepository>();
			catrepository.Setup(x => x.Items).Returns(() => new Item[] { item1, item2 }.AsQueryable());
			catrepository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			catrepository.SetupAllProperties();

			var store = new Store { StoreId = orderGroup.StoreId, Catalog = orderGroup.OrderForms[0].LineItems[0].Catalog };
			var storeRepository = new Mock<IStoreRepository>();
			storeRepository.Setup(x => x.Stores).Returns(() => new[] { store }.AsQueryable());

			var priceList = new Pricelist { PricelistId = "default", Currency = "USD" };
			var priceList2 = new Pricelist { PricelistId = "sale", Currency = "USD" };
			var prices = new[] 
            {
                new Price { List = 100, Sale = 90, MinQuantity = 1, ItemId = "v-9948444183" , PricelistId = "default"},
                new Price { List = 95, Sale = 85, MinQuantity = 5, ItemId = "v-9948444183", PricelistId = "default"},
                new Price { List = 98, Sale = 88, MinQuantity = 1, ItemId = "v-9948444183" , PricelistId = "sale"},
                new Price { List = 93, Sale = 83, MinQuantity = 5, ItemId = "v-9948444183", PricelistId = "sale"},
                new Price { List = 60, Sale = 50, MinQuantity = 1, ItemId = "v-b000068ilf" , PricelistId = "default"},
                new Price { List = 55, Sale = 45, MinQuantity = 5, ItemId = "v-b000068ilf", PricelistId = "default"},
                new Price { List = 58, Sale = 48, MinQuantity = 1, ItemId = "v-b000068ilf" , PricelistId = "sale"},
                new Price { List = 53, Sale = 43, MinQuantity = 5, ItemId = "v-b000068ilf", PricelistId = "sale"}

            };

			var priceRepository = new Mock<IPricelistRepository>();
			priceRepository.Setup(x => x.Pricelists).Returns(() => new[] { priceList2, priceList }.AsQueryable());
			priceRepository.Setup(x => x.Prices).Returns(prices.AsQueryable);

			var customerService = new CustomerSessionService();
			var session = customerService.CustomerSession;
			session.Currency = "USD";
			session.Pricelists = new[] { "Sale", "Default" };

			var currencyService = new CurrencyService();

			var cacheRepository = new Mock<ICacheRepository>();
			cacheRepository.SetupAllProperties();

			var activity = new ValidateLineItemsActivity(repository.Object, catrepository.Object, storeRepository.Object, customerService, priceRepository.Object, currencyService, null, null, cacheRepository.Object);

			var result = InvokeActivity(activity, orderGroup);

			var order = result.OrderGroup;
			// now check totals            

			// Order totals
			// Order form totals
			var form = order.OrderForms[0];

			Assert.True(form.LineItems.Count == 2);
			Assert.True(form.LineItems[0].InStockQuantity == 9);
			Assert.True(form.LineItems[0].PreorderQuantity == 3);
			Assert.True(form.LineItems[0].ListPrice == 88);
			Assert.True(form.LineItems[1].ListPrice == 43);
		}

		[Fact(Skip = "Cannot credit payment, because payment is settled in 24h for authorize.net")]

		public void can_create_payment_sale_and_credit()
		{
			var order = CreateOrder();

			var mockUnitOfWork = new Mock<IUnitOfWork>();
			var repository = new Mock<IOrderRepository>();
			repository.Setup(x => x.Orders).Returns(() => new[] { order }.AsQueryable());
			repository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);
			repository.SetupAllProperties();

			var paymentMethodsRepository = new Mock<IPaymentMethodRepository>();

            var gateway = new PaymentGateway
            {
                GatewayId = "gwAuthorizeNet",
                ClassType = "VirtoCommerce.PaymentGateways.AuthorizeNetPaymentGateway, VirtoCommerce.PaymentGateways",
                Name = "Authorize.Net",
                SupportsRecurring = false,
                SupportedTransactionTypes = 0x1F
            };


			paymentMethodsRepository.Setup(x => x.PaymentGateways).Returns(() => new[] { gateway }.AsQueryable());

			var pm = new PaymentMethod
			{
				Description = "Credit Card",
				Name = "CreditCard",
				PaymentGateway = gateway,
				PaymentGatewayId = gateway.GatewayId,

			};
			var methodLanguage = new PaymentMethodLanguage
			{
				DisplayName = pm.Description,
				LanguageCode = "en-US",
				PaymentMethodId = pm.PaymentMethodId,
			};
			pm.PaymentMethodLanguages.Add(methodLanguage);

			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				Name = "MerchantLogin",
				ShortTextValue = "87WmkB7W"
			});
			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
				Name = "MerchantPassword",
				ShortTextValue = "8hAuD275892cBFcb"
			});
			pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
			{
				ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode(),
				Name = "TestMode",
				BooleanValue = true
			});
            //pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            //{
            //    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
            //    Name = "GatewayURL",
            //    ShortTextValue = "https://test.authorize.net/gateway/transact.dll"
            //});

			paymentMethodsRepository.Setup(x => x.PaymentMethods).Returns(() => new[] { pm }.AsQueryable());

			paymentMethodsRepository.SetupAllProperties();

			var storesrep = new Mock<IStoreRepository>();
			storesrep.Setup(x => x.Stores).Returns(() => new List<Store>(){new Store
			{
			    CreditCardSavePolicy = CreditCardSavePolicy.LastFourDigits.GetHashCode(),
                StoreId = "SampleStore"
			}}.AsQueryable());
			storesrep.SetupAllProperties();

			Payment payment = new CreditCardPayment
				{
					PaymentMethodId = pm.PaymentMethodId,
					PaymentType = PaymentType.CreditCard.GetHashCode(),
					Status = PaymentStatus.Pending.ToString(),
					CreditCardCustomerName = "John Doe",
					PaymentMethodName = "VisaCard",
					ValidationCode = "RE21321-21",
					Amount = 32.53m,
					CreditCardExpirationMonth = 12,
					CreditCardExpirationYear = 2014,
					CreditCardNumber = "4007000000027",
					CreditCardType = "VISA",
					CreditCardSecurityCode = "123",
					BillingAddressId = order.OrderForms[0].BillingAddressId,
					OrderFormId = order.OrderForms[0].OrderFormId,
					TransactionType = TransactionType.Sale.ToString()
				};

			var orderService = new OrderService(repository.Object, null, null, paymentMethodsRepository.Object, storesrep.Object);

			var result = orderService.CreatePayment(payment);

			Assert.True(result.IsSuccess, result.Message);
			Assert.True(order.OrderForms[0].Payments.Any(p => p.PaymentId == payment.PaymentId), "Sale payment was not added");

			//Check if sale can be credited
			var creditPayment = (Payment)new CreditCardPayment().InjectFrom(payment);
			creditPayment.PaymentId = Guid.NewGuid().ToString();
			creditPayment.TransactionType = TransactionType.Credit.ToString();
			payment.Status = PaymentStatus.Completed.ToString();

			var creditresult = orderService.CreatePayment(creditPayment);
			Assert.True(creditresult.IsSuccess, creditresult.Message);
			Assert.True(order.OrderForms[0].Payments.Any(p => p.PaymentId == creditPayment.PaymentId), "Credit payment was not added");
		}

        [Fact]
        public void can_create_payment_authorize_and_capture()
        {
            var orderGroup = CreateCart();

            var paymentMethodsRepository = new Mock<IPaymentMethodRepository>();

            var gateway = new PaymentGateway
            {
                GatewayId = "gwAuthorizeNet",
                ClassType = "VirtoCommerce.PaymentGateways.AuthorizeNetPaymentGateway, VirtoCommerce.PaymentGateways",
                Name = "Authorize.Net",
                SupportsRecurring = false,
                SupportedTransactionTypes = 0x1F
            };

            paymentMethodsRepository.Setup(x => x.PaymentGateways).Returns(() => new[] { gateway }.AsQueryable());

            var pm = new PaymentMethod
            {
                Description = "Credit Card",
                Name = "CreditCard",
                PaymentGateway = gateway,
                PaymentGatewayId = gateway.GatewayId,

            };
            var methodLanguage = new PaymentMethodLanguage
            {
                DisplayName = pm.Description,
                LanguageCode = "en-US",
                PaymentMethodId = pm.PaymentMethodId,
            };
            pm.PaymentMethodLanguages.Add(methodLanguage);

            pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            {
                ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
                Name = "MerchantLogin",
                ShortTextValue = "87WmkB7W"
            });
            pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            {
                ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
                Name = "MerchantPassword",
                ShortTextValue = "8hAuD275892cBFcb"
            });
            pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            {
                ValueType = GatewayProperty.ValueTypes.Boolean.GetHashCode(),
                Name = "TestMode",
                BooleanValue = true
            });
            //pm.PaymentMethodPropertyValues.Add(new PaymentMethodPropertyValue
            //{
            //    ValueType = GatewayProperty.ValueTypes.ShortString.GetHashCode(),
            //    Name = "GatewayURL",
            //    ShortTextValue = "https://test.authorize.net/gateway/transact.dll"
            //});

            paymentMethodsRepository.Setup(x => x.PaymentMethods).Returns(() => new[] { pm }.AsQueryable());

            paymentMethodsRepository.SetupAllProperties();

            var storesrep = new Mock<IStoreRepository>();
            storesrep.Setup(x => x.Stores).Returns(() => new List<Store>(){new Store
			{
			    CreditCardSavePolicy = CreditCardSavePolicy.Full.GetHashCode(),
                StoreId = "SampleStore"
			}}.AsQueryable());
            storesrep.SetupAllProperties();

            orderGroup.OrderForms[0].Payments.Clear();
            orderGroup.OrderForms[0].Payments.Add(new CreditCardPayment
            {
                PaymentType = PaymentType.CreditCard.GetHashCode(),
                CreditCardCustomerName = "John Doe",
                CreditCardExpirationMonth = 1,
                CreditCardExpirationYear = 2016,
                CreditCardNumber = "4007000000027",
                CreditCardType = "VISA",
                CreditCardSecurityCode = "123",
                PaymentMethodId = pm.PaymentMethodId,
                PaymentMethodName = pm.Description,
                Amount = 32.53m,
                TransactionType = TransactionType.Authorization.ToString(),
                Status = PaymentStatus.Pending.ToString(),
                OrderForm = orderGroup.OrderForms[0],
                BillingAddressId = orderGroup.OrderAddresses.First().OrderAddressId
            });

            orderGroup.Total = orderGroup.OrderForms.SelectMany(orderForm => orderForm.Payments).Sum(payment => payment.Amount);

            var activity = new ProcessPaymentActivity(paymentMethodsRepository.Object, storesrep.Object);

            var result = InvokeActivity(activity, orderGroup);


            foreach (var payment in result.OrderGroup.OrderForms[0].Payments)
            {
                Assert.True(payment.Status == PaymentStatus.Completed.ToString());
            }

            var authPayemnt =
                result.OrderGroup.OrderForms[0].Payments.FirstOrDefault(
                    x =>
                        x.TransactionType == TransactionType.Authorization.ToString() &&
                        x.Status == PaymentStatus.Completed.ToString());

            Assert.NotNull(authPayemnt);

            authPayemnt.TransactionType = TransactionType.Capture.ToString();
            authPayemnt.Status = PaymentStatus.Pending.ToString();

            result = InvokeActivity(activity, orderGroup);

            foreach (var payment in result.OrderGroup.OrderForms[0].Payments)
            {
                Assert.True(payment.Status == PaymentStatus.Completed.ToString());
            }

        }
		// [Fact]
		[Fact(Skip = "fails on build server")]
		public void Run_ProcessOrderStatusWorkQuartz_job()
		{
			ServiceLocator.SetLocatorProvider(() => Locator);
			var item = new ProcessOrderStatusWork(ServiceLocator.Current.GetInstance<IStoreRepository>(), ServiceLocator.Current.GetInstance<IOrderRepository>());
			item.Execute(new JobContext(new TraceContext("VirtoCommerce.ScheduleService.Trace"), null));
		}

		private Order CreateOrder(int items = 2)
		{
			var builder = TestOrderBuilder.BuildOrder();
			var order = builder.GetOrder();
			const string customerId = "3a6e29a3-d0c9-4a9b-8207-faf957015c60";

			builder.WithAddresess()
				   .WithShipment()
				   .WithLineItemsCount(items)
				   .WithStatus("InProgress")
				   .WithCustomer(customerId)
				   .WithOrderFormDiscounts(1)
				   .WithLineItemDiscounts(2);

			order.StoreId = "SampleStore";
			return order;
		}

		private ShoppingCart CreateCart(int items = 2)
		{
			var builder = TestOrderBuilder.BuildCart();
			var order = builder.GetCart();
			const string customerId = "3a6e29a3-d0c9-4a9b-8207-faf957015c60";

			builder.WithAddresess()
				   .WithPayments()
				   .WithShipment()
				   .WithLineItemsCount(items)
				   .WithStatus("InProgress")
				   .WithCustomer(customerId)
				   .WithOrderFormDiscounts(1)
				   .WithLineItemDiscounts(2);

			order.StoreId = "SampleStore";
			return order;
		}

		private ShoppingCart CreateCartConstant()
		{
			var builder = TestOrderBuilder.BuildCart();
			var order = builder.GetCart();
			const string customerId = "3a6e29a3-d0c9-4a9b-8207-faf957015c60";

			builder.WithAddresess()
					.WithShipmentCount(1, 123.23m)
				   .WithShipmentDiscount(20.11m)
				   .WithLineItemsConstant()
				   .WithLineItemDiscounts(1, 5.65m)
				   .WithOrderFormDiscounts(1, 50m)
				   .WithCustomer(customerId);

			order.StoreId = "SampleStore";
			return order;
		}
	}
}