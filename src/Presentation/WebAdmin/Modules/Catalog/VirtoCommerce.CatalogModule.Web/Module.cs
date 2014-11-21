using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Search.Providers.Elastic;
using ICatalogService = VirtoCommerce.CatalogModule.Services.ICatalogService;
using VirtoCommerce.Foundation.Frameworks.Caching;

namespace VirtoCommerce.CatalogModule.Web
{
    [Module(ModuleName = "CatalogsModule", OnDemand = true)]
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            #region VCF dependencies
            var appConfigRepository = new EFAppConfigRepository("VirtoCommerce");
			 _container.RegisterInstance<IAppConfigRepository>(appConfigRepository);
            _container.RegisterType<ISearchProvider, ElasticSearchProvider>();
            _container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            _container.RegisterInstance<ISearchConnection>(searchConnection);
            #endregion

            #region module services

            _container.RegisterType<Func<IFoundationCatalogRepository>>(new InjectionFactory(x => new Func<IFoundationCatalogRepository>(() => new FoundationCatalogRepositoryImpl("VirtoCommerce"))));
			_container.RegisterType<Func<IFoundationAppConfigRepository>>(new InjectionFactory(x => new Func<IFoundationAppConfigRepository>(() => new FoundationAppConfigRepositoryImpl("VirtoCommerce"))));
			var cacheManager = new CacheManager(x => new InMemoryCachingProvider(), x => new CacheSettings("", TimeSpan.FromMinutes(1), "", true));
            _container.RegisterInstance(cacheManager);
            _container.RegisterType<ICatalogService, CatalogServiceImpl>();
            _container.RegisterType<IPropertyService, PropertyServiceImpl>();
            _container.RegisterType<ICategoryService, CategoryServiceImpl>();
            _container.RegisterType<IItemService, ItemServiceImpl>();
            _container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();

            #region Mock
            //var localPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_data\priceRuCategoryTest.yml");
            //var mockCatalogService = new MockCatalogService(localPath);
            //_container.RegisterInstance<ICatalogService>(mockCatalogService);
            //_container.RegisterInstance<ICategoryService>(mockCatalogService);
            //_container.RegisterInstance<IItemSearchService>(mockCatalogService);
            //_container.RegisterInstance<IItemService>(mockCatalogService);

            //_container.RegisterInstance<IPropertyService>(mockCatalogService);

            #endregion

            #endregion
        }
    }
}
