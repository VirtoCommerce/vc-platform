using Microsoft.Practices.Unity;
using System.Web.Hosting;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PackagingModule.Data.Services;
using VirtoCommerce.PackagingModule.Web.Controllers.Api;
using VirtoCommerce.PackagingModule.Data.Repositories;
using NuGet;

namespace VirtoCommerce.PackagingModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            var sourcePath = HostingEnvironment.MapPath("~/App_Data/SourcePackages");
            var packagesPath = HostingEnvironment.MapPath("~/App_Data/InstalledPackages");

            var manifestProvider = _container.Resolve<IModuleManifestProvider>();
            var modulesPath = manifestProvider.RootPath;

            var projectSystem = new WebsiteProjectSystem(modulesPath);

            var nugetProjectManager = new ProjectManager(
                new WebsiteLocalPackageRepository(sourcePath),
                new DefaultPackagePathResolver(modulesPath),
                projectSystem,
                new ManifestPackageRepository(manifestProvider, new WebsitePackageRepository(packagesPath, projectSystem))
            );

            var packageService = new PackageService(nugetProjectManager);

            _container.RegisterType<ModulesController>(new InjectionConstructor(packageService, sourcePath));
        }

        #endregion
    }
}
