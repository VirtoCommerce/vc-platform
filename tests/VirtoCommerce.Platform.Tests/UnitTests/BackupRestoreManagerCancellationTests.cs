using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using VirtoCommerce.Platform.BackupRestore;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class BackupRestoreManagerCancellationTests
    {
        private static IPlatformExportImportManager CreateManager()
        {
            // UserManager / RoleManager have abstract dependencies; mock through their stores.
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<Role>>();
            var roleManager = new RoleManager<Role>(
                roleStore.Object, null, null, null, null);

            return new BackupRestoreManager(
                userManager,
                roleManager,
                Mock.Of<ISettingsManager>(),
                Mock.Of<IDynamicPropertyService>(),
                Mock.Of<IDynamicPropertySearchService>(),
                Mock.Of<IModuleService>(),
                Mock.Of<IDynamicPropertyDictionaryItemsService>(),
                Mock.Of<IDynamicPropertyDictionaryItemsSearchService>(),
                Mock.Of<IUserApiKeyService>(),
                Mock.Of<IUserApiKeySearchService>(),
                Mock.Of<IZipBackupArchiveFactory>());
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
    }
}
