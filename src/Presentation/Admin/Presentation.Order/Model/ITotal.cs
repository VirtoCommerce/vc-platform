using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Order.Model
{
	public interface ITotal
	{
		decimal ItemSubtotal { get;	}
		decimal ShippingCost { get;	}
		decimal TotalBeforeTax { get; }
		decimal ItemTax { get; }
		decimal ShippingTax { get; }
		decimal LessReStockingFree { get; }
		decimal Total { get; }
	}
}
