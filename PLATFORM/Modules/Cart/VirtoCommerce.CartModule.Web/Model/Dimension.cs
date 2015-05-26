using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Web.Model
{
	public class Dimension : ValueObject<Coupon>
	{
		public string Unit { get; set; }
		public decimal Height { get; set; }
		public decimal Length { get; set; }
		public decimal Width { get; set; }
	
	}
}
