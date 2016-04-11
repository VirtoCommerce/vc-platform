using System;
using System.IO;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.CatalogModule.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase()
        {
            base.SetupDatabase();

            using (var db = new CatalogRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<CatalogRepositoryImpl, Data.Migrations.Configuration>();

                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            #region Catalog dependencies

            Func<ICatalogRepository> catalogRepFactory = () =>
                new CatalogRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>(),
                    new ChangeLogInterceptor(_container.Resolve<Func<IPlatformRepository>>(), ChangeLogPolicy.Cumulative, new[] { typeof(Item).Name }, _container.Resolve<IUserNameResolver>()));

            _container.RegisterInstance(catalogRepFactory);

            _container.RegisterType<IItemService, ItemServiceImpl>();
            _container.RegisterType<ICategoryService, CategoryServiceImpl>();
            _container.RegisterType<ICatalogService, CatalogServiceImpl>();
            _container.RegisterType<IPropertyService, PropertyServiceImpl>();
            _container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();
            _container.RegisterType<ISkuGenerator, DefaultSkuGenerator>();
            _container.RegisterType<ISeoDuplicatesDetector, CatalogSeoDublicatesDetector>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOutlineService, OutlineService>();

            #endregion
        }

        public override void PostInitialize()
        {
            var securityScopeService = _container.Resolve<IPermissionScopeService>();
            securityScopeService.RegisterSope(() => new CatalogSelectedScope());
            securityScopeService.RegisterSope(() => new CatalogSelectedCategoryScope(_container.Resolve<ICategoryService>()));         
        }

        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CatalogExportImport>();
            exportJob.DoExport(outStream, manifest, progressCallback);
        }

        public void DoImport(Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CatalogExportImport>();
            exportJob.DoImport(inputStream, manifest, progressCallback);
        }


        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Catalog.ExportImport.Description", String.Empty);
            }
        }
        #endregion
    }
}
