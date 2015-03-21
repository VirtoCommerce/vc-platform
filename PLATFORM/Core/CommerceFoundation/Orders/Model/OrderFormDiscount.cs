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
	[EntitySet("OrderFormDiscounts")]
	public class OrderFormDiscount : Discount
	{

		private string _OrderFormId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string OrderFormId
		{
			get
			{
				return _OrderFormId;
			}
			set
			{
				SetValue(ref _OrderFormId, () => this.OrderFormId, value);
			}
		}

		[DataMember]
        [ForeignKey("OrderFormId")]
        [Parent]
		public OrderForm OrderForm { get; set; }
	}
}
