using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class SearchCriteria
	{
		public SearchCriteria()
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
