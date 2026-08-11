using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    /// <summary>
    /// Extends module export with sidecar binary entries while keeping the primary module payload readable.
    /// </summary>
    /// <remarks>
    /// Export orchestrators use this optional contract when they can expose binary entries separately from the
    /// primary module stream. Modules must continue to implement <see cref="IExportSupport"/> for compatibility
    /// with orchestrators that do not support sidecar entries.
    /// </remarks>
    public interface IExportBinaryDataSupport : IExportSupport
    {
        Task ExportAsync(
            Stream outStream,
            IExportBinaryDataWriter binaryDataWriter,
            ExportImportOptions options,
            Action<ExportImportProgressInfo> progressCallback,
            CancellationToken cancellationToken);
    }
}
