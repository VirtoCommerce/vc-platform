using System.IO;

namespace VirtoCommerce.Platform.Core.ExportImport;

public interface IZipBackupArchiveFactory
{
    /// <summary>
    /// Open the underlying ZIP backend in write mode.
    /// </summary>
    /// <param name="output">Destination stream. Left open by the archive on dispose.</param>
    /// <param name="password">
    /// When non-empty, entries are AES-256 encrypted using this password (except those
    /// created with <c>leaveUnencrypted=true</c>). When null/empty, all entries are
    /// written in cleartext (legacy unencrypted format).
    /// </param>
    IZipBackupArchive CreateForWriting(Stream output, string password);

    /// <summary>
    /// Open the underlying ZIP backend in read mode.
    /// </summary>
    /// <param name="input">Source stream. Must be readable and seekable.</param>
    /// <param name="password">
    /// Password used to decrypt encrypted entries. Pass null/empty when reading a legacy
    /// unencrypted backup or when only reading the (always-cleartext) manifest entry.
    /// </param>
    IZipBackupArchive OpenForReading(Stream input, string password);
}
