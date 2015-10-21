using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.SwaggerApiClient.Client;
using VirtoCommerce.SwaggerApiClient.Model;


namespace VirtoCommerce.SwaggerApiClient.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IShoppingCartModuleApi
    {
        
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword">Gets or sets the value of search criteria keyword</param>
        /// <param name="criteriaCustomerId">Gets or sets the value of search criteria customer id</param>
        /// <param name="criteriaStoreId">Gets or sets the value of search criteria store id</param>
        /// <param name="criteriaStart">Gets or sets the value of search criteria skip records count</param>
        /// <param name="criteriaCount">Gets or sets the value of search criteria page size</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        VirtoCommerceCartModuleWebModelSearchResult CartModuleSearchCarts (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, int? criteriaStart, int? criteriaCount);
  
        /// <summary>
        /// Search for shopping carts by criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword">Gets or sets the value of search criteria keyword</param>
        /// <param name="criteriaCustomerId">Gets or sets the value of search criteria customer id</param>
        /// <param name="criteriaStoreId">Gets or sets the value of search criteria store id</param>
        /// <param name="criteriaStart">Gets or sets the value of search criteria skip records count</param>
        /// <param name="criteriaCount">Gets or sets the value of search criteria page size</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchCartsAsync (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, int? criteriaStart, int? criteriaCount);
        
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
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);
        
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns></returns>
        void CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart);
  
        /// <summary>
        /// Create shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cart">Shopping cart model</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids);
        
        /// <summary>
        /// Apply coupon for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        VirtoCommerceCartModuleWebModelShoppingCart CartModuleApplyCoupon (string cartId, string couponCode);
  
        /// <summary>
        /// Apply coupon for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleApplyCouponAsync (string cartId, string couponCode);
        
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId);
  
        /// <summary>
        /// Get payment methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId);
        
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId);
  
        /// <summary>
        /// Get shipping methods for shopping cart
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId);
        
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns></returns>
        void CartModuleGetCartById (string id);
  
        /// <summary>
        /// Get shopping cart by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Shopping cart id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CartModuleGetCartByIdAsync (string id);
        
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId);
  
        /// <summary>
        /// Get payment methods for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId);
        
        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns></returns>
        void CartModuleGetCurrentCart (string storeId, string customerId);
  
        /// <summary>
        /// Get shopping cart by store id and customer id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CartModuleGetCurrentCartAsync (string storeId, string customerId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ShoppingCartModuleApi : IShoppingCartModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ShoppingCartModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ShoppingCartModuleApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        
        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteriaKeyword">Gets or sets the value of search criteria keyword</param> 
        /// <param name="criteriaCustomerId">Gets or sets the value of search criteria customer id</param> 
        /// <param name="criteriaStoreId">Gets or sets the value of search criteria store id</param> 
        /// <param name="criteriaStart">Gets or sets the value of search criteria skip records count</param> 
        /// <param name="criteriaCount">Gets or sets the value of search criteria page size</param> 
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>            
        public VirtoCommerceCartModuleWebModelSearchResult CartModuleSearchCarts (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleSearchCarts: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleSearchCarts: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCartModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelSearchResult), response.Headers);
        }
    
        /// <summary>
        /// Search for shopping carts by criteria 
        /// </summary>
        /// <param name="criteriaKeyword">Gets or sets the value of search criteria keyword</param>
        /// <param name="criteriaCustomerId">Gets or sets the value of search criteria customer id</param>
        /// <param name="criteriaStoreId">Gets or sets the value of search criteria store id</param>
        /// <param name="criteriaStart">Gets or sets the value of search criteria skip records count</param>
        /// <param name="criteriaCount">Gets or sets the value of search criteria page size</param>
        /// <returns>VirtoCommerceCartModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelSearchResult> CartModuleSearchCartsAsync (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleSearchCarts: " + response.Content, response.Content);

            return (VirtoCommerceCartModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>            
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleUpdate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleUpdate");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(cart); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCartModuleWebModelShoppingCart) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelShoppingCart), response.Headers);
        }
    
        /// <summary>
        /// Update shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleUpdateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleUpdate");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(cart); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleUpdate: " + response.Content, response.Content);

            return (VirtoCommerceCartModuleWebModelShoppingCart) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelShoppingCart), response.Headers);
        }
        
        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param> 
        /// <returns></returns>            
        public void CartModuleCreate (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleCreate");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(cart); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create shopping cart 
        /// </summary>
        /// <param name="cart">Shopping cart model</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CartModuleCreateAsync (VirtoCommerceCartModuleWebModelShoppingCart cart)
        {
            // verify the required parameter 'cart' is set
            if (cart == null) throw new ApiException(400, "Missing required parameter 'cart' when calling CartModuleCreate");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = ApiClient.Serialize(cart); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleCreate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param> 
        /// <returns></returns>            
        public void CartModuleDeleteCarts (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CartModuleDeleteCarts");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleDeleteCarts: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleDeleteCarts: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete shopping carts by ids 
        /// </summary>
        /// <param name="ids">Array of shopping cart ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CartModuleDeleteCartsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CartModuleDeleteCarts");
            
    
            var path = "/api/cart/carts";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleDeleteCarts: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Apply coupon for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <param name="couponCode">Coupon code</param> 
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>            
        public VirtoCommerceCartModuleWebModelShoppingCart CartModuleApplyCoupon (string cartId, string couponCode)
        {
            
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleApplyCoupon");
            
            // verify the required parameter 'couponCode' is set
            if (couponCode == null) throw new ApiException(400, "Missing required parameter 'couponCode' when calling CartModuleApplyCoupon");
            
    
            var path = "/api/cart/carts/{cartId}/coupons/{couponCode}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            if (couponCode != null) pathParams.Add("couponCode", ApiClient.ParameterToString(couponCode)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleApplyCoupon: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleApplyCoupon: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCartModuleWebModelShoppingCart) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelShoppingCart), response.Headers);
        }
    
        /// <summary>
        /// Apply coupon for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>VirtoCommerceCartModuleWebModelShoppingCart</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCartModuleWebModelShoppingCart> CartModuleApplyCouponAsync (string cartId, string couponCode)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleApplyCoupon");
            // verify the required parameter 'couponCode' is set
            if (couponCode == null) throw new ApiException(400, "Missing required parameter 'couponCode' when calling CartModuleApplyCoupon");
            
    
            var path = "/api/cart/carts/{cartId}/coupons/{couponCode}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            if (couponCode != null) pathParams.Add("couponCode", ApiClient.ParameterToString(couponCode)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleApplyCoupon: " + response.Content, response.Content);

            return (VirtoCommerceCartModuleWebModelShoppingCart) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCartModuleWebModelShoppingCart), response.Headers);
        }
        
        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethods (string cartId)
        {
            
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetPaymentMethods");
            
    
            var path = "/api/cart/carts/{cartId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethods: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCartModuleWebModelPaymentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>), response.Headers);
        }
    
        /// <summary>
        /// Get payment methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsAsync (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetPaymentMethods");
            
    
            var path = "/api/cart/carts/{cartId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethods: " + response.Content, response.Content);

            return (List<VirtoCommerceCartModuleWebModelPaymentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>), response.Headers);
        }
        
        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceCartModuleWebModelShippingMethod> CartModuleGetShipmentMethods (string cartId)
        {
            
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetShipmentMethods");
            
    
            var path = "/api/cart/carts/{cartId}/shipmentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCartModuleWebModelShippingMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>), response.Headers);
        }
    
        /// <summary>
        /// Get shipping methods for shopping cart 
        /// </summary>
        /// <param name="cartId">Shopping cart id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelShippingMethod>> CartModuleGetShipmentMethodsAsync (string cartId)
        {
            // verify the required parameter 'cartId' is set
            if (cartId == null) throw new ApiException(400, "Missing required parameter 'cartId' when calling CartModuleGetShipmentMethods");
            
    
            var path = "/api/cart/carts/{cartId}/shipmentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (cartId != null) pathParams.Add("cartId", ApiClient.ParameterToString(cartId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetShipmentMethods: " + response.Content, response.Content);

            return (List<VirtoCommerceCartModuleWebModelShippingMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelShippingMethod>), response.Headers);
        }
        
        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param> 
        /// <returns></returns>            
        public void CartModuleGetCartById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CartModuleGetCartById");
            
    
            var path = "/api/cart/carts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCartById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCartById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get shopping cart by id 
        /// </summary>
        /// <param name="id">Shopping cart id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CartModuleGetCartByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CartModuleGetCartById");
            
    
            var path = "/api/cart/carts/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCartById: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceCartModuleWebModelPaymentMethod> CartModuleGetPaymentMethodsForStore (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetPaymentMethodsForStore");
            
    
            var path = "/api/cart/stores/{storeId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCartModuleWebModelPaymentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>), response.Headers);
        }
    
        /// <summary>
        /// Get payment methods for store 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCartModuleWebModelPaymentMethod>> CartModuleGetPaymentMethodsForStoreAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetPaymentMethodsForStore");
            
    
            var path = "/api/cart/stores/{storeId}/paymentMethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetPaymentMethodsForStore: " + response.Content, response.Content);

            return (List<VirtoCommerceCartModuleWebModelPaymentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCartModuleWebModelPaymentMethod>), response.Headers);
        }
        
        /// <summary>
        /// Get shopping cart by store id and customer id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="customerId">Customer id</param> 
        /// <returns></returns>            
        public void CartModuleGetCurrentCart (string storeId, string customerId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetCurrentCart");
            
            // verify the required parameter 'customerId' is set
            if (customerId == null) throw new ApiException(400, "Missing required parameter 'customerId' when calling CartModuleGetCurrentCart");
            
    
            var path = "/api/cart/{storeId}/{customerId}/carts/current";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) pathParams.Add("customerId", ApiClient.ParameterToString(customerId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCurrentCart: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCurrentCart: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get shopping cart by store id and customer id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="customerId">Customer id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CartModuleGetCurrentCartAsync (string storeId, string customerId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling CartModuleGetCurrentCart");
            // verify the required parameter 'customerId' is set
            if (customerId == null) throw new ApiException(400, "Missing required parameter 'customerId' when calling CartModuleGetCurrentCart");
            
    
            var path = "/api/cart/{storeId}/{customerId}/carts/current";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (customerId != null) pathParams.Add("customerId", ApiClient.ParameterToString(customerId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CartModuleGetCurrentCart: " + response.Content, response.Content);

            
            return;
        }
        
    }
    
}
