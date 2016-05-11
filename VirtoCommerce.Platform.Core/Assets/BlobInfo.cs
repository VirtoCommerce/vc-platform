using System;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class BlobInfo
    {
        /// <summary>
        /// Relative url
        /// </summary>
		public string Key { get; set; }
        /// <summary>
        /// Absolute url
        /// </summary>
        public string Url { get; set; }
        public string RelativeUrl { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
