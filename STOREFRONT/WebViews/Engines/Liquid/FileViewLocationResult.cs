using System;
using System.IO;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public class FileViewLocationResult : ViewLocationResult
    {
        private readonly DateTime _lastModifiedUtc;

        public FileViewLocationResult(FileInfo file, string name) : base(file.FullName, name, file.Extension, () => new StreamReader(VirtualPathProviderHelper.Open(file.FullName)))
        {
            this._lastModifiedUtc = file.LastWriteTimeUtc;
        }

        #region Overrides of ViewLocationResult

        /// <summary>
        /// Gets a value indicating whether the current item is stale
        /// </summary>
        /// <returns>True if stale, false otherwise</returns>
        public override bool IsStale()
        {
            if (!File.Exists(this.Location))
                return false;

            return File.GetLastWriteTimeUtc(this.Location) > _lastModifiedUtc;
        }

        public virtual bool Reload()
        {
            if (!File.Exists(this.Location))
                return false;

            this.Contents = () => new StreamReader(VirtualPathProviderHelper.Open(new FileInfo(this.Location).FullName));

            return true;
        }

        #endregion
    }
}
