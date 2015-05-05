using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	public class DynamicContentFolder : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Outline { get; set; }
		public string Path { get; set; }
		public string ParentFolderId { get; set; }
		public string ImageUrl { get; set; }
		
	}
}