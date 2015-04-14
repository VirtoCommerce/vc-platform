using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	public class DynamicContentPlace : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Name { get; set; }
		public string Description { get; set; }

		public string FolderId { get; set; }
		public string Outline { get; set; }
		public string Path { get; set; }
	}
}
