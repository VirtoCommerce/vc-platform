using System.Linq;
using System.Threading;
using FunctionalTests.Orders.Helpers;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using Xunit;

namespace FunctionalTests.Orders
{
	[Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class OrderScenarios : OrderTestBase
	{
		[RepositoryTheory]
		public void Can_repository_track_collection_changes()
		{
			var order = TestOrderBuilder.BuildOrder()
				.WithLineItemsConstant()
				.WithReturns()
				.GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			order = repository.Orders.ExpandAll()
				.Where(x => x.RmaRequests.Any())
				.FirstOrDefault();

			Assert.NotNull(order);
			Assert.NotNull(order.RmaRequests[0]);

			order.RmaRequests[0].Status = "Completed";
			order.RmaRequests[0].Status = "Canceled";

			repository.UnitOfWork.Commit();

			//RefreshRepository(ref repository);
			order = repository.Orders.ExpandAll().Where(x => x.OrderGroupId == order.OrderGroupId).SingleOrDefault();

			Assert.NotNull(order);
			Assert.NotNull(order.RmaRequests[0]);
			Assert.Equal("Canceled", order.RmaRequests[0].Status);
		}

		//[RepositoryTheory(Skip = "Cannot insert the value NULL into column 'LineItemId', table 'OrdersTest.dbo.ShipmentItem'")]
		[RepositoryTheory] // fixed be setting LineItemId explicitly
		public void Can_add_shipmentitem()
		{
			var order = TestOrderBuilder.BuildOrder()
				.WithAddresess()
				.WithShipment()
				.GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);
			order = repository.Orders
				.ExpandAll()
				.Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem")
				.Expand("RmaRequests/Order")
				.Expand("OrderForms/Shipments/OrderForm/OrderGroup")
				.Expand("OrderForms/Shipments/ShipmentItems/LineItem")
				.Expand("OrderForms/Shipments/ShipmentItems/Shipment/OrderForm/OrderGroup")
				.FirstOrDefault();

			// adding new LineItem first
			var newLineItem = new LineItem
			{
				CatalogItemId = "ItemId",
				CatalogItemCode = "ItemCode",
				Quantity = 1,
				PlacedPrice = 10,
				TaxTotal = 1
			};
			order.OrderForms[0].LineItems.Add(newLineItem);

			// adding new ShipmentItem
			var item = new ShipmentItem();
			item.LineItem = newLineItem;
			item.LineItemId = item.LineItem.LineItemId; // this line shouldn't be necessary. But it is.
			//item.Shipment = order.OrderForms[0].Shipments[0]; // this line of code was causing exception in original	code
			order.OrderForms[0].Shipments[0].ShipmentItems.Add(item);
			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);
			order = repository.Orders
				.ExpandAll()
				.Expand("OrderForms/Shipments/ShipmentItems")
				.Expand("OrderForms/LineItems")
				.FirstOrDefault();

			Assert.True(order.OrderForms[0].LineItems.Any(x => x.LineItemId == newLineItem.LineItemId));
			Assert.True(order.OrderForms[0].Shipments[0].ShipmentItems.Any(x => x.LineItemId == newLineItem.LineItemId));
		}

		//note: it's important that line order.OrderForms[0].Shipments.Add(newShipment); was moved to the very end, just before commit
		[RepositoryTheory]
		public void Can_add_collection_item_items()
		{
			var order = TestOrderBuilder.BuildOrder().WithLineItemsCount(1).GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			order = repository.Orders.ExpandAll().FirstOrDefault();

			// adding new ShipmentItem
			var item = new ShipmentItem();
			item.Quantity = 1;
			item.LineItem = order.OrderForms[0].LineItems[0];

			// adding new Shipment
			var newShipment = new Shipment();
			newShipment.ShipmentItems.Add(item);
			order.OrderForms[0].Shipments.Add(newShipment);

			repository.UnitOfWork.Commit();
		}

		[RepositoryTheory]
		public void Can_add_collection_item_without_parentid()
		{
			var order = TestOrderBuilder.BuildOrder().WithLineItemsCount(1).GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			order = repository.Orders.ExpandAll().FirstOrDefault();

			// adding new Shipment
			var newShipment = new Shipment();
			order.OrderForms[0].Shipments.Add(newShipment);

			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);
			order = repository.Orders.ExpandAll().FirstOrDefault();
			var savedShipment = order.OrderForms[0].Shipments.FirstOrDefault(x => x.ShipmentId == newShipment.ShipmentId);

