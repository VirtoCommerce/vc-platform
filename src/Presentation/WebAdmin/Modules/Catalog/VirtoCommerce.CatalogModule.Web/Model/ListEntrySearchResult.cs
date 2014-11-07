using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntrySearchResult
	{
		public ListEntrySearchResult()
		{
			ListEntries = new List<ListEntryBase>();
		}
		public int TotalCount { get; set; }


		public ICollection<ListEntryBase> ListEntries { get; set; }
	}
}
