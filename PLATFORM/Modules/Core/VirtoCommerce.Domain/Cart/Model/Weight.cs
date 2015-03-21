using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Weight : ValueObject<Weight>
	{
		public string Unit { get; set; }
		public decimal Value { get; set; }

	}
}
