using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Cart.Model
{
	[Flags]
	public enum AddressType
	{
		Billing = 1,
		Shipping = 2,
		BillingAndShipping = Billing | Shipping
	}
}
