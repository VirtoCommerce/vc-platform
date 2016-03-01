using System;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Order;
using VirtoCommerce.Storefront.Model.Order.Events;
using VirtoCommerce.Storefront.Model.Quote;
using VirtoCommerce.Storefront.Model.Quote.Events;

namespace VirtoCommerce.Storefront.Services
{
    public class CustomerServiceImpl : ICustomerService, IAsyncObserver<OrderPlacedEvent>, IAsyncObserver<QuoteRequestUpdatedEvent>
    {
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly IOrderModuleApi _orderApi;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly IQuoteModuleApi _quoteApi;
        private readonly IStoreModuleApi _storeApi;
        private readonly ICacheManager<object> _cacheManager;

        public CustomerServiceImpl(Func<WorkContext> workContextFactory, ICustomerManagementModuleApi customerApi, IOrderModuleApi orderApi,
                                   IQuoteModuleApi quoteApi, IStoreModuleApi storeApi, ICacheManager<object> cacheManager)
        {
            _workContextFactory = workContextFactory;
            _customerApi = customerApi;
            _orderApi = orderApi;
            _quoteApi = quoteApi;
            _storeApi = storeApi;
            _cacheManager = cacheManager;
        }

        #region ICustomerService Members
        public async Task<CustomerInfo> GetCustomerByIdAsync(string customerId)
        {
            var retVal = await _cacheManager.GetAsync(GetCacheKey(customerId), "ApiRegion", async () =>
            {
                var workContext = _workContextFactory();

                //TODO: Make parallels call
                var contact = await _customerApi.CustomerModuleGetContactByIdAsync(customerId);
                CustomerInfo result = null;
                if (contact != null)
                {
                    result = contact.ToWebModel();
                    var currentOrderCriteria = workContext.CurrentOrderSearchCriteria;
                    var orderSearchcriteria = new VirtoCommerceDomainOrderModelSearchCriteria
                    {
                        CustomerId = customerId,
                        ResponseGroup = "full",
                        Start = currentOrderCriteria.Start,
                        Count = currentOrderCriteria.PageSize
                    };
                    var ordersResponse = await _orderApi.OrderModuleSearchAsync(orderSearchcriteria);
                    result.Orders = new StorefrontPagedList<CustomerOrder>(ordersResponse.CustomerOrders.Select(x => x.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage)),
                                                                            currentOrderCriteria.PageNumber,
                                                                            currentOrderCriteria.PageSize,
                                                                            ordersResponse.TotalCount.Value, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());

                    if (workContext.CurrentStore.QuotesEnabled)
                    {
                        var currentQuoteCriteria = workContext.CurrentQuoteSearchCriteria;
                        var quoteSearchCriteria = new VirtoCommerceDomainQuoteModelQuoteRequestSearchCriteria
                        {
                            Count = currentQuoteCriteria.PageSize,
                            CustomerId = customerId,
                            Start = currentQuoteCriteria.Start,
                            StoreId = workContext.CurrentStore.Id
                        };
                        var quoteRequestsResponse = await _quoteApi.QuoteModuleSearchAsync(quoteSearchCriteria);
                        result.QuoteRequests = new StorefrontPagedList<QuoteRequest>(quoteRequestsResponse.QuoteRequests.Select(x => x.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage)),
                                                                                     currentQuoteCriteria.PageNumber,
                                                                                     currentQuoteCriteria.PageSize,
                                                                                     quoteRequestsResponse.TotalCount.Value, page => workContext.RequestUrl.SetQueryParameter("page", page.ToString()).ToString());
                    }
                }

                return result;
            });

            if (retVal != null)
            {
                var clone = retVal.JsonClone();
                clone.Orders = retVal.Orders;
                clone.QuoteRequests = retVal.QuoteRequests;
                retVal = clone;
            }

            return retVal;
        }

        public async Task CreateCustomerAsync(CustomerInfo customer)
        {
            var contact = customer.ToServiceModel();
            await _customerApi.CustomerModuleCreateContactAsync(contact);
        }

        public async Task UpdateCustomerAsync(CustomerInfo customer)
        {
            var contact = customer.ToServiceModel();
            await _customerApi.CustomerModuleUpdateContactAsync(contact);
            _cacheManager.Remove(GetCacheKey(customer.Id), "ApiRegion");
        }

        public async Task<bool> CanLoginOnBehalfAsync(string storeId, string customerId)
        {
            var info = await _storeApi.StoreModuleGetLoginOnBehalfInfoAsync(storeId, customerId);
            return info.CanLoginOnBehalf == true;
        }
        #endregion

        #region IObserver<CreateOrderEvent> Members
        public async Task OnNextAsync(OrderPlacedEvent eventArgs)
        {
            if (eventArgs.Order != null)
            {
                //Invalidate cache
                var cacheKey = GetCacheKey(eventArgs.Order.CustomerId);
                _cacheManager.Remove(cacheKey, "ApiRegion");

                var workContext = _workContextFactory();
                //Add addresses to contact profile
                if (workContext.CurrentCustomer.IsRegisteredUser)
                {
                    workContext.CurrentCustomer.Addresses.AddRange(eventArgs.Order.Addresses);
                    workContext.CurrentCustomer.Addresses.AddRange(eventArgs.Order.Shipments.Select(x => x.DeliveryAddress));

                    foreach (var address in workContext.CurrentCustomer.Addresses)
                    {
                        address.Name = string.Format("{0} {1}", address.FirstName, address.LastName);
                    }

                    await UpdateCustomerAsync(workContext.CurrentCustomer);
                }

            }
        }
        #endregion

        #region IAsyncObserver<QuoteRequestUpdatedEvent> Members

        public Task OnNextAsync(QuoteRequestUpdatedEvent quoteRequestCreatedEvent)
        {
            if (quoteRequestCreatedEvent.QuoteRequest != null)
            {
                var cacheKey = GetCacheKey(quoteRequestCreatedEvent.QuoteRequest.CustomerId);
                _cacheManager.Remove(cacheKey, "ApiRegion");
            }

            return Task.Factory.StartNew(() => { });
        }

        #endregion

        private static string GetCacheKey(string customerId)
        {
            return "GetCustomerById-" + customerId;
        }
    }
}