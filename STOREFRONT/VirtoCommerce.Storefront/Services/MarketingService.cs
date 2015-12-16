using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class MarketingService : IMarketingService
    {
        private readonly IMarketingModuleApi _marketingApi;

        public MarketingService(IMarketingModuleApi marketingApi)
        {
            _marketingApi = marketingApi;
        }

        public async Task<ICollection<PromotionReward>> EvaluatePromotionRewardsAsync(PromotionEvaluationContext evaluationContext)
        {
            var rewards = new List<PromotionReward>();

            var allRewards = await _marketingApi.MarketingModulePromotionEvaluatePromotionsAsync(evaluationContext.ToServiceModel());
            if (allRewards != null)
            {
                rewards = allRewards.Select(r => r.ToWebModel(evaluationContext.Currency)).ToList();
            }

            return rewards;
        }

        public async Task<string> GetDynamicContentHtmlAsync(string storeId, string placeholderName)
        {
            string htmlContent = null;

            var dynamicContent = await _marketingApi.MarketingModuleDynamicContentEvaluateDynamicContentAsync(storeId, placeholderName);
            if (dynamicContent != null)
            {
                var htmlDynamicContent = dynamicContent.FirstOrDefault(dc => !string.IsNullOrEmpty(dc.ContentType) && dc.ContentType.Equals("Html", StringComparison.OrdinalIgnoreCase));
                if (htmlDynamicContent != null)
                {
                    var dynamicProperty = htmlDynamicContent.DynamicProperties.FirstOrDefault(dp => !string.IsNullOrEmpty(dp.Name) && dp.Name.Equals("Html", StringComparison.OrdinalIgnoreCase));
                    if (dynamicProperty != null && dynamicProperty.Values.Any(v => v.Value != null))
                    {
                        htmlContent = dynamicProperty.Values.First().Value.ToString();
                    }
                }
            }

            return htmlContent;
        }
    }
}