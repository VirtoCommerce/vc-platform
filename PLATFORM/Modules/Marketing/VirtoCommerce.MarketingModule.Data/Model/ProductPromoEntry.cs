using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MarketingModule.Data
{
	public class ProductPromoEntry
	{
		public string Code { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
		public object Owner { get; set; }
		public string Outline { get; set; }

		public Dictionary<string, string> Attributes { get; set; }
        
	}
}
