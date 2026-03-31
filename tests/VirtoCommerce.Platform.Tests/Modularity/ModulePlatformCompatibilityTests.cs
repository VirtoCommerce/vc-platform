using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    [Collection("Modularity")]
    public class ModulePlatformCompatibilityTests
    {
        private readonly ModuleBootstrapper _bootstrapper = new(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());

        [Theory]
        [InlineData("3.1.0", "3.61.0", true)]
        [InlineData("3.61.0", "3.61.0", true)]
        [InlineData("2.12.0", "3.61.0", false)]
        [InlineData("3.70.0", "3.61.0", false)]
        [InlineData("3.61.3", "3.61.3", true)]
        [InlineData("3.61.4", "3.61.3", false)]
        public void ValidateInstall_PlatformVersionCheck(string targetPlatformVersion, string runningPlatformVersion, bool isCompatible)
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse(runningPlatformVersion);
            var module = CreateModule("A", "1.0.0", targetPlatformVersion);

            // Act
            var errors = _bootstrapper.ValidateInstall(module, installedModules: [], platformVersion);

            // Assert
            Assert.Equal(isCompatible, errors.Count == 0);

            if (!isCompatible)
            {
                Assert.Equal([$"Target platform version {targetPlatformVersion} is incompatible with current {runningPlatformVersion}"], errors);
            }
        }

        [Theory]
        [InlineData("3.1111.0", "3.800.0", false)] // Module targets higher platform → error
        [InlineData("3.800.0", "3.800.0", true)]   // Same version → ok
        [InlineData("3.1.0", "3.800.0", true)]     // Module targets lower platform → ok
        [InlineData("2.12.0", "3.800.0", false)]   // Different major → error
        public void ValidateModules_PlatformVersionIncompatibility_AddsError(string modulePlatformVersion, string runningPlatformVersion, bool isCompatible)
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse(runningPlatformVersion);
            var module = CreateModule("A", "1.0.0", modulePlatformVersion);

            // Act
            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            // Assert
            Assert.Equal(isCompatible, module.Errors.Count == 0);

            if (!isCompatible)
            {
                Assert.Equal([$"Module requires platform version {modulePlatformVersion}, which is incompatible with current version {runningPlatformVersion}"], module.Errors);
            }
        }

        [Fact]
        public void ValidateModules_MissingDependency_AddsError()
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = CreateModule("A", "1.0.0", platformVersion: "3.800.0", CreateDependency("B", "3.1111.0"));

            // Act
            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            // Assert
            Assert.Equal(["Module dependency B 3.1111.0 is not installed"], module.Errors);
        }

        [Fact]
        public void ValidateModules_IncompatibleDependencyVersion_AddsError()
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = CreateModule("A", "1.0.0", platformVersion: "3.800.0", CreateDependency("B", "3.1111.0"));
            var dependency = CreateModule("B", "3.100.0", platformVersion: "3.800.0");

            // Act
            _bootstrapper.ValidateModulesInternal([module, dependency], platformVersion);

            // Assert
            Assert.Equal(["Module dependency B 3.1111.0 is incompatible with installed 3.100.0"], module.Errors);
        }

        [Fact]
        public void ValidateModules_CompatibleDependency_NoError()
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = CreateModule("A", "1.0.0", platformVersion: "3.800.0", CreateDependency("B", "3.200.0"));
            var dependency = CreateModule("B", "3.500.0", platformVersion: "3.800.0");

            // Act
            _bootstrapper.ValidateModulesInternal([module, dependency], platformVersion);

            // Assert
            Assert.Empty(module.Errors);
        }

        [Fact]
        public void ValidateModules_OptionalMissingDependency_NoError()
        {
            // Arrange
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = CreateModule("A", "1.0.0", platformVersion: "3.800.0", CreateDependency("VirtoCommerce.Optional", "3.0.0", isOptional: true));

            // Act
            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            // Assert
            Assert.Empty(module.Errors);
        }

        [Fact]
        public void ValidateModules_CascadesErrorsToDependents()
        {
            // Arrange
            // Catalog targets too-high platform → fails validation.
            // xCart depends on Catalog → should also be marked as failed.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = CreateModule("A", "3.100.0", platformVersion: "3.800.0", CreateDependency("B", "3.800.0"));
            var dependency = CreateModule("B", "3.900.0", platformVersion: "3.1111.0");

            // Act
            _bootstrapper.ValidateModulesInternal([dependency, module], platformVersion);

            // Assert
            Assert.Equal(["Module requires platform version 3.1111.0, which is incompatible with current version 3.800.0"], dependency.Errors);
            Assert.Equal(["Module skipped because its dependency 'B' has errors"], module.Errors);
        }

        [Fact]
        public void ValidateModules_CascadesTransitively()
        {
            // Arrange
            // A fails → B depends on A → C depends on B → all three fail.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var moduleA = CreateModule("A", "1.0.0", platformVersion: "3.1111.0"); // fails
            var moduleB = CreateModule("B", "1.0.0", platformVersion: "3.800.0", CreateDependency("A", "1.0.0"));
            var moduleC = CreateModule("C", "1.0.0", platformVersion: "3.800.0", CreateDependency("B", "1.0.0"));

            // Act
            _bootstrapper.ValidateModulesInternal([moduleA, moduleB, moduleC], platformVersion);

            // Assert
            Assert.Equal(["Module requires platform version 3.1111.0, which is incompatible with current version 3.800.0"], moduleA.Errors);
            Assert.Equal(["Module skipped because its dependency 'A' has errors"], moduleB.Errors);
            Assert.Equal(["Module skipped because its dependency 'B' has errors"], moduleC.Errors);
        }

        [Fact]
        public void ValidateModules_OptionalDependencyFailure_DoesNotCascade()
        {
            // Arrange
            // A fails, B has optional dependency on A → B should NOT fail.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var a = CreateModule("A", "1.0.0", platformVersion: "3.1111.0"); // fails
            var b = CreateModule("B", "1.0.0", platformVersion: "3.800.0", CreateDependency("A", "1.0.0", isOptional: true));

            // Act
            _bootstrapper.ValidateModulesInternal([a, b], platformVersion);

            // Assert
            Assert.NotEmpty(a.Errors);
            Assert.Empty(b.Errors);
        }


        private static ManifestModuleInfo CreateModule(string id, string version, string platformVersion, ManifestDependency dependency = null)
        {
            var manifest = new ModuleManifest
            {
                Id = id,
                Version = version,
                PlatformVersion = platformVersion,
                Dependencies = dependency != null ? [dependency] : null,
            };

            var module = AbstractTypeFactory<ManifestModuleInfo>.TryCreateInstance();
            module.LoadFromManifest(manifest);

            return module;
        }

        private static ManifestDependency CreateDependency(string id, string version, bool isOptional = false)
        {
            return new ManifestDependency
            {
                Id = id,
                Version = version,
                Optional = isOptional,
            };
        }
    }
}
