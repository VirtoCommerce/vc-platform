using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class ShipmentItemConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShipmentItem source, ShipmentItem target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			//Simply properties patch
			if (source.Quantity != null)
				target.Quantity = source.Quantity;

		}

	}

	public class ShipmentItemComparer : IEqualityComparer<ShipmentItem>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(ShipmentItem x, ShipmentItem y)
		{
			return x.CartItem.Id == y.CartItem.Id;
		}

		public int GetHashCode(ShipmentItem obj)
		{
			return obj.CartItem.Id.GetHashCode();
		}

		#endregion
	}
}
