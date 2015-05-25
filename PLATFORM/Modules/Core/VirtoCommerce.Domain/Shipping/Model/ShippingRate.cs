using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Shipping.Model
{
	public class ShippingRate
	{
		public decimal Rate { get; set; }
		public CurrencyCodes Currency { get; set; }

		public ShippingMethod ShippingMethod { get; set; }
	}
}
