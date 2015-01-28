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
		WithItems = 1,
		WithShipments = 2,
		WithInPayments = 4,
		Full = WithItems | WithShipments | WithInPayments
	}
}
