using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Frameworks.Logging;

namespace VirtoCommerce.Foundation.Orders.Search
{
    /*
	public class OrderIndexBuilder : IndexBuilderBase
	{
		public override void BuildIndex(IndexContext context, bool rebuild)
		{
			// Initialize base
			base.BuildIndex(context, rebuild);

			OnEvent(String.Format("OrderIndexBuilder Started"), 0);

			DateTime lastBuild = DateTime.MinValue;
			DateTime newBuildDate = context.BuildDate;

			// On complete rebuild no need to check date
			if (!rebuild)
				lastBuild = context.GetBuildConfig().LastBuildDate;

			if (lastBuild == DateTime.MinValue)
				rebuild = true;

			// do complete rebuild
			if (rebuild)
			{
				RebuildOrders(context);
			}
			else
			{
				BuildOrders(context, lastBuild, newBuildDate);
			}            

		}

		protected virtual void BuildOrders(IndexContext context, DateTime lastBuild, DateTime currentBuild)
		{
			//IOrderService svc = OrderContext.Current;
			//LogEntry[] logs = svc.GetOrdersLog(lastBuild, currentBuild);

			//// logs are in sequential order, so process them that way
			//foreach (LogEntry log in logs)
			//{
			//    //if (log.Op == LogOperation.Deleted || log.Op == LogOperation.Updated)
			//    {
			//        Console.WriteLine("Deleting #{0}.", log.Key);
			//        context.DeleteContent("__key", log.Key);
			//    }

			//    if (log.Op == LogOperation.Added || log.Op == LogOperation.Updated)
			//    {
			//        // Load item
			//        OrderGroup order = svc.GetOrderById(log.Key);
			//        if (order != null)
			//        {
			//            ResultDocument doc = new ResultDocument();
			//            IndexOrder(ref doc, order);
			//            context.AddDocument(doc);
			//            Console.WriteLine("Indexing #{0}.", log.Key);
			//        }
			//    }
			//}
		}

		protected virtual void RebuildOrders(IndexContext context)
		{
			//IOrderService svc = OrderContext.Current;

			//int index = 0;

			//// Now start indexing
			//OrderQueryCriteria criteria = new OrderQueryCriteria();
			//criteria.OrderType = "order";
			//criteria.RecordsToRetrieve = 100;

			//bool hasMore = false;

			//do
			//{
			//    OrderQueryResults results = svc.QueryOrders(criteria);

			//    foreach (OrderGroup order in results.Orders)
			//    {

			//        index++;
			//        ResultDocument doc = new ResultDocument();
			//        IndexOrder(ref doc, order);
			//        context.AddDocument(doc);
			//        Console.WriteLine("Indexing #{0}.", index);
			//    }

			//    hasMore = results.HasMoreRecords;
			//}
			//while (hasMore);
		}

		protected virtual void IndexOrder(ref ResultDocument doc, OrderGroup order)
		{
			doc.Add(new DocumentField("__key", order.OrderGroupId, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("__loc", "en-us", new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("__type", "order", new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("__sort", order.Name, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("orderid", order.OrderGroupId, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("currency", order.BillingCurrency, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("customerid", order.CustomerId, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("organizationid", order.OrganizationId, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("customername", order.CustomerName, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("status", order.Status, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("storeid", order.StoreId, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("total", order.Total, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));

			// Index addresses
			IndexOrderAddresses(ref doc, order);

			// add to content
			doc.Add(new DocumentField("__content", order.Name, new string[] { IndexStore.YES, IndexType.ANALYZED }));
		}

		protected virtual void IndexOrderAddresses(ref ResultDocument doc, OrderGroup order)
		{
			if (order.OrderAddresses == null || order.OrderAddresses.Count == 0)
				return;

			foreach (OrderAddress address in order.OrderAddresses)
			{
				if(address.OrderAddressId.Equals(order.AddressId, StringComparison.OrdinalIgnoreCase))
					IndexOrderAddress(ref doc, address);
			}
		}

		protected virtual void IndexOrderAddress(ref ResultDocument doc, OrderAddress address)
		{
			doc.Add(new DocumentField("address.name", address.Name, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.organization", address.Organization, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.postalcode", address.PostalCode, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.stateprovince", address.StateProvince, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.city", address.City, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.country", address.CountryName, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.countrycode", address.CountryCode, new string[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
			doc.Add(new DocumentField("address.firstname", address.FirstName, new string[] { IndexStore.YES }));
			doc.Add(new DocumentField("address.lastname", address.LastName, new string[] { IndexStore.YES }));
		}
	}
     * */
}
