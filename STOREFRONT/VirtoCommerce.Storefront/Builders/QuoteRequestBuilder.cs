using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Common.Exceptions;

namespace VirtoCommerce.Storefront.Builders
{
    public class QuoteRequestBuilder : IQuoteRequestBuilder
    {
        private readonly IQuoteService _quoteService;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICacheManager<object> _cacheManager;

        private QuoteRequest _quoteRequest;
        private const string _quoteRequestCacheRegion = "QuoteRequestRegion";

        public QuoteRequestBuilder(IQuoteService quoteService, IPromotionEvaluator promotionEvaluator, ICacheManager<object> cacheManager)
        {
            _quoteService = quoteService;
            _promotionEvaluator = promotionEvaluator;
            _cacheManager = cacheManager;
        }

        public string QuoteRequestCacheKey
        {
            get
            {
                if (_quoteRequest == null)
                {
                    throw new StorefrontException("Quote request is not set");
                }

                return GetQuoteRequestCacheKey(_quoteRequest.StoreId, _quoteRequest.CustomerId);
            }
        }

        public IQuoteRequestBuilder TakeQuoteRequest(QuoteRequest quoteRequest)
        {
            _quoteRequest = quoteRequest;

            return this;
        }

        public async Task<IQuoteRequestBuilder> GetOrCreateNewTransientQuoteRequestAsync(Store store, CustomerInfo customer, Language language, Currency currency)
        {
            var cacheKey = GetQuoteRequestCacheKey(store.Id, customer.Id);

            _quoteRequest = await _cacheManager.GetAsync(cacheKey, _quoteRequestCacheRegion, async () =>
            {
                QuoteRequest quoteRequest = null;

                var quoteRequests = await _quoteService.GetQuoteRequestsAsync(store.Id, customer.Id, 0, 1, "actual");
                if (!quoteRequests.Any())
                {
                    quoteRequest = new QuoteRequest();
                    quoteRequest.Currency = currency;
                    quoteRequest.CustomerId = customer.Id;
                    quoteRequest.Language = language;
                    quoteRequest.Status = "Processing";
                    quoteRequest.StoreId = store.Id;
                    quoteRequest.Tag = "actual";

                    if (!customer.IsRegisteredUser)
                    {
                        quoteRequest.CustomerName = StorefrontConstants.AnonymousUsername;
                    }
                    else
                    {
                        quoteRequest.CustomerName = string.Format("{0} {1}", customer.FirstName, customer.LastName);
                    }
                }
                else
                {
                    var matchedQuoteRequest = quoteRequests.FirstOrDefault();
                    if (matchedQuoteRequest != null)
                    {
                        quoteRequest = await _quoteService.GetQuoteRequestAsync(customer.Id, matchedQuoteRequest.Id);
                    }
                }

                quoteRequest.Customer = customer;

                return quoteRequest;
            });

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
            _cacheManager.Remove(QuoteRequestCacheKey, _quoteRequestCacheRegion);

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
            _quoteRequest.Comment = quoteRequest.Comment;

            foreach (var quoteItem in quoteRequest.Items)
            {
                _quoteRequest.Items.Add(quoteItem);
            }

            if (quoteRequest.Addresses != null && quoteRequest.Addresses.Any())
            {
                _quoteRequest.Addresses = quoteRequest.Addresses;
            }

            await _quoteService.RemoveQuoteRequestAsync(quoteRequest.Id);
            _cacheManager.Remove(QuoteRequestCacheKey, _quoteRequestCacheRegion);

            return this;
        }

        public async Task SaveAsync()
        {
            _cacheManager.Remove(QuoteRequestCacheKey, _quoteRequestCacheRegion);

            if (_quoteRequest.IsTransient())
            {
                await _quoteService.CreateQuoteRequestAsync(_quoteRequest);
            }
            else
            {
                await _quoteService.UpdateQuoteRequestAsync(_quoteRequest);
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
    }
}