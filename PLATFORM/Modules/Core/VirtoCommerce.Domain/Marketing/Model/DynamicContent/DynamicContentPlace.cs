using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class DynamicContentPlace : Entity, IAuditable, IsHasFolder
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }

		#region IHasFolder Members
		public string FolderId { get; set; }
		public DynamicContentFolder Folder { get; set; }
		#endregion
	}
}
