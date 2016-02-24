using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.CartModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Test
{

    [TestClass]
    public class ShoppingCartControllerTest
    {
        [TestMethod]
        public void CreateMultishipmentCart()
        {
            var controller = GetCartController();

            var cart = new webModel.ShoppingCart
            {
                Currency = "USD",
                CustomerId = "et",
                CustomerName = "et",
                Name = "default",
                StoreId = "Clothing"
            };
            var item = new webModel.LineItem
            {
                CatalogId = "Samsung",
                CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                ProductId = "v-b000bkzs9w",
                ListPrice = 10.44m,
                PlacedPrice = 20.33m,
                Quantity = 1,
                Sku = "v-b000bkzs9w",
                Name = "Samsung YP-T7JX 512 MB Digital Audio Player with FM Tuner & Recorder",
                Currency = cart.Currency
            };
            cart.Items = new List<webModel.LineItem>();
            cart.Items.Add(item);
            var deliveryAddress = new webModel.Address
            {
                Type = AddressType.Shipping,
                City = "london",
                Phone = "+68787687",
                PostalCode = "2222",
                CountryCode = "ENG",
                CountryName = "England",
                Email = "user@mail.com",
                FirstName = "first name",
                LastName = "last name",
                Organization = "org1",
                Line1 = "sss"
            };
            //Select appropriate shipment method
            var shipment = new webModel.Shipment
            {
                DeliveryAddress = deliveryAddress,
                Currency = "USD",
                ShipmentMethodCode = "",
                ShippingPrice = 10
            };
            cart.Shipments = new List<webModel.Shipment>();
            cart.Shipments.Add(shipment);
            shipment.Items = new List<webModel.ShipmentItem>();
            shipment.Items.Add(new Web.Model.ShipmentItem { Quantity = 10, LineItem = item });

            cart = (controller.Create(cart) as OkNegotiatedContentResult<webModel.ShoppingCart>).Content;

            //Add exist line items in cart
            var existLineItem = cart.Items.First();
            var shipment2 = new webModel.Shipment
            {
                DeliveryAddress = deliveryAddress,
                Currency = "USD",
                ShipmentMethodCode = "sss",
                ShippingPrice = 11
            };
            cart.Shipments.Add(shipment2);
            shipment2.Items = new List<webModel.ShipmentItem>();
            shipment2.Items.Add(new Web.Model.ShipmentItem { Quantity = 5, LineItem = existLineItem });
            existLineItem.Quantity += 5;

            cart = (controller.Update(cart) as OkNegotiatedContentResult<webModel.ShoppingCart>).Content;

            //Add new item to exist shipment
            var item2 = new webModel.LineItem
            {
                CatalogId = "Sony",
                CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                ProductId = "v-sssss",
                ListPrice = 10.44m,
                PlacedPrice = 20.33m,
                Quantity = 1,
                Sku = "v-ssss",
                Name = "Sony",
                Currency = cart.Currency
            };
            cart.Items.Add(item2);
            cart.Shipments.First().Items.Add(new Web.Model.ShipmentItem { Quantity = 3, LineItem = item2 });
            cart = (controller.Update(cart) as OkNegotiatedContentResult<webModel.ShoppingCart>).Content;
        }

        [TestMethod]
        public void GetCurrentCartTest()
        {
            var controller = GetCartController();
            var result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void AddItemToShoppingCart()
        {
            var controller = GetCartController();
            var result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            var cart = result.Content;

            var item = new webModel.LineItem
            {
                CatalogId = "Samsung",
                CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                ProductId = "v-b000bkzs9w",
                ListPrice = 10.44m,
                PlacedPrice = 20.33m,
                Quantity = 1,
                Name = "Samsung YP-T7JX 512 MB Digital Audio Player with FM Tuner & Recorder",
                Currency = cart.Currency
            };
            cart.Items.Add(item);

            controller.Update(cart);

            result = controller.GetCartById(cart.Id) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            cart = result.Content;
        }

        [TestMethod]
        public void TestCheckout()
        {
            var controller = GetCartController();
            var result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            var cart = result.Content;

            var deliveryAddress = cart.Addresses.FirstOrDefault(x => x.Type == AddressType.Shipping);
            if (deliveryAddress == null)
            {
                //Enter delivery address
                deliveryAddress = new webModel.Address
                {
                    Type = AddressType.Shipping,
                    City = "london",
                    Phone = "+68787687",
                    PostalCode = "2222",
                    CountryCode = "ENG",
                    CountryName = "England",
                    Email = "user@mail.com",
                    FirstName = "first name",
                    LastName = "last name",
                    Organization = "org1"
                };
                //cart.Addresses.Add(deliveryAddress);

            }
            deliveryAddress.Line1 = "Wishing Zephyr Limits, Coffee Creek";

            //Save changes
            controller.Update(cart);

            result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            cart = result.Content;

            //Select appropriate shipment method
            var shipmentMethodResult = controller.GetShipmentMethods(cart.Id) as OkNegotiatedContentResult<webModel.ShippingMethod[]>;
            var shipmentMethod = shipmentMethodResult.Content.FirstOrDefault();
            var shipment = new webModel.Shipment
            {
                DeliveryAddress = deliveryAddress,
                Currency = shipmentMethod.Currency,
                ShipmentMethodCode = shipmentMethod.ShipmentMethodCode,
                ShippingPrice = shipmentMethod.Price
            };
            cart.Shipments.Add(shipment);

            //Save changes
            controller.Update(cart);
            result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            cart = result.Content;

            //Select payment method

            var paymentMethodResults = controller.GetPaymentMethods(cart.Id) as OkNegotiatedContentResult<webModel.PaymentMethod[]>;
            var paymentMethod = paymentMethodResults.Content.FirstOrDefault();

            //Enter billing address
            var billingAddress = new webModel.Address
            {
                Type = AddressType.Billing,
                City = "london",
                Phone = "+68787687",
                PostalCode = "2222",
                CountryCode = "ENG",
                CountryName = "England",
                Email = "user@mail.com",
                FirstName = "first name",
                LastName = "last name",
                Line1 = "line 1",
                Organization = "org1"
            };

            var payment = new webModel.Payment
            {
                PaymentGatewayCode = paymentMethod.GatewayCode,
                BillingAddress = billingAddress,
                Currency = cart.Currency,
                Amount = cart.Total
            };
            cart.Payments.Add(payment);
            //Save changes
            controller.Update(cart);
            result = controller.GetCurrentCart("testSite", null) as OkNegotiatedContentResult<webModel.ShoppingCart>;
            cart = result.Content;

            //Next it call customer order method create order form cart
        }

        //[TestMethod]
        //public void SearchCarts()
        //{
        //	var controller = GetCartController();
        //	var result = controller.SearchCarts(new webModel.SearchCriteria { SiteId = "test" }) as OkNegotiatedContentResult<webModel.SearchResult>;
        //	Assert.IsNotNull(result.Content);
        //}

        private static CartModuleController GetCartController()
        {
            Func<ICartRepository> repositoryFactory = () =>
            {
                return new CartRepositoryImpl("VirtoCommerce", new AuditableInterceptor(null), new EntityPrimaryKeyGeneratorInterceptor());
            };
            //Business logic for core model


            var cartService = new ShoppingCartServiceImpl(repositoryFactory, new Mock<IEventPublisher<CartChangeEvent>>().Object, new Mock<IItemService>().Object, new Mock<IDynamicPropertyService>().Object);
            var searchService = new ShoppingCartSearchServiceImpl(repositoryFactory);
            //var memoryPaymentGatewayManager = new InMemoryPaymentGatewayManagerImpl();


            var controller = new CartModuleController(cartService, searchService, new Mock<IStoreService>().Object);
            return controller;
        }
    }
}
