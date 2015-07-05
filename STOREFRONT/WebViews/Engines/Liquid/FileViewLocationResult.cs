using System;
using System.IO;
using System.Web.Hosting;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public class FileViewLocationResult : ViewLocationResult
    {
        private readonly string _fileHash;

        public FileViewLocationResult(VirtualFile file, string name) : base(file.VirtualPath, name, () => new StreamReader(VirtualPathProviderHelper.Open(file)))
        {
            //_fileHash = VirtualPathProviderHelper.GetFileHash(file.VirtualPath);
            //this._lastModifiedUtc = file.LastWriteTimeUtc;
         }

        #region Overrides of ViewLocationResult

        /// <summary>
        /// Gets a value indicating whether the current item is stale
        /// </summary>
        /// <returns>True if stale, false otherwise</returns>
        public override bool IsStale()
        {
            if (!VirtualPathProviderHelper.FileExists(this.Location))
                return false;

            return VirtualPathProviderHelper.GetFileHash(this.Location) != _fileHash;
        }

        public virtual bool Reload()
        {
            if (!VirtualPathProviderHelper.FileExists(this.Location))
                return false;

            var file = VirtualPathProviderHelper.GetFile(this.Location);

            this.Contents = () => new StreamReader(VirtualPathProviderHelper.Open(file));

            return true;
        }

        #endregion
    }
}
