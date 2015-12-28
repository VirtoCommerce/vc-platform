using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Common.PromotionEvaluator
{
    public interface IDiscountable
    {
        ICollection<Discount> Discounts { get; }

        void ApplyRewards(IEnumerable<PromotionReward> rewards);
    }
}