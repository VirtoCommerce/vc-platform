using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class ShipmentItem : AuditableEntity
	{
		public ShipmentItem()
		{
		}

		public ShipmentItem(LineItem lineItem)
		{
			LineItem = lineItem;
			LineItemId = lineItem.Id;

			Quantity = lineItem.Quantity;
		}

		public string LineItemId { get; set; }
		public LineItem LineItem { get; set; }

		public string BarCode { get; set; }

		public int Quantity { get; set; }
	
	}
}
