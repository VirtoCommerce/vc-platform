using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class MarketingSearchCriteria
	{
		public MarketingSearchCriteria()
		{
			Count = 20;
		}

		public SearchResponseGroup ResponseGroup { get; set; }
		public string Keyword { get; set; }

		public int Start { get; set; }

		public int Count { get; set; }
	}
}
