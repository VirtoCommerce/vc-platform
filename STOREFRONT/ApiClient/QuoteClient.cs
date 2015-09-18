using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Quotes;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class QuoteClient : BaseClient
    {
        public QuoteClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public QuoteClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public async Task<QuoteRequestSearchResult> SearchAsync(QuoteRequestSearchCriteria searchCriteria)
        {
            var requestUrl = CreateRequestUri(RelativePaths.QuoteRequests, searchCriteria.GetQueryString());

            var response = await GetAsync<QuoteRequestSearchResult>(requestUrl).ConfigureAwait(false);

            return response;
        }

        public async Task<QuoteRequest> GetByIdAsync(string id)
        {
            var requestUrl = CreateRequestUri(string.Format(RelativePaths.QuoteRequest, id));

            var response = await GetAsync<QuoteRequest>(requestUrl).ConfigureAwait(false);

            return response;
        }

        public async Task<QuoteRequest> CreateAsync(QuoteRequest quoteRequest)
        {
            var requestUrl = CreateRequestUri(RelativePaths.QuoteRequests);

            var response = await SendAsync<QuoteRequest, QuoteRequest>(requestUrl, HttpMethod.Post, quoteRequest).ConfigureAwait(false);

            return response;
        }

        public async Task<QuoteRequest> UpdateAsync(QuoteRequest quoteRequest)
        {
            var requestUrl = CreateRequestUri(RelativePaths.QuoteRequests);

            var response = await SendAsync<QuoteRequest, QuoteRequest>(requestUrl, HttpMethod.Put, quoteRequest).ConfigureAwait(false);

            return response;
        }

        public async Task<QuoteRequest> RecalculateAsync(QuoteRequest quoteRequest)
        {
            var requestUrl = CreateRequestUri(RelativePaths.Recalculate);

            var response = await SendAsync<QuoteRequest, QuoteRequest>(requestUrl, HttpMethod.Put, quoteRequest).ConfigureAwait(false);

            return response;
        }

        protected class RelativePaths
        {
            public const string QuoteRequests = "quote/requests";

            public const string QuoteRequest = "quote/requests/{0}";

            public const string Recalculate = "quote/requests/recalculate";
        }
    }
}