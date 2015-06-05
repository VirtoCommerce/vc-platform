using System;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Caching;
using VirtoCommerce.Platform.Data.ChangeLog;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.PricingModule.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.SearchModule.Data.Model;
using VirtoCommerce.SearchModule.Data.Providers.Lucene;
using VirtoCommerce.SearchModule.Data.Services;
using Xunit;

namespace VirtoCommerce.SearchModule.Tests
{
    public class SearchTest
    {
        [Fact]
        public void SettingManagerTest()
        {
            Func<IPlatformRepository> platformRepositoryFactory = GetPlatformRepository;

            var cacheManager = new CacheManager(new InMemoryCachingProvider(), null);
            var settingManager = new SettingsManager(null, platformRepositoryFactory, cacheManager);

            var name = Guid.NewGuid().ToString();

            settingManager.SetValue(name, 1); // сохраняется
            settingManager.SetValue(name, 2); // не сохраняется
        }

        [Fact]
        public void SearchCatalogBuilderTest()
        {
            var controller = GetSearchIndexController();
            controller.Process("default", CatalogIndexedSearchCriteria.DocType, true);
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
            var catalogIndexBuilder = new CatalogItemIndexBuilder(searchProvider, GetSearchService(), GetItemService(), GetPricingService(), GetPropertyService(), GetChangeLogService(),  new CatalogOutlineBuilder(GetCategoryService(), cacheManager));
            var searchController = new SearchIndexController(settingManager.Object, catalogIndexBuilder);
            return searchController;
        }


        private ICommerceService GetCommerceService()
        {
            return new CommerceServiceImpl(GetCommerceRepository);
        }

        private ICatalogSearchService GetSearchService()
        {
            return new CatalogSearchServiceImpl(GetCatalogRepository, GetItemService(), GetCatalogService(), GetCategoryService(), null);
        }

        private IPricingService GetPricingService()
        {
            return new PricingServiceImpl(GetPricingRepository);
        }

        private IPropertyService GetPropertyService()
        {
            return new PropertyServiceImpl(GetCatalogRepository);
        }

        private ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(GetCatalogRepository, GetCommerceService());
        }

        private ICatalogService GetCatalogService()
        {
            return new CatalogServiceImpl(GetCatalogRepository);
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(GetCatalogRepository, GetCommerceService());
        }

        private IChangeLogService GetChangeLogService()
        {
            return new ChangeLogService(GetPlatformRepository);
        }


        private IPlatformRepository GetPlatformRepository()
        {
            var result = new PlatformRepository("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            return result;
        }

        private IPricingRepository GetPricingRepository()
        {
            var result = new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            return result;
        }

        private ICatalogRepository GetCatalogRepository()
        {
            var result = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            return result;
        }

        private static IСommerceRepository GetCommerceRepository()
        {
            var result = new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            return result;
        }
    }
}
