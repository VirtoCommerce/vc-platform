using System;
using System.IO;
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

namespace VirtoCommerce.Platform.Tests.Modularity
{
    [Collection("Modularity")]
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
        [InlineData(false, null, true)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void NonExecutableFilesCopyTest(
            bool targetExists,
            bool? isSourceNewByDate,
            bool expectedCopyRequired)
        {
            //Arrange
            var metadataProvider = new Mock<IFileMetadataProvider>();
            var sourcePath = @"c:\source\non_executable.xml";
            var targetPath = @"c:\target\non_executable.xml";
            var targetDate = DateTime.UtcNow;
            var sourceDate = isSourceNewByDate == true ? targetDate.AddDays(1) : targetDate;

            AddFile(metadataProvider, sourcePath, sourceDate, version: null, architecture: null);

            if (targetExists)
            {
                AddFile(metadataProvider, targetPath, targetDate, version: null, architecture: null);
            }

            //Act
            var copyFilePolicy = new FileCopyPolicy(metadataProvider.Object);
            var actualCopyRequired = copyFilePolicy.IsCopyRequired(Architecture.X64, sourcePath, targetPath, out _);

            //Assert
            Assert.Equal(expectedCopyRequired, actualCopyRequired);
        }

        [Theory]
        [InlineData(null, false, null, null, true)]
        [InlineData(null, true, "1.0.0.0", true, false)]
        [InlineData(null, true, "1.0.0.0", false, false)]
        [InlineData("1.0.0.0", false, null, null, true)]
        [InlineData("1.0.0.0", true, "1.0.0.0", false, false)]
        [InlineData("1.0.0.0", true, "1.0.0.0", true, true)]
        [InlineData("1.0.0.0", true, "1.0.0.1", false, false)]
        [InlineData("1.0.0.0", true, "1.0.0.1", true, false)]
        [InlineData("1.0.0.2", true, "1.0.0.1", false, true)]
        [InlineData("1.0.0.2", true, "1.0.0.1", true, true)]
        public void ExecutableFilesWithDifferentVersionsCopyTest(
            string sourceVersion,
            bool targetExists,
            string targetVersion,
            bool? isSourceNewByDate,
            bool expectedCopyRequired)
        {
            //Arrange
            var metadataProvider = new Mock<IFileMetadataProvider>();
            var sourcePath = @"c:\source\assembly.dll";
            var targetPath = @"c:\target\assembly.dll";
            var targetDate = DateTime.UtcNow;
            var sourceDate = isSourceNewByDate == true ? targetDate.AddDays(1) : targetDate;

            AddFile(metadataProvider, sourcePath, sourceDate, sourceVersion, Architecture.X64);

            if (targetExists)
            {
                AddFile(metadataProvider, targetPath, targetDate, targetVersion, Architecture.X64);
            }

            //Act
            var copyFilePolicy = new FileCopyPolicy(metadataProvider.Object);
            var actualCopyRequired = copyFilePolicy.IsCopyRequired(Architecture.X64, sourcePath, targetPath, out _);

            //Assert
            Assert.Equal(expectedCopyRequired, actualCopyRequired);
        }

        [Theory]
        [InlineData(Architecture.X64, null, false, null, null, true)]
        [InlineData(Architecture.X64, null, true, null, true, true)]
        [InlineData(Architecture.X64, null, true, null, false, false)]
        [InlineData(Architecture.X64, null, true, Architecture.X64, true, false)]
        [InlineData(Architecture.X64, null, true, Architecture.X86, true, false)]
        [InlineData(Architecture.X64, Architecture.X64, false, null, null, true)]
        [InlineData(Architecture.X64, Architecture.X86, false, null, null, true)]
        [InlineData(Architecture.X86, Architecture.X86, false, null, null, true)]
        [InlineData(Architecture.X86, Architecture.X64, false, null, null, false)]
        [InlineData(Architecture.X64, Architecture.X64, true, null, true, true)]
        [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X64, false, false)]
        [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X86, false, true)]
        [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X64, false, false)]
        [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X86, false, false)]
        [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X64, true, true)]
        [InlineData(Architecture.X64, Architecture.X64, true, Architecture.X86, true, true)]
        [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X64, true, false)]
        [InlineData(Architecture.X64, Architecture.X86, true, Architecture.X86, true, true)]
        [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X64, false, false)]
        [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X86, false, false)]
        [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X64, false, true)]
        [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X86, false, false)]
        [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X64, true, true)]
        [InlineData(Architecture.X86, Architecture.X64, true, Architecture.X86, true, false)]
        [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X64, true, true)]
        [InlineData(Architecture.X86, Architecture.X86, true, Architecture.X86, true, true)]
        public void ExecutableFilesWithDifferentArchitectureCopyTest(
            Architecture environment,
            Architecture? sourceArchitecture,
            bool targetExists,
            Architecture? targetArchitecture,
            bool? isSourceNewByDate,
            bool expectedCopyRequired)
        {
            //Arrange
            var metadataProvider = new Mock<IFileMetadataProvider>();
            var sourcePath = @"c:\source\assembly.dll";
            var targetPath = @"c:\target\assembly.dll";
            var targetDate = DateTime.UtcNow;
            var sourceDate = isSourceNewByDate == true ? targetDate.AddDays(1) : targetDate;

            AddFile(metadataProvider, sourcePath, sourceDate, "1.0.0.0", sourceArchitecture);

            if (targetExists)
            {
                AddFile(metadataProvider, targetPath, targetDate, "1.0.0.0", targetArchitecture);
            }

            //Act
            var copyFilePolicy = new FileCopyPolicy(metadataProvider.Object);
            var actualCopyRequired = copyFilePolicy.IsCopyRequired(environment, sourcePath, targetPath, out _);

            //Assert
            Assert.Equal(expectedCopyRequired, actualCopyRequired);
        }

        private static void AddFile(Mock<IFileMetadataProvider> metadataProvider, string path, DateTime date, string version, Architecture? architecture)
        {
            metadataProvider.Setup(x => x.Exists(path)).Returns(true);
            metadataProvider.Setup(x => x.GetDate(path)).Returns(date);
            metadataProvider.Setup(x => x.GetVersion(path)).Returns(version is null ? null : new Version(version));
            metadataProvider.Setup(x => x.GetArchitecture(path)).Returns(architecture);
        }

        private static ExternalModuleCatalog CreateExternalModuleCatalog(ExternalModuleManifest[] manifests, bool includePrerelease = false)
        {
            var localModulesCatalog = new Mock<ILocalModuleCatalog>();
            localModulesCatalog.Setup(x => x.Modules).Returns(GetManifestModuleInfos(new[] { new ModuleManifest { Id = "B", Version = "1.3.0", PlatformVersion = "3.0.0" } }));
            var json = JsonConvert.SerializeObject(manifests);
            var client = new Mock<IExternalModulesClient>();
            client.Setup(x => x.OpenRead(It.IsAny<Uri>())).Returns(new MemoryStream(Encoding.UTF8.GetBytes(json ?? "")));
            var logger = new Mock<ILogger<ExternalModuleCatalog>>();

            var options = Options.Create(new ExternalModuleCatalogOptions() { ModulesManifestUrl = new Uri("http://nowhere.mock"), IncludePrerelease = includePrerelease });
            var boostOptions = Options.Create(new ModuleSequenceBoostOptions());
            var result = new ExternalModuleCatalog(localModulesCatalog.Object, client.Object, options, logger.Object, boostOptions);
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
