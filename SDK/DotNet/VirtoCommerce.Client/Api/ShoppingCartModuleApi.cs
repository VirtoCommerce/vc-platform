using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;

namespace VirtoCommerce.Client.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IShoppingCartModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns></returns>
        void CartModuleDeleteCarts (List<string> ids);

        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CartModuleDeleteCartsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCartById (string id);

        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdWithHttpInfo (string id);
        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCurrentCart (string storeId, string customerId);

        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCurrentCartWithHttpInfo (string storeId, string customerId);
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId);

        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsWithHttpInfo (string cartId);
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId);

        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreWithHttpInfo (string storeId);
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId);

        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsWithHttpInfo (string cartId);
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        VirtoCommerceCartModuleWebModelSearchResult CartModuleSearch (VirtoCommerceCartModuleWebModelSearchCriteria criteria);

        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria);
        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleUpdate (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleCreateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids);

        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CartModuleDeleteCartsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdAsync (string id);

        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCartByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCurrentCartAsync (string storeId, string customerId);

        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCurrentCartAsyncWithHttpInfo (string storeId, string customerId);
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId);

        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsAsyncWithHttpInfo (string cartId);
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId);

        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo (string storeId);
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId);

        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>> CartModuleGetShipmentMethodsAsyncWithHttpInfo (string cartId);
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchAsync (VirtoCommerceCartModuleWebModelSearchCriteria criteria);

        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>> CartModuleSearchAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria);
        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleUpdateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ShoppingCartModuleApi : IShoppingCartModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public ShoppingCartModuleApi(Configuration configuration)
        {
            if (configuration == null) // use the default one in Configuration
                this.Configuration = Configuration.Default;
            else
                this.Configuration = configuration;

            // ensure API client has configuration ready
            if (Configuration.ApiClient.Configuration == null)
            {
                this.Configuration.ApiClient.Configuration = this.Configuration;
            }
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuraiton.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(String basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration {get; set;}

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public Dictionary<String, String> DefaultHeader()
        {
            return this.Configuration.DefaultHeader;
        }

        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            this.Configuration.AddDefaultHeader(key, value);
        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = CartModuleCreateWithHttpInfo(cart);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleCreateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleCreate");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cart.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                localVarPostBody = cart; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = await CartModuleCreateAsyncWithHttpInfo(cart);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleCreateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleCreate");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cart.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                localVarPostBody = cart; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns></returns>
        public void CartModuleDeleteCarts (List<string> ids)
        {
             CartModuleDeleteCartsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CartModuleDeleteCartsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling ShoppingCartModuleApi->CartModuleDeleteCarts");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleDeleteCarts: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleDeleteCarts: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids)
        {
             await CartModuleDeleteCartsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CartModuleDeleteCartsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling ShoppingCartModuleApi->CartModuleDeleteCarts");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleDeleteCarts: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleDeleteCarts: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCartById (string id)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = CartModuleGetCartByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleGetCartByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ShoppingCartModuleApi->CartModuleGetCartById");

            var localVarPath = "/api/cart/carts/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCartById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCartById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = await CartModuleGetCartByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCartByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ShoppingCartModuleApi->CartModuleGetCartById");

            var localVarPath = "/api/cart/carts/{id}";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCartById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCartById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCurrentCart (string storeId, string customerId)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = CartModuleGetCurrentCartWithHttpInfo(storeId, customerId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleGetCurrentCartWithHttpInfo (string storeId, string customerId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling ShoppingCartModuleApi->CartModuleGetCurrentCart");
            // verify the required parameter 'customerId' is set
            if (customerId == null)
                throw new ApiException(400, "Missing required parameter 'customerId' when calling ShoppingCartModuleApi->CartModuleGetCurrentCart");

            var localVarPath = "/api/cart/{storeId}/{customerId}/carts/current";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) localVarPathParams.Add("customerId", Configuration.ApiClient.ParameterToString(customerId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCurrentCart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCurrentCart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCurrentCartAsync (string storeId, string customerId)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = await CartModuleGetCurrentCartAsyncWithHttpInfo(storeId, customerId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCurrentCartAsyncWithHttpInfo (string storeId, string customerId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling ShoppingCartModuleApi->CartModuleGetCurrentCart");
            // verify the required parameter 'customerId' is set
            if (customerId == null)
                throw new ApiException(400, "Missing required parameter 'customerId' when calling ShoppingCartModuleApi->CartModuleGetCurrentCart");

            var localVarPath = "/api/cart/{storeId}/{customerId}/carts/current";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) localVarPathParams.Add("customerId", Configuration.ApiClient.ParameterToString(customerId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCurrentCart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetCurrentCart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> localVarResponse = CartModuleGetPaymentMethodsWithHttpInfo(cartId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelPaymentMethod> > CartModuleGetPaymentMethodsWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethods");

            var localVarPath = "/api/cart/carts/{cartId}/paymentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cartId != null) localVarPathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> localVarResponse = await CartModuleGetPaymentMethodsAsyncWithHttpInfo(cartId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsAsyncWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethods");

            var localVarPath = "/api/cart/carts/{cartId}/paymentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cartId != null) localVarPathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> localVarResponse = CartModuleGetPaymentMethodsForStoreWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelPaymentMethod> > CartModuleGetPaymentMethodsForStoreWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethodsForStore");

            var localVarPath = "/api/cart/stores/{storeId}/paymentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> localVarResponse = await CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethodsForStore");

            var localVarPath = "/api/cart/stores/{storeId}/paymentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> localVarResponse = CartModuleGetShipmentMethodsWithHttpInfo(cartId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelShippingMethod> > CartModuleGetShipmentMethodsWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetShipmentMethods");

            var localVarPath = "/api/cart/carts/{cartId}/shipmentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cartId != null) localVarPathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelShippingMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>)));
            
        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> localVarResponse = await CartModuleGetShipmentMethodsAsyncWithHttpInfo(cartId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>> CartModuleGetShipmentMethodsAsyncWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetShipmentMethods");

            var localVarPath = "/api/cart/carts/{cartId}/shipmentMethods";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cartId != null) localVarPathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetShipmentMethods: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleGetShipmentMethods: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelShippingMethod>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>)));
            
        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        public VirtoCommerceCartModuleWebModelSearchResult CartModuleSearch (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> localVarResponse = CartModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelSearchResult > CartModuleSearchWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling ShoppingCartModuleApi->CartModuleSearch");

            var localVarPath = "/api/cart/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchAsync (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> localVarResponse = await CartModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>> CartModuleSearchAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling ShoppingCartModuleApi->CartModuleSearch");

            var localVarPath = "/api/cart/search";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleUpdate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = CartModuleUpdateWithHttpInfo(cart);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleUpdateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleUpdate");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cart.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                localVarPostBody = cart; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> localVarResponse = await CartModuleUpdateAsyncWithHttpInfo(cart);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleUpdateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleUpdate");

            var localVarPath = "/api/cart/carts";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            String localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (cart.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                localVarPostBody = cart; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CartModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CartModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }

    }
}
