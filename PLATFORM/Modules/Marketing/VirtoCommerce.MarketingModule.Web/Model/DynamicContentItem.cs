using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	/// <summary>
	/// Represent content entry for presentation (Images, Html, Banner etc) 
	/// </summary>
	public class DynamicContentItem : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public string FolderId { get; set; }
		/// <summary>
		/// all parent folders ids concatenated (1;21;344)
		/// </summary>
		public string Outline { get; set; }
		/// <summary>
		/// all parent folders names concatenated (Root\Child\Child2)
		/// </summary>
		public string Path { get; set; }

		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
	}
}
