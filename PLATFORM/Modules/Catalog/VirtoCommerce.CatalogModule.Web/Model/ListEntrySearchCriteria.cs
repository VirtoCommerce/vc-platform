using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntrySearchCriteria
	{
		public ListEntrySearchCriteria()
		{
			Count = 20;
		}
	
		public ResponseGroup ResponseGroup { get; set; }
		public string Keyword { get; set; }
		public string CategoryId { get; set; }
		public string CatalogId { get; set; }

		public int Start { get; set; }

		public int Count { get; set; }
	}
}
