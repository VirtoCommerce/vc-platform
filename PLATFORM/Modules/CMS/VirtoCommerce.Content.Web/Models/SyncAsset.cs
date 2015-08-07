using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Sync asset element
	/// </summary>
    public class SyncAsset
    {
		/// <summary>
		/// Asset element id
		/// </summary>
        public string Id { get; set; }

		/// <summary>
		/// Asset element name
		/// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Asset element text content
		/// </summary>
        public string Content { get; set; }

		/// <summary>
		/// Asset element byte content
		/// </summary>
        public byte[] ByteContent { get; set; }

		/// <summary>
		/// 
		/// </summary>
        public string AssetUrl { get; set; }

		/// <summary>
		/// Asset element content type
		/// </summary>
        public string ContentType { get; set; }

		/// <summary>
		/// Asset element updated date
		/// </summary>
        public DateTime Updated { get; set; }
    }
}