using System;
using System.Linq;
using FunctionalTests.Catalogs.Helpers;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Xunit;

namespace FunctionalTests.Catalogs
{
	[Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class PriceListScenarios : CatalogTestBase, IDisposable
	{
		#region Helper Methods

		private new IPricelistRepository GetRepository()
		{
			return (IPricelistRepository)base.GetRepository();
		}

		private ICatalogRepository GetCatalogRepository()
		{
			return base.GetRepository();
		}

		private string CreatePriceList(string name)
		{
			using (var client = GetRepository())
			{
				var pricelist = new Pricelist { Name = name, Currency = "USD" };
				client.Add(pricelist);
				client.UnitOfWork.Commit();
				return pricelist.PricelistId;
			}
		}

		private void CreateCatalogGraf()
		{
			var client = GetCatalogRepository();
			var catalogBuilder = CatalogBuilder.BuildCatalog("Test catalog").WithCategory("category").WithProducts(2);
			var catalog = catalogBuilder.GetCatalog();
			var items = catalogBuilder.GetItems();

			client.Add(catalog);

			foreach (var item in items)
			{
				client.Add(item);
			}

			client.UnitOfWork.Commit();
		}

		private Item GetCatalogItems()
		{
			var client = GetCatalogRepository();
			var items = client.Items.ToList();
			return (items.Any()) ? items[0] : null;
		}

		#endregion

		[RepositoryTheory]
		public void Can_double_add_pricelist_item()
		{
			CreateCatalogGraf();
			var item = GetCatalogItems();

			var pricelistId = CreatePriceList("Test pricelist");

			var client = GetRepository();

			// load to detail view
			var innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);

			//add a price
			var price = new Price { List = 10, MinQuantity = 1, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			innerItem.Prices.Add(price);

			// remove just added price
			innerItem.Prices.Remove(price);

			//add a price again with the same CatalogItem
			price = new Price { List = 20, MinQuantity = 1, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			var priceId = price.PriceId;
			innerItem.Prices.Add(price);

			client.UnitOfWork.Commit();

			// check 
			client = GetRepository();
			innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();

			Assert.NotNull(innerItem);
			Assert.True(innerItem.Prices.Any());
			Assert.True(innerItem.Prices[0].PriceId == priceId);

		}

		[RepositoryTheory]
		public void Can_modify_pricelist_price()
		{
			CreateCatalogGraf();
			var item = GetCatalogItems();

			var pricelistId = CreatePriceList("Test pricelist2");

			var client = GetRepository();

			// load to detail view
			var innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);

			//add a price
			var price = new Price { List = 10, MinQuantity = 1, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			innerItem.Prices.Add(price);

			client.UnitOfWork.Commit();

			innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);
			price = innerItem.Prices[0];
			//price.Pricelist = innerItem;
			price.Sale = 1;
			client.Update(innerItem);
			client.UnitOfWork.Commit();

			price.Sale = 2;
			client.UnitOfWork.Commit();

			// check 
			client = GetRepository();
			innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();

			Assert.NotNull(innerItem);
			Assert.True(innerItem.Prices.Any());
			//Assert.True(innerItem.Prices[0].PriceId == priceId);

		}
		[RepositoryTheory]
		public void Can_delete_exist_pricelist_item_and_add_it_again()
		{
			CreateCatalogGraf();
			var item = GetCatalogItems();

			var pricelistId = CreatePriceList("Test pricelist");

			var client = GetRepository();

			// load to detail view
			var innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);

			//add a price
			var price = new Price { List = 10, MinQuantity = 1, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			var priceId = price.PriceId;
			innerItem.Prices.Add(price);

			client.UnitOfWork.Commit();

			// we have one price list in DB now

			client = GetRepository();

			// load to detail view
			innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);
			Assert.True(innerItem.Prices.Any());
			price = innerItem.Prices[0];
			Assert.True(price.PriceId == priceId);


			// remove price
			innerItem.Prices.Remove(price);

			//add a price again with the same CatalogItem
			price = new Price { List = 20, MinQuantity = 1, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			priceId = price.PriceId;
			innerItem.Prices.Add(price);

			client.UnitOfWork.Commit();

			//check
			innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(innerItem);
			Assert.True(innerItem.Prices.Any());
			price = innerItem.Prices[0];
			Assert.True(price.PriceId == priceId);

		}

		[RepositoryTheory]
		public void Can_and_price_with_the_same_ItemId_and_different_Quantity_Method1()
		{

			CreateCatalogGraf();
			var item = GetCatalogItems();

			var pricelistId = CreatePriceList("Test pricelist");

			var client1 = GetRepository();

			// load to detail view
			var pricelist = client1.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(pricelist);

			//add a price
			var price = new Price { List = 10, MinQuantity = 3, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			var priceId = price.PriceId;
			pricelist.Prices.Add(price);

			client1.UnitOfWork.Commit();

			// we have one price list in DB now

			var client2 = GetRepository();

			// load to detail view
			pricelist = client2.Pricelists.Where(x => x.PricelistId == pricelistId)
						.Expand("Prices/CatalogItem")
						.SingleOrDefault();
			Assert.NotNull(pricelist);
			Assert.True(pricelist.Prices.Any());
			price = pricelist.Prices[0];
			Assert.True(price.PriceId == priceId && price.MinQuantity == 3 && price.ItemId == item.ItemId);

			//add a price again with different Quantity
			price = new Price { List = 10, MinQuantity = 2, /*CatalogItem = item,*/ ItemId = item.ItemId, PricelistId = pricelistId };
			pricelist.Prices.Add(price);

			client2.UnitOfWork.Commit();



			//check
			//innerItem = client.Pricelists.Where(x => x.PricelistId == pricelistId)
			//			.Expand("Prices/CatalogItem")
			//			.SingleOrDefault();
			//Assert.NotNull(innerItem);
			//Assert.True(innerItem.Prices.Any());
			//price = innerItem.Prices[0];
			//Assert.True(price.PriceId == priceId);

		}

		//[RepositoryTheory]
		public void Can_and_price_with_the_same_ItemId_and_different_Quantity_Method2()
		{

			CreateCatalogGraf();
			var item = GetCatalogItems();

			var pricelistId = CreatePriceList("Test pricelist");

			var client1 = GetRepository();

			// load to detail view
			var pricelist = client1.Pricelists.Where(x => x.PricelistId == pricelistId)
				.Expand("Prices/CatalogItem")
				.SingleOrDefault();
			Assert.NotNull(pricelist);

			//add a price
			var price = new Price
			{
				List = 10,
				MinQuantity = 3,
				/*CatalogItem = item,*/
				ItemId = item.ItemId,
				PricelistId = pricelistId
			};
			var priceId = price.PriceId;
			pricelist.Prices.Add(price);

			client1.UnitOfWork.Commit();

			// we have one price list in DB now

			var client2 = GetRepository();

			// load to detail view
			pricelist = client2.Pricelists.Where(x => x.PricelistId == pricelistId)
				.Expand("Prices/CatalogItem")
				.SingleOrDefault();
			Assert.NotNull(pricelist);
			Assert.True(pricelist.Prices.Any());
			price = pricelist.Prices[0];
			Assert.True(price.PriceId == priceId && price.MinQuantity == 3 && price.ItemId == item.ItemId);

			//add a price again with different Quantity
			price = new Price
			{
				List = 10,
				MinQuantity = 2,
				/*CatalogItem = item,*/
				ItemId = item.ItemId,
				PricelistId = pricelistId
			};

			// this is the difference with Can_and_price_with_the_same_ItemId_and_different_Quantity_Method1
			price.CatalogItem = item;
			pricelist.Prices.Add(price);

			client2.UnitOfWork.Commit();
		}
	}
}
