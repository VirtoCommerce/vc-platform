using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ProductAssociation 
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public string AssociatedProductId { get; set; }
		public Product AssociatedProduct { get; set; }
	}
}
