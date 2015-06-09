using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using Address = VirtoCommerce.Domain.Order.Model.Address;
using AddressType = VirtoCommerce.Domain.Order.Model.AddressType;
using Coupon = VirtoCommerce.Domain.Order.Model.Coupon;
using Discount = VirtoCommerce.Domain.Order.Model.Discount;
using LineItem = VirtoCommerce.Domain.Order.Model.LineItem;
using Shipment = VirtoCommerce.Domain.Order.Model.Shipment;

using CartAddressType = VirtoCommerce.Domain.Cart.Model.AddressType;
using CartAddress = VirtoCommerce.Domain.Cart.Model.Address;
using CartCoupon = VirtoCommerce.Domain.Cart.Model.Coupon;
using CartDiscount = VirtoCommerce.Domain.Cart.Model.Discount;
using CartLineItem = VirtoCommerce.Domain.Cart.Model.LineItem;
using CartShipment = VirtoCommerce.Domain.Cart.Model.Shipment;
using CartPayment = VirtoCommerce.Domain.Cart.Model.Payment;

namespace AvaTax.TaxModule.Test
{
    [TestClass]
    public class TaxModuleTest
    {
        private AvaTaxController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _controller = GetTaxController();
        }

        [TestMethod]
        public void GetOrderTaxTotals()
        {
            var testOrder = GetTestOrder("order1");
            var result = _controller.Total(testOrder) as OkNegotiatedContentResult<CustomerOrder>;
            Assert.IsNotNull(result.Content);
            Assert.AreNotEqual(result.Content.Tax, 0);
        }

        [TestMethod]
        public void GetCartTaxTotals()
        {
            var testCart = GetTestCart("cart1");
            var result = _controller.CartTotal(testCart) as OkNegotiatedContentResult<ShoppingCart>;
            Assert.IsNotNull(result.Content);
            Assert.AreNotEqual(result.Content.TaxTotal, 0);
        }

        private static CustomerOrder GetTestOrder(string id)
        {
            var order = new CustomerOrder
            {
                Id = id,
                Currency = CurrencyCodes.USD,
                CustomerId = "Test Customer",
                EmployeeId = "employee",
                StoreId = "test store",
                CreatedDate = DateTime.Now,
                Addresses = new []
				{
					new Address {	
					AddressType = AddressType.Shipping, 
					Phone = "+68787687",
					PostalCode = "60602",
					CountryCode = "US",
					CountryName = "United states",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "45 Fremont Street",
                    City = "Los Angeles",
                    RegionId = "CA",
					Organization = "org1"
					}
				}.ToList(),
                Discount = new Discount
                {
                    PromotionId = "testPromotion",
                    Currency = CurrencyCodes.USD,
                    DiscountAmount = 12,
                    Coupon = new Coupon
                    {
                        Code = "ssss"
                    }
                }
            };
            var item1 = new LineItem
            {
                BasePrice = 10,
                Price = 20,
                ProductId = "shoes",
                CatalogId = "catalog",
                Currency = CurrencyCodes.USD,
                CategoryId = "category",
                Name = "shoes",
                Quantity = 2,
                ShippingMethodCode = "EMS",
                Discount = new Discount
                {
                    PromotionId = "itemPromotion",
                    Currency = CurrencyCodes.USD,
                    DiscountAmount = 12,
                    Coupon = new Coupon
                    {
                        Code = "ssss"
                    }
                }
            };
            var item2 = new LineItem
            {
                BasePrice = 100,
                Price = 100,
                ProductId = "t-shirt",
                CatalogId = "catalog",
                CategoryId = "category",
                Currency = CurrencyCodes.USD,
                Name = "t-shirt",
                Quantity = 2,
                ShippingMethodCode = "EMS",
                Discount = new Discount
                {
                    PromotionId = "testPromotion",
                    Currency = CurrencyCodes.USD,
                    DiscountAmount = 12,
                    Coupon = new Coupon
                    {
                        Code = "ssss"
                    }
                }
            };
            order.Items = new List<LineItem>();
            order.Items.Add(item1);
            order.Items.Add(item2);

            var shipment = new Shipment
            {
                Currency = CurrencyCodes.USD,
                DeliveryAddress = new Address
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
                Discount = new Discount
                {
                    PromotionId = "testPromotion",
                    Currency = CurrencyCodes.USD,
                    DiscountAmount = 12,
                    Coupon = new Coupon
                    {
                        Code = "ssss"
                    }
                }
            };
            order.Shipments = new List<Shipment>();
            order.Shipments.Add(shipment);

            var payment = new PaymentIn
            {
                GatewayCode = "PayPal",
                Currency = CurrencyCodes.USD,
                Sum = 10,
                CustomerId = "et"
            };
            order.InPayments = new List<PaymentIn> { payment };

            return order;
        }

