using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Shipping.Model
{
	public class ShippingEvaluationContext : IEvaluationContext
	{
		public ShippingEvaluationContext(string storeId)
		{
			StoreId = storeId;
		}
		public string StoreId { get; set; }

		public CurrencyCodes Currency { get; set; }

		public Shipment CartShipment { get; set; }

		public ShoppingCart ShoppingCart { get; set; }
	}
}
