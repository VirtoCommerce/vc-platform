using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	public class DynamicContentItem : AuditableEntity
	{

		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public string FolderId { get; set; }
		public string Outline { get; set; }
		public string Path { get; set; }
		public ICollection<Property> Properties { get; set; }
	}
}
