using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public enum ResponseGroup
	{
		WithItems = 1,
		WithShipments = 2,
		WithInPayments = 4,
	}
}