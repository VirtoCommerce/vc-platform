using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PackagingModule.Data.Repositories;
using VirtoCommerce.PackagingModule.Data.Services;
using VirtoCommerce.PackagingModule.Model;
using VirtoCommerce.PackagingModule.Services;

namespace VirtoCommerce.PackagingModule.Tests
{
    [TestClass]
    public class PackageServiceTests
    {
        [TestMethod]
        public void ValidatePackage()
        {
            var service = GetPackageService();

            // Load module descriptor from package
            var module = service.OpenPackage(@"source\TestModule2.1.0.0.0.nupkg");
            WriteModuleLine(module);

            // Check if all dependencies are installed
            var modules = service.GetModules();
            var allDependeciesAreInstalled = module.Dependencies.All(dependency => modules.Any(m => m.Id == dependency));
        }

        [TestMethod]
        public void InstallUpdateUninstall()
        {
            const string package1 = "TestModule1";
            const string package2 = "TestModule2";

            var service = GetPackageService();
            ListModules(service);

            service.Install(package2, "1.0", null);
            ListModules(service);

            service.Update(package2, "1.1", null);
            ListModules(service);

            service.Uninstall(package2, null);
            ListModules(service);

            service.Uninstall(package1, null);
            ListModules(service);
        }


        private static PackageService GetPackageService()
        {
            var sourcePath = Path.GetFullPath("source");
            var modulesPath = Path.GetFullPath(@"target\modules");
            var packagesPath = Path.GetFullPath(@"target\packages");

            var manifestProvider = new ModuleManifestProvider(modulesPath);
            var projectSystem = new WebsiteProjectSystem(modulesPath);

            var nugetProjectManager = new ProjectManager(new WebsiteLocalPackageRepository(sourcePath),
                new DefaultPackagePathResolver(modulesPath),
                projectSystem,
                new ManifestPackageRepository(manifestProvider, new WebsitePackageRepository(packagesPath, projectSystem)));

            var service = new PackageService(nugetProjectManager);
            return service;
        }

        static void ListModules(IPackageService service)
        {
            var modules = service.GetModules();
            Debug.WriteLine("Modules count: {0}", modules.Length);

            foreach (var module in modules)
            {
                WriteModuleLine(module);
            }
        }

        static void WriteModuleLine(ModuleDescriptor module)
        {
            Debug.WriteLine("{0} {1}", module.Id, module.Version);
        }
    }
}
