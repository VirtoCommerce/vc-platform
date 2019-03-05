using Common.Logging;
using Common.Logging.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Testing.Bases;
using VirtoCommerce.Platform.Web.Modularity;
using Xunit;
using Xunit.Abstractions;

namespace VirtoCommerce.Platform.Tests
{
    [Trait("Category", "CI")]
    public class ModuleCatalogScenarios : FunctionalTestBase
    {
        private static string _tempDir;
        private static string _CatalogSourceFolder = "source";
        private static string _CatalogDependencySourceFolder = "";
        private readonly ITestOutputHelper _output;

        public ModuleCatalogScenarios(ITestOutputHelper output)
        {
            _output = output;
            var folderVariable = Environment.GetEnvironmentVariable("xunit_virto_modules_folder");
            if (!string.IsNullOrEmpty(folderVariable))
                _CatalogSourceFolder = folderVariable;

            var folderVariable2 = Environment.GetEnvironmentVariable("xunit_virto_dependency_modules_folder");
            if (!string.IsNullOrEmpty(folderVariable2))
                _CatalogDependencySourceFolder = folderVariable2;

            _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
        }

        [Fact]
        public void Can_list_all_modules()
        {
            var catalog = GetModuleCatalog(_CatalogSourceFolder);
            ListModules(catalog);
        }

        [Fact]
        public void Can_install_and_uninstall_modules()
        {
            // load all modules from local folder
            var catalogInternal = GetModuleCatalog(_CatalogSourceFolder);
            var modules = catalogInternal.Modules.OfType<ManifestModuleInfo>().ToArray();

            // when local dependency defined, use that to create catalog
            if(!string.IsNullOrEmpty(_CatalogDependencySourceFolder))
            {
                catalogInternal = GetModuleCatalog(_CatalogDependencySourceFolder);
                if (modules != null)
                {
                    foreach (var module in modules)
                    {
                        catalogInternal.AddModule(module);
                    }
                }
            }

            // initialize exteranl dependency loader
            var catalog = GetExternalModuleCatalog(catalogInternal);

            // make sure modules are populated
            catalog.Initialize();

            // create installer
            var moduleInstaller = new ModuleInstaller(_tempDir, catalog);
            var progress = new Progress<ProgressMessage>(WriteProgressMessage);

            // list what we have defined so far
            ListModules(catalog);

            // install all modules with dependencies
            var modulesWithDependencies = catalog.CompleteListWithDependencies(modules)
                                       .OfType<ManifestModuleInfo>()
                                       .Where(x => !x.IsInstalled)
                                       .Except(modules)
                                       .ToArray();

            moduleInstaller.Install(modules.Union(modulesWithDependencies), progress);
            ListModules(catalog);

            // validate installed modules, if it doesn't equal then modules were not installed
            Assert.Equal(
                modules.Union(modulesWithDependencies).Count(), 
                modules.Union(modulesWithDependencies).OfType<ManifestModuleInfo>().Where(x => x.IsInstalled).Count()
            );

            moduleInstaller.Uninstall(modules.Union(modulesWithDependencies), progress);
            ListModules(catalog);
            Assert.Empty(modules.Union(modulesWithDependencies).OfType<ManifestModuleInfo>().Where(x => x.IsInstalled));
        }

        public override void Dispose()
        {
            Directory.Delete(_tempDir, true);
        }

        private IModuleCatalog GetModuleCatalog(string folder)
        {
            var catalog = new ModuleCatalog();

            var modules = ReadAllModules(folder);
            foreach (var module in modules)
            {
                catalog.AddModule(module);
            }

            return catalog;
        }

        private IModuleCatalog GetExternalModuleCatalog(IModuleCatalog local)
        {
            var logger = new ConsoleOutLogger("test", LogLevel.All, true, true, true, "yyyy/MM/dd HH:mm:ss:fff");
            var externalModuleCatalog = new ExternalManifestModuleCatalog(local.Modules, new[] { "https://raw.githubusercontent.com/VirtoCommerce/vc-modules/master/modules.json" }, logger);
            return externalModuleCatalog;
        }

        void ListModules(IModuleCatalog service)
        {
            var modules = service.Modules.OfType<ManifestModuleInfo>().ToArray();
            Debug.WriteLine("Modules count: {0}", modules.Length);

            foreach (var module in modules)
            {
                WriteModuleLine(module);
            }
        }

        private ManifestModuleInfo[] ReadAllModules(string folder)
        {
            var allModules = new List<ManifestModuleInfo>();
            var moduleFiles = Directory.GetFiles(folder);
            foreach (var moduleFile in moduleFiles)
            {
                var module = ReadModule(moduleFile);
                allModules.Add(module);
            }

            return allModules.ToArray();
        }

        private ManifestModuleInfo ReadModule(string moduleFile)
        {
            using (var packageStream = File.Open(moduleFile, FileMode.Open))
            using (var package = new ZipArchive(packageStream, ZipArchiveMode.Read))
            {
                var entry = package.GetEntry("module.manifest");
                if (entry != null)
                {
                    using (var manifestStream = entry.Open())
                    {
                        var manifest = ManifestReader.Read(manifestStream);
                        var module = new ManifestModuleInfo(manifest);
                        module.Ref = packageStream.Name;
                        return module;
                    }
                }
            }

            return null;
        }

        void WriteModuleLine(ManifestModuleInfo module)
        {
            _output.WriteLine("{0} {1} {2}", module.IsInstalled ? "INSTALLED" : "", module.Id, module.Version);
        }

        void WriteProgressMessage(ProgressMessage message)
        {
            _output.WriteLine("{0}: {1}", message.Level, message.Message);
        }
    }
}
