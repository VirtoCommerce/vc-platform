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
        public OrderClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public OrderClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public Task<CustomerOrder> GetCustomerOrderAsync(string customerId, string orderId)
        {
            return GetAsync<CustomerOrder>(
                this.CreateRequestUri(string.Format(RelativePaths.GetSingleOrder, orderId)),
                userId: customerId,
                useCache: false);
        }

        public Task<OrderSearchResult> GetCustomerOrdersAsync(string storeId, string customerId, string query, int skip, int take)
        {
            var queryStringParameters = new List<KeyValuePair<string, string>>();
            queryStringParameters.Add(new KeyValuePair<string, string>("q", query));
            queryStringParameters.Add(new KeyValuePair<string, string>("site", storeId));
            queryStringParameters.Add(new KeyValuePair<string, string>("customer", customerId));
            queryStringParameters.Add(new KeyValuePair<string, string>("start", skip.ToString()));
            queryStringParameters.Add(new KeyValuePair<string, string>("count", take.ToString()));

            return GetAsync<OrderSearchResult>(
                this.CreateRequestUri(RelativePaths.GetMultipleOrders, queryStringParameters.ToArray()),
                userId: customerId,
                useCache: false);
        }

        protected class RelativePaths
        {
            public const string GetSingleOrder = "order/customerOrders/{0}";
            public const string GetMultipleOrders = "order/customerOrders";
        }
    }
}