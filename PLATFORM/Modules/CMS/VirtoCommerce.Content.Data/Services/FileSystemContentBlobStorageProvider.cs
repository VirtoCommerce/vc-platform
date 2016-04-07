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
       
        public void MoveContent(string srcUrl, string dstUrl)
        {
            var srcPath = GetStoragePathFromUrl(srcUrl);
            var dstPath = GetStoragePathFromUrl(dstUrl);
        
            if (srcPath != dstPath)
            {
                if (Directory.Exists(srcPath) && !Directory.Exists(dstPath))
                {
                    Directory.Move(srcPath, dstPath);
                }
                else if (File.Exists(srcPath) && !File.Exists(dstPath))
                {
                    File.Move(srcPath, dstPath);
                }
            }
        }
  
        public void CopyContent(string srcUrl, string destUrl)
        {
            var srcPath = GetStoragePathFromUrl(srcUrl);
            var destPath = GetStoragePathFromUrl(destUrl);

            CopyDirectoryRecursive(srcPath, destPath);
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

        private static void CopyDirectoryRecursive(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (string folder in Directory.GetDirectories(sourcePath))
            {
                string dest = Path.Combine(destPath, Path.GetFileName(folder));
                CopyDirectoryRecursive(folder, dest);
            }
        }
    }
}
