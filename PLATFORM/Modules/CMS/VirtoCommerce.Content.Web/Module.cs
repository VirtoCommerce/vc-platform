using System;
using System.IO;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Controllers.Api;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Content.Web.ExportImport;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Content.Web.Security;
using VirtoCommerce.Domain.Store.Services;
using System.Configuration;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {

        private readonly IUnityContainer _container;


        public Module(IUnityContainer container)
        {
            _container = container;
        }


        #region Public Methods and Operators

        public override void Initialize()
        {
            Func<IMenuRepository> menuRepFactory = () =>
                new ContentRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor());

            _container.RegisterInstance(menuRepFactory);
            _container.RegisterType<IMenuService, MenuServiceImpl>();

            var settingsManager = _container.Resolve<ISettingsManager>();
            var contentStoragePath = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Storefront.AppData.Path", settingsManager.GetValue("VirtoCommerce.Content.StoragePath", string.Empty));

            Func<IContentStorageProvider> contentProviderFactory = () =>  new ContentStorageProviderImpl(NormalizePath(contentStoragePath));
            _container.RegisterInstance(contentProviderFactory);
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            //Create EnableQuote dynamic property for  Store 
            var dynamicPropertyService = _container.Resolve<IDynamicPropertyService>();

            var defaultThemeNameProperty = new DynamicProperty
            {
                Id = "Default_Theme_Name_Property",
                Name = "DefaultThemeName",
                ObjectType = typeof(Store).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };

            dynamicPropertyService.SaveProperties(new[] { defaultThemeNameProperty });

            //Register bounded security scope types
            var securityScopeService = _container.Resolve<IPermissionScopeService>();
            securityScopeService.RegisterSope(() => new ContentSelectedStoreScope());
        }

        public override void SetupDatabase()
        {
            using (var context = new ContentRepositoryImpl())
            {
				var initializer = new SetupDatabaseInitializer<ContentRepositoryImpl, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }
        }

        #endregion

		#region ISupportExportImportModule Members

		public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
		{
			var exportJob = _container.Resolve<ContentExportImport>();
			exportJob.DoExport(outStream, manifest, progressCallback);
		}

		public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
		{
			var exportJob = _container.Resolve<ContentExportImport>();
			exportJob.DoImport(inputStream, manifest, progressCallback);
		}

		public string ExportDescription
		{
			get
			{
				var settingManager = _container.Resolve<ISettingsManager>();
				return settingManager.GetValue("VirtoCommerce.Content.ExportImport.Description", String.Empty);
			}
		}

		#endregion

        private string NormalizePath(string path)
        {
            var retVal = path;
            if(path.StartsWith("~"))
            {
                retVal = HostingEnvironment.MapPath(path);
            }
            else if(Path.IsPathRooted(path))
            {
                retVal = path;
            }
            else
            {
                retVal = HostingEnvironment.MapPath("~/");
                retVal += path;
            }
            return retVal;
        }
	}
}
