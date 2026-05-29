using System;
using System.IO;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport;

public interface IZipBackupArchive : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Begin a new entry in the archive and return the writable stream for its content.
    /// Disposing the stream finalizes (closes) the entry; do NOT dispose the archive
    /// in the middle of writing an entry.
    /// </summary>
    /// <param name="name">Entry path inside the ZIP (forward-slash separators).</param>
    /// <param name="leaveUnencrypted">
    /// When true, write the entry WITHOUT encryption even if the archive was created with
    /// a password. Used for the <c>Manifest.json</c> entry so the import side can detect
    /// the encryption flag before prompting for a password.
    /// </param>
    Task<Stream> CreateEntryAsync(string name, bool leaveUnencrypted = false);

    /// <summary>
    /// Open an existing entry for reading. Returns <c>null</c> when the entry is missing.
    /// On a wrong password, the stream throws when first read (<see cref="System.IO.InvalidDataException"/>
    /// wrapping the SharpZipLib exception); callers in the manager re-throw as a
    /// PlatformException so the SPA can render an inline error.
    /// </summary>
    Task<Stream> OpenEntryAsync(string name);
}
