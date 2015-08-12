using System.Collections.Generic;
using System.Linq;

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
        
        /// <summary>
        /// List prices for the products. It includes tiered prices also. (Depending on the quantity, for example)
        /// </summary>
		public ICollection<Price> Prices { get; set; }
	}
}