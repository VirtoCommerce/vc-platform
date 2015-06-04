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
		public ShippingEvaluationContext(ShoppingCart shoppingCart)
		{
			ShoppingCart = shoppingCart;
		}

		public ShoppingCart ShoppingCart { get; set; }
	}
}
