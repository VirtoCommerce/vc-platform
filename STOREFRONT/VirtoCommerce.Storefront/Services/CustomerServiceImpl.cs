using System;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.LiquidThemeEngine.Extensions;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Order;
using VirtoCommerce.Storefront.Model.Order.Events;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Services
{
    public class CustomerServiceImpl : ICustomerService, IAsyncObserver<OrderPlacedEvent>
    {
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly IOrderModuleApi _orderApi;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly IQuoteModuleApi _quoteApi;
        private readonly ICacheManager<object> _cacheManager;

        public CustomerServiceImpl(Func<WorkContext> workContextFactory, ICustomerManagementModuleApi customerApi, IOrderModuleApi orderApi,
                                   IQuoteModuleApi quoteApi, ICacheManager<object> cacheManager)
        {
            _workContextFactory = workContextFactory;
            _customerApi = customerApi;
            _orderApi = orderApi;
            _quoteApi = quoteApi;
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

        public async Task<bool> CanLoginOnBehalfAsync(string customerId)
        {
            var info = await _customerApi.CustomerModuleGetLoginOnBehalfInfoAsync(customerId);
            return info.CanLoginOnBehalf == true;
        }
        #endregion

        #region IObserver<CreateOrderEvent> Members
        public Task OnNextAsync(OrderPlacedEvent value)
        {
            if (value.Order != null)
            {
                var cacheKey = GetCacheKey(value.Order.CustomerId);
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
