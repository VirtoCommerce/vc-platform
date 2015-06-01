using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Cart.Events
{
	public class CartChangeEvent
	{
		public CartChangeEvent(EntryState state, ShoppingCart origCart, ShoppingCart modifiedCart)
		{
			ChangeState = state;
			OrigCart = origCart;
			ModifiedCart = modifiedCart;
		}

		public EntryState ChangeState { get; set; }
		public ShoppingCart OrigCart { get; set; }
		public ShoppingCart ModifiedCart { get; set; }
	}
}
