using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Dimension : ValueObject<Dimension>
	{
		public string Unit { get; set; }
		public decimal Height { get; set; }
		public decimal Length { get; set; }
		public decimal Width { get; set; }
	
	}
}
