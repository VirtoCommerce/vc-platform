using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Exceptions;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Order.Events;

namespace VirtoCommerce.Storefront.Services
{
    public class CustomerServiceImpl : ICustomerService, IObserver<OrderPlacedEvent>
    {
        private readonly ICustomerManagementModuleApi _customerApi;
        private readonly IOrderModuleApi _orderApi;
        private readonly Func<WorkContext> _workContextFactory;
        private readonly ICacheManager<object> _cacheManager;

        public CustomerServiceImpl(Func<WorkContext> workContextFactory, ICustomerManagementModuleApi customerApi, IOrderModuleApi orderApi,
                                   ICacheManager<object> cacheManager)
        {
            _workContextFactory = workContextFactory;
            _customerApi = customerApi;
            _orderApi = orderApi;
            _cacheManager = cacheManager;
        }

        #region ICustomerService Members
        public async Task<CustomerInfo> GetCustomerByIdAsync(string customerId)
        {
            var retVal = await _cacheManager.GetAsync(GetCacheKey(customerId), "ApiRegion", async () =>
            {
                //TODO: Make parallels call
                var contact = await _customerApi.CustomerModuleGetContactByIdAsync(customerId);
                CustomerInfo result = null;
                if (contact != null)
                {
                    var criteria = new VirtoCommerceDomainOrderModelSearchCriteria
                    {
                        CustomerId = customerId,
                        ResponseGroup = "full",
                    };

                    var ordersResponse = await _orderApi.OrderModuleSearchAsync(criteria);
                    result = contact.ToWebModel();
                    result.OrdersCount = ordersResponse.TotalCount.Value;
                    var workContext = _workContextFactory();
                    result.Orders = ordersResponse.CustomerOrders.Select(x => x.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage)).ToList();
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
        #endregion

        #region IObserver<CreateOrderEvent> Members
        public void OnNext(OrderPlacedEvent value)
        {
            if (value.Order != null)
            {
                var cacheKey = GetCacheKey(value.Order.CustomerId);
                _cacheManager.Remove(cacheKey, "ApiRegion");
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

        private string GetCacheKey(string customerId)
        {
            return "GetCustomerById-" + customerId;
        }


    }
}