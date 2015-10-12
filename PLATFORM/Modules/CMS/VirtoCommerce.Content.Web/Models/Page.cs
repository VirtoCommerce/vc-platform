using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class Page
	{
		public string Id { get; set; }
		/// <summary>
        /// Page element name, contains the path relative to the root pages folder
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Page element text content (text page element), based on content type
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Page element byte content (non-text page element), bases on content type
		/// </summary>
		public byte[] ByteContent { get; set; }

		public string ContentType { get; set; }

		/// <summary>
		/// Locale
		/// </summary>
		public string Language { get; set; }

		public DateTime ModifiedDate { get; set; }

		public string FileUrl { get; set; }

        public string[] SecurityScopes { get; set; }

    }
}