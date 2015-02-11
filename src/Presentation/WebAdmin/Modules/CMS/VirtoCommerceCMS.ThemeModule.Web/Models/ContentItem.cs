using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerceCMS.ThemeModule.Web.Models
{
	public class ContentItem
	{
		public DateTime Created { get; set; }
		public string Content { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public ContentType Type { get; set; }
		public string ParentContentItemId { get; set; }
	}
}