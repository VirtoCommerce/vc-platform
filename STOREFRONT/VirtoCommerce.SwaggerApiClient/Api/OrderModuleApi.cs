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
    public interface IOrderModuleApi
    {
        
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaEmployeeId"></param>
        /// <param name="criteriaStoreIds"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (string criteriaResponseGroup, string criteriaKeyword, string criteriaCustomerId, string criteriaEmployeeId, List<string> criteriaStoreIds, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount);
  
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaEmployeeId"></param>
        /// <param name="criteriaStoreIds"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (string criteriaResponseGroup, string criteriaKeyword, string criteriaCustomerId, string criteriaEmployeeId, List<string> criteriaStoreIds, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount);
        
        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns></returns>
        void OrderModuleUpdate (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
  
        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns></returns>
        System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        
        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrder (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
  
        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        
        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns></returns>
        void OrderModuleDeleteOrdersByIds (List<string> ids);
  
        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns></returns>
        System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids);
        
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id);
  
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id);
        
        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">shopping cart id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrderFromCart (string id);
  
        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">shopping cart id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id);
        
        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns></returns>
        void OrderModuleDelete (string id, string operationId);
  
        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task OrderModuleDeleteAsync (string id, string operationId);
        
        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        VirtoCommerceOrderModuleWebModelPaymentIn OrderModuleGetNewPayment (string id);
  
        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id);
        
        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        VirtoCommerceOrderModuleWebModelShipment OrderModuleGetNewShipment (string id);
  
        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id);
        
        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleProcessOrderPayments (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);
  
        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleProcessOrderPaymentsAsync (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);
        
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start, DateTime? end);
  
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start, DateTime? end);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OrderModuleApi : IOrderModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public OrderModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public OrderModuleApi(String basePath)
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
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteriaResponseGroup"></param> 
        /// <param name="criteriaKeyword"></param> 
        /// <param name="criteriaCustomerId"></param> 
        /// <param name="criteriaEmployeeId"></param> 
        /// <param name="criteriaStoreIds"></param> 
        /// <param name="criteriaStartDate"></param> 
        /// <param name="criteriaEndDate"></param> 
        /// <param name="criteriaStart"></param> 
        /// <param name="criteriaCount"></param> 
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>            
        public VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (string criteriaResponseGroup, string criteriaKeyword, string criteriaCustomerId, string criteriaEmployeeId, List<string> criteriaStoreIds, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/order/customerOrders";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaEmployeeId != null) queryParams.Add("criteria.employeeId", ApiClient.ParameterToString(criteriaEmployeeId)); // query parameter
            if (criteriaStoreIds != null) queryParams.Add("criteria.storeIds", ApiClient.ParameterToString(criteriaStoreIds)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelSearchResult), response.Headers);
        }
    
        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaEmployeeId"></param>
        /// <param name="criteriaStoreIds"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (string criteriaResponseGroup, string criteriaKeyword, string criteriaCustomerId, string criteriaEmployeeId, List<string> criteriaStoreIds, DateTime? criteriaStartDate, DateTime? criteriaEndDate, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/order/customerOrders";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaEmployeeId != null) queryParams.Add("criteria.employeeId", ApiClient.ParameterToString(criteriaEmployeeId)); // query parameter
            if (criteriaStoreIds != null) queryParams.Add("criteria.storeIds", ApiClient.ParameterToString(criteriaStoreIds)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleSearch: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns></returns>            
        public void OrderModuleUpdate (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleUpdate");
            
    
            var path = "/api/order/customerOrders";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(customerOrder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleUpdate");
            
    
            var path = "/api/order/customerOrders";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(customerOrder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>            
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrder (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleCreateOrder");
            
    
            var path = "/api/order/customerOrders";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(customerOrder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrder: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
    
        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleCreateOrder");
            
    
            var path = "/api/order/customerOrders";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(customerOrder); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrder: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
        
        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param> 
        /// <returns></returns>            
        public void OrderModuleDeleteOrdersByIds (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleDeleteOrdersByIds");
            
    
            var path = "/api/order/customerOrders";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleDeleteOrdersByIds");
            
    
            var path = "/api/order/customerOrders";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>            
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetById");
            
    
            var path = "/api/order/customerOrders/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
    
        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetById");
            
    
            var path = "/api/order/customerOrders/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetById: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
        
        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>            
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrderFromCart (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleCreateOrderFromCart");
            
    
            var path = "/api/order/customerOrders/{id}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
    
        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleCreateOrderFromCart");
            
    
            var path = "/api/order/customerOrders/{id}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
        
        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <param name="operationId">operation id</param> 
        /// <returns></returns>            
        public void OrderModuleDelete (string id, string operationId)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleDelete");
            
            // verify the required parameter 'operationId' is set
            if (operationId == null) throw new ApiException(400, "Missing required parameter 'operationId' when calling OrderModuleDelete");
            
    
            var path = "/api/order/customerOrders/{id}/operations/{operationId}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) pathParams.Add("operationId", ApiClient.ParameterToString(operationId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OrderModuleDeleteAsync (string id, string operationId)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleDelete");
            // verify the required parameter 'operationId' is set
            if (operationId == null) throw new ApiException(400, "Missing required parameter 'operationId' when calling OrderModuleDelete");
            
    
            var path = "/api/order/customerOrders/{id}/operations/{operationId}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) pathParams.Add("operationId", ApiClient.ParameterToString(operationId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>            
        public VirtoCommerceOrderModuleWebModelPaymentIn OrderModuleGetNewPayment (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewPayment");
            
    
            var path = "/api/order/customerOrders/{id}/payments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewPayment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewPayment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelPaymentIn) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelPaymentIn), response.Headers);
        }
    
        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewPayment");
            
    
            var path = "/api/order/customerOrders/{id}/payments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewPayment: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelPaymentIn) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelPaymentIn), response.Headers);
        }
        
        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>            
        public VirtoCommerceOrderModuleWebModelShipment OrderModuleGetNewShipment (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewShipment");
            
    
            var path = "/api/order/customerOrders/{id}/shipments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewShipment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewShipment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelShipment) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelShipment), response.Headers);
        }
    
        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewShipment");
            
    
            var path = "/api/order/customerOrders/{id}/shipments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetNewShipment: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelShipment) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelShipment), response.Headers);
        }
        
        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param> 
        /// <param name="orderId">customer order id</param> 
        /// <param name="paymentId">payment id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>            
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleProcessOrderPayments (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
            
            // verify the required parameter 'bankCardInfo' is set
            if (bankCardInfo == null) throw new ApiException(400, "Missing required parameter 'bankCardInfo' when calling OrderModuleProcessOrderPayments");
            
            // verify the required parameter 'orderId' is set
            if (orderId == null) throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleProcessOrderPayments");
            
            // verify the required parameter 'paymentId' is set
            if (paymentId == null) throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleProcessOrderPayments");
            
    
            var path = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
    
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
            if (orderId != null) pathParams.Add("orderId", ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) pathParams.Add("paymentId", ApiClient.ParameterToString(paymentId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleProcessOrderPayments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleProcessOrderPayments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
    
        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleProcessOrderPaymentsAsync (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
            // verify the required parameter 'bankCardInfo' is set
            if (bankCardInfo == null) throw new ApiException(400, "Missing required parameter 'bankCardInfo' when calling OrderModuleProcessOrderPayments");
            // verify the required parameter 'orderId' is set
            if (orderId == null) throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleProcessOrderPayments");
            // verify the required parameter 'paymentId' is set
            if (paymentId == null) throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleProcessOrderPayments");
            
    
            var path = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
    
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
            if (orderId != null) pathParams.Add("orderId", ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) pathParams.Add("paymentId", ApiClient.ParameterToString(paymentId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleProcessOrderPayments: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelCustomerOrder) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder), response.Headers);
        }
        
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param> 
        /// <param name="end">end interval date</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>            
        public VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start, DateTime? end)
        {
            
    
            var path = "/api/order/dashboardStatistics";
    
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
            
            if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (end != null) queryParams.Add("end", ApiClient.ParameterToString(end)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult), response.Headers);
        }
    
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start, DateTime? end)
        {
            
    
            var path = "/api/order/dashboardStatistics";
    
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
            
            if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
            if (end != null) queryParams.Add("end", ApiClient.ParameterToString(end)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.Content, response.Content);

            return (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult), response.Headers);
        }
        
    }
    
}
