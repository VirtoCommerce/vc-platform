using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using AvaTax.TaxModule.Web.Controller;
using AvaTax.TaxModule.Web.Services;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

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
    public class TaxModuleTest
    {
        private readonly AvaTaxController _controller;

        public TaxModuleTest()
        {
            _controller = GetTaxController();
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
            
            const string _usernamePropertyName = "Avalara.Tax.Credentials.AccountNumber";
            const string _passwordPropertyName = "Avalara.Tax.Credentials.LicenseKey";
            const string _serviceUrlPropertyName = "Avalara.Tax.Credentials.ServiceUrl";
            const string _companyCodePropertyName = "Avalara.Tax.Credentials.CompanyCode";
            const string _isEnabledPropertyName = "Avalara.Tax.IsEnabled";
            const string _isValidateAddressPropertyName = "Avalara.Tax.IsValidateAddress";

            var settings = new List<SettingEntry>
            {
                new SettingEntry
                {
                    Value = avalaraUsername,
                    Name = _usernamePropertyName,
                    ValueType = SettingValueType.ShortText
                },
                new SettingEntry
                {
                    Value = avalaraPassword,
                    Name = _passwordPropertyName,
                    ValueType = SettingValueType.ShortText
                },
                new SettingEntry
                {
                    Value = avalaraServiceUrl,
                    Name = _serviceUrlPropertyName,
                    ValueType = SettingValueType.ShortText
                },
                new SettingEntry
                {
                    Value = avalaraCompanyCode,
                    Name = _companyCodePropertyName,
                    ValueType = SettingValueType.ShortText
                },
                new SettingEntry { Value = "True", Name = _isEnabledPropertyName, ValueType = SettingValueType.Boolean }
            };

            var settingsManager = new Moq.Mock<ISettingsManager>();

            settingsManager.Setup(manager => manager.GetValue(_usernamePropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _usernamePropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_passwordPropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _passwordPropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_serviceUrlPropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _serviceUrlPropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_companyCodePropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _companyCodePropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_isEnabledPropertyName, true)).Returns(() => true);
            settingsManager.Setup(manager => manager.GetValue(_isValidateAddressPropertyName, true)).Returns(() => true);
            
            var avalaraTax = new AvaTaxSettings(_usernamePropertyName, _passwordPropertyName, _serviceUrlPropertyName, _companyCodePropertyName, _isEnabledPropertyName, _isValidateAddressPropertyName, settingsManager.Object);

            var controller = new AvaTaxController(avalaraTax);
            return controller;
        }
    }
}
