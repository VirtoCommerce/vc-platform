using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	public class ShipmentReward : PromotionReward
	{
		private string _shippingMethodId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ShippingMethodId
		{
			get
			{
				return _shippingMethodId;
			}
			set
			{
				SetValue(ref _shippingMethodId, () => this.ShippingMethodId, value);
			}
		}
	}
}
