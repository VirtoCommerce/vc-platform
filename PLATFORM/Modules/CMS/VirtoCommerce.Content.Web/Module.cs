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

             Func<string, IContentBlobStorageProvider> contentProviderFactory = (chrootPath) =>
            {
                if (string.Equals(blobConnectionString.Provider, FileSystemBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    var storagePath = Path.Combine(NormalizePath(blobConnectionString.RootPath), chrootPath.Replace("/", "\\"));
                    var publicUrl = blobConnectionString.PublicUrl + "/" + chrootPath;
                    //Do not export default theme (Themes/default) its will distributed with code
                    return new FileSystemContentBlobStorageProvider(storagePath, publicUrl, "/Themes/default");
                }
                else if (string.Equals(blobConnectionString.Provider, AzureBlobProvider.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    return new AzureContentBlobStorageProvider(blobConnectionString.ConnectionString, Path.Combine(blobConnectionString.RootPath, chrootPath));
                }
                throw new NotImplementedException();
            };
            _container.RegisterInstance(contentProviderFactory);
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            //Register BaseThemes setting in store module, allows to individual configuration base themes names for each store
            var settingManager = _container.Resolve<ISettingsManager>();
            var baseThemesSetting = settingManager.GetModuleSettings("VirtoCommerce.Content").FirstOrDefault(x => x.Name == "VirtoCommerce.Content.BaseThemes");
            settingManager.RegisterModuleSettings("VirtoCommerce.Store", baseThemesSetting);

            var dynamicPropertyService = _container.Resolve<IDynamicPropertyService>();

            //https://jekyllrb.com/docs/frontmatter/
            //Register special ContentItem.FrontMatterHeaders type which will be used to define YAML headers for pages, blogs and posts
            var frontMatterHeaderType = "VirtoCommerce.Content.Web.FrontMatterHeaders";
            dynamicPropertyService.RegisterType(frontMatterHeaderType);
            //Title
            var titleHeader = new DynamicProperty
            {
                Id = "Title_FrontMatterHeader",
                Name = "title",
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };

            //If set, this specifies the layout file to use. Use the layout file name without the file extension. 
            var layoutHeader = new DynamicProperty
            {
                Id = "Layout_FrontMatterHeader",
                Name = "layout",                 
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            //If you need your processed blog post URLs to be something other than the site-wide style (default /year/month/day/title.html), then you can set this variable and it will be used as the final URL.
            var permalinkHeader = new DynamicProperty
            {
                Id = "Permalink_FrontMatterHeader",
                Name = "permalink",
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            //Set to false if you don’t want a specific post to show up when the site is generated.
            var publishedHeader = new DynamicProperty
            {
                Id = "Published_FrontMatterHeader",
                Name = "published",
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            //Instead of placing posts inside of folders, you can specify one or more categories that the post belongs to. When the site is generated the post will act as though it had been set with these categories normally. Categories (plural key) can be specified as a YAML list or a comma-separated string.
            var categoryHeader = new DynamicProperty
            {
                Id = "Category_FrontMatterHeader",
                Name = "category",
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            var categoriesHeader = new DynamicProperty
            {
                Id = "Categories_FrontMatterHeader",
                Name = "categories",
                IsArray = true,
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };
            //Similar to categories, one or multiple tags can be added to a post. Also like categories, tags can be specified as a YAML list or a comma-separated string.
            var tagsHeader = new DynamicProperty
            {
                Id = "Tags_FrontMatterHeader",
                Name = "tags",
                IsArray = true,
                ObjectType = frontMatterHeaderType,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };


            //Create DefaultTheme dynamic property for  Store 
            var defaultThemeNameProperty = new DynamicProperty
            {
                Id = "Default_Theme_Name_Property",
                Name = "DefaultThemeName",
                ObjectType = typeof(Store).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto"
            };

            dynamicPropertyService.SaveProperties(new[] { titleHeader, defaultThemeNameProperty, permalinkHeader, layoutHeader, publishedHeader, categoryHeader, categoriesHeader, tagsHeader });

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
