using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    /// <summary>
    /// Writes binary sidecar data to the export container owned by the export orchestrator.
    /// </summary>
    public interface IExportBinaryDataWriter
    {
        /// <summary>
        /// Writes <paramref name="sourceStream"/> to the package entry identified by <paramref name="reference"/>.
        /// </summary>
        /// <remarks>
        /// References identify entries package-wide and use forward-slash separators. The writer validates the
        /// reference and controls the lifetime of the package entry. The caller retains ownership of
        /// <paramref name="sourceStream"/>.
        /// </remarks>
        Task WriteAsync(string reference, Stream sourceStream, CancellationToken cancellationToken);
    }
}