        private static ShoppingCart GetTestCart(string id)
        {
            var cart = new ShoppingCart
            {
                Id = id,
                Currency = CurrencyCodes.USD,
                CustomerId = "Test Customer",
                StoreId = "test store",
                CreatedDate = DateTime.Now,
                Addresses = new[]
				{
					new CartAddress {	
					AddressType = CartAddressType.Shipping, 
					Phone = "+68787687",
					PostalCode = "60602",
					CountryCode = "US",
					CountryName = "United states",
					Email = "user@mail.com",
					FirstName = "first name",
					LastName = "last name",
					Line1 = "45 Fremont Street",
                    City = "Los Angeles",
                    RegionId = "CA",
					Organization = "org1"
					}
				}.ToList(),
                Discounts = new[] { new CartDiscount
                    {
                        PromotionId = "testPromotion",
                        Currency = CurrencyCodes.USD,
                        DiscountAmount = 12,
                        
                    }
                },
                Coupon = new CartCoupon
                {
                    CouponCode = "ssss"
                }
                
            };
            var item1 = new CartLineItem
            {
                ListPrice = 10,
                ExtendedPrice = 20,
                ProductId = "shoes",
                CatalogId = "catalog",
                Currency = CurrencyCodes.USD,
                CategoryId = "category",
                Name = "shoes",
                Quantity = 2,
                ShipmentMethodCode = "EMS",
                Discounts = new[] { 
                    new CartDiscount
                    {
                        PromotionId = "itemPromotion",
                        Currency = CurrencyCodes.USD,
                        DiscountAmount = 12
                        
                    }
                }
            };
            var item2 = new CartLineItem
            {
                ListPrice = 100,
                ExtendedPrice = 200,
                ProductId = "t-shirt",
                CatalogId = "catalog",
                CategoryId = "category",
                Currency = CurrencyCodes.USD,
                Name = "t-shirt",
                Quantity = 2,
                ShipmentMethodCode = "EMS",
                Discounts = new[]{ 
                    new CartDiscount
                    {
                        PromotionId = "testPromotion",
                        Currency = CurrencyCodes.USD,
                        DiscountAmount = 12
                    }
                }
            };
            cart.Items = new List<CartLineItem> { item1, item2 };

            var shipment = new CartShipment
            {
                Currency = CurrencyCodes.USD,
                DeliveryAddress = new CartAddress
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
                Discounts = new[] {
                    new CartDiscount
                    {
                        PromotionId = "testPromotion",
                        Currency = CurrencyCodes.USD,
                        DiscountAmount = 12,
                        
                    }
                }
            };
            cart.Shipments = new List<CartShipment> { shipment };

            var payment = new CartPayment
            {
                PaymentGatewayCode = "PayPal",
                Currency = CurrencyCodes.USD,
                Amount = 10,
                OuterId = "et"
            };
            cart.Payments = new List<CartPayment> { payment };

            return cart;
        }

        private static AvaTaxController GetTaxController()
        {
            var avalaraUsername = "1100165101";
            var avalaraPassword = "AE5F97FA88A8D87D";
            var avalaraServiceUrl = "https://development.avalara.net";
            var avalaraCompanyCode = "APITrialCompany";

            var avalaraCode = "";
            var avalaraDescription = "";
            var avalaraLogoUrl = "";


            var avalaraTax = new AvaTaxImpl(avalaraUsername, avalaraPassword, avalaraServiceUrl, avalaraCompanyCode, avalaraCode, avalaraDescription, avalaraLogoUrl);

            var controller = new AvaTaxController(avalaraTax);
            return controller;
        }
    }
}
