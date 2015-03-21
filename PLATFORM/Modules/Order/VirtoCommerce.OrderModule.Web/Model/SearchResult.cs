using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			CustomerOrders = new List<CustomerOrder>();
		}
		public int TotalCount { get; set; }

		public List<CustomerOrder> CustomerOrders { get; set; }

	}
}
