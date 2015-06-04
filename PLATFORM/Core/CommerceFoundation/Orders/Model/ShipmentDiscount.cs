using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("ShipmentDiscounts")]
	public class ShipmentDiscount : Discount
	{
	
		private string _ShipmentId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ShipmentId
		{
			get
			{
				return _ShipmentId;
			}
			set
			{
				SetValue(ref _ShipmentId, () => this.ShipmentId, value);
			}
		}

		[DataMember]
        [ForeignKey("ShipmentId")]
        [Parent]
		public Shipment Shipment { get; set; }
	}
}
