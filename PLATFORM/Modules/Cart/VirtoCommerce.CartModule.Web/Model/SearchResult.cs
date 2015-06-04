using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CartModule.Web.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			ShopingCarts = new List<ShoppingCart>();
		}
		public int TotalCount { get; set; }

		public List<ShoppingCart> ShopingCarts { get; set; }

	}
}
