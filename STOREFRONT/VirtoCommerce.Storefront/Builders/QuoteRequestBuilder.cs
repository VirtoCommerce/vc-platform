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
using VirtoCommerce.Storefront.Model.Order.Events;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Quote.Events;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Builders
{
    public class QuoteRequestBuilder : IQuoteRequestBuilder, IAsyncObserver<UserLoginEvent>
    {
        private readonly IQuoteModuleApi _quoteApi;
        private readonly ILocalCacheManager _cacheManager;
        private readonly IEventPublisher<QuoteRequestUpdatedEvent> _quoteRequestUpdatedEventPublisher;

        private QuoteRequest _quoteRequest;
        private const string _quoteRequestCacheRegion = "QuoteRequestRegion";

        public QuoteRequestBuilder(IQuoteModuleApi quoteApi, ILocalCacheManager cacheManager,
            IEventPublisher<QuoteRequestUpdatedEvent> quoteRequestUpdatedEventPublisher)
        {
            _quoteApi = quoteApi;
            _cacheManager = cacheManager;
            _quoteRequestUpdatedEventPublisher = quoteRequestUpdatedEventPublisher;
        }
        #region IQuoteRequestBuilder Members

        public async Task<IQuoteRequestBuilder> LoadQuoteRequestAsync(string number, Language language, IEnumerable<Currency> availCurrencies)
        {
            var quoteRequest = await _quoteApi.QuoteModuleGetByIdAsync(number);
            if (quoteRequest == null)
            {
                throw new StorefrontException("Quote request for number " + number + " not found");
            }
            _quoteRequest = quoteRequest.ToWebModel(availCurrencies, language);

            return this;
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
                var activeQuoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
                {
                    Tag = "actual",
                    CustomerId = customer.Id,
                    StoreId = store.Id
                };
                var searchResult = await _quoteApi.QuoteModuleSearchAsync(activeQuoteSearchCriteria);
                quoteRequest = searchResult.QuoteRequests.Select(x => x.ToWebModel(store.Currencies, language)).FirstOrDefault();
                if (quoteRequest == null)
                {
                    quoteRequest = new QuoteRequest(currency, language);
                    quoteRequest.Currency = currency;
                    quoteRequest.CustomerId = customer.Id;
                    quoteRequest.Language = language;
                    quoteRequest.Status = "New";
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
                    quoteRequest = (await _quoteApi.QuoteModuleGetByIdAsync(quoteRequest.Id)).ToWebModel(store.Currencies, language);
                }

                quoteRequest.Customer = customer;

                return quoteRequest;
            });

            return this;
        }

        public IQuoteRequestBuilder Submit()
        {
            if (_quoteRequest.ItemsCount == 0)
            {
                throw new StorefrontException("Can not submit an empty quote request");
            }

            if (_quoteRequest.Status == "Ordered")
            {
                throw new StorefrontException("Can not submit an ordered quote request");
            }

            _quoteRequest.Tag = null;
            _quoteRequest.Status = "Processing";

            return this;
        }

        public IQuoteRequestBuilder Reject()
        {
            if (_quoteRequest.Status == "New" || _quoteRequest.Status == "Ordered")
            {
                throw new StorefrontException("Can not reject new or ordered quote request");
            }

            _quoteRequest.Tag = null;
            _quoteRequest.Status = "Rejected";

            return this;
        }

        public IQuoteRequestBuilder Confirm()
        {
            if (_quoteRequest.Status != "Proposal sent")
            {
                throw new StorefrontException("Can not confirm an quote request");
            }

            _quoteRequest.Tag = null;
            _quoteRequest.Status = "Ordered";

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
            _cacheManager.Remove(GetQuoteRequestCacheKey(_quoteRequest.StoreId, _quoteRequest.CustomerId), _quoteRequestCacheRegion);

            _quoteRequest.Comment = quoteRequest.Comment;
            _quoteRequest.Status = quoteRequest.Status;
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
                        existingItem.SelectedTierPrice = new TierPrice(new Money(item.SelectedTierPrice.Price, _quoteRequest.Currency), item.SelectedTierPrice.Quantity);
                        existingItem.ProposalPrices.Clear();
                        foreach (var proposalPrice in item.ProposalPrices)
                        {
                            existingItem.ProposalPrices.Add(new TierPrice(new Money(proposalPrice.Price, _quoteRequest.Currency), proposalPrice.Quantity));
                        }
                    }
                }
            }

            return this;
        }

        public async Task<IQuoteRequestBuilder> MergeFromOtherAsync(QuoteRequest quoteRequest)
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

            await _quoteApi.QuoteModuleDeleteAsync(new string[] { quoteRequest.Id }.ToList());
            _cacheManager.Remove(GetQuoteRequestCacheKey(_quoteRequest.StoreId, _quoteRequest.CustomerId), _quoteRequestCacheRegion);

            return this;
        }

        public async Task SaveAsync()
        {
            _cacheManager.Remove(GetQuoteRequestCacheKey(_quoteRequest.StoreId, _quoteRequest.CustomerId), _quoteRequestCacheRegion);

            var quoteDto = _quoteRequest.ToServiceModel();
            if (_quoteRequest.IsTransient())
            {
                await _quoteApi.QuoteModuleCreateAsync(quoteDto);
            }
            else
            {
                await _quoteApi.QuoteModuleUpdateAsync(quoteDto);
            }

            await _quoteRequestUpdatedEventPublisher.PublishAsync(new QuoteRequestUpdatedEvent(_quoteRequest));
        }

        public QuoteRequest QuoteRequest
        {
            get
            {
                return _quoteRequest;
            }
        }

        public async Task<IQuoteRequestBuilder> CalculateTotalsAsync()
        {
            var result = await _quoteApi.QuoteModuleCalculateTotalsAsync(_quoteRequest.ToServiceModel());
            _quoteRequest.Totals = result.Totals.ToWebModel(_quoteRequest.Currency);
            return this;
        }

        #endregion
        #region IObserver<UserLoginEvent> Members
        /// <summary>
        /// Merge anonymous user quote to newly logined user quote by loging event
        /// </summary>
        /// <param name="userLoginEvent"></param>
        public async Task OnNextAsync(UserLoginEvent userLoginEvent)
        {
            //If previous user was anonymous and it has not empty cart need merge anonymous cart to personal
            if (userLoginEvent.WorkContext.CurrentStore.QuotesEnabled && !userLoginEvent.PrevUser.IsRegisteredUser
                 && userLoginEvent.WorkContext.CurrentQuoteRequest != null && userLoginEvent.WorkContext.CurrentQuoteRequest.Items.Any())
            {
                await GetOrCreateNewTransientQuoteRequestAsync(userLoginEvent.WorkContext.CurrentStore, userLoginEvent.NewUser, userLoginEvent.WorkContext.CurrentLanguage, userLoginEvent.WorkContext.CurrentCurrency);
                await MergeFromOtherAsync(userLoginEvent.WorkContext.CurrentQuoteRequest);
                await SaveAsync();
            }
        }

        #endregion

        private string GetQuoteRequestCacheKey(string storeId, string customerId)
        {
            return string.Format("QuoteRequest-{0}-{1}", storeId, customerId);
        }
    }
}