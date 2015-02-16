using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Order;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class OrderClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string SingleOrder = "order/customerOrders/{0}";
            public const string MultipleOrders = "order/customerOrders?q={0}&site={1}&customer={2}&start={3}&count={4}";
        }

        public OrderClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        public OrderClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public Task<CustomerOrder> GetCustomerOrderAsync(string customerId, string orderId)
        {
            var requestUrl = CreateRequestUri(string.Format(RelativePaths.SingleOrder, orderId));

            return GetAsync<CustomerOrder>(requestUrl, userId: customerId, useCache: false);
        }

        public Task<OrderSearchResult> GetCustomerOrdersAsync(string query, string site, string customerId, int skip, int take)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("q", query));
            parameters.Add(new KeyValuePair<string, string>("site", site));
            parameters.Add(new KeyValuePair<string, string>("customer", customerId));
            parameters.Add(new KeyValuePair<string, string>("start", skip.ToString()));
            parameters.Add(new KeyValuePair<string, string>("count", take.ToString()));

            var requestUrl = CreateRequestUri(string.Format(RelativePaths.MultipleOrders, query, site, customerId, skip, take), parameters.ToArray());

            var response = GetAsync<OrderSearchResult>(requestUrl, userId: customerId, useCache: false);

            return response;
        }
    }
}