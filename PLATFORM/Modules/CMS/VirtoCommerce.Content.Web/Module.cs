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

namespace VirtoCommerce.Content.Web
{
    public class Module : ModuleBase
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

            var githubLogin =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.Login", string.Empty);

            var githubPassword =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.Password", string.Empty);

            var githubProductHeaderValue =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.ProductHeaderValue", string.Empty);

            var githubOwnerName =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.OwnerName", string.Empty);

            var githubRepositoryName =
                settingsManager.GetValue("VirtoCommerce.Content.GitHub.RepositoryName", string.Empty);

            #region Themes_Initialize

            var githubMainPath = "Themes/";
            var fileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Themes/");

            var uploadPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
            var uploadPathFiles = HostingEnvironment.MapPath("~/App_Data/Uploads/Files/");



            Func<string, IThemeService> themesFactory = x =>
            {
                switch (x)
                {
                    case "GitHub":
                        return new ThemeServiceImpl(
                            new GitHubContentRepositoryImpl(
                                githubLogin,
                                githubPassword,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                githubMainPath));

                    case "Database":
                        return new ThemeServiceImpl(
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
                        return new ThemeServiceImpl(new FileSystemContentRepositoryImpl(fileSystemMainPath));

                    case "Azure and Database":
                        return new ThemeServiceImpl(
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()),
                            uploadPath); // TODO: It could be not the Azure provider.

                    default:
                        return new ThemeServiceImpl(new FileSystemContentRepositoryImpl(fileSystemMainPath));
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

            _container.RegisterType<ThemeController>(new InjectionConstructor(themesFactory, settingsManager, uploadPath, uploadPathFiles, themePath));

            #endregion

            #region Pages_Initialize

            var pagesGithubMainPath = "/Pages/";
            var pagesFileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Pages/");

            Func<string, IPagesService> pagesFactory = (x) =>
            {
                switch (x)
                {
                    case "GitHub":
                        return new PagesServiceImpl(
                            new GitHubContentRepositoryImpl(
                                githubLogin,
                                githubPassword,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                pagesGithubMainPath));

                    case "Database":
                        return new PagesServiceImpl(
                            new DatabaseContentRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
                        return new PagesServiceImpl(new FileSystemContentRepositoryImpl(pagesFileSystemMainPath));

                    default:
                        return new PagesServiceImpl(new FileSystemContentRepositoryImpl(pagesFileSystemMainPath));
                }
            };

			var chosenPagesRepositoryName = settingsManager.GetValue("VirtoCommerce.Content.MainProperties.PagesRepositoryType", string.Empty);
			var currentPagesService = pagesFactory(chosenThemeRepositoryName);
			_container.RegisterInstance<IPagesService>(currentPagesService);

            if (!Directory.Exists(fileSystemMainPath))
            {
                Directory.CreateDirectory(fileSystemMainPath);
            }

            _container.RegisterType<PagesController>(new InjectionConstructor(pagesFactory, settingsManager));

            #endregion

            #region Sync_Initialize
            _container.RegisterType<SyncController>(new InjectionConstructor(themesFactory, pagesFactory, settingsManager));
            #endregion
        }

        public override void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            var options = _container.Resolve<IModuleInitializerOptions>();
            var modulePath = options.GetModuleDirectoryPath("VirtoCommerce.Content");
            var themePath = Path.Combine(modulePath, "Default_Theme");

            using (var context = new DatabaseContentRepositoryImpl())
            {
                var initializer = new SqlContentDatabaseInitializer(themePath);
                initializer.InitializeDatabase(context);
            }
        }

        #endregion
    }
}
