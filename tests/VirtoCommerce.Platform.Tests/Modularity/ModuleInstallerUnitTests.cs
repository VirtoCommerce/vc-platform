using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;
using Xunit;
using Xunit.Extensions.Ordering;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    //the Order need for saparate runing UnitTests where use static Platform.CurrentVersion
    [Collection("Modularity"), Order(3)]
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
        [ClassData(typeof(ModularityTestData))]
        public void Install_Release_Installed(string currentVersionPlatform, ModuleManifest[] moduleManifests, ModuleManifest[] installedModuleManifests, bool isInstalled)
        {
            //Arrange
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(currentVersionPlatform);
            var progress = new Progress<ProgressMessage>();

            var modules = GetManifestModuleInfos(moduleManifests);
            var installedModules = GetManifestModuleInfos(installedModuleManifests);
            
            _extModuleCatalogMock.Setup(x => x.Modules)
                .Returns(installedModules.Select(x => { x.IsInstalled = true; return x; }));

            var service = GetModuleInstaller();

            //Act
            service.Install(modules, progress);            

            //Assert
            modules.All(x => x.IsInstalled).Should().Be(isInstalled);
        }

        private ManifestModuleInfo[] GetManifestModuleInfos(ModuleManifest[] moduleManifests)
        {
            return moduleManifests.Select(x =>
            {
                var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                module.LoadFromManifest(x);
                return module;
            }).ToArray();
        }

        private ModuleInstaller GetModuleInstaller()
        {
            return new ModuleInstaller(_extModuleCatalogMock.Object,
                _externalClientMock.Object,
                _fileManagerMock.Object,
                Options.Create(_options),
                new MockFileSystem(),
                _zipFileWrapperMock.Object);
        }


        class ModularityTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Scenario 1
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } }
                    },
                    true
                };
                //2
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.2.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } } }
                    },
                    true
                };
                //3
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.2.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //4
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //5
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "4.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } },
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } } }
                    },
                    true
                };
                //6
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }} },
                    new[]
                    {
                        //installed
                        new ModuleManifest { Id = "B", Version = "4.0.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } } }
                    },
                    true
                };
                //7
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                        new ModuleManifest { Id = "C", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }},
                        new ModuleManifest { Id = "D", Version = "3.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.0.0" } }}
                    },
                    Array.Empty<ModuleManifest>(),
                    true
                };
                //8
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "3.5.0" } }},
                        new ModuleManifest { Id = "B", Version = "3.6.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }}
                    },
                    new[] { new ModuleManifest { Id = "B", Version = "3.0.0", PlatformVersion = "3.0.0" }},
                    true
                };
                //9
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0" }
                    },
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "D", Version = "3.2.0" } }},
                        new ModuleManifest { Id = "B", Version = "3.5.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "C", Version = "3.0.0" } }},
                        new ModuleManifest { Id = "C", Version = "3.9.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }},
                        new ModuleManifest { Id = "D", Version = "3.2.0", PlatformVersion = "3.0.0" }
                    },
                    true
                };
                //10
                yield return new object[]
                {
                    "3.0.0",
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "4.0.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "B", Version = "4.0.0" } }}
                    },
                    new[]
                    {
                        new ModuleManifest { Id = "A", Version = "3.8.0", PlatformVersion = "3.0.0" },
                        new ModuleManifest { Id = "C", Version = "3.9.0", PlatformVersion = "3.0.0", Dependencies = new []{ new ManifestDependency { Id = "A", Version = "3.0.0" } }}
                    },
                    false
                };

                yield return new object[] { "3.1.0-preview-5555", new[] { new ModuleManifest { Id = "A", Version = "3.2.0-prerelease-22", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.1.0", new[] { new ModuleManifest { Id = "A", Version = "3.2.0-prerelease-22", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.0.1", PlatformVersion = "3.0.1" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[] { "3.0.0-alpha1", new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[] { "3.0.0-alpha1", new[] { new ModuleManifest { Id = "A", Version = "3.0.1", PlatformVersion = "3.0.1" } }, Array.Empty<ModuleManifest>(), false };
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0",
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.1.0", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[] { "3.0.0", new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } }, Array.Empty<ModuleManifest>(), true };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.0.0", PlatformVersion = "3.0.0" } }, //installed
                    false
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.1.0-alpha002", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha001", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
                yield return new object[]
                {
                    "3.0.0-alpha001",
                    new[] {new ModuleManifest {Id = "A", Version = "3.0.0-alpha001", PlatformVersion = "3.0.0-alpha001" } },
                    new[] { new ModuleManifest { Id = "A", Version = "3.1.0-alpha002", PlatformVersion = "3.0.0-alpha001" } }, //installed
                    true
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
