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
		
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductCode { get; set; }
		public string ProductImg { get; set; }
	}
}
