using System;
using System.IO;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Web.Infrastructure.ExportImport;
using Xunit;

// This suite intentionally exercises the obsolete back-compat IPlatformExportImportManager surface.
#pragma warning disable VC0014

namespace VirtoCommerce.Platform.Web.Tests.Infrastructure.ExportImport
{
    [Trait("Category", "Unit")]
    public class UninstalledPlatformExportImportManagerTests
    {
        // Mirrors the registration in Startup: when no module registered the real implementation,
        // TryAddScoped installs the throwing fallback.
        private static IPlatformExportImportManager ResolveFallback()
        {
            var services = new ServiceCollection();
            services.TryAddScoped<IPlatformExportImportManager, UninstalledPlatformExportImportManager>();
            return services.BuildServiceProvider().GetRequiredService<IPlatformExportImportManager>();
        }

        [Fact]
        public void Fallback_IsResolvable_AndIsTheStub()
        {
            var manager = ResolveFallback();
            manager.Should().BeOfType<UninstalledPlatformExportImportManager>();
        }

        [Fact]
        public void GetNewExportManifest_Throws_WithActionableMessage()
        {
            var manager = ResolveFallback();

            var act = () => manager.GetNewExportManifest("admin");

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*VirtoCommerce.BackupRestore*not installed*");
        }

        [Fact]
        public void ReadExportManifest_Throws_InvalidOperationException()
        {
            var manager = ResolveFallback();

            var act = () => manager.ReadExportManifest(Stream.Null);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async System.Threading.Tasks.Task ExportAsync_Throws_InvalidOperationException()
        {
            var manager = ResolveFallback();

            var act = () => manager.ExportAsync(Stream.Null, new PlatformExportManifest(), _ => { }, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async System.Threading.Tasks.Task ImportAsync_Throws_InvalidOperationException()
        {
            var manager = ResolveFallback();

            var act = () => manager.ImportAsync(Stream.Null, new PlatformExportManifest(), _ => { }, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
