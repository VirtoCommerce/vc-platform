using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Asset;

namespace VirtoCommerce.Content.Data.Services
{
    public class FileSystemContentStorageProviderImpl : FileSystemBlobProvider, IContentStorageProvider
    {
        public FileSystemContentStorageProviderImpl(string storagePath, string publicUrl = "")
            : base(storagePath, publicUrl)
        {
        }

        #region IContentStorageProvider Members
        public void MoveContentItem(string oldUrl, string newUrl)
        {
            var oldPath = GetStoragePathFromUrl(oldUrl);
            var newPath = GetStoragePathFromUrl(newUrl);
        
            if (oldPath != newPath)
            {
                if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
                {
                    Directory.Move(oldPath, newPath);
                }
                else if (File.Exists(oldPath) && !File.Exists(newPath))
                {
                    File.Move(oldPath, newPath);
                }
            }
        }
        #endregion
    }
}
