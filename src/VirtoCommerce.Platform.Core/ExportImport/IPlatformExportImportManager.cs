using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    [Obsolete("Backup/restore moved to the VirtoCommerce.BackupRestore module. Use VirtoCommerce.BackupRestore.Core.IBackupRestoreManager instead. The module's implementation also satisfies this interface for a deprecation period, so existing consumers keep working.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IPlatformExportImportManager
    {
        PlatformExportManifest GetNewExportManifest(string author);
        PlatformExportManifest ReadExportManifest(Stream stream);

        Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken сancellationToken);

        [Obsolete("Use the cancellation-aware overload instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
            => throw new NotImplementedException();

        Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, CancellationToken сancellationToken);

        [Obsolete("Use the cancellation-aware overload instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken)
            => throw new NotImplementedException();
    }
}
