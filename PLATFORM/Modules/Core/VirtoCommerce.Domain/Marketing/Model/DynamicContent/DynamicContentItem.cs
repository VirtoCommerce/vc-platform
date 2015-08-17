using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class DynamicContentItem : AuditableEntity, IsHasFolder, IHasDynamicProperties
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public string ImageUrl { get; set; }

		#region IHasFolder Members
		public string FolderId { get; set; }
		public DynamicContentFolder Folder { get; set; }
		#endregion

		#region IHasDynamicProperties Members
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
		#endregion
	}
}
