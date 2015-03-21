using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.Foundation.Marketing.Repositories
{
    public interface IMarketingRepository : IRepository
	{
		IQueryable<Promotion> Promotions { get; }
		IQueryable<PromotionReward> PromotionRewards { get; }
		IQueryable<CouponSet> CouponSets { get; }
		IQueryable<Coupon> Coupons { get; }
        IQueryable<PromotionUsage> PromotionUsages { get; }
	}
}
