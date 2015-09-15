using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class TierPrice : ValueObject<TierPrice>
	{
		public decimal Price { get; set; }
		public long Quantity { get; set; }
	}
}
