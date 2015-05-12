using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
	public class Page
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Content { get; set; }
		public byte[] ByteContent { get; set; }
		public string ContentType { get; set; }
		public string Language { get; set; }
		public DateTime ModifiedDate { get; set; }

		public string FileUrl { get; set; }
	}
}