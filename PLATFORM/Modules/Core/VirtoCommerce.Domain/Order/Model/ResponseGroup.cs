using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	[Flags]
	public enum ResponseGroup
	{
		Default = 0,
		WithItems = 1,
		WithInPayments = 2,
		WithShipments = 4,
		WithAddresses = 8,
		WithDiscounts = 16,
		Full = WithItems | WithInPayments | WithShipments | WithAddresses | WithDiscounts
	}
}
