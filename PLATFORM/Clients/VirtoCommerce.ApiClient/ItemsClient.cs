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
    public class ItemsClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string AddProduct = "products/{0}";
            public const string UpdateProduct = "products/{0}";
            public const string DeleteProduct = "products/{0}";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public ItemsClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public ItemsClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// List items matching the given query
        /// </summary>
        public Task AddAsync(string categoryId, Product product)
        {
            var requestUri = this.CreateRequestUri(String.Format(RelativePaths.AddProduct, categoryId));
            return this.SendAsync(requestUri, HttpMethod.Post, product);
        }

        public Task UpdateAsync(string categoryId, Product product)
        {
            var requestUri = this.CreateRequestUri(String.Format(RelativePaths.UpdateProduct, categoryId));
            return this.SendAsync(requestUri, new HttpMethod("PATCH"), product);
        }

        public Task DeleteAsync(string productId)
        {
            var requestUri = this.CreateRequestUri(String.Format(RelativePaths.DeleteProduct, productId));
            return this.SendAsync(requestUri, HttpMethod.Delete);
        }
    }
}
