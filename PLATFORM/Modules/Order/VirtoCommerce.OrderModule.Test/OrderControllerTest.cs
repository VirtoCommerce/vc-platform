using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Web.Controllers.Api;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.OrderModule.Data.Workflow;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Test
{
	[TestClass]
	public class OrderControllerTest
	{
		private CustomerOrderController _controller;
		[TestInitialize]
		public void Initialize()
		{
			_controller = GetCustomerOrderController();
			//var testOrder = GetTestOrder("order1");
			//_controller.CreateOrder(testOrder);
		}

		[TestMethod]
		public void CreateNewOrderByShoppingCart()
		{
			var result = _controller.CreateOrderFromCart("b5cf3acd-216b-41c7-b998-61f96a1608f6") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void CreateNewManualOrder()
		{
			var testOrder = GetTestOrder("order");
			var result = _controller.CreateOrder(testOrder) as OkNegotiatedContentResult<webModel.CustomerOrder>;
			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void ProcessPaymentForOrder()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var payment = testOrder.InPayments.FirstOrDefault();

			var mockPaymentManager = new Mock<IPaymentGatewayManager>();
			var gateway = mockPaymentManager.Object.PaymentGateways.FirstOrDefault(x => x.GatewayCode == payment.GatewayCode);
			var externalPaymentInfo = gateway.GetPaymentById(payment.OuterId);

			payment.IsApproved = externalPaymentInfo.IsApproved;

			_controller.Update(testOrder);
		}


		[TestMethod]
		public void FulfilOrderWithSingleShipmentAndPartialUpdate()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var partialChangeOrder = new webModel.CustomerOrder
			{
				Id = testOrder.Id,
				Shipments = testOrder.Shipments
			};

			var shipment = partialChangeOrder.Shipments.FirstOrDefault();
			shipment.Items = new List<webModel.LineItem>();
			foreach (var item in testOrder.Items)
			{
				item.Id = null;
				shipment.Items.Add(item);
			}
			shipment.IsApproved = true;

			_controller.Update(partialChangeOrder);
		}

		//
		[TestMethod]
		public void CancelOrderItem()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var item = testOrder.Items.FirstOrDefault();
			testOrder.Items.Remove(item);

			_controller.Update(testOrder);
			result = _controller.GetById(testOrder.Id) as OkNegotiatedContentResult<webModel.CustomerOrder>;
			testOrder = result.Content;

		}

		[TestMethod]
		public void DescreaseOrderItem()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var item = testOrder.Items.FirstOrDefault();
			item.Quantity -= 1;

			_controller.Update(testOrder);
			result = _controller.GetById(testOrder.Id) as OkNegotiatedContentResult<webModel.CustomerOrder>;
			testOrder = result.Content;
		}

		[TestMethod]
		public void AddNewOrderItem()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var item1 = new webModel.LineItem
			{
				BasePrice = 77,
				Price = 77,
				DisplayName = "boots",
				ProductId = "boots",
				Name = "boots",
				Quantity = 2,
				FulfilmentLocationCode = "warehouse1",
				ShippingMethodCode = "EMS"
			};
			testOrder.Items.Add(item1);

			_controller.Update(testOrder);
			result = _controller.GetById(testOrder.Id) as OkNegotiatedContentResult<webModel.CustomerOrder>;
			testOrder = result.Content;
		}

		[TestMethod]
		public void ApplyCoupon()
		{
		}


		[TestMethod]
		public void FulfilOrderWithMultipleShipment()
		{
			var result = _controller.GetById("d2a855c7-dc88-44b3-ab4e-4dba3fc89057") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var newShipment = new webModel.Shipment
			{
				Id = Guid.NewGuid().ToString(),
				Currency = testOrder.Currency,
				DeliveryAddress = testOrder.Addresses.FirstOrDefault(),
				IsApproved = true,
				Items = testOrder.Items
			};
			testOrder.IsApproved = true;

			testOrder.Shipments.Add(newShipment);
			//Aprove shipment
			foreach (var shipment in testOrder.Shipments)
			{
				shipment.IsApproved = true;
			}
			_controller.Update(testOrder);

			result = _controller.GetById(testOrder.Id) as OkNegotiatedContentResult<webModel.CustomerOrder>;
			testOrder = result.Content;
		}

		[TestMethod]
		public void FulfilOrderPartialy()
		{
		}

		private static webModel.CustomerOrder GetTestOrder(string id)
		{
			var order = new webModel.CustomerOrder
			{
				Id = id,
				Currency = CurrencyCodes.USD,
				CustomerId = "vasja customer",
				EmployeeId = "employe",
				StoreId = "test store",
				Addresses = new webModel.Address[]
				{
					new webModel.Address {	
					AddressType = coreModel.AddressType.Shipping, 
					City = "london",
					Phone = "+68787687",
					PostalCode = "22222",
					CountryCode = "ENG",
					CountryName = "England",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "line 1",
					Organization = "org1"
					}
				}.ToList(),
				Discount = new webModel.Discount
				{
					PromotionId = "testPromotion",
					Currency = CurrencyCodes.USD,
					DiscountAmount = 12,
					Coupon = new webModel.Coupon
					{
						Code = "ssss"
					}
				}
			};
			var item1 = new webModel.LineItem
			{
				BasePrice = 10,
				Price = 9,
				DisplayName = "shoes",
				ProductId = "shoes",
				CatalogId = "catalog",
				Currency = CurrencyCodes.USD,
				CategoryId = "category",
				Name = "shoes",
				Quantity = 2,
				FulfilmentLocationCode = "warehouse1",
				ShippingMethodCode = "EMS",
				Discount = new webModel.Discount
				{
					PromotionId = "itemPromotion",
					Currency = CurrencyCodes.USD,
					DiscountAmount = 12,
					Coupon = new webModel.Coupon
					{
						Code = "ssss"
					}
				}
			};
			var item2 = new webModel.LineItem
			{
				BasePrice = 100,
				Price = 100,
				DisplayName = "t-shirt",
				ProductId = "t-shirt",
				CatalogId = "catalog",
				CategoryId = "category",
				Currency = CurrencyCodes.USD,
				Name = "t-shirt",
				Quantity = 2,
				FulfilmentLocationCode = "warehouse1",
				ShippingMethodCode = "EMS",
				Discount = new webModel.Discount
				{
					PromotionId = "testPromotion",
					Currency = CurrencyCodes.USD,
					DiscountAmount = 12,
					Coupon = new webModel.Coupon
					{
						Code = "ssss"
					}
				}
			};
			order.Items = new List<webModel.LineItem>();
			order.Items.Add(item1);
			order.Items.Add(item2);

			var shipment = new webModel.Shipment
			{
				Currency = CurrencyCodes.USD,
				DeliveryAddress = new webModel.Address
				{
					City = "london",
					CountryName = "England",
					Phone = "+68787687",
					PostalCode = "2222",
					CountryCode = "ENG",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "line 1",
					Organization = "org1"
				},
				Discount = new webModel.Discount
				{
					PromotionId = "testPromotion",
					Currency = CurrencyCodes.USD,
					DiscountAmount = 12,
					Coupon = new webModel.Coupon
					{
						Code = "ssss"
					}
				}
			};
			order.Shipments = new List<webModel.Shipment>();
			order.Shipments.Add(shipment);

			var payment = new webModel.PaymentIn
			{
				GatewayCode = "PayPal",
				Currency = CurrencyCodes.USD,
				Sum = 10,
				CustomerId = "et"
			};
			order.InPayments = new List<webModel.PaymentIn>();
			order.InPayments.Add(payment);

			return order;
		}

		private static CustomerOrderController GetCustomerOrderController()
		{
			var mockInventory = new Mock<IInventoryService>();
			Func<ICartRepository> repositoryFactory = () =>
			{
				return new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor());
			};

			var mockWorkflow = new Mock<IWorkflowService>();
			var cartService = new ShoppingCartServiceImpl(repositoryFactory, mockWorkflow.Object);

			Func<IOrderRepository> orderRepositoryFactory = () => { return new OrderRepositoryImpl("VirtoCommerce", 
																		   new AuditableInterceptor(),
																		   new EntityPrimaryKeyGeneratorInterceptor());
			};
			var orderWorkflowService = new ObservableWorkflowService<CustomerOrderStateBasedEvalContext>();
			//Subscribe to order changes. Calculate totals  
			orderWorkflowService.Subscribe(new CalculateTotalsActivity());
			var orderService = new CustomerOrderServiceImpl(orderRepositoryFactory, new TimeBasedNumberGeneratorImpl(), orderWorkflowService, cartService);

			var controller = new CustomerOrderController(orderService, null, new TimeBasedNumberGeneratorImpl());
			return controller;
		}


	}
}
