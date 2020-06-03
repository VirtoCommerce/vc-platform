using System.IO.Compression;

namespace VirtoCommerce.Platform.Core.ZipFile
{
    public interface IZipFileWrapper
    {
        ZipArchive OpenRead(string fileName);
        void Extract(string zipFile, string destinationDir);
    }
}
