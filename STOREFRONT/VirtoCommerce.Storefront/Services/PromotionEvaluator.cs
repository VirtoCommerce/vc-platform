using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
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

        public async Task EvaluateDiscountsAsync(PromotionEvaluationContext context, IEnumerable<IDiscountable> owners)
        {
            var rewards = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(context.ToServiceModel());
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