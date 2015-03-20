//using FunctionalTests.Orders.Helpers;
//using System;
//using System.Linq;
//using VirtoCommerce.Foundation.Data.Orders;
//using VirtoCommerce.Foundation.Frameworks.Extensions;
//using VirtoCommerce.Foundation.Orders.Factories;
//using VirtoCommerce.Foundation.Orders.Model;
//using VirtoCommerce.Foundation.Orders.Repositories;
//using Xunit;

//namespace CommerceFoundation.UnitTests.Orders
//{
//    public class OrderDataService : OrderTestBase
//    {
//        /// <summary>
//        /// solving DSOrderClient data loading issue. Error on getting Order: The context is already tracking a different entity with the same resource Uri.
//        /// </summary>
//        /*[Fact]
//        public void Can_load_any_order()
//        {
//            test moved to OrderScenarios.cs as the DSOrderClient failed to load orders in this file :/
//        }
//        */
//        [Fact]
//        public void Create_order_with_return()
//        {
//            var order = CreateOrder();
//            var client = GetClient();
//            client.Add(order);
//            client.UnitOfWork.Commit();
//            client = GetClient();
//            var item = client.Orders.Where(x => x.OrderGroupId == order.OrderGroupId).FirstOrDefault();
//            Assert.NotNull(item);
//        }

//        /// <summary>
//        /// Updates the order: adds a return.
//        /// </summary>
//        /*[Fact]
//        public void Can_update_order_add_return()
//        { 
//            test moved to OrderScenarios.cs as the DSOrderClient failed to load orders in this file :/
//        }
//         */

//        [Fact]
//        public void Can_update_order_expanded()
//        {
//            var client = GetClient();
//            var order = client.Orders.ExpandAll()
//                    .Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem")
//                    .Expand("RmaRequests/Order")
//                    .Expand("OrderForms/Shipments/ShipmentItems/LineItem")
//                    .Expand("OrderForms/Shipments/ShipmentItems/Shipment/OrderForm/OrderGroup")
//                    .Take(1).First();

//            order.Status = "InProgress";
//            order.Status = "OnHold";

//            client.UnitOfWork.Commit();
//            client = GetClient();
//            var item = client.Orders.ExpandAll().Where(x => x.OrderGroupId == order.OrderGroupId).FirstOrDefault();
//            Assert.NotNull(item);
//            Assert.True(item.Status == "OnHold");
//        }

//        private IOrderRepository GetClient()
//        {
//            //var client = this.OrderRepository;
//            var ServiceUri = new Uri("http://localhost/store/virto/DataServices/OrderDataService.svc");
//            var client = new DSOrderClient(ServiceUri, new OrderEntityFactory(), null);
//            // var client = new EFOrderRepository();

//            return client;
//        }

//        private Order CreateOrder(int items = 2)
//        {
//            var customerId = "3a6e29a3-d0c9-4a9b-8207-faf957015c60";
//            var builder = TestOrderBuilder.BuildOrder();
//            builder.WithAddresess()
//                   .WithPayments()
//                   .WithShipment()
//                   .WithLineItemsCount(items)
//                   .WithStatus("InProgress")
//                   .WithCustomer(customerId)
//                   .WithOrderFormDiscounts(1)
//                   .WithLineItemDiscounts(2)
//                   .WithReturns();
//            var order = builder.GetOrder();

//            //order.StoreId = "SampleStore";
//            order.StoreId = "-noStore";
//            return order;
//        }


//    }
//}
