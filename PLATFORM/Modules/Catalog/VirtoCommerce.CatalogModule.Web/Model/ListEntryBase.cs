using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	/// <summary>
	/// Represent base class for all enties used in borowsing categories
	/// </summary>
	public abstract class ListEntryBase
	{
		public ListEntryBase(string typeName)
		{
			Type = typeName;
		}
		public string Id { get; set; }
		public string Type { get; set; }

		public string ImageUrl { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }
		public ListEntryLink[] Links { get; set; }
	
	}
}
