using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    public class ModuleInstallerUnitTests
    {
        private readonly LocalStorageModuleCatalogOptions _options;
        private readonly Mock<IExternalModulesClient> _externalClientMock;
        private readonly Mock<ITransactionFileManager> _fileManagerMock;
        private readonly Mock<IExternalModuleCatalog> _extModuleCatalogMock;
        private readonly Mock<IZipFileWrapper> _zipFileWrapperMock;

        public ModuleInstallerUnitTests()
        {
            _options = new LocalStorageModuleCatalogOptions { DiscoveryPath = "modules"};
            _externalClientMock = new Mock<IExternalModulesClient>();
            _fileManagerMock = new Mock<ITransactionFileManager>();
            _extModuleCatalogMock = new Mock<IExternalModuleCatalog>();
            _zipFileWrapperMock = new Mock<IZipFileWrapper>();
        }

        [Theory]
        [InlineData("3.0.0", "3.0.0", "3.0.0", "", true)]
        [InlineData("3.0.0", "3.1.0", "3.1.0", "", false)]
        [InlineData("3.0.0-alpha1", "3.0.0", "3.0.0", "", false)]
        [InlineData("3.1.0-alpha1", "3.1.0", "3.1.0", "", false)]
        [InlineData("3.1.0-alpha1", "3.1.0-alpha1", "3.1.0-alpha1", "3.0.0", false)]
        public void Install_Release_Installed(string platformCurrentVersion, string modulePlatformVersion, string moduleVersion, string moduleInstalledVersion, bool isInstalled)
        {
            //Arrange
            
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(platformCurrentVersion);
            var progress = new Progress<ProgressMessage>();
            var manifest = new ModuleManifest
            {
                Id = "SomeModule",
                Version = moduleVersion,
                PlatformVersion = modulePlatformVersion,
            };
            var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
            module.LoadFromManifest(manifest);
            
            var modules = new List<ManifestModuleInfo> { module };
            var fileSystemMock = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { Path.Combine(_options.DiscoveryPath, module.Id, $"{module.Id}_{module.Version}.zip"), new MockFileData("Testing is meh.") },
            });

            if (!string.IsNullOrEmpty(moduleInstalledVersion))
            {
                var installedModuleManifest = new ModuleManifest
                {
                    Id = "SomeModule",
                    Version = moduleInstalledVersion,
                    PlatformVersion = platformCurrentVersion,
                };
                var installedModule = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                installedModule.LoadFromManifest(installedModuleManifest);
                installedModule.IsInstalled = true;
                _extModuleCatalogMock.Setup(x => x.Modules)
                    .Returns(new List<ModuleInfo> { installedModule });
            }

            var assembly = typeof(ModuleInstallerUnitTests).Assembly;
            var zipFile = $"{typeof(ModuleInstallerUnitTests).Namespace}.{module.Id}.zip";

            using (var stream = assembly.GetManifestResourceStream(zipFile))
            {
                var zip = new ZipArchive(stream, ZipArchiveMode.Read, true);
                _zipFileWrapperMock.Setup(z => z.OpenRead(It.IsAny<string>()))
                    .Returns(zip);
                var service = GetModuleInstaller(fileSystemMock);

                //Act
                service.Install(modules, progress);
            }

            //Assert
            Assert.Equal(isInstalled, module.IsInstalled);
        }

        private ModuleInstaller GetModuleInstaller(IFileSystem fileSystem)
        {
            return new ModuleInstaller(_extModuleCatalogMock.Object,
                _externalClientMock.Object,
                _fileManagerMock.Object,
                Options.Create(_options),
                fileSystem,
                _zipFileWrapperMock.Object);
        }
    }
}
