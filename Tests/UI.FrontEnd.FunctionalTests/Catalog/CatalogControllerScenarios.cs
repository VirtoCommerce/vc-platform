using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FunctionalTests.TestHelpers;
using UI.FrontEnd.FunctionalTests.Properties;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.PowerShell.Stores;
using VirtoCommerce.Web.Client.Services.Cache;
using VirtoCommerce.Web.Controllers;
using VirtoCommerce.Web.Models;
using Xunit;

namespace UI.FrontEnd.FunctionalTests.Catalog
{
	[Variant(RepositoryProvider.EntityFramework)]
	public class CatalogControllerScenarios : ControllerTestBase
	{

		public CatalogControllerScenarios()
		{
			//var context = new Mock<HttpContextBase>();
			//var request = new Mock<HttpRequestBase>();
			//context
			//	.Setup(c => c.Request)
			//	.Returns(request.Object);

			CustomerSessionService.CustomerSession.Currency = "USD";
			CustomerSessionService.CustomerSession.CustomerName = "John Doe";
			CustomerSessionService.CustomerSession.CustomerId = "1";
			CustomerSessionService.CustomerSession.IsRegistered = true;
			CustomerSessionService.CustomerSession.StoreId = "SampleStore";
			CustomerSessionService.CustomerSession.StoreName = "Electronics";
		}

		[RepositoryTheory]
		public void can_display_category()
		{
			Item[] items = null;
			CreateFullGraphCatalog("testcatlog", ref items);

			var catalog = CatalogRepository.Catalogs.First();
			var category = catalog.CategoryBases.First();

			CustomerSessionService.CustomerSession.CatalogId = catalog.CatalogId;
			CustomerSessionService.CustomerSession.CategoryId = category.Code;

			var controller = (CatalogController)DependencyResolver.Current.GetService(typeof(CatalogController));
			var result = controller.Display(new CategoryPathModel {Category = category.Code}) as ViewResult;
			var model = result.Model as CategoryModel;

			Assert.Equal(result.ViewName, "Category");
			Assert.NotNull(model);
			Assert.Equal(model.CategoryId, category.CategoryId);
         
		}

        [RepositoryTheory]
	    public void can_clear_databaseCache()
	    {
            Item[] items = null;
            CreateFullGraphCatalog("testcatlog", ref items);

            var client = Locator.GetInstance<CatalogClient>();
            var catalog =client.GetCatalog("testcatlog");

            Assert.NotNull(catalog);

            //Check that there are items in cache
            var cachedItems = CacheRepository.GetEnumerator();
            var catalogCachedItems = new List<object>();
            while (cachedItems.MoveNext())
            {
                if (cachedItems.Key.ToString().StartsWith(CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix)))
                {
                    catalogCachedItems.Add(cachedItems.Entry);
                }
            }
            Assert.NotEmpty(catalogCachedItems);

            //Clear cache
            var cacheService = Locator.GetInstance<ICacheService>();
            var removedCount = cacheService.ClearDatabaseCache(Constants.CatalogCachePrefix);

            Assert.True(removedCount > 0, "Nothing was removed from cache");

            catalogCachedItems.Clear();
            cachedItems = CacheRepository.GetEnumerator();
            while (cachedItems.MoveNext())
            {
                if (cachedItems.Key.ToString().StartsWith(CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix)))
                {
                    catalogCachedItems.Add(cachedItems.Entry);
                }
            }

            Assert.Empty(catalogCachedItems);
	    }

		[RepositoryTheory]
		public void can_display_item()
		{
			var session = CustomerSessionService.CustomerSession;

			//Generate data
			const string catalogId = "testcatlog";
			Item[] items = null;
			CreateFullGraphCatalog(catalogId, ref items);
			SqlStoreSampleDatabaseInitializer.CreateFulfillmentCenter((EFStoreRepository)StoreRepository);
			SqlStoreSampleDatabaseInitializer.CreateStores((EFStoreRepository)StoreRepository);

			//Modify data
			var item = items.First();

			item.IsBuyable = true;
			item.MaxQuantity = 10;
			item.MinQuantity = 1;
			CatalogRepository.Update(item);
			CatalogRepository.UnitOfWork.Commit();

			var priceListAssigment = GeneratePrices(items, catalogId);
			var price = priceListAssigment.Pricelist.Prices.First(x => x.ItemId == item.ItemId);

			//Add price discount 50%
			const int discountPercent = 50;
			var promotion = AddCatalogPromotion(catalogId, string.Format(Resources.PromotionExpression_Item0, item.ItemId));
			AddCatalogReward(promotion, discountPercent, RewardAmountType.Relative);


			//var catalog = CatalogRepository.Catalogs.First(x => x.CatalogId == item.CatalogId);
			var category = CatalogRepository.CategoryItemRelations.First(x => x.ItemId == item.ItemId);

			var priceListHelper = Locator.GetInstance<PriceListClient>();

			session.Pricelists = priceListHelper.GetPriceListStack(catalogId, session.Currency, session.GetCustomerTagSet(), false);
			Assert.Equal(priceListAssigment.Pricelist.PricelistId, session.Pricelists[0]);

			session.CatalogId = catalogId;
			session.CategoryId = category.CategoryId;

			var controller = (CatalogController)DependencyResolver.Current.GetService(typeof(CatalogController));
			var result = controller.DisplayItem(item.Code) as ViewResult;
			var model = result.Model as CatalogItemWithPriceModel;

			Assert.Equal(result.ViewName, "Item"); //Default view
			Assert.NotNull(model);
			Assert.NotNull(model.Price); // price returned
			Assert.Equal(model.Price.Currency, session.Currency); // curecncy matches
			Assert.Equal(model.Price.SalePrice, price.Sale.Value * discountPercent * 0.01m);
			Assert.True(model.CatalogItem.ItemId.Equals(item.ItemId), "Requested and returned itemId do not match");
			Assert.True(model.Availability.IsAvailable, "Item is not available"); //item is available
		}

		private Promotion AddCatalogPromotion(string catalogId, string expression, string promotionName = "test")
		{
			var promotion = new CatalogPromotion
				{
					Name = promotionName,
					CatalogId = catalogId,
					PredicateSerialized = expression,
					Status = "Active",
					StartDate = DateTime.UtcNow,
					
				};

			MarketingRepository.Add(promotion);
			MarketingRepository.UnitOfWork.Commit();

			return promotion;
		}

		private PromotionReward AddCatalogReward(Promotion promotion, decimal amount, RewardAmountType amountType)
		{
			var promotionReward = new CatalogItemReward
			{
				Amount = amount,
				AmountTypeId = amountType.GetHashCode(),
				PromotionId = promotion.PromotionId
			};

			MarketingRepository.Add(promotionReward);
			MarketingRepository.UnitOfWork.Commit();

			return promotionReward;
		}
	}
}
