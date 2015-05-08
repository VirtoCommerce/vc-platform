using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.ApiClient.Extensions;
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

        public Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(string[] placeHolders, TagQuery query)
        {
            var parameters = new { placeHolders = String.Join(",", placeHolders) };
            return GetAsync<ResponseCollection<DynamicContentItemGroup>>(
                CreateRequestUri(RelativePaths.DynamicContents, query.GetQueryString(parameters)));
        }

        protected class RelativePaths
        {
            public const string GetPromotionRewards = "mp/marketing/promotions/evaluate";
            public const string DynamicContents = "mp/marketing/contentitems";
        }
    }
}