using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class Dimension : ValueObject<Coupon>
	{
		public string Unit { get; set; }
		public decimal Height { get; set; }
		public decimal Length { get; set; }
		public decimal Width { get; set; }
	
	}
}
