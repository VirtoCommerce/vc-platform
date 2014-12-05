using System;
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
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Search.Providers.Elastic;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
			var cacheManager = new CacheManager(x => new InMemoryCachingProvider(), x => new CacheSettings("", TimeSpan.FromMinutes(1), "", true));
			Func<IFoundationCatalogRepository> catalogRepFactory = () => new FoundationCatalogRepositoryImpl("VirtoCommerce");
			Func<IFoundationAppConfigRepository> appConfigRepFactory = () => new FoundationAppConfigRepositoryImpl("VirtoCommerce");

			var catalogService = new CatalogServiceImpl(catalogRepFactory, cacheManager);
			var propertyService = new PropertyServiceImpl(catalogRepFactory, cacheManager);
			var categoryService = new CategoryServiceImpl(catalogRepFactory, appConfigRepFactory, cacheManager);
			var itemService = new ItemServiceImpl(catalogRepFactory, appConfigRepFactory, cacheManager);
			var itemSearchService = new CatalogSearchServiceImpl(catalogRepFactory, itemService, catalogService, categoryService);

			_container.RegisterInstance<ICatalogService>("MP", catalogService);
			_container.RegisterInstance<IPropertyService>("MP", propertyService);
			_container.RegisterInstance<ICategoryService>("MP", categoryService);
			_container.RegisterInstance<IItemService>("MP", itemService);
			_container.RegisterInstance<ICatalogSearchService>("MP", itemSearchService);

			#region VCF dependencies
			var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
			var elasticSearchProvider = new ElasticSearchProvider(new ElasticSearchQueryBuilder(), searchConnection);
			_container.RegisterInstance<ISearchProvider>("MP",elasticSearchProvider);
			//_container.RegisterType<IAppConfigRepository>("MP", new InjectionFactory(x => new EFAppConfigRepository("VirtoCommerce")));
			#endregion

			#region Dynamic content
			var httpCacheRep = new HttpCacheRepository();
			var dynamicContentRep = new EFDynamicContentRepository("VirtoCommerce");
			var dynamicContentEval = new DynamicContentEvaluator(dynamicContentRep, null,  httpCacheRep);
			var dynamicContentService = new DynamicContentService(dynamicContentRep, dynamicContentEval);
			_container.RegisterInstance<IDynamicContentService>("MP", dynamicContentService);
			#endregion
		}
    }
}
