namespace VirtoCommerce.ThemeModule.Web
{
    #region

    using System;

    using Microsoft.Practices.Unity;

    using VirtoCommerce.Content.Data.Repositories;
    using VirtoCommerce.Framework.Web.Modularity;
    using VirtoCommerce.Framework.Web.Settings;
    using VirtoCommerce.ThemeModule.Web.Controllers.Api;

    #endregion

    public class Module : IModule //, IDatabaseModule
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
            var settingsManager = this._container.Resolve<ISettingsManager>();

            var githubLogin = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Login", string.Empty);
            var githubPassword = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.Password", string.Empty);
            var githubProductHeaderValue =
                settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.ProductHeaderValue", string.Empty);
            var githubOwnerName = settingsManager.GetValue("VirtoCommerce.ThemeModule.GitHub.OwnerName", string.Empty);
            var githubRepositoryName = settingsManager.GetValue(
                "VirtoCommerce.ThemeModule.GitHub.RepositoryName",
                string.Empty);

            Func<string, IFileRepository> factory = (x) =>
            {
                switch (x)
                {
                    case "GitHub":
                        return new GitHubFileRepositoryImpl(
                            githubLogin,
                            githubPassword,
                            githubProductHeaderValue,
                            githubOwnerName,
                            githubRepositoryName);

                    case "Database":
                        return new DatabaseFileRepositoryImpl();

                    case "File System":
                        return new FileSystemFileRepositoryImpl();

                    default:
                        return new FileSystemFileRepositoryImpl();
                }
            };

            //if(!Directory.Exists(HostingEnvironment.MapPath("~/Themes/")))
            //{
            //	Directory.CreateDirectory(HostingEnvironment.MapPath("~/Themes/"));
            //}

            this._container.RegisterType<ThemeController>(new InjectionConstructor(factory, settingsManager));
        }

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        #endregion
    }
}