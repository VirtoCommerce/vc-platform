using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;

namespace VirtoCommerce.Storefront.Services
{
    public class CustomerServiceImpl : ICustomerService
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
               Debug.WriteLine("#" + Thread.CurrentThread.ManagedThreadId + " GetCustomerByIdAsync");
               //TODO: Make parallels call
               var contact =  await _customerApi.CustomerModuleGetContactByIdAsync(customerId);
               var ordersResponse = await _orderApi.OrderModuleSearchAsync(criteriaCustomerId: customerId, criteriaResponseGroup: "full");
               var result = contact.ToWebModel();
               result.OrdersCount = ordersResponse.TotalCount.Value;
               var workContext = _workContextFactory();
               result.Orders = ordersResponse.CustomerOrders.Select(x => x.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage)).ToList();
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

        private string GetCacheKey(string customerId)
        {
            return "GetCustomerById-" + customerId;
        }
    }
}