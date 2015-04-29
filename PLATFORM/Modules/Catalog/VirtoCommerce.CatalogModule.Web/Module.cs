using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.CatalogModule.Web.SampleData;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Settings;

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
			using (var db = new CatalogRepositoryImpl(_connectionStringName))
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

			//using (var db = new CatalogRepositoryImpl(_connectionStringName))
			//{
			//	var initializer = new SqlImportDatabaseInitializer();
			//	initializer.InitializeDatabase(db);
			//}
        }

        #endregion

        #region IModule Members

        public void Initialize()
        {
            #region Catalog dependencies

            var settingsManager = _container.Resolve<ISettingsManager>();

            Func<ICatalogRepository> catalogRepFactory = () => new CatalogRepositoryImpl(_connectionStringName);
        
            var catalogService = new CatalogServiceImpl(catalogRepFactory, CacheManager.NoCache);
            var propertyService = new PropertyServiceImpl(catalogRepFactory, CacheManager.NoCache);
            var categoryService = new CategoryServiceImpl(catalogRepFactory, CacheManager.NoCache);
            var itemService = new ItemServiceImpl(catalogRepFactory);
            var catalogSearchService = new CatalogSearchServiceImpl(catalogRepFactory, itemService, catalogService, categoryService, CacheManager.NoCache);

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
            _container.RegisterType<CatalogsController>(new InjectionConstructor(catalogService, catalogSearchService, settingsManager, propertyService));
            #endregion

            #region Import dependencies
			//Func<IImportRepository> importRepFactory = () => new EFImportingRepository(_connectionStringName);

			//Func<IImportService> imporServiceFactory = () => new ImportService(importRepFactory(), null, catalogRepFactory(), null, null);

			//_container.RegisterType<ImportController>(new InjectionConstructor(importRepFactory, imporServiceFactory, catalogRepFactory, _container.Resolve<INotifier>()));

            #endregion


        }

        #endregion
    }
}
