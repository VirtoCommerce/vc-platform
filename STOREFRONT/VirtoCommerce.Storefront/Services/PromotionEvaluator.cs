using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class PromotionEvaluator : IPromotionEvaluator
    {
        private readonly IMarketingModuleApi _marketingApi;

        public PromotionEvaluator(IMarketingModuleApi marketingApi)
        {
            _marketingApi = marketingApi;
        }

        #region IPromotionEvaluator Members
        public async Task EvaluateDiscountsAsync(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners)
        {
            var rewards = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(context.ToServiceModel());
            InnerEvaluateDiscounts(rewards, owners);
        }

        public void EvaluateDiscounts(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners)
        {
            var rewards = _marketingApi.MarketingModulePromotionEvaluatePromotions(context.ToServiceModel());
            InnerEvaluateDiscounts(rewards, owners);
        } 
        #endregion

        private void InnerEvaluateDiscounts(IEnumerable<VirtoCommerceMarketingModuleWebModelPromotionReward> rewards, IEnumerable<IDiscountable> owners)
        {
            if (rewards == null)
            {
                return;
            }

            foreach (var owner in owners)
            {
                owner.ApplyRewards(rewards.Select(r => r.ToWebModel(owner.Currency)));
            }
        }
    }
}