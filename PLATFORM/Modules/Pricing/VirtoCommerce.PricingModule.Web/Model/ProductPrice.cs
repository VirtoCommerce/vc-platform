using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.PricingModule.Web.Model
{
	public class ProductPrice
	{
		public ProductPrice(string productId, IEnumerable<Price> prices)
		{
			Prices = prices.Where(x => x.ProductId == productId).ToList();
			ProductId = productId;

		}
		public string ProductId { get; set; }
		public string ProductName { get; set; }

		public ICollection<Price> Prices { get; set; }
	}
}