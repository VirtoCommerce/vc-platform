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
    public interface IOrderModuleApi
    {
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleUpdateWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleUpdateAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        
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
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleDeleteOrdersByIdsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids);

        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="number">customer order number</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetByNumber (string number);
  
        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="number">customer order number</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberWithHttpInfo (string number);

        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="number">customer order number</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberAsync (string number);

        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="number">customer order number</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByNumberAsyncWithHttpInfo (string number);
        
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (VirtoCommerceDomainOrderModelSearchCriteria criteria);
  
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria);

        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (VirtoCommerceDomainOrderModelSearchCriteria criteria);

        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>> OrderModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria);
        
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id);
  
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdWithHttpInfo (string id);

        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id);

        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByIdAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartWithHttpInfo (string id);

        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id);

        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderFromCartAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleDeleteWithHttpInfo (string id, string operationId);

        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task OrderModuleDeleteAsync (string id, string operationId);

        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteAsyncWithHttpInfo (string id, string operationId);
        
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
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentWithHttpInfo (string id);

        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id);

        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelPaymentIn)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>> OrderModuleGetNewPaymentAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelShipment</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentWithHttpInfo (string id);

        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelShipment</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id);

        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelShipment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelShipment>> OrderModuleGetNewShipmentAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        VirtoCommerceOrderModuleWebModelProcessPaymentResult OrderModuleProcessOrderPayments (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);
  
        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsWithHttpInfo (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);

        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsAsync (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);

        /// <summary>
        /// Registration customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in front-end checkout or manual order payment registration
        /// </remarks>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelProcessPaymentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>> OrderModuleProcessOrderPaymentsAsyncWithHttpInfo (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId);
        
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start = null, DateTime? end = null);
  
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsWithHttpInfo (DateTime? start = null, DateTime? end = null);

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start = null, DateTime? end = null);

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>> OrderModuleGetDashboardStatisticsAsyncWithHttpInfo (DateTime? start = null, DateTime? end = null);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class OrderModuleApi : IOrderModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public OrderModuleApi(Configuration configuration)
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
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns></returns>
        public void OrderModuleUpdate (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             OrderModuleUpdateWithHttpInfo(customerOrder);
        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> OrderModuleUpdateWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleUpdate");
            
    
            var path_ = "/api/order/customerOrders";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (customerOrder.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                postBody = customerOrder; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             await OrderModuleUpdateAsyncWithHttpInfo(customerOrder);

        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleUpdateAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleUpdate");
            
    
            var path_ = "/api/order/customerOrders";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrder (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = OrderModuleCreateOrderWithHttpInfo(customerOrder);
             return response.Data;
        }

        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleCreateOrderWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleCreateOrder");
            
    
            var path_ = "/api/order/customerOrders";
    
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
            
            
            
            
            if (customerOrder.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                postBody = customerOrder; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrder: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
    
        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = await OrderModuleCreateOrderAsyncWithHttpInfo(customerOrder);
             return response.Data;

        }

        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null) throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleCreateOrder");
            
    
            var path_ = "/api/order/customerOrders";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrder: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
        
        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param> 
        /// <returns></returns>
        public void OrderModuleDeleteOrdersByIds (List<string> ids)
        {
             OrderModuleDeleteOrdersByIdsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> OrderModuleDeleteOrdersByIdsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleApi->OrderModuleDeleteOrdersByIds");
            
    
            var path_ = "/api/order/customerOrders";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids)
        {
             await OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleDeleteOrdersByIds");
            
    
            var path_ = "/api/order/customerOrders";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleDeleteOrdersByIds: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="number">customer order number</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetByNumber (string number)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = OrderModuleGetByNumberWithHttpInfo(number);
             return response.Data;
        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="number">customer order number</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleGetByNumberWithHttpInfo (string number)
        {
            
            // verify the required parameter 'number' is set
            if (number == null)
                throw new ApiException(400, "Missing required parameter 'number' when calling OrderModuleApi->OrderModuleGetByNumber");
            
    
            var path_ = "/api/order/customerOrders/number/{number}";
    
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
            if (number != null) pathParams.Add("number", Configuration.ApiClient.ParameterToString(number)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetByNumber: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetByNumber: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
    
        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="number">customer order number</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberAsync (string number)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = await OrderModuleGetByNumberAsyncWithHttpInfo(number);
             return response.Data;

        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="number">customer order number</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByNumberAsyncWithHttpInfo (string number)
        {
            // verify the required parameter 'number' is set
            if (number == null) throw new ApiException(400, "Missing required parameter 'number' when calling OrderModuleGetByNumber");
            
    
            var path_ = "/api/order/customerOrders/number/{number}";
    
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
            if (number != null) pathParams.Add("number", Configuration.ApiClient.ParameterToString(number)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetByNumber: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetByNumber: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
        
        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteria">criteria</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> response = OrderModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteria">criteria</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelSearchResult > OrderModuleSearchWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling OrderModuleApi->OrderModuleSearch");
            
    
            var path_ = "/api/order/customerOrders/search";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelSearchResult)));
            
        }
    
        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> response = await OrderModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>> OrderModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling OrderModuleSearch");
            
    
            var path_ = "/api/order/customerOrders/search";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelSearchResult)));
            
        }
        
        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = OrderModuleGetByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleGetByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetById");
            
    
            var path_ = "/api/order/customerOrders/{id}";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
    
        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = await OrderModuleGetByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetById");
            
    
            var path_ = "/api/order/customerOrders/{id}";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
        
        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrderFromCart (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = OrderModuleCreateOrderFromCartWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleCreateOrderFromCartWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleCreateOrderFromCart");
            
    
            var path_ = "/api/order/customerOrders/{id}";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
    
        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> response = await OrderModuleCreateOrderFromCartAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderFromCartAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleCreateOrderFromCart");
            
    
            var path_ = "/api/order/customerOrders/{id}";
    
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
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleCreateOrderFromCart: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }
        
        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <param name="operationId">operation id</param> 
        /// <returns></returns>
        public void OrderModuleDelete (string id, string operationId)
        {
             OrderModuleDeleteWithHttpInfo(id, operationId);
        }

        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <param name="operationId">operation id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> OrderModuleDeleteWithHttpInfo (string id, string operationId)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleDelete");
            
            // verify the required parameter 'operationId' is set
            if (operationId == null)
                throw new ApiException(400, "Missing required parameter 'operationId' when calling OrderModuleApi->OrderModuleDelete");
            
    
            var path_ = "/api/order/customerOrders/{id}/operations/{operationId}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) pathParams.Add("operationId", Configuration.ApiClient.ParameterToString(operationId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task OrderModuleDeleteAsync (string id, string operationId)
        {
             await OrderModuleDeleteAsyncWithHttpInfo(id, operationId);

        }

        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteAsyncWithHttpInfo (string id, string operationId)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleDelete");
            // verify the required parameter 'operationId' is set
            if (operationId == null) throw new ApiException(400, "Missing required parameter 'operationId' when calling OrderModuleDelete");
            
    
            var path_ = "/api/order/customerOrders/{id}/operations/{operationId}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) pathParams.Add("operationId", Configuration.ApiClient.ParameterToString(operationId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public VirtoCommerceOrderModuleWebModelPaymentIn OrderModuleGetNewPayment (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> response = OrderModuleGetNewPaymentWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelPaymentIn > OrderModuleGetNewPaymentWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewPayment");
            
    
            var path_ = "/api/order/customerOrders/{id}/payments/new";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewPayment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewPayment: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelPaymentIn) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelPaymentIn)));
            
        }
    
        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> response = await OrderModuleGetNewPaymentAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelPaymentIn)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>> OrderModuleGetNewPaymentAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewPayment");
            
    
            var path_ = "/api/order/customerOrders/{id}/payments/new";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewPayment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewPayment: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelPaymentIn) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelPaymentIn)));
            
        }
        
        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        public VirtoCommerceOrderModuleWebModelShipment OrderModuleGetNewShipment (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelShipment> response = OrderModuleGetNewShipmentWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelShipment</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelShipment > OrderModuleGetNewShipmentWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewShipment");
            
    
            var path_ = "/api/order/customerOrders/{id}/shipments/new";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewShipment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewShipment: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelShipment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelShipment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelShipment)));
            
        }
    
        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelShipment</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelShipment> response = await OrderModuleGetNewShipmentAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelShipment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelShipment>> OrderModuleGetNewShipmentAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleGetNewShipment");
            
    
            var path_ = "/api/order/customerOrders/{id}/shipments/new";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewShipment: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetNewShipment: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelShipment>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelShipment) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelShipment)));
            
        }
        
        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param> 
        /// <param name="orderId">customer order id</param> 
        /// <param name="paymentId">payment id</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public VirtoCommerceOrderModuleWebModelProcessPaymentResult OrderModuleProcessOrderPayments (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> response = OrderModuleProcessOrderPaymentsWithHttpInfo(bankCardInfo, orderId, paymentId);
             return response.Data;
        }

        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param> 
        /// <param name="orderId">customer order id</param> 
        /// <param name="paymentId">payment id</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelProcessPaymentResult > OrderModuleProcessOrderPaymentsWithHttpInfo (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
            
            // verify the required parameter 'bankCardInfo' is set
            if (bankCardInfo == null)
                throw new ApiException(400, "Missing required parameter 'bankCardInfo' when calling OrderModuleApi->OrderModuleProcessOrderPayments");
            
            // verify the required parameter 'orderId' is set
            if (orderId == null)
                throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");
            
            // verify the required parameter 'paymentId' is set
            if (paymentId == null)
                throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");
            
    
            var path_ = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
    
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
            if (orderId != null) pathParams.Add("orderId", Configuration.ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) pathParams.Add("paymentId", Configuration.ApiClient.ParameterToString(paymentId)); // path parameter
            
            
            
            
            if (bankCardInfo.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            }
            else
            {
                postBody = bankCardInfo; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleProcessOrderPayments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleProcessOrderPayments: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelProcessPaymentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelProcessPaymentResult)));
            
        }
    
        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsAsync (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> response = await OrderModuleProcessOrderPaymentsAsyncWithHttpInfo(bankCardInfo, orderId, paymentId);
             return response.Data;

        }

        /// <summary>
        /// Registration customer order payment in external payment system Used in front-end checkout or manual order payment registration
        /// </summary>
        /// <param name="bankCardInfo">banking card information</param>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelProcessPaymentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>> OrderModuleProcessOrderPaymentsAsyncWithHttpInfo (VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo, string orderId, string paymentId)
        {
            // verify the required parameter 'bankCardInfo' is set
            if (bankCardInfo == null) throw new ApiException(400, "Missing required parameter 'bankCardInfo' when calling OrderModuleProcessOrderPayments");
            // verify the required parameter 'orderId' is set
            if (orderId == null) throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleProcessOrderPayments");
            // verify the required parameter 'paymentId' is set
            if (paymentId == null) throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleProcessOrderPayments");
            
    
            var path_ = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
    
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
            if (orderId != null) pathParams.Add("orderId", Configuration.ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) pathParams.Add("paymentId", Configuration.ApiClient.ParameterToString(paymentId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleProcessOrderPayments: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleProcessOrderPayments: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelProcessPaymentResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelProcessPaymentResult)));
            
        }
        
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param> 
        /// <param name="end">end interval date</param> 
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start = null, DateTime? end = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> response = OrderModuleGetDashboardStatisticsWithHttpInfo(start, end);
             return response.Data;
        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param> 
        /// <param name="end">end interval date</param> 
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelDashboardStatisticsResult > OrderModuleGetDashboardStatisticsWithHttpInfo (DateTime? start = null, DateTime? end = null)
        {
            
    
            var path_ = "/api/order/dashboardStatistics";
    
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
            
            if (start != null) queryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (end != null) queryParams.Add("end", Configuration.ApiClient.ParameterToString(end)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)));
            
        }
    
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start = null, DateTime? end = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> response = await OrderModuleGetDashboardStatisticsAsyncWithHttpInfo(start, end);
             return response.Data;

        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <param name="start">start interval date</param>
        /// <param name="end">end interval date</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>> OrderModuleGetDashboardStatisticsAsyncWithHttpInfo (DateTime? start = null, DateTime? end = null)
        {
            
    
            var path_ = "/api/order/dashboardStatistics";
    
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
            
            if (start != null) queryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (end != null) queryParams.Add("end", Configuration.ApiClient.ParameterToString(end)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling OrderModuleGetDashboardStatistics: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)));
            
        }
        
    }
    
}
