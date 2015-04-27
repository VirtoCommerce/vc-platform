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

        public Task<DynamicContentItem> GetDynamicContentItemByIdAsync(string id)
        {
            var item = GetAsync<DynamicContentItem>(
                CreateRequestUri(string.Format(RelativePaths.GetDynamicContentItemById, id)), useCache: false);

            return item;
        }

        public Task<DynamicContentPlace> GetDynamicContentPlacesByIdAsync(string id)
        {
            var place = GetAsync<DynamicContentPlace>(
                CreateRequestUri(string.Format(RelativePaths.GetDynamicContentPlaceById, id)), useCache: false);

            return place;
        }

        public Task<DynamicContentPublication> GetDynamicContentPublicationByIdAsync(string id)
        {
            var publication = GetAsync<DynamicContentPublication>(
                CreateRequestUri(string.Format(RelativePaths.GetDynamicContentPublicationById, id)), useCache: false);

            return publication;
        }

        protected class RelativePaths
        {
            public const string GetPromotionRewards = "marketing/promotions/evaluate";

            public const string GetDynamicContentItemById = "marketing/contentitems/{0}";

            public const string GetDynamicContentPlaceById = "marketing/contentplaces/{0}";

            public const string GetDynamicContentPublicationById = "marketing/contentpublications/{0}";
        }
    }
}