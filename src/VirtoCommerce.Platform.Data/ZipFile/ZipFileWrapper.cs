using System.IO.Compression;
using VirtoCommerce.Platform.Core.ZipFile;

namespace VirtoCommerce.Platform.Data.ZipFile
{
    public class ZipFileWrapper : IZipFileWrapper
    {
        public ZipArchive OpenRead(string fileName)
        {
            return System.IO.Compression.ZipFile.OpenRead(fileName);
        }
    }
}
