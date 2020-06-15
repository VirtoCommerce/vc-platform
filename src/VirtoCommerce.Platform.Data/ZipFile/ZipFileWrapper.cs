using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Core.ZipFile;

namespace VirtoCommerce.Platform.Data.ZipFile
{
    public class ZipFileWrapper : IZipFileWrapper
    {
        private readonly IFileSystem _fileSystem;
        private readonly ITransactionFileManager _fileManager;

        public ZipFileWrapper(IFileSystem fileSystem, ITransactionFileManager fileManager)
        {
            _fileSystem = fileSystem;
            _fileManager = fileManager;
        }

        public ZipArchive OpenRead(string fileName)
        {
            return System.IO.Compression.ZipFile.OpenRead(fileName);
        }
        
        public void Extract(string zipFile, string destinationDir)
        {
            using (var archive = OpenRead(zipFile))
            {
                foreach (var entry in archive.Entries.Where(e => !string.IsNullOrEmpty(e.Name)))
                {
                    var filePath = Path.Combine(destinationDir, entry.FullName);
                    //Create directory if not exist
                    var directoryPath = Path.GetDirectoryName(filePath);
                    _fileManager.CreateDirectory(directoryPath);

                    using (var entryStream = entry.Open())
                    using (var fileStream = _fileSystem.File.Create(filePath))
                    {
                        entryStream.CopyTo(fileStream);
                    }
                    _fileSystem.File.SetLastWriteTime(filePath, entry.LastWriteTime.LocalDateTime);
                }
            }
        }
    }
}
