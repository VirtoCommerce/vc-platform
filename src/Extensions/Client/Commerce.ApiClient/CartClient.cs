using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.ApiClient
{
    public class CartClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string CurrentCart = "cart/{0}/carts/current";
            public const string UpdateCart = "cart/carts";
        }

        /// <summary>
        /// Initializes a new instance of the CartClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public CartClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the CartClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public CartClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        /// <summary>
        /// Gets the current cart
        /// </summary>
        public Task<ShoppingCart> GetCurrentCartAsync(string storeId)
        {
            return GetAsync<ShoppingCart>(CreateRequestUri(string.Format(RelativePaths.CurrentCart, storeId)), useCache: false);
        }

        public Task<ShoppingCart> UpdateCurrentCartAsync(ShoppingCart cart)
        {
            return SendAsync<ShoppingCart, ShoppingCart>(CreateRequestUri(RelativePaths.UpdateCart), HttpMethod.Put, cart);
        }
    }
}
