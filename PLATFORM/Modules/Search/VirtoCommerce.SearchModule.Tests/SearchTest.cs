using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Caching;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.PricingModule.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.SearchModule.Data.Provides.Lucene;
using VirtoCommerce.SearchModule.Data.Services;
using Xunit;

namespace VirtoCommerce.SearchModule.Tests
{
    public class SearchTest
    {
		[Fact]
		public void SearchCatalogBuilderTest()
		{
			var controller = GetSearchIndexController();
			controller.Process("default", "catalogitem");
		}

		private SearchIndexController GetSearchIndexController()
		{
			var cacheSettings = new[] 
			{
				new CacheSettings("CatalogItemIndexBuilder.IndexItemCategories", TimeSpan.FromMinutes(30))
			};

			var settingManager = new Moq.Mock<ISettingsManager>();
			var cacheManager = new CacheManager(new InMemoryCachingProvider(), cacheSettings);
			var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
			var searchProvider = new LuceneSearchProvider(new LuceneSearchQueryBuilder(), searchConnection);
			var catalogIndexBuilder = new CatalogItemIndexBuilder(searchProvider, GetSearchService(), GetItemService(), GetPricingService(), GetCategoryService(), GetPropertyService(), cacheManager);
			var searchController = new SearchIndexController(settingManager.Object, searchProvider, catalogIndexBuilder);
			return searchController;
		}
		private ICommerceService GetCommerceService()
		{
			return new CommerceServiceImpl(() => { return new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor()); });
		}


		private ICatalogSearchService GetSearchService()
		{
			return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService(), null);
		}

		private IPricingService GetPricingService()
		{
			return new PricingServiceImpl(() => { return GetPricingRepository(); });
		}

		private IPropertyService GetPropertyService()
		{
			return new PropertyServiceImpl(() => { return GetRepository(); });
		}

		private ICategoryService GetCategoryService()
		{
			return new CategoryServiceImpl(() => { return GetRepository(); }, GetCommerceService());
		}

		private ICatalogService GetCatalogService()
		{
			return new CatalogServiceImpl(() => { return GetRepository(); });
		}

		private IItemService GetItemService()
		{
			return new ItemServiceImpl(() => { return GetRepository(); }, GetCommerceService());
		}
		private IPricingRepository GetPricingRepository()
		{
			var retVal = new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			return retVal;
		}

		private ICatalogRepository GetRepository()
		{
			var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			return retVal;
		}
    }
}
