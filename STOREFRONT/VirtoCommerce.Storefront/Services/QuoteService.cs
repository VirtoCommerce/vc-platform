using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteModuleApi _quoteApi;

        public QuoteService(IQuoteModuleApi quoteApi)
        {
            _quoteApi = quoteApi;
        }

        public async Task<ICollection<QuoteRequest>> GetQuoteRequestsAsync(string storeId, string customerId, int skip, int take, string tag)
        {
            var quoteRequests = new List<QuoteRequest>();

            var quoteRequestsResponse = await _quoteApi.QuoteModuleSearchAsync(new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
            {
                Count = take,
                CustomerId = customerId,
                Start = skip,
                StoreId = storeId,
                Tag = tag
            });

            if (quoteRequestsResponse != null && quoteRequestsResponse.TotalCount > 0)
            {
                foreach (var quoteRequestServiceModel in quoteRequestsResponse.QuoteRequests)
                {
                    quoteRequests.Add(quoteRequestServiceModel.ToWebModel());
                }
            }

            return quoteRequests;
        }

        public async Task<QuoteRequest> GetQuoteRequestAsync(string customerId, string quoteRequestNumber)
        {
            QuoteRequest quoteRequest = null;

            // TODO: Remake with qoute request number
            var quoteRequestServiceModel = await _quoteApi.QuoteModuleGetByIdAsync(quoteRequestNumber);
            if (quoteRequestServiceModel != null)
            {
                var quoteRequestWithTotalsServiceModel = await _quoteApi.QuoteModuleCalculateTotalsAsync(quoteRequestServiceModel);
                if (quoteRequestWithTotalsServiceModel != null)
                {
                    quoteRequest = quoteRequestWithTotalsServiceModel.ToWebModel();
                }
            }

            return quoteRequest;
        }

        public async Task<QuoteRequestTotals> GetQuoteRequestTotalsAsync(QuoteRequest quoteRequest)
        {
            var totals = quoteRequest.Totals;

            var recalculatedQuoteRequest = await _quoteApi.QuoteModuleCalculateTotalsAsync(quoteRequest.ToServiceModel());
            if (recalculatedQuoteRequest != null)
            {
                totals = recalculatedQuoteRequest.Totals.ToWebModel(quoteRequest.Currency);
            }

            return totals;
        }

        public async Task CreateQuoteRequestAsync(QuoteRequest quoteRequest)
        {
            await _quoteApi.QuoteModuleCreateAsync(quoteRequest.ToServiceModel());
        }

        public async Task UpdateQuoteRequestAsync(QuoteRequest quoteRequest)
        {
            await _quoteApi.QuoteModuleUpdateAsync(quoteRequest.ToServiceModel());
        }

        public async Task RemoveQuoteRequestAsync(string quoteRequestId)
        {
            var quoteRequest = await _quoteApi.QuoteModuleGetByIdAsync(quoteRequestId);
            if (quoteRequest != null)
            {
                await _quoteApi.QuoteModuleDeleteAsync(new List<string> { quoteRequestId });
            }
        }
    }
}