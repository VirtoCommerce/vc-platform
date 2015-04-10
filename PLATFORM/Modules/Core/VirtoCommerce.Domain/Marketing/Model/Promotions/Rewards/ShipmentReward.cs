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
		public ShipmentReward()
		{
		}
		//Copy constructor
		protected ShipmentReward(ShipmentReward other)
			:base(other)
		{
			ShippingMethod = other.ShippingMethod;
		}
		public string ShippingMethod { get; set; }

		public override PromotionReward Clone()
		{
			return new ShipmentReward(this);
		}


		public override string ToString()
		{
			return String.Format("{0} {1}", base.ToString(), ShippingMethod);
		}
	}
}
