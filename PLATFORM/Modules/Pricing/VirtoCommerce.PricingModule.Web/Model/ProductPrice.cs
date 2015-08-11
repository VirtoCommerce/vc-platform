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
        /// List of product prices with different quantity of batch
        /// </summary>
		public ICollection<Price> Prices { get; set; }
	}
}