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
        
        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleUpdate (VirtoCommerceCartModuleWebModelShoppingCart cart);
  
        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleUpdateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart);
  
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);

        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleCreateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart);
        
        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns></returns>
        void CartModuleDeleteCarts (List<string> ids);
  
        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CartModuleDeleteCartsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids);

        /// <summary>
        /// Delete shopping carts by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CartModuleDeleteCartsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId);
  
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsWithHttpInfo (string cartId);

        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId);

        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsAsyncWithHttpInfo (string cartId);
        
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId);
  
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsWithHttpInfo (string cartId);

        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId);

        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>> CartModuleGetShipmentMethodsAsyncWithHttpInfo (string cartId);
        
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCartById (string id);
  
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdWithHttpInfo (string id);

        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdAsync (string id);

        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCartByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        VirtoCommerceCartModuleWebModelSearchResult CartModuleSearch (VirtoCommerceCartModuleWebModelSearchCriteria criteria);
  
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria);

        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchAsync (VirtoCommerceCartModuleWebModelSearchCriteria criteria);

        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>> CartModuleSearchAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria);
        
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId);
  
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreWithHttpInfo (string storeId);

        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId);

        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCurrentCartWithHttpInfo (string storeId, string customerId);

        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// Returns shopping cart or null if it is not found
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCurrentCartAsyncWithHttpInfo (string storeId, string customerId);
        
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
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleUpdate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = CartModuleUpdateWithHttpInfo(cart);
             return response.Data;
        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleUpdateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleUpdate");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (cart.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                postBody = cart; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
    
        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = await CartModuleUpdateAsyncWithHttpInfo(cart);
             return response.Data;

        }

        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleUpdateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleUpdate");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
        
        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = CartModuleCreateWithHttpInfo(cart);
             return response.Data;
        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleCreateWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            
            // verify the required parameter 'cart' is set
            if (cart == null)
                throw new ApiException(400, "Missing required parameter 'cart' when calling ShoppingCartModuleApi->CartModuleCreate");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (cart.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            }
            else
            {
                postBody = cart; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
    
        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = await CartModuleCreateAsyncWithHttpInfo(cart);
             return response.Data;

        }

        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleCreateAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleCreate");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(cart); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleCreate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
        
        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param> 
        /// <returns></returns>
        public void CartModuleDeleteCarts (List<string> ids)
        {
             CartModuleDeleteCartsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CartModuleDeleteCartsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling ShoppingCartModuleApi->CartModuleDeleteCarts");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleDeleteCarts: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleDeleteCarts: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids)
        {
             await CartModuleDeleteCartsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CartModuleDeleteCartsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CartModuleDeleteCarts");
            
    
            var path_ = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleDeleteCarts: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleDeleteCarts: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> response = CartModuleGetPaymentMethodsWithHttpInfo(cartId);
             return response.Data;
        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelPaymentMethod> > CartModuleGetPaymentMethodsWithHttpInfo (string cartId)
        {
            
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethods");
            
    
            var path_ = "/api/cart/carts/{cartId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }
    
        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> response = await CartModuleGetPaymentMethodsAsyncWithHttpInfo(cartId);
             return response.Data;

        }

        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsAsyncWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetPaymentMethods");
            
    
            var path_ = "/api/cart/carts/{cartId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethods: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }
        
        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> response = CartModuleGetShipmentMethodsWithHttpInfo(cartId);
             return response.Data;
        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelShippingMethod> > CartModuleGetShipmentMethodsWithHttpInfo (string cartId)
        {
            
            // verify the required parameter 'cartId' is set
            if (cartId == null)
                throw new ApiException(400, "Missing required parameter 'cartId' when calling ShoppingCartModuleApi->CartModuleGetShipmentMethods");
            
    
            var path_ = "/api/cart/carts/{cartId}/shipmentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelShippingMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>)));
            
        }
    
        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>> response = await CartModuleGetShipmentMethodsAsyncWithHttpInfo(cartId);
             return response.Data;

        }

        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelShippingMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>> CartModuleGetShipmentMethodsAsyncWithHttpInfo (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetShipmentMethods");
            
    
            var path_ = "/api/cart/carts/{cartId}/shipmentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", Configuration.ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelShippingMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelShippingMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>)));
            
        }
        
        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCartById (string id)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = CartModuleGetCartByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param> 
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelShoppingCart > CartModuleGetCartByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling ShoppingCartModuleApi->CartModuleGetCartById");
            
    
            var path_ = "/api/cart/carts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetCartById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetCartById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
    
        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCartByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = await CartModuleGetCartByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCartByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CartModuleGetCartById");
            
    
            var path_ = "/api/cart/carts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetCartById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetCartById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
        
        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        public VirtoCommerceCartModuleWebModelSearchResult CartModuleSearch (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> response = CartModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteria">Search criteria</param> 
        /// <returns>ApiResponse of VirtoCommerceCartModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceCartModuleWebModelSearchResult > CartModuleSearchWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling ShoppingCartModuleApi->CartModuleSearch");
            
    
            var path_ = "/api/cart/search";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (criteria.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                postBody = criteria; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelSearchResult)));
            
        }
    
        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchAsync (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelSearchResult> response = await CartModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>> CartModuleSearchAsyncWithHttpInfo (VirtoCommerceCartModuleWebModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling CartModuleSearch");
            
    
            var path_ = "/api/cart/search";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelSearchResult)));
            
        }
        
        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> response = CartModuleGetPaymentMethodsForStoreWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceCartModuleWebModelPaymentMethod> > CartModuleGetPaymentMethodsForStoreWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling ShoppingCartModuleApi->CartModuleGetPaymentMethodsForStore");
            
    
            var path_ = "/api/cart/stores/{storeId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }
    
        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>> response = await CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCartModuleWebModelPaymentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>> CartModuleGetPaymentMethodsForStoreAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetPaymentMethodsForStore");
            
    
            var path_ = "/api/cart/stores/{storeId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCartModuleWebModelPaymentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCartModuleWebModelPaymentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>)));
            
        }
        
        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="customerId">Customer id</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleGetCurrentCart (string storeId, string customerId)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = CartModuleGetCurrentCartWithHttpInfo(storeId, customerId);
             return response.Data;
        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
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
            
    
            var path_ = "/api/cart/{storeId}/{customerId}/carts/current";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) pathParams.Add("customerId", Configuration.ApiClient.ParameterToString(customerId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetCurrentCart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetCurrentCart: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
    
        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleGetCurrentCartAsync (string storeId, string customerId)
        {
             ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart> response = await CartModuleGetCurrentCartAsyncWithHttpInfo(storeId, customerId);
             return response.Data;

        }

        /// <summary>
        /// Get shopping cart by store id and customer id Returns shopping cart or null if it is not found
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCartModuleWebModelShoppingCart)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>> CartModuleGetCurrentCartAsyncWithHttpInfo (string storeId, string customerId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetCurrentCart");
            // verify the required parameter 'customerId' is set
            if (customerId == null) throw new ApiException(400, "Missing required parameter 'customerId' when calling CartModuleGetCurrentCart");
            
    
            var path_ = "/api/cart/{storeId}/{customerId}/carts/current";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            Object postBody = null;

            // to determine the Content-Type header
            String[] httpContentTypes = new String[] {
                
            };
            String httpContentType = Configuration.ApiClient.SelectHeaderContentType(httpContentTypes);

            // to determine the Accept header
            String[] httpHeaderAccepts = new String[] {
                "application/json", "text/json"
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) pathParams.Add("customerId", Configuration.ApiClient.ParameterToString(customerId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling CartModuleGetCurrentCart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CartModuleGetCurrentCart: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCartModuleWebModelShoppingCart>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCartModuleWebModelShoppingCart) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCartModuleWebModelShoppingCart)));
            
        }
        
    }
    
}
