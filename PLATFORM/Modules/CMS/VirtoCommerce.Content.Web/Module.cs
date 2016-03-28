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
using System.Linq;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Content.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;
        private static string[] _possibleContentTypes = new [] { "pages", "themes" };


        public Module(IUnityContainer container)
        {
            _container = container;
        }


        #region Public Methods and Operators

        public override void Initialize()
        {
            Func<IMenuRepository> menuRepFactory = () =>
                new ContentRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor());

            _container.RegisterInstance(menuRepFactory);
            _container.RegisterType<IMenuService, MenuServiceImpl>();

            var settingsManager = _container.Resolve<ISettingsManager>();
            var blobConnectionString = BlobConnectionString.Parse(ConfigurationManager.ConnectionStrings["CmsContentConnectionString"].ConnectionString);

             Func<string, string, IContentBlobStorageProvider> contentProviderFactory = (contentType, storeId) =>
            {
                if (string.Equals(blobConnectionString.Provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    var storagePath = Path.Combine(NormalizePath(blobConnectionString.RootPath), contentType, storeId);
                    var publicUrl = blobConnectionString.PublicUrl + "/" + contentType + "/" + storeId;
                    //Do not export default theme (Themes/default) its will distributed with code
                    return new FileSystemContentBlobStorageProvider(storagePath, publicUrl, "/Themes/default");
                }
                else if (string.Equals(blobConnectionString.Provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    return new AzureContentBlobStorageProvider(blobConnectionString.ConnectionString, Path.Combine(blobConnectionString.RootPath, contentType.FirstCharToUpper(), storeId));
                }
                throw new NotImplementedException();
            };
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
            base.SetupDatabase();

            using (var context = new ContentRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
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
            if (path.StartsWith("~"))
            {
                retVal = HostingEnvironment.MapPath(path);
            }
            else if (Path.IsPathRooted(path))
            {
                retVal = path;
            }
            else
            {
                retVal = HostingEnvironment.MapPath("~/");
                retVal += path;
            }
            return Path.GetFullPath(retVal);
        }
    }
}
