using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Data.Packaging;

namespace VirtoCommerce.Platform.Tests
{
    [TestClass]
    public class ZipPackageServiceTests
    {
        private static string _tempDir;

        [TestMethod]
        public void ValidatePackage()
        {
            var service = GetPackageService();

            // Load module descriptor from package
            var module = service.OpenPackage(@"source\Sample.TestModule1_1.0.0.zip");
            WriteModuleLine(module);

            // Check if all dependencies are installed
            var dependencyErrors = service.GetDependencyErrors(module);
        }

        [TestMethod]
        public void InstallUpdateUninstall()
        {
            var service = GetPackageService();
            var progress = new Progress<ProgressMessage>(WriteProgressMessage);

            ListModules(service);

            service.Install(@"source\TestModule2_v1.0.0.zip", progress);
            ListModules(service);

            service.Install(@"source\TestModule1_v1.0.0.zip", progress);
            ListModules(service);

            service.Install(@"source\TestModule2_v1.0.0.zip", progress);
            ListModules(service);

            service.Update("TestModule2", @"source\TestModule2_v1.1.0.zip", progress);
            ListModules(service);

            service.Update("TestModule1", @"source\TestModule1_v1.1.0.zip", progress);
            ListModules(service);

            service.Update("TestModule2", @"source\TestModule2_v1.1.0.zip", progress);
            ListModules(service);

            service.Uninstall("TestModule1", progress);
            ListModules(service);

            service.Uninstall("TestModule2", progress);
            ListModules(service);

            service.Uninstall("TestModule1", progress);
            ListModules(service);
        }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Directory.Delete(_tempDir, true);
        }

        private static IPackageService GetPackageService()
        {
            var modulesPath = Path.Combine(_tempDir, @"modules");
            var packagesPath = Path.Combine(_tempDir, @"packages");

            var manifestProvider = new ModuleManifestProvider(modulesPath);

            var service = new ZipPackageService(null, manifestProvider, packagesPath);
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

        static void WriteProgressMessage(ProgressMessage message)
        {
            Debug.WriteLine("{0}: {1}", message.Level, message.Message);
        }
    }
}
