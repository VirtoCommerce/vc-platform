using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Data.Asset;
using VirtoCommerce.Foundation.Data.Azure.Asset;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Importing;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.Foundation;
using System.Collections.Generic;

namespace VirtoCommerce.CatalogModule.Web
{
    public class Module : IModule, IDatabaseModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IDatabaseModule Members

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

        #endregion

        #region IModule Members

        public void Initialize()
        {
            #region Catalog dependencies

			 var settingsManager = _container.Resolve<ISettingsManager>();

	        Func<IFoundationCatalogRepository> catalogRepFactory = () => new FoundationCatalogRepositoryImpl(_connectionStringName);
            Func<IFoundationAppConfigRepository> appConfigRepFactory = () => new FoundationAppConfigRepositoryImpl(_connectionStringName);
					
			var catalogService = new CatalogServiceImpl(catalogRepFactory, CacheManager.NoCache);
			var propertyService = new PropertyServiceImpl(catalogRepFactory, CacheManager.NoCache);
			var categoryService = new CategoryServiceImpl(catalogRepFactory, appConfigRepFactory, CacheManager.NoCache);
			var itemService = new ItemServiceImpl(catalogRepFactory, appConfigRepFactory, CacheManager.NoCache);
			var catalogSearchService = new CatalogSearchServiceImpl(catalogRepFactory, itemService, catalogService, categoryService, CacheManager.NoCache);

            var assetsConnectionString = ConnectionHelper.GetConnectionString("AssetsConnectionString");
            var blobStorageProvider = new AzureBlobAssetRepository(assetsConnectionString, null);

            _container.RegisterInstance<IItemService>(itemService);
            _container.RegisterInstance<ICategoryService>(categoryService);
            _container.RegisterInstance<ICatalogService>(catalogService);
            _container.RegisterInstance<IPropertyService>(propertyService);
            _container.RegisterInstance<ICatalogSearchService>(catalogSearchService);

            _container.RegisterType<AssetsController>(new InjectionConstructor(blobStorageProvider));
            _container.RegisterType<ProductsController>(new InjectionConstructor(itemService, propertyService, blobStorageProvider));
            _container.RegisterType<PropertiesController>(new InjectionConstructor(propertyService, categoryService, catalogService));
            _container.RegisterType<ListEntryController>(new InjectionConstructor(catalogSearchService, categoryService, itemService, blobStorageProvider));
            _container.RegisterType<CategoriesController>(new InjectionConstructor(catalogSearchService, categoryService, propertyService, catalogService));
            _container.RegisterType<CatalogsController>(new InjectionConstructor(catalogService, catalogSearchService, appConfigRepFactory, propertyService));
            #endregion

            #region Search dependencies
            //var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            //var elasticSearchProvider = new ElasticSearchProvider(new ElasticSearchQueryBuilder(), searchConnection);
            //_container.RegisterInstance<ISearchProvider>("Catalog", elasticSearchProvider);
            #endregion


            #region Import dependencies
            Func<IImportRepository> importRepFactory = () => new EFImportingRepository(_connectionStringName);

            var fileSystemAssetRep = new FileSystemBlobAssetRepository("~", new AssetEntityFactory());
            var assetService = new AssetService(fileSystemAssetRep, fileSystemAssetRep);
            Func<IImportService> imporServiceFactory = () => new ImportService(importRepFactory(), assetService, catalogRepFactory(), null, null);

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

        #endregion
    }
}
