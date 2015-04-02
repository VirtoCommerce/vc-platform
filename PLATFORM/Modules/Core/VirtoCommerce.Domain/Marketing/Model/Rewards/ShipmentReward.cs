using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Shipment reward
	/// </summary>
	public class ShipmentReward : AmountBasedReward
	{
		public string ShippingMethod
		{
			get;
			set;
		}
	}
}
