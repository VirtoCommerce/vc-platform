using System;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Services;
using System.Collections.Generic;

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

            var criteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
            {
                Count = 1,
                CustomerId = _customer.Id,
                StoreId = _store.Id,
                Tag = "actual",
            };

            var searchResult = await _quoteApi.QuoteModuleSearchAsync(criteria);
            if (searchResult == null || searchResult.QuoteRequests == null || searchResult.TotalCount == 0)
            {
                _quoteRequest = CreateNewTransientQuoteRequest();
            }
            else
            {
                var matchedQuoteRequest = searchResult.QuoteRequests.FirstOrDefault();
                if (matchedQuoteRequest != null)
                {
                    var detalizedQuoteRequest = await _quoteApi.QuoteModuleGetByIdAsync(matchedQuoteRequest.Id);
                    _quoteRequest = detalizedQuoteRequest.ToWebModel();
                }
            }

            return this;
        }

        public IQuoteRequestBuilder AddItem(Product product, long quantity)
        {
            _quoteRequest.Items.Add(product.ToQuoteItem(quantity));

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

        public IQuoteRequestBuilder Update(QuoteRequestFormModel quoteRequest)
        {
            _quoteRequest.Comment = quoteRequest.Comment;
            _quoteRequest.Tag = quoteRequest.Tag;

            _quoteRequest.Addresses.Clear();
            if (quoteRequest.BillingAddress != null)
            {
                _quoteRequest.Addresses.Add(quoteRequest.BillingAddress);
            }
            if (quoteRequest.ShippingAddress != null)
            {
                _quoteRequest.Addresses.Add(quoteRequest.ShippingAddress);
            }

            if (quoteRequest.Items != null)
            {
                foreach (var item in quoteRequest.Items)
                {
                    var existingItem = _quoteRequest.Items.FirstOrDefault(i => i.Id == item.Id);
                    if (existingItem != null)
                    {
                        existingItem.Comment = item.Comment;
                        existingItem.ProposalPrices.Clear();
                        foreach (var proposalPrice in item.ProposalPrices)
                        {
                            existingItem.ProposalPrices.Add(new TierPrice
                            {
                                Price = existingItem.SalePrice,
                                Quantity = proposalPrice.Quantity
                            });
                        }
                    }
                }
            }

            return this;
        }

        public async Task<IQuoteRequestBuilder> MergeWithQuoteRequest(QuoteRequest quoteRequest)
        {
            foreach (var quoteItem in quoteRequest.Items)
            {
                _quoteRequest.Items.Add(quoteItem);
            }

            await _quoteApi.QuoteModuleDeleteAsync(new List<string> { quoteRequest.Id });
            _cacheManager.Remove(_quoteRequestCacheKey, _quoteRequestCacheRegion);

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
            quoteRequest.Status = "Processing";
            quoteRequest.StoreId = _store.Id;
            quoteRequest.Tag = "actual";

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