using System;
using System.Web;
using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MerchandisingModule.Web.Controllers;
using VirtoCommerce.Search.Providers.Elastic;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : IModule
    {

        private const string MarketPlaceConnectionString = "VirtoCommerce";
        private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
			var cacheManager = new CacheManager(x => new InMemoryCachingProvider(), x => new CacheSettings("", TimeSpan.FromMinutes(1), "", true));
            Func<IFoundationCatalogRepository> catalogRepFactory = () => new FoundationCatalogRepositoryImpl(MarketPlaceConnectionString);
            Func<IFoundationAppConfigRepository> appConfigRepFactory = () => new FoundationAppConfigRepositoryImpl(MarketPlaceConnectionString);

			var catalogService = new CatalogServiceImpl(catalogRepFactory, cacheManager);
			var propertyService = new PropertyServiceImpl(catalogRepFactory, cacheManager);
			var categoryService = new CategoryServiceImpl(catalogRepFactory, appConfigRepFactory, cacheManager);
			var itemService = new ItemServiceImpl(catalogRepFactory, appConfigRepFactory, cacheManager);
			var itemSearchService = new CatalogSearchServiceImpl(catalogRepFactory, itemService, catalogService, categoryService);

			#region VCF dependencies
			var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
			var elasticSearchProvider = new ElasticSearchProvider(new ElasticSearchQueryBuilder(), searchConnection);
	
			Func<IReviewRepository> reviewRepFactory = () => new EFReviewRepository(MarketPlaceConnectionString);
		
			#endregion

			#region Dynamic content
			Func<IDynamicContentRepository> dynamicRepositoryFactory = () => { return new EFDynamicContentRepository(MarketPlaceConnectionString); };
			Func<IDynamicContentEvaluator> dynamicContentEval = () => { return  new DynamicContentEvaluator(dynamicRepositoryFactory(), null, new HttpCacheRepository()); };
			Func<IDynamicContentService> dynamicContentServiceFactory = () => { return new DynamicContentService(dynamicRepositoryFactory(), dynamicContentEval()); };
			#endregion

			

			_container.RegisterType<ReviewController>(new InjectionConstructor(reviewRepFactory));
			_container.RegisterType<ProductController>(new InjectionConstructor(itemService, elasticSearchProvider, searchConnection, catalogRepFactory));
			_container.RegisterType<ContentController>(new InjectionConstructor(dynamicContentServiceFactory()));
			_container.RegisterType<CategoryController>(new InjectionConstructor(itemSearchService, categoryService, propertyService, catalogRepFactory));

		}
    }
}
