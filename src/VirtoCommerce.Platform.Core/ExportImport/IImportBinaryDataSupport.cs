using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    /// <summary>
    /// Extends module import with access to binary sidecar entries stored next to the primary module payload.
    /// </summary>
    /// <remarks>
    /// Modules must continue to implement <see cref="IImportSupport"/> so legacy single-stream packages remain
    /// importable by orchestrators that do not support sidecar entries.
    /// </remarks>
    public interface IImportBinaryDataSupport : IImportSupport
    {
        Task ImportAsync(
            Stream inputStream,
            IImportBinaryDataReader binaryDataReader,
            ExportImportOptions options,
            Action<ExportImportProgressInfo> progressCallback,
            CancellationToken cancellationToken);
    }
}
