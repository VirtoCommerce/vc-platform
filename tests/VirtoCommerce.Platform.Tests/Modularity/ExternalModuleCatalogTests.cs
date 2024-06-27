using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;
using VirtoCommerce.Platform.Modules.Local;
using Xunit;
using Xunit.Extensions.Ordering;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    //the Order is needed for running separate UnitTests where static Platform.CurrentVersion is used
    [Collection("Modularity"), Order(1)]
    public class ExternalModuleCatalogTests
    {
        private static Mutex _mutex = new Mutex();

        [Fact]
        public void PublishNewVersionTest()
        {
            //Arrange
            var v3_0_0_beta1 = new ModuleManifest
            {
                Version = "3.0.0",
                VersionTag = "beta1",
                PlatformVersion = "3.0.0"
            };
            var v3_0_0 = new ModuleManifest
            {
                Version = "3.0.0",
                PlatformVersion = "3.0.0"
            };
            var v3_1_0_beta1 = new ModuleManifest
            {
                Version = "3.1.0",
                VersionTag = "beta1",
                PlatformVersion = "3.0.0"
            };
            var v3_1_0_beta2 = new ModuleManifest
            {
                Version = "3.1.0",
                VersionTag = "beta2",
                PlatformVersion = "3.0.0"
            };
            var v3_1_0 = new ModuleManifest
            {
                Version = "3.1.0",
                PlatformVersion = "3.0.0"
            };

            var extModuleManifest = new ExternalModuleManifest
            {
                Id = "A"
            };

            //Act
            extModuleManifest.PublishNewVersion(v3_0_0_beta1);
            //Assert
            Assert.True(extModuleManifest.Versions.Count() == 1);
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_0_0_beta1)));

            //Act
            extModuleManifest.PublishNewVersion(v3_0_0);
            //Assert
            Assert.True(extModuleManifest.Versions.Count() == 2);
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_0_0)));

            //Act
            extModuleManifest.PublishNewVersion(v3_1_0_beta1);
            //Assert
            Assert.True(extModuleManifest.Versions.Count() == 2);
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_0_0)));
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_1_0_beta1)));

            //Act
            extModuleManifest.PublishNewVersion(v3_1_0_beta2);
            //Assert
            Assert.True(extModuleManifest.Versions.Count() == 2);
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_0_0)));
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_1_0_beta2)));

            //Act
            extModuleManifest.PublishNewVersion(v3_1_0);
            //Assert
            Assert.True(extModuleManifest.Versions.Count() == 2);
            Assert.True(extModuleManifest.Versions.Contains(ExternalModuleManifestVersion.FromManifest(v3_1_0)));
        }

        [Theory]
        [InlineData("2.12.0", new[] { "1.5.0" }, false)]
        [InlineData("3.1.0", new[] { "2.1.0" }, false)]
        [InlineData("3.0.0", new[] { "2.1.0", "2.2.0-beta" }, true)]
        public void CreateDirectory_CreateTestDirectory(string platformVersion, string[] expectedModuleVersions, bool includePrerelease)
        {
            //Mutex is required to synhronize access to the static  PlatformVersion.CurrentVersion for one thread
            _mutex.WaitOne();
            //Arrange
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(platformVersion);
            var moduleA = new ExternalModuleManifest
            {
                Id = "A",
                Versions = new[]
                    {
                        new ExternalModuleManifestVersion
                        {
                             Version = "1.5.0",
                             PlatformVersion = "2.14.0"
                        },
                        new ExternalModuleManifestVersion
                        {
                             Version = "1.4.0",
                             PlatformVersion = "2.12.0"
                        },
                        new ExternalModuleManifestVersion
                        {
                             Version = "1.3.0",
                             PlatformVersion = "2.12.0"
                        },
                        new ExternalModuleManifestVersion
                        {
                             Version = "2.0.0",
                             PlatformVersion = "3.0.0"
                        },
                        new ExternalModuleManifestVersion
                        {
                             Version = "2.1.0",
                             PlatformVersion = "3.2.0"
                        },
                         new ExternalModuleManifestVersion
                        {
                             Version = "2.2.0",
                             PlatformVersion = "3.2.0",
                             VersionTag= "beta"
                        },
                    }
            };

            //Act
            var extCatalog = CreateExternalModuleCatalog(new[] { moduleA }, includePrerelease);
            extCatalog.Load();


            //Assert
            var actualVersions = extCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.Id == moduleA.Id).Select(x => x.Version);
            var expectedVersions = expectedModuleVersions.Select(x => SemanticVersion.Parse(x));

            Assert.Equal(expectedVersions, actualVersions);

            _mutex.ReleaseMutex();
        }

        [Theory]
        [InlineData("1.2.0", "1.3.0")]
        [InlineData("1.3.0-alpha1", "1.3.0")]
        [InlineData("1.4.0-alpha1", "1.4.0-alpha1")]
        [InlineData("1.4.0", "1.4.0")]
        public void CreateDirectory_NoDowngrades(string externalModuleVersion, string effectiveModuleVersion)
        {
            //Mutex is required to synhronize access to the static  PlatformVersion.CurrentVersion
            _mutex.WaitOne();
            //Arrange
            PlatformVersion.CurrentVersion = SemanticVersion.Parse("3.0.0");
            var modules = new[]
            {
                new ExternalModuleManifest
                {
                    Id = "B",
                    Versions = new []
                    {
                        new ExternalModuleManifestVersion
                        {
                             Version = externalModuleVersion,
                             PlatformVersion = "3.0.0"
                        }
                    }
                }
             };

            //Act
            var extCatalog = CreateExternalModuleCatalog(modules);
            extCatalog.Load();

            //Assert
            var module = extCatalog.Modules.FirstOrDefault() as ManifestModuleInfo;
            Assert.NotNull(module);
            Assert.Equal(SemanticVersion.Parse(effectiveModuleVersion), module.Version);

            _mutex.ReleaseMutex();
        }

        [Theory]
        [InlineData("1.0.0.0", null, true, true, true)]
        [InlineData("1.0.0.0", "1.0.0.0", false, true, false)]
        [InlineData("1.0.0.0", "1.0.0.0", false, false, true)]
        [InlineData("1.0.0.0", "1.0.0.1", false, true, false)]
        [InlineData("1.0.0.0", "1.0.0.1", false, false, false)]
        [InlineData("1.0.0.2", "1.0.0.1", false, true, true)]
        [InlineData("1.0.0.2", "1.0.0.1", false, false, true)]
        public void CopyManagedFilePolicyTests(string sourceVersion, string targetVersion, bool noTarget, bool isSourceNewByDate, bool copyRequired)
        {
            //Arrange
            var fileSystem = new MockFileSystem();
            var libraryVersionProvider = new Mock<IAssemblyMetadataProvider>();
            var sourceFilePath = @"c:\managed.dll";
            var targetFilePath = @"c:\target\managed.dll";

            fileSystem.AddFile(sourceFilePath, new MockFileData(new byte[] { 0x00, 0x01, 0x02, 0x03 }));

            var sourceFile = new MockFileInfo(fileSystem, sourceFilePath);

            libraryVersionProvider.Setup(x => x.GetVersion(sourceFilePath))
                .Returns(new Version(sourceVersion));
            libraryVersionProvider.Setup(x => x.IsManaged(sourceFilePath))
                .Returns(true);

            if (!noTarget)
            {
                fileSystem.AddFile(targetFilePath, new MockFileData(new byte[] { 0x00, 0x01, 0x02, 0x03 }));
                var targetFile = new MockFileInfo(fileSystem, targetFilePath);
                if (!isSourceNewByDate)
                {
                    targetFile.LastWriteTimeUtc = sourceFile.LastWriteTimeUtc.AddDays(-1);
                }
                libraryVersionProvider.Setup(x => x.GetVersion(targetFilePath))
                    .Returns(new Version(targetVersion));
                libraryVersionProvider.Setup(x => x.IsManaged(targetFilePath))
                    .Returns(true);
            }


            //Act
            var copyFilePolicy = new CopyFileRequirementValidator(fileSystem, libraryVersionProvider.Object);
            var requireCopy = copyFilePolicy.RequireCopy(Architecture.X64, sourceFilePath, targetFilePath);

            //Assert
            Assert.Equal(copyRequired, requireCopy.CopyRequired);
        }

        [Theory]
        [InlineData(Architecture.X64, Architecture.X64, Architecture.X64, true, false)]
        [InlineData(Architecture.X64, Architecture.X64, Architecture.X86, true, true)]
        [InlineData(Architecture.X64, Architecture.X86, Architecture.X64, true, false)]
        [InlineData(Architecture.X64, Architecture.X86, Architecture.X86, true, false)]
        [InlineData(Architecture.X64, Architecture.X64, Architecture.X64, false, true)]
        [InlineData(Architecture.X64, Architecture.X64, Architecture.X86, false, true)]
        [InlineData(Architecture.X64, Architecture.X86, Architecture.X64, false, false)]
        [InlineData(Architecture.X64, Architecture.X86, Architecture.X86, false, true)]
        [InlineData(Architecture.X86, Architecture.X64, Architecture.X64, true, false)]
        [InlineData(Architecture.X86, Architecture.X64, Architecture.X86, true, false)]
        [InlineData(Architecture.X86, Architecture.X86, Architecture.X64, true, true)]
        [InlineData(Architecture.X86, Architecture.X86, Architecture.X86, true, false)]
        [InlineData(Architecture.X86, Architecture.X64, Architecture.X64, false, true)]
        [InlineData(Architecture.X86, Architecture.X64, Architecture.X86, false, false)]
        [InlineData(Architecture.X86, Architecture.X86, Architecture.X64, false, true)]
        [InlineData(Architecture.X86, Architecture.X86, Architecture.X86, false, true)]
        public void CopyUnmanagedFilePolicyTests(Architecture currentArchitecture,
            Architecture sourceArchitecture, Architecture targetArchitecture,
            bool isSourceNewByDate, bool copyRequired)
        {
            //Arrange
            var fileSystem = new MockFileSystem();
            var libraryVersionProvider = new Mock<IAssemblyMetadataProvider>();
            var sourceFilePath = @"c:\unmanaged.dll";
            var targetFilePath = @"c:\target\unmanaged.dll";

            fileSystem.AddFile(sourceFilePath, new MockFileData(new byte[] { 0x00, 0x01, 0x02, 0x03 }));
            fileSystem.AddFile(targetFilePath, new MockFileData(new byte[] { 0x00, 0x01, 0x02, 0x03 }));

            var sourceFile = new MockFileInfo(fileSystem, sourceFilePath);
            var targetFile = new MockFileInfo(fileSystem, targetFilePath);
            if (!isSourceNewByDate)
            {
                targetFile.LastWriteTimeUtc = sourceFile.LastWriteTimeUtc.AddDays(-1);
            }

            libraryVersionProvider.Setup(x => x.GetVersion(sourceFilePath))
                .Returns(new Version("1.0.0.0"));
            libraryVersionProvider.Setup(x => x.IsManaged(sourceFilePath))
                .Returns(false);
            libraryVersionProvider.Setup(x => x.GetArchitecture(sourceFilePath))
                .Returns(sourceArchitecture);

            libraryVersionProvider.Setup(x => x.GetVersion(targetFilePath))
                .Returns(new Version("1.0.0.0"));
            libraryVersionProvider.Setup(x => x.IsManaged(targetFilePath))
                .Returns(false);
            libraryVersionProvider.Setup(x => x.GetArchitecture(targetFilePath))
                .Returns(targetArchitecture);

            //Act
            var copyFilePolicy = new CopyFileRequirementValidator(fileSystem, libraryVersionProvider.Object);
            var requireCopy = copyFilePolicy.RequireCopy(currentArchitecture, sourceFilePath, targetFilePath);

            //Assert
            Assert.Equal(copyRequired, requireCopy.CopyRequired);
        }

        private static ExternalModuleCatalog CreateExternalModuleCatalog(ExternalModuleManifest[] manifests, bool includePrerelease = false)
        {
            var localModulesCatalog = new Moq.Mock<ILocalModuleCatalog>();
            localModulesCatalog.Setup(x => x.Modules).Returns(GetManifestModuleInfos(new[] { new ModuleManifest { Id = "B", Version = "1.3.0", PlatformVersion = "3.0.0" } }));
            var json = JsonConvert.SerializeObject(manifests);
            var client = new Moq.Mock<IExternalModulesClient>();
            client.Setup(x => x.OpenRead(Moq.It.IsAny<Uri>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(json ?? "")));
            var logger = new Moq.Mock<ILogger<ExternalModuleCatalog>>();

            var options = Options.Create(new ExternalModuleCatalogOptions() { ModulesManifestUrl = new Uri("http://nowhere.mock"), IncludePrerelease = includePrerelease });
            var result = new ExternalModuleCatalog(localModulesCatalog.Object, client.Object, options, logger.Object);
            return result;
        }

        private static ManifestModuleInfo[] GetManifestModuleInfos(ModuleManifest[] moduleManifests)
        {
            return moduleManifests.Select(x =>
            {
                var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
                module.LoadFromManifest(x);
                return module;
            }).ToArray();
        }
    }
}
