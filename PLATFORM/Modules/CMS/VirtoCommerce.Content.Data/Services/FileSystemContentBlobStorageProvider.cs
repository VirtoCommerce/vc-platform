using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Data.Asset;

namespace VirtoCommerce.Content.Data.Services
{
    public class FileSystemContentBlobStorageProvider : FileSystemBlobProvider, IContentBlobStorageProvider
    {
        private readonly string[] _ignoreUrls;
        public FileSystemContentBlobStorageProvider(string storagePath, string publicUrl, params string[] ignoreUrls)
            : base(storagePath, publicUrl)
        {
            _ignoreUrls = ignoreUrls;
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

        public override BlobSearchResult Search(string folderUrl, string keyword)
        {
            if(!string.IsNullOrEmpty(folderUrl) && _ignoreUrls != null && _ignoreUrls.Any(x=> folderUrl.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            {
                return new BlobSearchResult();
            }
            return base.Search(folderUrl, keyword);
        }
    }
}
