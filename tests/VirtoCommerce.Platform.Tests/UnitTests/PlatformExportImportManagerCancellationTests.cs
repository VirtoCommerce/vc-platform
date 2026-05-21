using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.ExportImport;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class PlatformExportImportManagerCancellationTests
    {
        private static PlatformExportImportManager CreateManager()
        {
            // UserManager / RoleManager have abstract dependencies; mock through their stores.
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<Role>>();
            var roleManager = new RoleManager<Role>(
                roleStore.Object, null, null, null, null);

            return new PlatformExportImportManager(
                userManager,
                roleManager,
                Mock.Of<ISettingsManager>(),
                Mock.Of<IDynamicPropertyService>(),
                Mock.Of<IDynamicPropertySearchService>(),
                Mock.Of<IModuleService>(),
                Mock.Of<IDynamicPropertyDictionaryItemsService>(),
                Mock.Of<IDynamicPropertyDictionaryItemsSearchService>(),
                Mock.Of<IUserApiKeyService>(),
                Mock.Of<IUserApiKeySearchService>());
        }

        [Fact]
        public async Task ExportAsync_ModernOverload_PreCancelledToken_ThrowsOperationCanceledException()
        {
            var manager = CreateManager();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await using var stream = new MemoryStream();
            var manifest = new PlatformExportManifest { HandleSecurity = true };

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                manager.ExportAsync(stream, manifest, _ => { }, cts.Token));
        }

        [Fact]
        public async Task ImportAsync_ModernOverload_PreCancelledToken_ThrowsOperationCanceledException()
        {
            var manager = CreateManager();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await using var stream = new MemoryStream();
            var manifest = new PlatformExportManifest();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                manager.ImportAsync(stream, manifest, _ => { }, cts.Token));
        }

        [Fact]
        public async Task ExportAsync_LegacyOverload_DropsCancellation()
        {
            var manager = CreateManager();

            var alwaysCancelled = new AlwaysCancelledToken();
            await using var stream = new MemoryStream();
            // Empty manifest: no work inside, so CancellationToken.None won't throw.
            var manifest = new PlatformExportManifest();

#pragma warning disable VC0014 // Test intentionally exercises the legacy obsolete overload.
            await manager.ExportAsync(stream, manifest, _ => { }, alwaysCancelled);
#pragma warning restore VC0014

            Assert.False(alwaysCancelled.ThrowWasCalled,
                "Legacy ICancellationToken should not be consulted — the path now uses CancellationToken.None.");
        }

        private sealed class AlwaysCancelledToken : ICancellationToken
        {
            public bool ThrowWasCalled { get; private set; }

            public void ThrowIfCancellationRequested()
            {
                ThrowWasCalled = true;
                throw new OperationCanceledException();
            }
        }
    }
}
