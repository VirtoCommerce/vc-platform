using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class DiscountConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Discount source, Discount target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply properties patch
			target.DiscountAmount = source.DiscountAmount;

			if (source.Coupon != null)
				target.Coupon = source.Coupon;

		}

	}

	public class DiscountComparer : IEqualityComparer<Discount>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Discount x, Discount y)
		{
			return x.PromotionId == y.PromotionId;
		}

		public int GetHashCode(Discount obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
