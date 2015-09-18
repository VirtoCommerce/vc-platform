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
            }
            else
            {
                quoteRequest = await _quoteClient.UpdateAsync(quoteRequest);
            }

            if (quoteRequest != null)
            {
                quoteRequestModel = quoteRequest.ToViewModel();
            }

            return quoteRequestModel;
        }
    }
}