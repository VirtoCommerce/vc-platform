using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class ShipmentItem : ValueObject<ShipmentItem>
	{
		public CartItem CartItem { get; set; }
		public int Quantity { get; set; }
	}
}
