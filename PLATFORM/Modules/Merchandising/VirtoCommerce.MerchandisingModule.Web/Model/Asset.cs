using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class Asset
	{
        /// <summary>
        /// Asset absolute URL
        /// </summary>
		public string Url { get; set; }

        /// <summary>
        /// Asset group
        /// </summary>
		public string Group { get; set; }

        /// <summary>
        /// Asset name
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Asset file size in bytes
        /// </summary>
		public long Size { get; set; }

        /// <summary>
        /// Asset MIME type (i.e. image/jpeg)
        /// </summary>
		public string MimeType { get; set; }
	}
}