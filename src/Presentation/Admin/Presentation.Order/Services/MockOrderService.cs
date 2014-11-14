using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Search;
using VirtoCommerce.Foundation.Search;
using OrderModel = VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Services
{
    public class MockSearchResults : SearchResults
    {
        int _DocCount = 20;
        public override int DocCount
        {
            get
            {
                return _DocCount;
            }
        }

        int _TotalCount = 20;
        public override int TotalCount
        {
            get
            {
                return _TotalCount;
            }
        }

        public MockSearchResults(ISearchCriteria criteria, int docCount, int totalCount)
            : base(criteria, null)
        {
            _TotalCount = totalCount;
        }
    }

    public class MockOrderService : IOrderRepository, ICountryRepository, IPaymentMethodRepository, IShippingRepository, ITaxRepository
    {
        private List<OrderModel.Order> MockOrderList;
        private List<Payment> MockPaymentList;
        private List<ShippingOption> MockShippingOptionList;
        private List<PaymentGateway> MockPaymentGatewayList;        
        private List<PaymentMethod> MockPaymentMethodList;
        private List<Country> MockCountryList;


        private void PopulateTestOrder()
        {
            if (MockOrderList == null)
            {
                MockOrderList = new List<OrderModel.Order>();
                MockPaymentList = new List<Payment>();
                MockShippingOptionList = new List<ShippingOption>();
                MockPaymentGatewayList = new List<PaymentGateway>();
                MockPaymentMethodList = new List<PaymentMethod>();
                MockCountryList = new List<Country>();

                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "1", TrackingNumber = "PO32313", Status = "OnHold", Total = 123.43m, CustomerId = "1" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "2", TrackingNumber = "PO62316", Status = "Completed", Total = 444.12m, CustomerId = "2" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "3", TrackingNumber = "PO75423", Status = "OnHold", Total = 765.32m, CustomerId = "3" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "4", TrackingNumber = "PO98743", Status = "Completed", Total = 775.22m, CustomerId = "4" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "5", TrackingNumber = "PO36572", Status = "InProgress", Total = 66.43m, CustomerId = "5" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "6", TrackingNumber = "PO65432", Status = "InProgress", Total = 632.12m, CustomerId = "6" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "7", TrackingNumber = "PO97898", Status = "OnHold", Total = 642.21m, CustomerId = "7" });

                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "10", TrackingNumber = "POS2313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "20", TrackingNumber = "POS2316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "30", TrackingNumber = "POS5423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "40", TrackingNumber = "POS8743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "50", TrackingNumber = "POS6572", Status = "InProgress", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "60", TrackingNumber = "POS5432", Status = "InProgress", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "70", TrackingNumber = "POS7898", Total = 642.21m, CustomerId = "17" });

                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "11", TrackingNumber = "PO62313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "12", TrackingNumber = "PO92316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "13", TrackingNumber = "PO25423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "14", TrackingNumber = "PO98743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "15", TrackingNumber = "PO66572", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "16", TrackingNumber = "PO35432", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "US Store", OrderGroupId = "17", TrackingNumber = "PO07898", Total = 642.21m, CustomerId = "17" });

                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "11", TrackingNumber = "PSS2313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "21", TrackingNumber = "PSS2316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "31", TrackingNumber = "PSS5423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "41", TrackingNumber = "PSS8743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "51", TrackingNumber = "PSS6572", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "61", TrackingNumber = "PSS5432", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "71", TrackingNumber = "PSS7898", Total = 642.21m, CustomerId = "17" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "21", TrackingNumber = "PS62313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "22", TrackingNumber = "PS92316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "23", TrackingNumber = "PS25423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "24", TrackingNumber = "PS98743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "25", TrackingNumber = "PS66572", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "26", TrackingNumber = "PS35432", Status = "Completed", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "27", TrackingNumber = "PS07898", Total = 642.21m, CustomerId = "17" });

                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "12", TrackingNumber = "PSO2313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "22", TrackingNumber = "PSO2316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "32", TrackingNumber = "PSO5423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "42", TrackingNumber = "PSO8743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "52", TrackingNumber = "PSO6572", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "62", TrackingNumber = "PSO5432", Status = "AwaitingExchange", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "72", TrackingNumber = "PSO7898", Total = 642.21m, CustomerId = "17" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "32", TrackingNumber = "PSO2313", Total = 123.43m, CustomerId = "11" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "32", TrackingNumber = "PSO2316", Status = "Completed", Total = 444.12m, CustomerId = "12" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "33", TrackingNumber = "PSO5423", Total = 765.32m, CustomerId = "13" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "34", TrackingNumber = "PSO8743", Status = "Completed", Total = 775.22m, CustomerId = "14" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "35", TrackingNumber = "PSO6572", Total = 66.43m, CustomerId = "15" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "36", TrackingNumber = "PSO5432", Status = "AwaitingExchange", Total = 632.12m, CustomerId = "16" });
                MockOrderList.Add(new OrderModel.Order { BillingCurrency = "USD", StoreId = "UK Store", OrderGroupId = "37", TrackingNumber = "PSO7898", Total = 642.21m, CustomerId = "17" });

                foreach (var order in MockOrderList)
                {
                    var payments = new Payment[] {
                        new CreditCardPayment() { PaymentType = PaymentType.CreditCard.GetHashCode(), PaymentMethodName="MasterCard", ValidationCode="RE21321-21", Amount=32.53m, Status="Processing", TransactionType=TransactionType.Credit.ToString() },
                        new CashCardPayment() { PaymentType = PaymentType.CashCard.GetHashCode(), PaymentMethodName="Visa", ValidationCode="RE6211-44", Amount=55.73m, Status="Processing", TransactionType=TransactionType.Credit.ToString() },
                        new InvoicePayment() { PaymentType = PaymentType.Invoice.GetHashCode(), PaymentMethodName="Bank transaction", ValidationCode="BE3-21", Amount=774.53m, Status="Confirmed", TransactionType=TransactionType.Authorization.ToString() }
													   };
                    var orderAddresses = new OrderAddress[] { 
						new OrderAddress() { OrderAddressId="1", City = "New Yourk", CountryCode="us", CountryName="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user@mail.com", Line1="str. 113", Line2="bld. 21", PostalCode="323232", StateProvince="WC" },
						new OrderAddress() { OrderAddressId="2", City = "Los Angeles", CountryCode="us", CountryName="USA", DaytimePhoneNumber="+7 (906) 4444-444", Email="user2@mail.com", Line1="av. 32", Line2="bld. 1", PostalCode="432142", StateProvince="LA" },
                        new OrderAddress() { OrderAddressId="3", City = "Yourk", CountryCode="us", CountryName="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user3@mail.com", Line1="str. 113", Line2="Pas Juozapa", PostalCode="12100" },
						new OrderAddress() { OrderAddressId="4", City = "Vilnius", CountryCode="lt", CountryName="Lithuania", DaytimePhoneNumber="+370 5 2744-444", Line1="Laisves pr. 125", PostalCode="12100" },
                        new OrderAddress() { OrderAddressId="5", City = "Yourk", CountryCode="us", CountryName="USA", DaytimePhoneNumber="+7 (906) 2121-321", Email="user4@mail.com", Line1="str. 113", Line2="Pas Juozapa", PostalCode="12100" },
						new OrderAddress() { OrderAddressId="6", City = "Vilnius", CountryCode="lt", CountryName="Lithuania", DaytimePhoneNumber="+370 5 2744-444", Line1="Laisves pr. 125", PostalCode="54821" }};

                    var lineItems = new LineItem[] {
                        new LineItem() { LineItemId = "1", DisplayName = "Chair black", Description="some chair description", Quantity = 3, ListPrice = 32.43m, CatalogItemCode = "x-200" },
						new LineItem() { LineItemId = "2", DisplayName = "Coca-Cola", Description="some coca description", Quantity = 4, ListPrice = 3.99m, CatalogItemCode = "x-201" },
						new LineItem() { LineItemId = "3", DisplayName = "Fujifilm 121MN", Description="some fuji description", Quantity = 6, ListPrice = 89.37m, CatalogItemCode = "x-202" },
						new LineItem() { LineItemId = "4", DisplayName = "Canon M32-Z", Description="some Canon description", Quantity = 9, ListPrice = 902.94m, CatalogItemCode = "x-203" },
						new LineItem() { LineItemId = "5", DisplayName = "Sony Qybershot", Description="some Sony description", Quantity = 1, ListPrice = 320.49m, CatalogItemCode = "x-204" },
						new LineItem() { LineItemId = "6", DisplayName = "Minolta 121-43s", Description="some Minolta description", Quantity = 2, ListPrice = 324.43m, CatalogItemCode = "x-205" },
						new LineItem() { LineItemId = "7", DisplayName = "Sony-Erricson", Description="some sony description", Quantity = 3, ListPrice = 62.13m, CatalogItemCode = "x-206" },
						new LineItem() { LineItemId = "8", DisplayName = "Booty-Sony", Description="some boty description",Quantity = 1, ListPrice = 8.03m, CatalogItemCode = "x-207" } };
                    var orderForm = new OrderForm() { OrderFormId = "21", Status = "Processing" };
                    foreach (var lineItem in lineItems)
                    {
                        lineItem.ExtendedPrice = lineItem.Quantity * lineItem.ListPrice;
                        lineItem.PlacedPrice = lineItem.ListPrice;
                        orderForm.LineItems.Add(lineItem);
                    }

                    // Status = "OnHold",
                    // Status = "InventoryAssigned",
                    // Status = "Packing"
                    var shipment1 = new Shipment() { ShipmentId = "13341-23", ShippingMethodId = "Ground Shipping", ShippingMethodName = "Ground Shipping", ShippingAddressId = "1", ShipmentTotal = 213.12m, Subtotal = 119, ShippingDiscountAmount = 5.99m };
                    shipment1.ShipmentItems.Add(new ShipmentItem() { LineItemId = "1", Quantity = 3 });
                    shipment1.ShipmentItems.Add(new ShipmentItem() { LineItemId = "2", Quantity = 4 });
                    shipment1.ShipmentItems.Add(new ShipmentItem() { LineItemId = "3", Quantity = 6 });
                    shipment1.ShipmentItems.Add(new ShipmentItem() { LineItemId = "4", Quantity = 9 });
                    shipment1.ShipmentItems.Add(new ShipmentItem() { LineItemId = "5", Quantity = 1 });

                    foreach (var shipmentItem in shipment1.ShipmentItems)
                    {
                        shipmentItem.Shipment = shipment1;
                    }

                    // Status = "AwaitingInventory"
                    // Status = "InventoryAssigned"
                    // Status = "Packing"
                    var shipment2 = new Shipment() { ShipmentId = "1499-67", ShippingMethodId = "USPS", ShippingMethodName = "USPS", ShippingAddressId = "2", ShipmentTotal = 913.82m, Subtotal = 900.99m, ShippingDiscountAmount = 55.9m };
                    shipment2.ShipmentItems.Add(new ShipmentItem() { LineItemId = "6", Quantity = 2 });
                    shipment2.ShipmentItems.Add(new ShipmentItem() { LineItemId = "7", Quantity = 3 });
                    shipment2.ShipmentItems.Add(new ShipmentItem() { LineItemId = "8", Quantity = 1 });

                    foreach (var shipmentItem in shipment2.ShipmentItems)
                    {
                        shipmentItem.Shipment = shipment2;
                    }

                    orderForm.Shipments.Add(shipment1);
                    shipment1.OrderForm = orderForm;

                    orderForm.Shipments.Add(shipment2);
                    shipment2.OrderForm = orderForm;

                    order.OrderForms.Add(orderForm);
                    orderForm.OrderGroup = order;


                    foreach (var payment in payments)
                    {
                        orderForm.Payments.Add(payment);
                    }
                    MockPaymentList.AddRange(payments);

                    foreach (var orderAddress in orderAddresses)
                    {
                        order.OrderAddresses.Add(orderAddress);
                    }

                    var rmaItems = new RmaReturnItem[] { 
						new RmaReturnItem{ ItemState = "AwaitingReturn", ReturnAmount = 21.32m, ReturnReason = "Corrupt"},
						new RmaReturnItem{ ItemState = "Received", ReturnAmount = 210.67m, ReturnReason = "Other"}};
                    rmaItems[0].RmaLineItems.Add(new RmaLineItem() { LineItemId = "8", Quantity = 1 });
                    rmaItems[1].RmaLineItems.Add(new RmaLineItem() { LineItemId = "1", Quantity = 2 });

                    var rmaRequest = new RmaRequest() { RmaRequestId = "RMA-13", Status = "AwaitingCompletion", ReturnTotal = 323.21m, RefundAmount = 301.89m, ReturnAddressId = "1", Order = order };
                    foreach (var rmaItem in rmaItems)
                    {
                        rmaRequest.RmaReturnItems.Add(rmaItem);
                    }
                    order.RmaRequests.Add(rmaRequest);
                }

                // ------------ 
                MockShippingOptionList.AddRange(GetAllShippingOptions());
                MockPaymentMethodList.AddRange(GetAllPaymentMethods());
                MockCountryList.AddRange(GetAllCountries());
            }

        }

        public MockOrderService()
        {
            PopulateTestOrder();
        }

        #region IOrderService Members

        public OrderSearchResults SearchOrders(string scope, ISearchCriteria criteria)
        {
            OrderGroup[] resultItems;

            var orderCriteria = criteria as OrderSearchCriteria;

            System.Threading.Thread.Sleep(100);
            var query = MockOrderList.Select(a => a);
            //var query = from item in MockOrderList
            //            select item;

            int totalCount = MockOrderList.Count;
            if (orderCriteria != null)
            {
                if (orderCriteria.Sort != null)
                    if (orderCriteria.Sort.GetSort()[0].IsDescending)
                        query = query.OrderByDescending(x => x.Total);
                    else
                        query = query.OrderBy(x => x.Total);

                if (!string.IsNullOrEmpty(orderCriteria.OrderStatus))
                {
                    query = query.Where(x => x.Status == orderCriteria.OrderStatus);
                }
                if (!string.IsNullOrEmpty(orderCriteria.TrackingNumber))
                {
                    query = query.Where(x => x.TrackingNumber.Contains(orderCriteria.TrackingNumber));
                }
                if (!string.IsNullOrEmpty(orderCriteria.CustomerEmail))
                {
                    query = query.Where(x => (x.OrderAddresses.Select(b => b.Email.StartsWith(orderCriteria.CustomerEmail)).Count()) > 0);
                }
                if (!string.IsNullOrEmpty(orderCriteria.CustomerId))
                {
                    query = query.Where(x => x.CustomerId == orderCriteria.CustomerId);
                }
                if (!string.IsNullOrEmpty(orderCriteria.Keyword))
                {
                    query = query.Where(x => x.TrackingNumber == orderCriteria.Keyword);
                }
                totalCount = query.Count();
                query = query.Skip(criteria.StartingRecord).Take(criteria.RecordsToRetrieve);
            }

            resultItems = query.ToArray();

            MockSearchResults searchResults = new MockSearchResults(criteria, resultItems.Length, totalCount);
            var retVal = new OrderSearchResults(criteria, resultItems, searchResults);
            return retVal;
        }

        public void SaveOrderGroups(OrderGroup[] orderGroups)
        {
            foreach (var order in orderGroups)
            {
                //order.SetEntityState(VirtoCommerce.Foundation.Frameworks.StorageEntityState.Unchanged);
            }
        }

        public OrderModel.Order[] GetAllCustomerOrders(string storeId, string customerId)
        {
            return MockOrderList.Where(x => x.CustomerId == customerId).ToArray();
        }

        private ShippingOption[] GetAllShippingOptions()
        {
            var retVal = new ShippingOption { ShippingOptionId = "ShippingOptionId01" };
            retVal.ShippingMethods.Add(new ShippingMethod() { Name = "USPS", ShippingMethodId = "USPS" });
            retVal.ShippingMethods.Add(new ShippingMethod() { Name = "Ground Shipping", ShippingMethodId = "Ground Shipping" });
            return new ShippingOption[] { retVal };
        }

        private PaymentMethod[] GetAllPaymentMethods()
        {
            return new PaymentMethod[] {
                new PaymentMethod() { Name = "MasterCard"},
                new PaymentMethod() { Name = "Visa"},
                new PaymentMethod() { Name = "Bank transaction"}
            };
        }

        private Country[] GetAllCountries()
        {
            var result = new Country[] { new Country() { CountryId = "ar", DisplayName = "Armenia", Name = "Republic of Armenia" },
                                        new Country() { CountryId = "lt", DisplayName = "Lithuania", Name = "Republic of Lithuania" },
                                        new Country() { CountryId = "ru", DisplayName = "Russia", Name = "Russian Federation" },
                                        new Country() { CountryId = "us", DisplayName = "USA", Name = "United States" }};

            result[0].Regions.Add(new Region() { Name = "region 1", DisplayName = "region 1", RegionId = "1" });
            result[0].Regions.Add(new Region() { Name = "region 2", DisplayName = "region 2", RegionId = "2" });

            result[2].Regions.Add(new Region() { Name = "Moscow", DisplayName = "Moscow", RegionId = "3" });
            result[2].Regions.Add(new Region() { Name = "Kaliningrad", DisplayName = "Kaliningrad", RegionId = "4" });

            result[3].Regions.Add(new Region() { Name = "AL", DisplayName = "Alabama", RegionId = "5" });
            result[3].Regions.Add(new Region() { Name = "AK", DisplayName = "Alaska", RegionId = "6" });
            result[3].Regions.Add(new Region() { Name = "AS", DisplayName = "American Samoa", RegionId = "7" });
            result[3].Regions.Add(new Region() { Name = "AZ", DisplayName = "Arizona", RegionId = "8" });

            return result;
        }

        public void SaveCountries(Country[] countries)
        {
            throw new NotImplementedException();
        }

        public ShippingRate[] GetShippingRates(string[] shippingMethods, LineItem[] items)
        {
            throw new NotImplementedException();
        }

        //public VirtoCommerce.Foundation.Orders.OrderQueryResults QueryOrders(VirtoCommerce.Foundation.Orders.OrderQueryCriteria criteria)
        //{
        //    OrderGroup[] resultItems;

        //    var orderCriteria = criteria as OrderQueryCriteria;

        //    System.Threading.Thread.Sleep(100);
        //    var query = MockOrderList.Select(a => a);
        //    //var query = from item in MockOrderList
        //    //            select item;
        //    var _custService = new MockCustomerService();

        //    var customers = _custService.GetAllUsers();

        //    var list = from ord in query
        //               join cust in customers
        //               on ord.CustomerId equals cust.MemberId
        //               select new
        //               {
        //                   ID = cust.MemberId,
        //                   Email = cust.Profile.Email,
        //                   FirstName = cust.Profile.FirstName,
        //                   LastName = cust.Profile.LastName,
        //                   FullName = cust.Profile.FullName
        //               };

        //    int totalCount = MockOrderList.Count;
        //    if (orderCriteria != null)
        //    {

        //        if (!string.IsNullOrEmpty(orderCriteria.Status))
        //        {
        //            query = query.Where(x => x.Status == orderCriteria.Status);
        //        }
        //        if (!string.IsNullOrEmpty(orderCriteria.TrackingNumber))
        //        {
        //            query = query.Where(x => x.TrackingNumber.ToUpperInvariant().StartsWith(orderCriteria.TrackingNumber.ToUpperInvariant()));
        //        }
        //        if (!string.IsNullOrEmpty(orderCriteria.CustomerEmail))
        //        {
        //            var _customers = list.Where(y => y.Email.ToUpperInvariant().StartsWith(orderCriteria.CustomerEmail.ToUpperInvariant()));
        //            if (_customers.Count() > 0)
        //            {
        //                foreach (var cust in _customers)
        //                {
        //                    query = query.Where(x => x.CustomerId.Contains(cust.ID));
        //                }
        //            }
        //            else
        //                query = Enumerable.Empty<OrderExt>();
        //        }
        //        if (!string.IsNullOrEmpty(orderCriteria.CustomerId))
        //        {
        //            query = query.Where(x => x.CustomerId == orderCriteria.CustomerId);
        //        }
        //        if (!string.IsNullOrEmpty(orderCriteria.Keyword))
        //        {
        //            var _customers = list.Where(y => y.FirstName.ToUpperInvariant().StartsWith(orderCriteria.Keyword.ToUpperInvariant()) || y.LastName.ToUpperInvariant().StartsWith(orderCriteria.Keyword.ToUpperInvariant()) || y.FullName.ToUpperInvariant().Contains(orderCriteria.Keyword.ToUpperInvariant()));
        //            if (_customers.Count() > 0)
        //            {
        //                foreach (var cust in _customers)
        //                {
        //                    query = query.Where(x => x.CustomerId.Contains(cust.ID));
        //                }
        //            }
        //            else
        //                query = Enumerable.Empty<OrderExt>();
        //        }

        //        totalCount = query.Count();

        //        query = query.Skip(criteria.StartingRecord).Take(criteria.RecordsToRetrieve);
        //    }

        //    resultItems = query.ToArray<OrderExt>();

        //    var searchResults = new OrderQueryResults(criteria, resultItems, totalCount);
        //    return searchResults;
        //}

        #endregion

        #region IOrderRepository
        public IQueryable<OrderModel.Order> Orders
        {
            get { return MockOrderList.AsQueryable(); }
        }

        public IQueryable<ShoppingCart> ShoppingCarts
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Shipment> Shipments
        {
            get { throw new NotImplementedException(); }
        }

		public IQueryable<RmaRequest> RmaRequests
		{
			get { throw new NotImplementedException(); }
		}

        public IQueryable<LineItem> LineItems
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<OrderAddress> OrderAddresses
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Payment> Payments
        {
            get { return MockPaymentList.AsQueryable(); }
        }

        public IQueryable<Jurisdiction> Jurisdictions
        {
            get { return GetAsQueryable<Jurisdiction>(); }
        }

        public IQueryable<JurisdictionGroup> JurisdictionGroups
        {
            get { return GetAsQueryable<JurisdictionGroup>(); }
        }
        #endregion


        #region IRepository
        MockUnitOfWork MockUnitOfWorkItem = new MockUnitOfWork();
        public IUnitOfWork UnitOfWork
        {
            get { return MockUnitOfWorkItem; }
        }

        public void Attach<T>(T item) where T : class
        {
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
        }

        public void Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Refresh(IEnumerable collection)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICountryRepository
        public IQueryable<Country> Countries
        {
            get { return MockCountryList.AsQueryable(); }
        }
        #endregion

        #region IPaymentMethodRepository
        public IQueryable<PaymentMethod> PaymentMethods
        {
            get { return MockPaymentMethodList.AsQueryable(); }
        }

        public IQueryable<PaymentGateway> PaymentGateways
        {
            get { return MockPaymentGatewayList.AsQueryable(); }
        }

        public IQueryable<PaymentMethodShippingMethod> PaymentMethodShippingMethods
        {
            get { return null; }
        }

        public IQueryable<PaymentMethodLanguage> PaymentMethodLanguages
        {
            get { return null; }
        }

        public IQueryable<PaymentMethodPropertyValue> PaymentPropertyValues
        {
            get { return null; }
        }

        #endregion

        #region IShippingRepository
        public IQueryable<ShippingOption> ShippingOptions
        {
            get { return MockShippingOptionList.AsQueryable(); }
        }

		public IQueryable<ShippingGateway> ShippingGateways
		{
			get { throw new NotImplementedException(); }
		}

        public IQueryable<ShippingMethod> ShippingMethods
        {
            get { return null; }
        }

        public IQueryable<ShippingMethodLanguage> ShippingMethodLanguages
        {
            get { return null; }
        }

        public IQueryable<ShippingMethodJurisdictionGroup> ShippingMethodJurisdictionGroups
        {
            get { return null; }
        }

        public IQueryable<ShippingPackage> ShippingPackages
        {
            get { return null; }
        }
        #endregion

        #region ITaxRepository
        public IQueryable<Tax> Taxes
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
	}

    public class MockUnitOfWork : IUnitOfWork
    {
        public int Commit()
        {
            return 0;
        }

        public void CommitAndRefreshChanges()
        {
        }

        public void RollbackChanges()
        {
        }
    }
}
