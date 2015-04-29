using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CatalogModule.Web
{
    public class Module : IModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var db = new EFCatalogRepository(_connectionStringName))
            {
                SqlCatalogDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                        initializer = new SqlCatalogSampleDatabaseInitializer();
                        break;
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCatalogReducedSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlCatalogDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFImportingRepository(_connectionStringName))
            {
                var initializer = new SqlImportDatabaseInitializer();
                initializer.InitializeDatabase(db);
            }
        }

        public void Initialize()
        {
            #region Catalog dependencies

            Func<IFoundationCatalogRepository> catalogRepFactory = () => new FoundationCatalogRepositoryImpl(_connectionStringName);
            Func<IFoundationAppConfigRepository> appConfigRepFactory = () => new FoundationAppConfigRepositoryImpl(_connectionStringName);

            var catalogService = new CatalogServiceImpl(catalogRepFactory, CacheManager.NoCache);
            var propertyService = new PropertyServiceImpl(catalogRepFactory, CacheManager.NoCache);
            var categoryService = new CategoryServiceImpl(catalogRepFactory, appConfigRepFactory, CacheManager.NoCache);
            var itemService = new ItemServiceImpl(catalogRepFactory, appConfigRepFactory, CacheManager.NoCache);
            var catalogSearchService = new CatalogSearchServiceImpl(catalogRepFactory, itemService, catalogService, categoryService, CacheManager.NoCache);

            var permissionService = _container.Resolve<IPermissionService>();
            var blobUrlResolver = _container.Resolve<IBlobUrlResolver>();

            _container.RegisterInstance<IItemService>(itemService);
            _container.RegisterInstance<ICategoryService>(categoryService);
            _container.RegisterInstance<ICatalogService>(catalogService);
            _container.RegisterInstance<IPropertyService>(propertyService);
            _container.RegisterInstance<ICatalogSearchService>(catalogSearchService);

            _container.RegisterType<ProductsController>(new InjectionConstructor(itemService, propertyService, blobUrlResolver));
            _container.RegisterType<PropertiesController>(new InjectionConstructor(propertyService, categoryService, catalogService));
            _container.RegisterType<ListEntryController>(new InjectionConstructor(catalogSearchService, categoryService, itemService, blobUrlResolver));
            _container.RegisterType<CategoriesController>(new InjectionConstructor(catalogSearchService, categoryService, propertyService, catalogService));
            _container.RegisterType<CatalogsController>(new InjectionConstructor(catalogService, catalogSearchService, appConfigRepFactory, propertyService, permissionService));

            #endregion

            #region Search dependencies
            //var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            //var elasticSearchProvider = new ElasticSearchProvider(new ElasticSearchQueryBuilder(), searchConnection);
            //_container.RegisterInstance<ISearchProvider>("Catalog", elasticSearchProvider);
            #endregion

            #region Import dependencies
            Func<IImportRepository> importRepFactory = () => new EFImportingRepository(_connectionStringName);

            Func<IImportService> imporServiceFactory = () => new ImportService(importRepFactory(), null, catalogRepFactory(), null, null);

            _container.RegisterType<ImportController>(new InjectionConstructor(importRepFactory, imporServiceFactory, catalogRepFactory, _container.Resolve<INotifier>()));

            #endregion

            #region Mock
            //var localPath = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_data\priceRuCategoryTest.yml");
            //var mockCatalogService = new MockCatalogService(localPath);
            //_container.RegisterInstance<ICatalogService>(mockCatalogService);
            //_container.RegisterInstance<ICategoryService>(mockCatalogService);
            //_container.RegisterInstance<IItemSearchService>(mockCatalogService);
            //_container.RegisterInstance<IItemService>(mockCatalogService);

            //_container.RegisterInstance<IPropertyService>(mockCatalogService);

            #endregion
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
