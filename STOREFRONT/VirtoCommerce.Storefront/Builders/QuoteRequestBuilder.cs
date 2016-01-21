using CacheManager.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Customer;

namespace VirtoCommerce.Storefront.Builders
{
    public class QuoteRequestBuilder : IQuoteRequestBuilder
    {
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICacheManager<object> _cacheManager;

        private Store _store;
        private CustomerInfo _customer;
        private Currency _currency;
        private Language _language;
        private QuoteRequest _quoteRequest;
        private string _quoteRequestCacheKey;
        private const string _quoteRequestCacheRegion = "QuoteRequestRegion";

        
        public QuoteRequestBuilder(IQuoteModuleApi quoteApi, IPromotionEvaluator promotionEvaluator, ICacheManager<object> cacheManager)
        {
            _quoteApi = quoteApi;
            _promotionEvaluator = promotionEvaluator;
            _cacheManager = cacheManager;
        }

        public async Task<IQuoteRequestBuilder> GetOrCreateNewTransientQuoteRequestAsync(Store store, CustomerInfo customer, Language language, Currency currency)
        {
            _store = store;
            _customer = customer;
            _currency = currency;
            _language = language;
            _quoteRequestCacheKey = GetQuoteRequestCacheKey(store.Id, customer.Id);

            _quoteRequest = await _cacheManager.GetAsync(_quoteRequestCacheKey, _quoteRequestCacheRegion, async () =>
            {
                QuoteRequest quoteRequest = null;

                var searchResult = await _quoteApi.QuoteModuleSearchAsync(
                    criteriaCount: 1,
                    criteriaCustomerId: _customer.Id,
                    criteriaStoreId: _store.Id,
                    criteriaTag: "actual");

                if (searchResult == null && searchResult.QuoteRequests != null)
                {
                    quoteRequest = CreateNewTransientQuoteRequest();
                }
                else
                {
                    var matchedQuoteRequest = searchResult.QuoteRequests.FirstOrDefault();
                    if (matchedQuoteRequest != null)
                    {
                        var detalizedQuoteRequest = await _quoteApi.QuoteModuleGetByIdAsync(matchedQuoteRequest.Id);
                        quoteRequest = detalizedQuoteRequest.ToWebModel();
                    }
                }

                return quoteRequest;
            });

            return this;
        }

        public IQuoteRequestBuilder AddItem(Product product)
        {
            _quoteRequest.Items.Add(product.ToQuoteItem());

            return this;
        }

        public IQuoteRequestBuilder RemoveItem(string quoteItemId)
        {
            var quoteItem = _quoteRequest.Items.FirstOrDefault(i => i.Id == quoteItemId);
            if (quoteItem != null)
            {
                _quoteRequest.Items.Remove(quoteItem);
            }

            return this;
        }

        public async Task SaveAsync()
        {
            var quoteRequest = _quoteRequest.ToServiceModel();

            _cacheManager.Remove(_quoteRequestCacheKey, _quoteRequestCacheRegion);

            if (_quoteRequest.IsTransient())
            {
                await _quoteApi.QuoteModuleCreateAsync(quoteRequest);
            }
            else
            {
                await _quoteApi.QuoteModuleUpdateAsync(quoteRequest);
            }
        }

        public QuoteRequest QuoteRequest
        {
            get
            {
                return _quoteRequest;
            }
        }

        private string GetQuoteRequestCacheKey(string storeId, string customerId)
        {
            return string.Format("QuoteRequest-{0}-{1}", storeId, customerId);
        }

        private QuoteRequest CreateNewTransientQuoteRequest()
        {
            var quoteRequest = new QuoteRequest();

            quoteRequest.Currency = _currency;
            quoteRequest.CustomerId = _customer.Id;
            quoteRequest.Language = _language;
            quoteRequest.StoreId = _store.Id;

            if (_customer.UserName.Equals(StorefrontConstants.AnonymousUsername, StringComparison.OrdinalIgnoreCase))
            {
                quoteRequest.CustomerName = StorefrontConstants.AnonymousUsername;
            }
            else
            {
                quoteRequest.CustomerName = string.Format("{0} {1}", _customer.FirstName, _customer.LastName);
            }

            return quoteRequest;
        }
    }
}