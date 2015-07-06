using System;
using System.IO;
using System.Web.Hosting;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public class FileViewLocationResult : ViewLocationResult
    {
        public FileViewLocationResult(VirtualFile file, string name) : base(file.VirtualPath, name, () => new StreamReader(VirtualPathProviderHelper.Open(file)))
        {
            //_fileHash = VirtualPathProviderHelper.GetFileHash(file.VirtualPath);
            //this._lastModifiedUtc = file.LastWriteTimeUtc;
         }

        #region Overrides of ViewLocationResult

        public virtual bool Reload()
        {
            if (!VirtualPathProviderHelper.FileExists(this.Location))
                return false;

            var file = VirtualPathProviderHelper.GetFile(this.Location);

            this.ContentsReader = () => new StreamReader(VirtualPathProviderHelper.Open(file));

            return true;
        }

        #endregion
    }
}
