using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

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
        public void GetTaxTotals()
        {
            var testOrder = GetTestOrder("order2");
            var result = _controller.Total(testOrder) as OkNegotiatedContentResult<CustomerOrder>;
            Assert.IsNotNull(result.Content);
            Assert.AreNotEqual(result.Content.Tax, 0);
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
            order.InPayments = new List<PaymentIn>();
            order.InPayments.Add(payment);

            return order;
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
