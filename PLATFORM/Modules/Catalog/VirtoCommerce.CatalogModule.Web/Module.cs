using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

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

        #region IDatabaseModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
			using (var db = new CatalogRepositoryImpl(_connectionStringName))
            {
				IDatabaseInitializer<CatalogRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                        initializer = new SqlCatalogSampleDatabaseInitializer();
                        break;
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCatalogReducedSampleDatabaseInitializer();
                        break;
                    default:
						initializer = new SetupDatabaseInitializer<CatalogRepositoryImpl, VirtoCommerce.CatalogModule.Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

        }

  
        public void Initialize()
        {
            #region Catalog dependencies

         	Func<ICatalogRepository> catalogRepFactory = () => new CatalogRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			_container.RegisterInstance<Func<ICatalogRepository>>(catalogRepFactory);

     
			_container.RegisterType<IItemService, ItemServiceImpl>();
			_container.RegisterType<ICategoryService, CategoryServiceImpl>();
			_container.RegisterType<ICatalogService, CatalogServiceImpl>();
			_container.RegisterType<IPropertyService, PropertyServiceImpl>();
			_container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();
            #endregion

            #region Import dependencies
			//Func<IImportRepository> importRepFactory = () => new EFImportingRepository(_connectionStringName);

			//Func<IImportService> imporServiceFactory = () => new ImportService(importRepFactory(), null, catalogRepFactory(), null, null);

			//_container.RegisterType<ImportController>(new InjectionConstructor(importRepFactory, imporServiceFactory, catalogRepFactory, _container.Resolve<INotifier>()));

            #endregion


        }


		public void PostInitialize()
		{
		
		}

		#endregion
	}
}
