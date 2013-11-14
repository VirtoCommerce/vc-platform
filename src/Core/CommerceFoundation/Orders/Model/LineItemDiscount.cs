using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract]
	[EntitySet("LineItemDiscounts")]
	public class LineItemDiscount : Discount
	{
		private string _LineItemId;
        [StringLength(128)]
		[DataMember]
		public string LineItemId
		{
			get
			{
				return _LineItemId;
			}
			set
			{
				SetValue(ref _LineItemId, () => this.LineItemId, value);
			}
		}

		[DataMember]
        [ForeignKey("LineItemId")]
        [Parent]
		public LineItem LineItem { get; set; }
	}
}
