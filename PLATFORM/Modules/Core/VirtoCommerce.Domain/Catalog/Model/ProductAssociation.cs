using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class ProductAssociation : ValueObject<ProductAssociation>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
	    public string Type { get; set; }
		public string AssociatedProductId { get; set; }
		public CatalogProduct AssociatedProduct { get; set; }
	}
}
