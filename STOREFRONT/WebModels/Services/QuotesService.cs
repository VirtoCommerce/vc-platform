using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Convertors;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Models.Services
{
    public class QuotesService
    {
        private readonly QuoteClient _quoteClient;

        public QuotesService()
        {
            _quoteClient = ClientContext.Clients.CreateQuoteClient();
        }

        public async Task<QuoteRequest> GetCurrentQuoteRequestAsync(string storeId, string customerId)
        {
            QuoteRequest quoteRequestModel = null;

            var criteria = new DataContracts.Quotes.QuoteRequestSearchCriteria
            {
                Count = 1,
                CustomerId = customerId,
                StoreId = storeId,
                Tag = "actual"
            };

            var quoteResponse = await _quoteClient.SearchAsync(criteria);

            if (quoteResponse != null)
            {
                var quoteRequest = quoteResponse.QuoteRequests.FirstOrDefault();
                if (quoteRequest != null)
                {
                    var detalizedQuoteRequest = await _quoteClient.GetByIdAsync(quoteRequest.Id);

                    if (detalizedQuoteRequest != null)
                    {
                        quoteRequestModel = detalizedQuoteRequest.ToViewModel();
                    }
                }
            }

            return quoteRequestModel;
        }

        public async Task<QuoteRequest> UpdateQuoteRequestAsync(QuoteRequest quoteRequestModel)
        {
            var quoteRequest = quoteRequestModel.ToServiceModel();

            if (quoteRequestModel.IsTransient)
            {
                quoteRequest = await _quoteClient.CreateAsync(quoteRequest);
                quoteRequestModel = quoteRequest.ToViewModel();
            }
            else
            {
                await _quoteClient.UpdateAsync(quoteRequest);
                quoteRequestModel = await GetCurrentQuoteRequestAsync(quoteRequest.StoreId, quoteRequest.CustomerId);
            }

            return quoteRequestModel;
        }

        public async Task<ItemCollection<QuoteRequest>> SearchAsync(QuoteRequestSearchCriteria criteria)
        {
            var searchCriteria = new DataContracts.Quotes.QuoteRequestSearchCriteria
            {
                Count = criteria.Take,
                CustomerId = criteria.CustomerId,
                Start = criteria.Skip,
                Status = criteria.Status,
                StoreId = criteria.StoreId,
                Tag = criteria.Tag
            };

            var quotesResponse = await _quoteClient.SearchAsync(searchCriteria);

            ItemCollection<QuoteRequest> quoteRequests = null;

            if (quotesResponse != null)
            {
                quoteRequests = new ItemCollection<QuoteRequest>(quotesResponse.QuoteRequests.Select(qr => qr.ToViewModel()));
            }

            return quoteRequests;
        }

        public async Task<QuoteRequest> GetByNumberAsync(string storeId, string customerId, string number)
        {
            var searchCriteria = new DataContracts.Quotes.QuoteRequestSearchCriteria
            {
                Count = 1,
                CustomerId = customerId,
                Start = 0,
                StoreId = storeId,
                Keyword = number
            };

            QuoteRequest quoteRequestModel = null;

            var quoteResponse = await _quoteClient.SearchAsync(searchCriteria);

            if (quoteResponse != null)
            {
                var quoteRequest = quoteResponse.QuoteRequests.FirstOrDefault();
                if (quoteRequest != null)
                {
                    var detailedQuoteRequest = await _quoteClient.GetByIdAsync(quoteRequest.Id);
                    if (detailedQuoteRequest != null)
                    {
                        quoteRequestModel = detailedQuoteRequest.ToViewModel();
                    }
                }
            }

            return quoteRequestModel;
        }

        public async Task<QuoteRequest> RecalculateAsync(QuoteRequest quoteRequestModel)
        {
            var quoteRequest = quoteRequestModel.ToServiceModel();

            var apiResponse = await _quoteClient.RecalculateAsync(quoteRequest);

            if (apiResponse != null)
            {
                quoteRequestModel = apiResponse.ToViewModel();
            }

            return quoteRequestModel;
        }
    }
}