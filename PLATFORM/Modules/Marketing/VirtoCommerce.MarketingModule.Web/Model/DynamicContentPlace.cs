using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	/// <summary>
	/// Represent presentation placeholders for dynamic content publication
	/// </summary>
	public class DynamicContentPlace : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public string FolderId { get; set; }
		/// <summary>
		/// all parent folders ids concatenated (1;21;344)
		/// </summary>
		public string Outline { get; set; }
		/// <summary>
		/// all parent folders names concatenated (Root\Child\Child2)
		/// </summary>
		public string Path { get; set; }
		public string ImageUrl { get; set; }
	}
}
