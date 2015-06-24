using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Model
{
	public class TaxDetail : ValueObject<TaxDetail>
	{
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public string Name { get; set; }
	}
}
