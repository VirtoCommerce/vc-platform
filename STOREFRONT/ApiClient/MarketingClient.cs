using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class MarketingClient : BaseClient
    {
        public MarketingClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public MarketingClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public Task<IEnumerable<PromotionReward>> GetPromotionRewardsAsync(PromotionEvaluationContext context)
        {
            var rewards = SendAsync<PromotionEvaluationContext, IEnumerable<PromotionReward>>(
                CreateRequestUri(RelativePaths.GetPromotionRewards), HttpMethod.Post, context);

            return rewards;
        }

        protected class RelativePaths
        {
            public const string GetPromotionRewards = "mp/promotions/evaluate";
        }
    }
}