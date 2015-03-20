using System;
using VirtoCommerce.Foundation.Marketing.Model;

namespace UI.FunctionalTests.Helpers.Marketing
{
    public class CartPromotionBuilder
    {
        private readonly CartPromotion _cartPromotion;

		private CartPromotionBuilder(CartPromotion cartPromotion)
        {
			_cartPromotion = cartPromotion;
        }

        public static CartPromotionBuilder BuildCartPromotion()
        {
            var cartPromotion = new CartPromotion()
            {
                Name = "default",
                Description = "default_Description",
                Priority = 5,
                Status = PromotionStatus.Active.ToString(),
                StartDate = DateTime.Now.Date.AddDays(-5),
                EndDate = DateTime.Now.Date.AddDays(5)
            };

			return new CartPromotionBuilder(cartPromotion);
        }

		public CartPromotionBuilder WithCoupon(Coupon coupon)
		{
			{
				_cartPromotion.CouponId = coupon.CouponId;
				_cartPromotion.Coupon = coupon;
			}

			return this;
		}

		public CartPromotionBuilder WithCoupon(string couponCode)
		{
			{
				var coupon = new Coupon()
					{
						Code = couponCode,
						Created = DateTime.UtcNow.AddDays(-5)
					};
				_cartPromotion.CouponId = coupon.CouponId;
				_cartPromotion.Coupon = coupon;
			}

			return this;
		}
		
	    public CartPromotion GetContentPublishingGroup()
        {
			return _cartPromotion;
        }
    }

	public class CatalogPromotionBuilder
	{
		private readonly CatalogPromotion _catalogPromotion;

		private CatalogPromotionBuilder(CatalogPromotion catalogPromotion)
		{
			_catalogPromotion = catalogPromotion;
		}
		
		public static CatalogPromotionBuilder BuildCartPromotion()
		{
			var catalogPromotion = new CatalogPromotion()
			{
				Name = "default",
				Description = "default_Description",
				Priority = 5,
				Status = PromotionStatus.Active.ToString(),
				StartDate = DateTime.Now.Date.AddDays(-5),
				EndDate = DateTime.Now.Date.AddDays(5)
			};

			return new CatalogPromotionBuilder(catalogPromotion);
		}

		public CatalogPromotion GetContentPublishingGroup()
		{
			return _catalogPromotion;
		}
	}
}
