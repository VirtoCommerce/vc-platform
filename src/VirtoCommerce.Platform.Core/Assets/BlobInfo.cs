using System;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public class BlobInfo : BlobEntry
    {
        public BlobInfo()
        {
            Type = "blob";
        }
        /// <summary>
        /// Relative url
        /// </summary>
		public string Key { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }

    }
}
