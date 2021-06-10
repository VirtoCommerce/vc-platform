using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.Modules;
using Xunit;
using Xunit.Extensions.Ordering;

namespace VirtoCommerce.Platform.Tests.Modularity
{
    [Collection("Modularity"), Order(5)]
    public class ModulePlatformCompatibilityTests
    {
        [Theory]
        [InlineData("3.1.0", "3.61.0", false)]
        [InlineData("3.61.0", "3.61.0", false)]
        [InlineData("2.12.0", "3.61.0", true)]
        [InlineData("3.70.0", "3.61.0", true)]
        [InlineData("3.61.3", "3.61.3", false)]
        [InlineData("3.61.4", "3.61.3", true)]
        public void Module(string targetPlatformVersion, string runningPlatformVersion, bool violation)
        {
            var catalogOptionsMock = new Mock<IOptions<LocalStorageModuleCatalogOptions>>();
            catalogOptionsMock.Setup(x => x.Value).Returns(new LocalStorageModuleCatalogOptions() { DiscoveryPath = string.Empty });
            var catalog = new LocalStorageModuleCatalog(catalogOptionsMock.Object, new Mock<IDistributedLockProvider>().Object, new Mock<ILogger<LocalStorageModuleCatalog>>().Object);
            PlatformVersion.CurrentVersion = SemanticVersion.Parse(runningPlatformVersion);
            var module = new ManifestModuleInfo().LoadFromManifest(new ModuleManifest() { PlatformVersion = targetPlatformVersion, Id="Fake", Version ="0.0.0" /*Does not matter (not used in test)*/ });
            catalog.AddModule(module);
            catalog.Validate();
            Assert.True(module.Errors.Count > 0 == violation);
        }
    }
}
