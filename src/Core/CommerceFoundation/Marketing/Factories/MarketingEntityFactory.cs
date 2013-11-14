using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Factories
{
	public class MarketingEntityFactory : FactoryBase, IMarketingEntityFactory
	{
		public MarketingEntityFactory()
		{
			RegisterStorageType(typeof(CartPromotion), "CartPromotion");
			RegisterStorageType(typeof(CatalogPromotion), "CatalogPromotion");
			RegisterStorageType(typeof(Coupon), "Coupon");
			RegisterStorageType(typeof(CouponSet), "CouponSet");
			RegisterStorageType(typeof(Promotion), "Promotion");
			RegisterStorageType(typeof(PromotionUsage), "PromotionUsage");
			RegisterStorageType(typeof(CatalogItemReward), "CatalogItemReward");
			RegisterStorageType(typeof(CartSubtotalReward), "CartSubtotalReward");
			RegisterStorageType(typeof(PromotionReward), "PromotionReward");
			RegisterStorageType(typeof(ShipmentReward), "ShipmentReward");
		}
	}
}
