using System;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class MarketingServiceImpl : IMarketingService
    {
        private readonly IMarketingModuleApi _marketingApi;
        private readonly ILocalCacheManager _cacheManager;

        public MarketingServiceImpl(IMarketingModuleApi marketingApi, ILocalCacheManager cacheManager)
        {
            _marketingApi = marketingApi;
            _cacheManager = cacheManager;
        }

        public async Task<string> GetDynamicContentHtmlAsync(string storeId, string placeholderName)
        {
            string htmlContent = null;

            //TODO: make full context
            var evaluationContext = new VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext
            {
                StoreId = storeId,
                PlaceName = placeholderName
            };

            var cacheKey = "MarketingServiceImpl.GetDynamicContentHtmlAsync-" + storeId + "-" + placeholderName;
            var dynamicContent = await _cacheManager.GetAsync(cacheKey, "ApiRegion", async () => await _marketingApi.MarketingModuleDynamicContentEvaluateDynamicContentAsync(evaluationContext));
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