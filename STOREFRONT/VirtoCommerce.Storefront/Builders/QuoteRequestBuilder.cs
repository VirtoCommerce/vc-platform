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
using VirtoCommerce.Storefront.Model.Order.Events;
using System;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;

namespace VirtoCommerce.Storefront.Builders
{
    public class QuoteRequestBuilder : IQuoteRequestBuilder, IObserver<UserLoginEvent>
    {
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IPromotionEvaluator _promotionEvaluator;
        private readonly ICacheManager<object> _cacheManager;

        private QuoteRequest _quoteRequest;
        private const string _quoteRequestCacheRegion = "QuoteRequestRegion";

        public QuoteRequestBuilder(IQuoteModuleApi quoteApi, IPromotionEvaluator promotionEvaluator, ICacheManager<object> cacheManager)
        {
            _quoteApi = quoteApi;
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
        #region IQuoteRequestBuilder Members

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
                    Tag = "active",
                    CustomerId = customer.Id,
                    StoreId = store.Id
                };
                var searchResult = await _quoteApi.QuoteModuleSearchAsync(activeQuoteSearchCriteria);
                quoteRequest = searchResult.QuoteRequests.Select(x => x.ToWebModel()).FirstOrDefault();
                if (quoteRequest == null)
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
                    quoteRequest = (await _quoteApi.QuoteModuleGetByIdAsync(quoteRequest.Id)).ToWebModel();
                }

                quoteRequest.Customer = customer;

                return quoteRequest;
            });

            return this;
        }

        public IQuoteRequestBuilder Reject()
        {
            _quoteRequest.Status = "Rejected";
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
            foreach (var quoteItem in quoteRequest.Items)
            {
                _quoteRequest.Items.Add(quoteItem);
            }

            if (quoteRequest.Addresses != null && quoteRequest.Addresses.Any())
            {
                _quoteRequest.Addresses = quoteRequest.Addresses;
            }

            await _quoteApi.QuoteModuleDeleteAsync(new string[] { quoteRequest.Id }.ToList());
            _cacheManager.Remove(QuoteRequestCacheKey, _quoteRequestCacheRegion);

            return this;
        }

        public async Task SaveAsync()
        {
            _cacheManager.Remove(QuoteRequestCacheKey, _quoteRequestCacheRegion);

            var quoteDto = _quoteRequest.ToServiceModel();
            if (_quoteRequest.IsTransient())
            {
                await _quoteApi.QuoteModuleCreateAsync(quoteDto);
            }
            else
            {
                await _quoteApi.QuoteModuleUpdateAsync(quoteDto);
            }
        }

        public QuoteRequest QuoteRequest
        {
            get
            {
                return _quoteRequest;
            }
        }

        #endregion

        #region IObserver<UserLoginEvent> Members
        /// <summary>
        /// Merge anonymous user quote to newly logined user quote by loging event
        /// </summary>
        /// <param name="userLoginEvent"></param>
        public void OnNext(UserLoginEvent userLoginEvent)
        {
            //If previous user was anonymous and it has not empty cart need merge anonymous cart to personal
            if (!userLoginEvent.PrevUser.IsRegisteredUser && userLoginEvent.WorkContext.QuoteRequest != null && userLoginEvent.WorkContext.QuoteRequest.Items.Any())
            {
                //Call async methods synchronously http://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
                var task = new TaskFactory().StartNew(async () =>
                {
                    await GetOrCreateNewTransientQuoteRequestAsync(userLoginEvent.WorkContext.CurrentStore, userLoginEvent.NewUser, userLoginEvent.WorkContext.CurrentLanguage, userLoginEvent.WorkContext.CurrentCurrency);
                    await MergeWithQuoteRequest(userLoginEvent.WorkContext.QuoteRequest).ConfigureAwait(false);
                    await SaveAsync().ConfigureAwait(false);
                });

                task.Wait();
            }
        }

        public void OnError(Exception error)
        {
            //Nothing todo
        }

        public void OnCompleted()
        {
            //Nothing todo
        }
        #endregion



        private string GetQuoteRequestCacheKey(string storeId, string customerId)
        {
            return string.Format("QuoteRequest-{0}-{1}", storeId, customerId);
        }
    }
}