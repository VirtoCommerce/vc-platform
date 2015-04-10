using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IPromotionService
	{
		Promotion[] GetActivePromotions();

		Promotion GetPromotionById(string id);
		Promotion CreatePromotion(Promotion promotion);
		void UpdatePromotions(Promotion[] prmotions);
		void DeletePromotions(string[] ids);

		Coupon GetCouponById(string id);
		Coupon[] GetPersonalCoupons(string customerId);
		Promotion CreateCoupon(Coupon coupon);
		void UpdateCoupons(Coupon[] coupons);
		void DeleteCoupons(string[] ids);
	}
}
