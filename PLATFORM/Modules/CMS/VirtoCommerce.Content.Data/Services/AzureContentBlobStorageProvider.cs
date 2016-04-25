using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Asset;

namespace VirtoCommerce.Content.Data.Services
{
    public class AzureContentBlobStorageProvider : AzureBlobProvider, IContentBlobStorageProvider
    {
        private readonly string _chrootPath;
        public AzureContentBlobStorageProvider(string connectionString, string chrootPath)
            : base(connectionString)
        {
            chrootPath = chrootPath.Replace('/', '\\');
            _chrootPath = "\\" + chrootPath.TrimStart('\\');
        }

        #region IContentStorageProvider Members
        public void MoveContent(string oldUrl, string newUrl)
        {
            throw new NotImplementedException();
        }

        public void CopyContent(string fromUrl, string toUrl)
        {
            throw new NotImplementedException();
        }
        #endregion
        public override Stream OpenRead(string url)
        {
            return base.OpenRead(NormalizeUrl(url));
        }

        public override void CreateFolder(BlobFolder folder)
        {
            if (folder.ParentUrl.IsNullOrEmpty())
            {
                folder.Name = NormalizeUrl(folder.Name);
            }
            base.CreateFolder(folder);
        }

        public override Stream OpenWrite(string url)
        {
            return base.OpenWrite(NormalizeUrl(url));
        }

        public override void Remove(string[] urls)
        {
            urls = urls.Select(x => NormalizeUrl(x)).ToArray();
          
            base.Remove(urls);
        }
        public override BlobSearchResult Search(string folderUrl, string keyword)
        {
            folderUrl = NormalizeUrl(folderUrl);
            return base.Search(folderUrl, keyword);
        }

        /// <summary>
        /// Chroot url (artificial add parent 'chroot' folder)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string NormalizeUrl(string url)
        {
            var retVal = _chrootPath;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.IsAbsoluteUrl())
                {
                    url = Uri.UnescapeDataString(new Uri(url).AbsolutePath);
                }
                retVal = "\\" + url.Replace('/', '\\').TrimStart('\\');
                retVal = _chrootPath + "\\" + retVal.Replace(_chrootPath, string.Empty);
                retVal = retVal.Replace("\\\\", "\\");
            }
            return retVal;
        }
    }
}
