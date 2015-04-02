using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			Promotions = new List<Promotion>();
		}
		public int TotalCount { get; set; }

		public List<Promotion> Promotions { get; set; }

	}
}
