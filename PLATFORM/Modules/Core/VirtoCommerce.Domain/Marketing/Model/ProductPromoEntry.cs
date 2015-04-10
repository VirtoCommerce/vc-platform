using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class ProductPromoEntry
	{
		public ProductPromoEntry()
		{
			Variations = new List<ProductPromoEntry>();
			Attributes = new Dictionary<string, string>();
		}
		public string Code { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Discount { get; set; }
		public string CatalogId { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
		public object Owner { get; set; }
		public string Outline { get; set; }

		public ICollection<ProductPromoEntry> Variations { get; set; }

		public Dictionary<string, string> Attributes { get; set; }
        
	}
}
