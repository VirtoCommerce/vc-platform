using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class BrowseClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string Products = "products/{0}";
            public const string Product = "products/{0}";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public BrowseClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public BrowseClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task<ResponseCollection<Product>> GetProductsAsync(BrowseQuery query, string outline = "")
        {
            return this.GetAsync<ResponseCollection<Product>>(this.CreateRequestUri(String.Format(RelativePaths.Products, outline), query.GetQueryString()));
        }

        public Task<Product> GetProductAsync(string productId)
        {
            return this.GetAsync<Product>(this.CreateRequestUri(String.Format(RelativePaths.Product, productId)));
        }

    }
}
