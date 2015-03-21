using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			Members = new List<Member>();
		}
		public int TotalCount { get; set; }

		public List<Member> Members { get; set; }

	}
}
