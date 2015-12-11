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

namespace VirtoCommerce.Content.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        #region Fields

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods and Operators

        public override void Initialize()
        {

            Func<IMenuRepository> menuRepFactory = () =>
                new DatabaseContentRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor());

            _container.RegisterInstance(menuRepFactory);
            _container.RegisterType<IMenuService, MenuServiceImpl>();


            var settingsManager = _container.Resolve<ISettingsManager>();

            var githubToken =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.Token", string.Empty);

            var githubProductHeaderValue =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.ProductHeaderValue", string.Empty);

            var githubOwnerName =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.OwnerName", string.Empty);

            var githubRepositoryName =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.RepositoryName", string.Empty);

            #region Themes_Initialize

            
            IThemeStorageProvider themeProvider = new ThemeStorageProviderImpl(@"rootPath=~/Themes;publicUrl=http://localhost/admin/Themes");
            _container.RegisterInstance(themeProvider);

            var githubMainPath = "Themes/";
            var fileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Themes/");

            var uploadPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
            var uploadPathFiles = HostingEnvironment.MapPath("~/App_Data/Uploads/Files/");



            Func<string, IThemeService> themesFactory = x =>
            {
                switch (x)
                {
                    case "GitHub":
                        return new ThemeServiceImpl(() =>
                            new GitHubContentRepositoryImpl(
                                githubToken,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                githubMainPath));

                    case "Database":
						return new ThemeServiceImpl(() =>
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
						return new ThemeServiceImpl(() => new FileSystemContentRepositoryImpl(fileSystemMainPath));

                    case "Azure and Database":
						return new ThemeServiceImpl(() =>
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()),
                            uploadPath); // TODO: It could be not the Azure provider.

                    default:
						return new ThemeServiceImpl(() => new FileSystemContentRepositoryImpl(fileSystemMainPath));
                }
            };

            var chosenThemeRepositoryName = settingsManager.GetValue("VirtoCommerce.Content.MainProperties.ThemesRepositoryType", string.Empty);
            var currentThemeService = themesFactory(chosenThemeRepositoryName);
            _container.RegisterInstance<IThemeService>(currentThemeService);

            if (!Directory.Exists(fileSystemMainPath))
            {
                Directory.CreateDirectory(fileSystemMainPath);
            }

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            else
            {
                var files = Directory.GetFiles(uploadPath);
                foreach (var file in files)
                    File.Delete(file);
            }

            if (!Directory.Exists(uploadPathFiles))
            {
                Directory.CreateDirectory(uploadPathFiles);
            }
            else
            {
                var files = Directory.GetFiles(uploadPathFiles);
                foreach (var file in files)
                    File.Delete(file);
            }

            var options = _container.Resolve<IModuleInitializerOptions>();
            var modulePath = options.GetModuleDirectoryPath("VirtoCommerce.Content");
            var themePath = Path.Combine(modulePath, "Default_Theme");

         

            #endregion

            #region Pages_Initialize

            var pagesGithubMainPath = "Pages/";
            var pagesFileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Pages/");

            Func<string, IPagesService> pagesFactory = (x) =>
            {
                switch (x)
                {
                    case "GitHub":
						return new PagesServiceImpl(() =>
                            new GitHubContentRepositoryImpl(
                                githubToken,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                pagesGithubMainPath));

                    case "Database":
						return new PagesServiceImpl(() =>
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
						return new PagesServiceImpl(() => new FileSystemContentRepositoryImpl(pagesFileSystemMainPath));

                    default:
						return new PagesServiceImpl(() => new FileSystemContentRepositoryImpl(pagesFileSystemMainPath));
                }
            };

			var chosenPagesRepositoryName = settingsManager.GetValue("VirtoCommerce.Content.MainProperties.PagesRepositoryType", string.Empty);
			var currentPagesService = pagesFactory(chosenPagesRepositoryName);
			_container.RegisterInstance(currentPagesService);

            if (!Directory.Exists(fileSystemMainPath))
            {
                Directory.CreateDirectory(fileSystemMainPath);
            }

            _container.RegisterType<PagesController>(new InjectionConstructor(pagesFactory, settingsManager, _container.Resolve<ISecurityService>(),
                                                                             _container.Resolve<IPermissionScopeService>()));

            _container.RegisterType<ContentExportImport>(new InjectionConstructor(_container.Resolve<IMenuService>(), themesFactory, pagesFactory, _container.Resolve<IStoreService>(), settingsManager));


            #endregion

        
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            //Create EnableQuote dynamic propertiy for  Store 
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
            var options = _container.Resolve<IModuleInitializerOptions>();
            var modulePath = options.GetModuleDirectoryPath("VirtoCommerce.Content");
            var themePath = Path.Combine(modulePath, "Default_Theme");

            using (var context = new DatabaseContentRepositoryImpl())
            {
				var initializer = new SetupDatabaseInitializer<DatabaseContentRepositoryImpl, Data.Migrations.Configuration>();
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
	}
}
