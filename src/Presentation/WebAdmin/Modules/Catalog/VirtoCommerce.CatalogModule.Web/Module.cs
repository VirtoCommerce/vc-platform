using Microsoft.Practices.Unity;
using System;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Asset;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Search.Providers.Elastic;
using ICatalogService = VirtoCommerce.CatalogModule.Services.ICatalogService;

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
            _container.RegisterType<IAppConfigRepository>(new InjectionFactory(x => new EFAppConfigRepository("VirtoCommerce")));
            _container.RegisterType<ISearchProvider, ElasticSearchProvider>();
            _container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            _container.RegisterInstance<ISearchConnection>(searchConnection);
            #endregion

            #region Import
            // _container.RegisterType<IAssetService, AssetService>();

            //_container.RegisterType<IImportRepository>(new InjectionFactory(x => new EFImportingRepository("VirtoCommerce")));
            _container.RegisterType<Func<IImportRepository>>(new InjectionFactory(x => new Func<IImportRepository>(() => new EFImportingRepository("VirtoCommerce"))));
            _container.RegisterType<Func<IImportService>>(new InjectionFactory(x => new Func<IImportService>(() =>
                {
                    var fileSystemBlobAssetRepository = new FileSystemBlobAssetRepository("~/Content/Uploads/", new AssetEntityFactory());
                    return new ImportService(
                        _container.Resolve<Func<IImportRepository>>()(),
                        _container.Resolve<IAssetService>(new ParameterOverrides
                        {
                            { "assetRepository", fileSystemBlobAssetRepository },
                            { "blobStorageProvider", fileSystemBlobAssetRepository }
                        }),
                        _container.Resolve<Func<IFoundationCatalogRepository>>()(),
                        null, null, null);
                })));
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
