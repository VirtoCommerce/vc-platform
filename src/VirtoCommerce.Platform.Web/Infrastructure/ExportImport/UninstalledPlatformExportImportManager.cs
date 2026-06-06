using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.Platform.Web.Infrastructure.ExportImport
{
    /// <summary>
    /// Fallback <see cref="IPlatformExportImportManager"/> registered by the platform when the
    /// <c>VirtoCommerce.BackupRestore</c> module is NOT installed.
    /// <para>
    /// Backup/restore (platform export/import) moved out of the platform into that module. Legacy or
    /// third-party modules may still resolve the back-compat <see cref="IPlatformExportImportManager"/>
    /// via constructor injection. Without this fallback the DI container would throw an opaque
    /// "Unable to resolve service for type ..." error. This stub lets construction succeed and instead
    /// throws a clear, actionable <see cref="InvalidOperationException"/> at the point the feature is
    /// actually used.
    /// </para>
    /// <para>
    /// It is registered via <c>TryAddScoped</c> AFTER module initialization, so when the module IS
    /// installed its real <c>BackupRestoreManager</c> registration wins and this stub is never used.
    /// </para>
    /// </summary>
#pragma warning disable VC0014 // IPlatformExportImportManager is obsolete; implemented intentionally as a back-compat fallback.
    public sealed class UninstalledPlatformExportImportManager : IPlatformExportImportManager
#pragma warning restore VC0014
    {
        internal const string ModuleId = "VirtoCommerce.BackupRestore";

        private const string Message =
            $"Platform backup/restore (export/import) has moved to the '{ModuleId}' module, which is not installed. " +
            "Install that module to use this feature.";

        public PlatformExportManifest GetNewExportManifest(string author) => throw NotInstalled();

        public PlatformExportManifest ReadExportManifest(Stream stream) => throw NotInstalled();

        public Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
            => throw NotInstalled();

        public Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
            => throw NotInstalled();

        private static InvalidOperationException NotInstalled() => new(Message);
    }
}
