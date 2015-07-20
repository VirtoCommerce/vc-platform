using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Xml;
using System.Xml.Schema;
using Moq;
using Shipstation.FulfillmentModule.Web.Helpers;
using Shipstation.FulfillmentModule.Web.Models.Order;
using Shipstation.FulfillmentModule.Web.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;
using Shipstation.FulfillmentModule.Web.Controllers;

namespace Shipstation.FulfillmentModule.Test
{
    public class ShipstationTest
    {
        private readonly ShipstationController _controller;

        public ShipstationTest()
        {
            _controller = GetShipstationController();
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
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                Addresses = new[]
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


        string Serialize<T>(MediaTypeFormatter formatter, T value)
        {
            // Create a dummy HTTP Content.
            Stream stream = new MemoryStream();
            var content = new StreamContent(stream);
            /// Serialize the object.
            formatter.WriteToStreamAsync(typeof(T), value, stream, content, null).Wait();
            // Read the serialized string.
            stream.Position = 0;
            return content.ReadAsStringAsync().Result;
        }

        T Deserialize<T>(MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }

        [Fact]
        public void TestApiExport()
        {
            const string dateFormat = "{0:MM'/'dd'/'yyyy  HH:mm:ss tt}";

            var retVal = _controller.GetNewOrders("export",
                String.Format(dateFormat, DateTime.UtcNow.AddDays(-1)),
                String.Format(dateFormat, DateTime.UtcNow), 1);
        }


        //[Fact]
        public void TestSerialization()
        {
            const string dateFormat = "{0:MM'/'dd'/'yyyy  HH:mm:ss tt}";

            var billAddress = new OrdersOrderCustomerBillTo
            {
                Company = "Home",
                Email = "test@email.com",
                Name = "Test Person"
            };

            var shipAddress = new OrdersOrderCustomerShipTo
            {
                Address1 = "45 Fremont street, 2",
                City = "Los Angeles",
                Company = "Home",
                Country = "US",
                Name = "Test Person",
                PostalCode = "91311",
                State = "California"
            };

            var customer = new OrdersOrderCustomer
            {
                CustomerCode = "testCustomer",
                BillTo = billAddress,
                ShipTo = shipAddress
            };

            var order = new OrdersOrder
            {
                OrderID = "1234567890",
                OrderNumber = "CU123456789",
                OrderDate = String.Format(dateFormat, DateTime.UtcNow),
                LastModified = String.Format(dateFormat, DateTime.UtcNow),
                OrderStatus = OrderStatusEnum.AwaitingShipment.ToString(),
                Customer = customer,
                OrderTotal = (float)111.2,
                TaxAmount = (float)11.2,
                ShippingAmount = (float)3.02,
                ShippingMethod = "USPS"
            };


            var value = new Orders
            {
                Order = new[] { order },
                pages = 5,
                pagesSpecified = true
            };

            var xml = new XmlMediaTypeFormatter { UseXmlSerializer = true }; ;
            xml.WriterSettings.OmitXmlDeclaration = true;
            
            var str = Serialize(xml, value);
            //IsValidXml(str);
            //var json = new JsonMediaTypeFormatter();
            //str = Serialize(json, value);

            // Round trip
            //var orders = Deserialize<Order>(xml, str);
        }

        private static void IsValidXml(string xml)
        {
            var ordersSettings = new XmlReaderSettings();
            var schemas = new XmlSchemaSet();
            schemas.Add("http://example.com/XMLSchema/1.0", XmlReader.Create(new StringReader(OrderXsdSchema.orderSchema)));
            ordersSettings.Schemas.Add(schemas);
            ordersSettings.ValidationType = ValidationType.Schema;
            var validationHandler = new ValidationEventHandler(ordersSettingsValidationEventHandler);

            var xmlReader = XmlReader.Create(new StringReader(xml));
            var orders = XmlReader.Create(xmlReader, ordersSettings);
            var document = new XmlDocument();
            document.Load(orders);
            document.Validate(validationHandler);
        }

        static void ordersSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    Console.Write("WARNING: ");
                    Console.WriteLine(e.Message);
                    break;
                case XmlSeverityType.Error:
                    Console.Write("ERROR: ");
                    Console.WriteLine(e.Message);
                    break;
            }
        }

        private static ShipstationController GetShipstationController()
        {
            var avalaraUsername = "1100165101";
            var avalaraPassword = "AE5F97FA88A8D87D";
            var avalaraServiceUrl = "https://development.avalara.net";

            const string _usernamePropertyName = "Avalara.Tax.Credentials.AccountNumber";
            const string _passwordPropertyName = "Avalara.Tax.Credentials.LicenseKey";
            const string _serviceUrlPropertyName = "Avalara.Tax.Credentials.ServiceUrl";

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
                }
                               
            };

            var settingsManager = new Moq.Mock<ISettingsManager>();

            settingsManager.Setup(manager => manager.GetValue(_usernamePropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _usernamePropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_passwordPropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _passwordPropertyName).Value);
            settingsManager.Setup(manager => manager.GetValue(_serviceUrlPropertyName, string.Empty)).Returns(() => settings.First(x => x.Name == _serviceUrlPropertyName).Value);
            
            var shipstationSettings = new ShipstationSettings(_usernamePropertyName, _passwordPropertyName, _serviceUrlPropertyName, settingsManager.Object);

            var orderSearchService = new Mock<ICustomerOrderSearchService>();
            orderSearchService.Setup(service => service.Search(It.IsAny<SearchCriteria>()))
                .Returns(() => new SearchResult { CustomerOrders = new List<CustomerOrder> { GetTestOrder("123") } });
            var orderService = new Mock<ICustomerOrderService>();

            var controller = new ShipstationController(shipstationSettings, orderService.Object, orderSearchService.Object);
            return controller;
        }
    }
}
