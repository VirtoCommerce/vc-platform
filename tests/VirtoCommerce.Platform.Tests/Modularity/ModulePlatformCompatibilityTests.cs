using System.Linq;
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
        private readonly ModuleBootstrapper _bootstrapper = new(
            NullLoggerFactory.Instance,
            new LocalStorageModuleCatalogOptions());

        [Theory]
        [InlineData("3.1.0", "3.61.0", false)]
        [InlineData("3.61.0", "3.61.0", false)]
        [InlineData("2.12.0", "3.61.0", true)]
        [InlineData("3.70.0", "3.61.0", true)]
        [InlineData("3.61.3", "3.61.3", false)]
        [InlineData("3.61.4", "3.61.3", true)]
        public void ValidateInstall_PlatformVersionCheck(string targetPlatformVersion, string runningPlatformVersion, bool violation)
        {
            var platformVersion = SemanticVersion.Parse(runningPlatformVersion);
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { PlatformVersion = targetPlatformVersion, Id = "Fake", Version = "0.0.0" });

            var errors = _bootstrapper.ValidateInstall(module, [], platformVersion);

            Assert.Equal(violation, errors.Count > 0);
        }

        [Theory]
        [InlineData("3.1111.0", "3.800.0", true)] // Module targets higher platform → error
        [InlineData("3.800.0", "3.800.0", false)] // Same version → ok
        [InlineData("3.1.0", "3.800.0", false)]   // Module targets lower platform → ok
        [InlineData("2.12.0", "3.800.0", true)]   // Different major → error
        public void ValidateModules_PlatformVersionIncompatibility_AddsError(string modulePlatformVersion, string runningPlatformVersion, bool hasError)
        {
            var platformVersion = SemanticVersion.Parse(runningPlatformVersion);
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { PlatformVersion = modulePlatformVersion, Id = "TestModule", Version = "1.0.0" });

            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            Assert.Equal(hasError, module.Errors.Count > 0);
        }

        [Fact]
        public void ValidateModules_MissingDependency_AddsError()
        {
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "TestModule",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "VirtoCommerce.Assets", Version = "3.1111.0" }],
                });

            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            Assert.Single(module.Errors);
            Assert.Contains("VirtoCommerce.Assets", module.Errors.First());
            Assert.Contains("not installed", module.Errors.First());
        }

        [Fact]
        public void ValidateModules_IncompatibleDependencyVersion_AddsError()
        {
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var dependency = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { Id = "VirtoCommerce.Assets", Version = "3.100.0", PlatformVersion = "3.800.0" });
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "TestModule",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "VirtoCommerce.Assets", Version = "3.1111.0" }],
                });

            _bootstrapper.ValidateModulesInternal([module, dependency], platformVersion);

            Assert.Single(module.Errors);
            Assert.Contains("incompatible", module.Errors.First());
        }

        [Fact]
        public void ValidateModules_CompatibleDependency_NoError()
        {
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var dependency = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { Id = "VirtoCommerce.Assets", Version = "3.500.0", PlatformVersion = "3.800.0" });
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "TestModule",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "VirtoCommerce.Assets", Version = "3.200.0" }],
                });

            _bootstrapper.ValidateModulesInternal([module, dependency], platformVersion);

            Assert.Empty(module.Errors);
        }

        [Fact]
        public void ValidateModules_OptionalMissingDependency_NoError()
        {
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var module = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "TestModule",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "VirtoCommerce.Optional", Version = "3.0.0", Optional = true }],
                });

            _bootstrapper.ValidateModulesInternal([module], platformVersion);

            Assert.Empty(module.Errors);
        }

        [Fact]
        public void ValidateModules_CascadesErrorsToDependents()
        {
            // Catalog targets too-high platform → fails validation.
            // xCart depends on Catalog → should also be marked as failed.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var catalog = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { Id = "VirtoCommerce.Catalog", Version = "3.900.0", PlatformVersion = "3.1111.0" });
            var xCart = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "VirtoCommerce.XCart",
                    Version = "3.100.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "VirtoCommerce.Catalog", Version = "3.800.0" }],
                });

            _bootstrapper.ValidateModulesInternal([catalog, xCart], platformVersion);

            Assert.NotEmpty(catalog.Errors);
            Assert.Contains("platform version", catalog.Errors.First());
            Assert.NotEmpty(xCart.Errors);
            Assert.Contains("VirtoCommerce.Catalog", xCart.Errors.First());
            Assert.Contains("has errors", xCart.Errors.First());
        }

        [Fact]
        public void ValidateModules_CascadesTransitively()
        {
            // A fails → B depends on A → C depends on B → all three fail.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var a = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { Id = "A", Version = "1.0.0", PlatformVersion = "3.1111.0" }); // fails
            var b = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "B",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "A", Version = "1.0.0" }],
                });
            var c = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "C",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "B", Version = "1.0.0" }],
                });

            _bootstrapper.ValidateModulesInternal([a, b, c], platformVersion);

            Assert.NotEmpty(a.Errors);
            Assert.NotEmpty(b.Errors);
            Assert.NotEmpty(c.Errors);
            Assert.Contains("'A' has errors", b.Errors.First());
            Assert.Contains("'B' has errors", c.Errors.First());
        }

        [Fact]
        public void ValidateModules_OptionalDependencyFailure_DoesNotCascade()
        {
            // A fails, B has optional dependency on A → B should NOT fail.
            var platformVersion = SemanticVersion.Parse("3.800.0");
            var a = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest { Id = "A", Version = "1.0.0", PlatformVersion = "3.1111.0" }); // fails
            var b = new ManifestModuleInfo().LoadFromManifest(
                new ModuleManifest
                {
                    Id = "B",
                    Version = "1.0.0",
                    PlatformVersion = "3.800.0",
                    Dependencies = [new ManifestDependency { Id = "A", Version = "1.0.0", Optional = true }],
                });

            _bootstrapper.ValidateModulesInternal([a, b], platformVersion);

            Assert.NotEmpty(a.Errors);
            Assert.Empty(b.Errors);
        }
    }
}
