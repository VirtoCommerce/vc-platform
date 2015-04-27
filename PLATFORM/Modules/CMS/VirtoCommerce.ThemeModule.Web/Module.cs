using System;
using System.IO;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.ThemeModule.Web.Controllers.Api;

namespace VirtoCommerce.ThemeModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            var options = _container.Resolve<IModuleInitializerOptions>();
            var modulePath = options.GetModuleDirectoryPath("VirtoCommerce.Theme");
            var themePath = Path.Combine(modulePath, "Default_Theme");

            using (var context = new DatabaseFileRepositoryImpl())
            {
                var initializer = new SqlThemeDatabaseInitializer(themePath);
                initializer.InitializeDatabase(context);
            }
        }

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();

            var githubLogin = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Login", string.Empty);
            var githubPassword = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Password", string.Empty);
            var githubProductHeaderValue =
                settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.ProductHeaderValue", string.Empty);
            var githubOwnerName = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.OwnerName", string.Empty);
            var githubRepositoryName = settingsManager.GetValue(
                "VirtoCommerce.ThemeModule.GitHub.RepositoryName",
                string.Empty);

            var githubMainPath = "Themes/";
            var fileSystemMainPath = HostingEnvironment.MapPath("~/App_Data/Themes/");

            var uploadPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
            var uploadPathFiles = HostingEnvironment.MapPath("~/App_Data/Uploads/Files/");

            Func<string, IThemeService> factory = x =>
            {
                switch (x)
                {
                    case "GitHub":
                        return new ThemeServiceImpl(new GitHubFileRepositoryImpl(
                            githubLogin,
                            githubPassword,
                            githubProductHeaderValue,
                            githubOwnerName,
                            githubRepositoryName,
                            githubMainPath));

                    case "Database":
                        return new ThemeServiceImpl(new DatabaseFileRepositoryImpl("VirtoCommerce",
                            new AuditableInterceptor(),
                            new EntityPrimaryKeyGeneratorInterceptor()));

                    case "File System":
                        return new ThemeServiceImpl(new FileSystemFileRepositoryImpl(fileSystemMainPath));

                    case "Azure and Database":
                        return new ThemeServiceImpl(new DatabaseFileRepositoryImpl("VirtoCommerce",
                            new AuditableInterceptor(),
                            new EntityPrimaryKeyGeneratorInterceptor()), _container.Resolve<IBlobStorageProvider>(), uploadPath); // TODO: It could be not the Azure provider.

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

            _container.RegisterType<ThemeController>(new InjectionConstructor(factory, settingsManager, uploadPath, uploadPathFiles));
        }

        public void PostInitialize()
        {
        }

        #endregion
    }
}
