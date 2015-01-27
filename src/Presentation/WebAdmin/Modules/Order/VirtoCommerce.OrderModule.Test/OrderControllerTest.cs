using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.Domain.Cart.Repositories;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Repositories;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Web.Controllers.Api;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Domain.Payment.Services;

namespace VirtoCommerce.OrderModule.Test
{
	[TestClass]
	public class OrderControllerTest
	{
		private CustomerOrderController _controller;
		[TestInitialize]
		public void Initialize()
		{
			_controller  = GetCustomerOrderController();
			var testOrder = GetTestOrder("order1");
			_controller.CreateOrder(testOrder);
		}

		[TestMethod]
		public void CreateNewOrderByShoppingCart()
		{
			var result = _controller.CreateOrderFromCart("123") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			Assert.IsNotNull(result.Content);
		}

		[TestMethod]
		public void CreateNewManualOrder()
		{
			var testOrder = GetTestOrder("order2");
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
			var gateway = mockPaymentManager.Object.PaymentGateways.FirstOrDefault(x => x.GatewayCode == payment.PaymentGatewayCode);
			var externalPaymentInfo = gateway.GetPaymentById(payment.OuterId);

			payment.IsApproved = externalPaymentInfo.IsApproved;

			_controller.Update(testOrder);
		}


		[TestMethod]
		public void FulfilOrderWithSingleShipment()
		{
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var shipment = testOrder.Shipments.FirstOrDefault();
			foreach(var item in testOrder.Items)
			{
				shipment.Items.Add(item);
			}
			shipment.IsApproved = true;
			testOrder.IsApproved = true;

			_controller.Update(testOrder);
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

			var item1 = new webModel.CustomerOrderItem
			{
				BasePrice = 77,
				Price = 77,
				DisplayName = "boots",
				ItemId = "boots",
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
			var result = _controller.GetById("order1") as OkNegotiatedContentResult<webModel.CustomerOrder>;
			var testOrder = result.Content;

			var newShipment = new webModel.Shipment
			{
				 Currency = testOrder.Currency,
				 DeliveryAddress = testOrder.ShippingAddresses.First(),
				 IsApproved = true
			};
			testOrder.IsApproved = true;

			testOrder.Shipments.Add(newShipment);
			//Aprove shipment
			foreach(var shipment in testOrder.Shipments)
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
				Currency = Foundation.Money.CurrencyCodes.USD,
				TargetAgentId = "vasja customer",
				SourceAgentId = "employe",
				SourceStoreId = "test store",
			};
			var item1 = new webModel.CustomerOrderItem
			{
				BasePrice = 10,
				Price = 9,
				DisplayName = "shoes",
				ItemId = "shoes",
				Name = "shoes",
				Quantity = 2,
				FulfilmentLocationCode = "warehouse1",
				ShippingMethodCode = "EMS"
			};
			var item2 = new webModel.CustomerOrderItem
			{
				BasePrice = 100,
				Price = 100,
				DisplayName = "t-shirt",
				ItemId = "t-shirt",
				Name = "t-shirt",
				Quantity = 2,
				FulfilmentLocationCode = "warehouse1",
				ShippingMethodCode = "EMS"
			};
			order.Items = new List<webModel.CustomerOrderItem>();
			order.Items.Add(item1);
			order.Items.Add(item2);

			var shipment = new webModel.Shipment
			{
				Currency = Foundation.Money.CurrencyCodes.USD,
				DeliveryAddress = new webModel.Address
				{
					City = "london",
					CountryCode = "ENG",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "line 1",
					Organization = "org1"
				}
			};
			order.Shipments = new List<webModel.Shipment>();
			order.Shipments.Add(shipment);
			return order;
		}

		private static CustomerOrderController GetCustomerOrderController()
		{
			var cartRepository = new InMemoryCartRepository();
			var orderRepository = new InMemoryOrderRepository();
			var cartService = new ShoppingCartServiceImpl(cartRepository);
			var mockInventory = new Mock<IInventoryService>();

			var orderService = new CustomerOrderServiceImpl(orderRepository, mockInventory.Object, cartService, new TimeBasedNumberGeneratorImpl());

			var controller = new CustomerOrderController(orderService);
			return controller;
		}


	}
}
