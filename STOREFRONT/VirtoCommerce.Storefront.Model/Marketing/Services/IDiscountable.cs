using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing.Services
{
    public interface IDiscountable
    {
        Currency Currency { get; }

        ICollection<Discount> Discounts { get; }

        void ApplyRewards(IEnumerable<PromotionReward> rewards);
    }
}