#region

using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{

    #region

    #endregion

    public class CartClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the CartClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public CartClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the CartClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public CartClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

        public Task CreateCartAsync(ShoppingCart cart)
        {
            return SendAsync<ShoppingCart, ShoppingCart>(
                CreateRequestUri(RelativePaths.UpdateCart),
                HttpMethod.Post,
                cart);
        }

        /// <summary>
        ///     Gets the current cart
        /// </summary>
        public Task<ShoppingCart> GetCartAsync(string storeId, string customerId)
        {
            return
                GetAsync<ShoppingCart>(
                    CreateRequestUri(string.Format(RelativePaths.Cart, storeId, customerId)),
                    useCache: false); 
        }

        public Task<ShipmentMethod[]> GetCartShippingMethods(string cartId)
        {
            return GetAsync<ShipmentMethod[]>(
                CreateRequestUri(string.Format(RelativePaths.GetShippingMethods, cartId)),
                useCache: false);
        }

        public Task<ShoppingCart> UpdateCurrentCartAsync(ShoppingCart cart)
        {
            return SendAsync<ShoppingCart, ShoppingCart>(
                CreateRequestUri(RelativePaths.UpdateCart),
                HttpMethod.Put,
                cart);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Cart = "cart/{0}/{1}/carts/current";

            public const string GetShippingMethods = "cart/carts/{0}/shipmentMethods";

            public const string UpdateCart = "cart/carts";

            #endregion
        }
    }
}
