using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
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
		public string FileName { get; set; }
		public long Size { get; set; }
		public string ContentType { get; set; }
        public DateTime? ModifiedDate { get; set; }

	}
}
