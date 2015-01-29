using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public class LineItem : Position
	{
		public bool? IsGift { get; set; }
		public string ShippingMethodCode { get; set; }
		public string FulfilmentLocationCode { get; set; }

		public Discount Discount { get; set; }
	}
}
