using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.CartModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class DiscountConverter
	{
		public static coreModel.Discount ToCoreModel(this DiscountEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new coreModel.Discount();
			retVal.InjectFrom(entity);

            retVal.Coupon = entity.CouponCode;
			if (entity.Currency != null)
			{
				retVal.Currency = entity.Currency;
			}
			return retVal;
		}


		public static DiscountEntity ToDataModel(this coreModel.Discount discount, PrimaryKeyResolvingMap pkMap)
		{
			if (discount == null)
				throw new ArgumentNullException("discount");

			var retVal = new DiscountEntity();
            pkMap.AddPair(discount, retVal);

            retVal.InjectFrom(discount);

			retVal.Currency = discount.Currency.ToString();

            retVal.CouponCode = discount.Coupon;
            return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this DiscountEntity source, DiscountEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
		}

	}

	public class DiscountComparer : IEqualityComparer<DiscountEntity>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(DiscountEntity x, DiscountEntity y)
		{
			return x.PromotionId == y.PromotionId;
		}

		public int GetHashCode(DiscountEntity obj)
		{
			return obj.PromotionId.GetHashCode();
		}

		#endregion
	}
}
