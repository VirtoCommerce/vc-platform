using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    /// <summary>
    /// Reads binary sidecar data from the import container owned by the import orchestrator.
    /// </summary>
    public interface IImportBinaryDataReader
    {
        /// <summary>
        /// Opens the package entry identified by <paramref name="reference"/> for reading.
        /// </summary>
        /// <remarks>The caller owns and must dispose the returned stream.</remarks>
        /// <returns>The readable entry stream, or <see langword="null"/> when the entry does not exist.</returns>
        Task<Stream> OpenReadAsync(string reference, CancellationToken cancellationToken);
    }
}
