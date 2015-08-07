using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Page element
	/// </summary>
	public class Page
	{
		/// <summary>
		/// Page element id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Page element name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Page element content
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Page element byte content
		/// </summary>
		public byte[] ByteContent { get; set; }

		/// <summary>
		/// Page element content type
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Page element language
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// Page element modified date
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// Page element file url
		/// </summary>
		public string FileUrl { get; set; }
	}
}