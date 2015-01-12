using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class CartItemConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this CartItem source, CartItem target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			target.Quantity = source.Quantity;
			target.SalePrice = source.SalePrice;
			target.PlacedPrice = source.PlacedPrice;
			target.ListPrice = source.ListPrice;

			if (source.Discounts != null)
			{
				if (target.Discounts == null)
					target.Discounts = new List<Discount>();

				source.Discounts.Patch(target.Discounts, new DiscountComparer(),
					(sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
		}

	}

	public class CartItemComparer : IEqualityComparer<CartItem>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(CartItem x, CartItem y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(CartItem obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
