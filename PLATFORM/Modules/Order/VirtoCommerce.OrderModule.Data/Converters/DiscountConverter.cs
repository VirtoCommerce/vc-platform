using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class DiscountConverter
	{
		public static Discount ToCoreModel(this DiscountEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new Discount();
			retVal.InjectFrom(entity);

			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}

			if(entity.CouponCode != null)
			{
				retVal.Coupon = new Coupon
				{
					Code = entity.CouponCode,
					InvalidDescription = entity.CouponInvalidDescription,
				};
			}
			return retVal;
		}

		public static Discount ToCoreModel(this coreModel.Discount discount)
		{
			if (discount == null)
				throw new ArgumentNullException("discount");

			var retVal = new Discount();
			retVal.InjectFrom(discount);
			retVal.Currency = discount.Currency;

			return retVal;
		}

		public static DiscountEntity ToDataModel(this Discount discount)
		{
			if (discount == null)
				throw new ArgumentNullException("discount");

			var retVal = new DiscountEntity();
			retVal.InjectFrom(discount);

			if (discount.Currency != null)
			{
				retVal.Currency = discount.Currency.ToString();
			}

			if(discount.Coupon != null)
			{
				retVal.CouponCode = discount.Coupon.Code;
				retVal.CouponInvalidDescription = discount.Coupon.InvalidDescription;
			
			}
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
