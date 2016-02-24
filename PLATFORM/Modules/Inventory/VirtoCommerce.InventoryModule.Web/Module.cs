using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.InventoryModule.Data.Repositories;
using VirtoCommerce.InventoryModule.Data.Services;
using VirtoCommerce.InventoryModule.Web.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.InventoryModule.Web
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
            using (var context = new InventoryRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<InventoryRepositoryImpl, VirtoCommerce.InventoryModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IInventoryRepository>(new InjectionFactory(c => new InventoryRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));

            _container.RegisterType<IInventoryService, InventoryServiceImpl>();
        }

        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<InventoryExportImport>();
            job.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<InventoryExportImport>();
            job.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Inventory.ExportImport.Description", String.Empty);
            }
        }
        #endregion


    }
}