			Assert.NotNull(savedShipment);
			Assert.Equal(order.OrderForms[0].OrderFormId, savedShipment.OrderFormId);
		}

		[RepositoryTheory]
		[Variant(RepositoryProvider.EntityFramework)]
		public void Can_save_update_order_concurently()
		{
			var order = TestOrderBuilder.BuildOrder().WithLineItemsCount(1).GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			var saveThread1 = new ThreadStart(() =>
				{
					var repository1 = GetRepository();
					var order1 = repository1.Orders.First();
					order1.Name = "1";
					repository1.UnitOfWork.Commit();
				});
			var saveThread2 = new ThreadStart(() =>
			{
				var repository2 = GetRepository();
				var order2 = repository2.Orders.First();
				order2.Name = "2";
				repository2.UnitOfWork.Commit();
			});

			var thread1 = new Thread(saveThread1);
			var thread2 = new Thread(saveThread2);

			thread1.Start();
			thread2.Start();

			thread1.Join();
			thread2.Join();
		}

		[RepositoryTheory]
		//[RepositoryTheory(Skip = "One or both of the ends of the relationship is in the added state.")]
		/// <summary>
		/// problem while creating the second ShipmentItem in a new Shipment.
		/// </summary>
		public void Can_create_shipment_with_2shipmentitems()
		{
			var order = TestOrderBuilder.BuildOrder()
				.WithAddresess()
				.WithShipment()
				.WithLineItemsCount(2).GetOrder();

			var repository = GetRepository();
			repository.Add(order);
			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);
			order = repository.Orders.ExpandAll()
				.Where(x => x.OrderGroupId == order.OrderGroupId)
				.ExpandAll()
				.Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem")
				.Expand("RmaRequests/Order")
				.Expand("OrderForms/OrderGroup")
				.Expand("OrderForms/Shipments/ShipmentItems/LineItem")
				.SingleOrDefault();

			//adding new Shipment
			var targetShipment = new Shipment();

			//adding new ShipmentItem 1
			var movingShipmentItem = order.OrderForms[0].Shipments[0].ShipmentItems[0];
			movingShipmentItem.Quantity--;

			var targetShipmentItem = new ShipmentItem();
			targetShipmentItem.Quantity = 1;
			targetShipmentItem.LineItemId = movingShipmentItem.LineItemId;
			targetShipmentItem.LineItem = movingShipmentItem.LineItem;
			targetShipmentItem.ShipmentId = targetShipment.ShipmentId;
			targetShipment.ShipmentItems.Add(targetShipmentItem);

			targetShipment.ShipmentId = targetShipment.GenerateNewKey();
			targetShipment.OrderFormId = order.OrderForms[0].OrderFormId;
			order.OrderForms[0].Shipments.Add(targetShipment);

			//adding new ShipmentItem 2
			movingShipmentItem = order.OrderForms[0].Shipments[0].ShipmentItems[1];
			movingShipmentItem.Quantity--;

			targetShipmentItem = new ShipmentItem();
			targetShipmentItem.Quantity = 1;
			targetShipmentItem.LineItemId = movingShipmentItem.LineItemId;
			targetShipmentItem.LineItem = movingShipmentItem.LineItem; // commenting out this single line makes the test to pass
			targetShipmentItem.ShipmentId = targetShipment.ShipmentId;
			targetShipment.ShipmentItems.Add(targetShipmentItem);

			repository.UnitOfWork.Commit();

			RefreshRepository(ref repository);
			order = repository.Orders.ExpandAll()
				.Where(x => x.OrderGroupId == order.OrderGroupId)
				.ExpandAll()
				.Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem")
				.Expand("RmaRequests/Order")
				.Expand("OrderForms/OrderGroup")
				.Expand("OrderForms/Shipments/ShipmentItems/LineItem")
				.SingleOrDefault();
			Assert.NotNull(order);
			Assert.Equal(2, order.OrderForms[0].Shipments.Count);
			Assert.Equal(2, order.OrderForms[0].Shipments[0].ShipmentItems.Count);
			Assert.Equal(2, order.OrderForms[0].Shipments[1].ShipmentItems.Count);
		}
	}
}
