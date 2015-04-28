using System;
using System.IO;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Controllers.Api;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Content.Web
{
    public class Module : IModule
    {
        #region Fields

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public Module(IUnityContainer container)
        {
            this._container = container;
        }

        #endregion

        #region Public Methods and Operators

        public void Initialize()
        {
            var repository = new DatabaseMenuRepositoryImpl(
                "VirtoCommerce",
                new AuditableInterceptor(),
                new EntityPrimaryKeyGeneratorInterceptor());

            var service = new MenuServiceImpl(repository);

            this._container.RegisterType<MenuController>(new InjectionConstructor(service));

            var settingsManager = this._container.Resolve<ISettingsManager>();

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
                            new GitHubFileRepositoryImpl(
                                githubLogin,
                                githubPassword,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                githubMainPath));

                    case "Database":
                        return new ThemeServiceImpl(
                            new DatabaseFileRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
                        return new ThemeServiceImpl(new FileSystemFileRepositoryImpl(fileSystemMainPath));

                    case "Azure and Database":
                        return new ThemeServiceImpl(
                            new DatabaseFileRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()),
                            this._container.Resolve<IBlobStorageProvider>(),
                            uploadPath); // TODO: It could be not the Azure provider.

                    default:
                        return new ThemeServiceImpl(new FileSystemFileRepositoryImpl(fileSystemMainPath));
                }
            };

            if (!Directory.Exists(fileSystemMainPath))
            {
                Directory.CreateDirectory(fileSystemMainPath);
            }

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (!Directory.Exists(uploadPathFiles))
            {
                Directory.CreateDirectory(uploadPathFiles);
            }

            this._container.RegisterType<ThemeController>(new InjectionConstructor(themesFactory, settingsManager, uploadPath, uploadPathFiles));

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
                            new GitHubPagesRepositoryImpl(
                                githubLogin,
                                githubPassword,
                                githubProductHeaderValue,
                                githubOwnerName,
                                githubRepositoryName,
                                pagesGithubMainPath));

                    case "Database":
                        return new PagesServiceImpl(
                            new DatabasePagesRepositoryImpl(
                                "VirtoCommerce",
                                new AuditableInterceptor(),
                                new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
                        return new PagesServiceImpl(new FileSystemPagesRepositoryImpl(pagesFileSystemMainPath));

                    default:
                        return new PagesServiceImpl(new FileSystemPagesRepositoryImpl(pagesFileSystemMainPath));
                }
            };

            if (!Directory.Exists(fileSystemMainPath))
            {
                Directory.CreateDirectory(fileSystemMainPath);
            }

            this._container.RegisterType<PagesController>(new InjectionConstructor(pagesFactory, settingsManager));

            #endregion

            #region Sync_Initialize
            this._container.RegisterType<SyncController>(new InjectionConstructor(themesFactory, pagesFactory, settingsManager));
            #endregion
        }

        public void PostInitialize()
        {
        }

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new DatabaseMenuRepositoryImpl())
            {
                var initializer = new SqlMenuDatabaseInitializer();
                initializer.InitializeDatabase(context);
            }

            #region Themes_SetupDatabase

            var options = this._container.Resolve<IModuleInitializerOptions>();
            var modulePath = options.GetModuleDirectoryPath("VirtoCommerce.Content");
            var themePath = Path.Combine(modulePath, "Default_Theme");

            using (var context = new DatabaseFileRepositoryImpl())
            {
                var initializer = new SqlThemeDatabaseInitializer(themePath);
                initializer.InitializeDatabase(context);
            }

            #endregion

            #region Pages_SetupDatabase

            using (var context = new DatabasePagesRepositoryImpl())
            {
                var initializer = new SqlPagesDatabaseInitializer();
                initializer.InitializeDatabase(context);
            }

            #endregion
        }

        #endregion
    }
}
