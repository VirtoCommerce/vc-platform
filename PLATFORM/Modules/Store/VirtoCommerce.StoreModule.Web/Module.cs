using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.StoreModule.Data.Notifications;
using VirtoCommerce.StoreModule.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Services;
using VirtoCommerce.StoreModule.Web.ExportImport;
using VirtoCommerce.StoreModule.Web.Security;

namespace VirtoCommerce.StoreModule.Web
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
            using (var db = new StoreRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<StoreRepositoryImpl, Data.Migrations.Configuration>();

                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IStoreRepository>(new InjectionFactory(c => new StoreRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));
            _container.RegisterType<IStoreService, StoreServiceImpl>();
        }

        public override void PostInitialize()
        {

            //Register bounded security scope types
            var securityScopeService = _container.Resolve<IPermissionScopeService>();
            securityScopeService.RegisterSope(() => new StoreSelectedScope());

            var notificationManager = _container.Resolve<INotificationManager>();

            notificationManager.RegisterNotificationType(() => new StoreDynamicEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Sending custom form from storefront",
                Description = "This notification sends by email to client when he complite some form on storefront, for example contact us form",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = "",
                    Subject = "",
                    Language = "en-US"
                }
            });
        }
        #endregion

        #region ISupportExportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<StoreExportImport>();
            exportJob.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<StoreExportImport>();
            exportJob.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Stores.ExportImport.Description", String.Empty);
            }
        }

        #endregion
    }
}
