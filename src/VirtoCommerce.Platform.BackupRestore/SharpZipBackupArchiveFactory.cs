using System.IO;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.Platform.BackupRestore;

public class SharpZipBackupArchiveFactory : IZipBackupArchiveFactory
{
    public virtual IZipBackupArchive CreateForWriting(Stream output, string password)
        => new SharpZipBackupArchive(output, password);

    public virtual IZipBackupArchive OpenForReading(Stream input, string password)
        => new SharpZipBackupArchive(input, password, _reader: true);
}
