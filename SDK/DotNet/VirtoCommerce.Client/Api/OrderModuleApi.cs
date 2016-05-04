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
        #region Synchronous Operations
        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrder (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrderFromCart (string id);

        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartWithHttpInfo (string id);
        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleDeleteWithHttpInfo (string id, string operationId);
        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns></returns>
        void OrderModuleDeleteOrdersByIds (List<string> ids);

        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleDeleteOrdersByIdsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id);

        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdWithHttpInfo (string id);
        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetByNumber (string number);

        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberWithHttpInfo (string number);
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start = null, DateTime? end = null);

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsWithHttpInfo (DateTime? start = null, DateTime? end = null);
        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        VirtoCommerceOrderModuleWebModelPaymentIn OrderModuleGetNewPayment (string id);

        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentWithHttpInfo (string id);
        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        VirtoCommerceOrderModuleWebModelShipment OrderModuleGetNewShipment (string id);

        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelShipment</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentWithHttpInfo (string id);
        /// <summary>
        /// Register customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in storefront checkout or manual order payment registration
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        VirtoCommerceOrderModuleWebModelProcessPaymentResult OrderModuleProcessOrderPayments (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null);

        /// <summary>
        /// Register customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in storefront checkout or manual order payment registration
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsWithHttpInfo (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null);
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (VirtoCommerceDomainOrderModelSearchCriteria criteria);

        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria);
        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns></returns>
        void OrderModuleUpdate (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> OrderModuleUpdateWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Add new customer order to system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id);

        /// <summary>
        /// Create new customer order based on shopping cart.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderFromCartAsyncWithHttpInfo (string id);
        /// <summary>
        /// Delete a concrete customer order operation (document)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteAsyncWithHttpInfo (string id, string operationId);
        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids);

        /// <summary>
        /// Delete a whole customer orders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id);

        /// <summary>
        /// Find customer order by id
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberAsync (string number);

        /// <summary>
        /// Find customer order by number
        /// </summary>
        /// <remarks>
        /// Return a single customer order with all nested documents or null if order was not found
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByNumberAsyncWithHttpInfo (string number);
        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start = null, DateTime? end = null);

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>> OrderModuleGetDashboardStatisticsAsyncWithHttpInfo (DateTime? start = null, DateTime? end = null);
        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id);

        /// <summary>
        /// Get new payment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new payment  document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelPaymentIn)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>> OrderModuleGetNewPaymentAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelShipment</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id);

        /// <summary>
        /// Get new shipment for specified customer order
        /// </summary>
        /// <remarks>
        /// Return new shipment document with populates all required properties.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelShipment)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelShipment>> OrderModuleGetNewShipmentAsyncWithHttpInfo (string id);
        /// <summary>
        /// Register customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in storefront checkout or manual order payment registration
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsAsync (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null);

        /// <summary>
        /// Register customer order payment in external payment system
        /// </summary>
        /// <remarks>
        /// Used in storefront checkout or manual order payment registration
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelProcessPaymentResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>> OrderModuleProcessOrderPaymentsAsyncWithHttpInfo (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null);
        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (VirtoCommerceDomainOrderModelSearchCriteria criteria);

        /// <summary>
        /// Search customer orders by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>> OrderModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria);
        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);

        /// <summary>
        /// Update a existing customer order
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleUpdateAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder);
        #endregion Asynchronous Operations
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
        /// Add new customer order to system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrder (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = OrderModuleCreateOrderWithHttpInfo(customerOrder);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleCreateOrderWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleCreateOrder");

            var localVarPath = "/api/order/customerOrders";
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
            if (customerOrder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = customerOrder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = await OrderModuleCreateOrderAsyncWithHttpInfo(customerOrder);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new customer order to system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleCreateOrder");

            var localVarPath = "/api/order/customerOrders";
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
            if (customerOrder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = customerOrder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleCreateOrderFromCart (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = OrderModuleCreateOrderFromCartWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleCreateOrderFromCartWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleCreateOrderFromCart");

            var localVarPath = "/api/order/customerOrders/{id}";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrderFromCart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrderFromCart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleCreateOrderFromCartAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = await OrderModuleCreateOrderFromCartAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create new customer order based on shopping cart. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">shopping cart id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleCreateOrderFromCartAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleCreateOrderFromCart");

            var localVarPath = "/api/order/customerOrders/{id}";
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
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrderFromCart: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleCreateOrderFromCart: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/order/customerOrders/{id}/operations/{operationId}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) localVarPathParams.Add("operationId", Configuration.ApiClient.ParameterToString(operationId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a concrete customer order operation (document) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <param name="operationId">operation id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteAsyncWithHttpInfo (string id, string operationId)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleDelete");
            // verify the required parameter 'operationId' is set
            if (operationId == null)
                throw new ApiException(400, "Missing required parameter 'operationId' when calling OrderModuleApi->OrderModuleDelete");

            var localVarPath = "/api/order/customerOrders/{id}/operations/{operationId}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (operationId != null) localVarPathParams.Add("operationId", Configuration.ApiClient.ParameterToString(operationId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns></returns>
        public void OrderModuleDeleteOrdersByIds (List<string> ids)
        {
             OrderModuleDeleteOrdersByIdsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> OrderModuleDeleteOrdersByIdsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleApi->OrderModuleDeleteOrdersByIds");

            var localVarPath = "/api/order/customerOrders";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task OrderModuleDeleteOrdersByIdsAsync (List<string> ids)
        {
             await OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a whole customer orders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">customer order ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleDeleteOrdersByIdsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling OrderModuleApi->OrderModuleDeleteOrdersByIds");

            var localVarPath = "/api/order/customerOrders";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleDeleteOrdersByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetById (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = OrderModuleGetByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleGetByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetById");

            var localVarPath = "/api/order/customerOrders/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = await OrderModuleGetByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find customer order by id Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetById");

            var localVarPath = "/api/order/customerOrders/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public VirtoCommerceOrderModuleWebModelCustomerOrder OrderModuleGetByNumber (string number)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = OrderModuleGetByNumberWithHttpInfo(number);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelCustomerOrder > OrderModuleGetByNumberWithHttpInfo (string number)
        {
            // verify the required parameter 'number' is set
            if (number == null)
                throw new ApiException(400, "Missing required parameter 'number' when calling OrderModuleApi->OrderModuleGetByNumber");

            var localVarPath = "/api/order/customerOrders/number/{number}";
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
            if (number != null) localVarPathParams.Add("number", Configuration.ApiClient.ParameterToString(number)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetByNumber: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetByNumber: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelCustomerOrder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelCustomerOrder> OrderModuleGetByNumberAsync (string number)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder> localVarResponse = await OrderModuleGetByNumberAsyncWithHttpInfo(number);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find customer order by number Return a single customer order with all nested documents or null if order was not found
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="number">customer order number</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelCustomerOrder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>> OrderModuleGetByNumberAsyncWithHttpInfo (string number)
        {
            // verify the required parameter 'number' is set
            if (number == null)
                throw new ApiException(400, "Missing required parameter 'number' when calling OrderModuleApi->OrderModuleGetByNumber");

            var localVarPath = "/api/order/customerOrders/number/{number}";
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
            if (number != null) localVarPathParams.Add("number", Configuration.ApiClient.ParameterToString(number)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetByNumber: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetByNumber: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelCustomerOrder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelCustomerOrder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelCustomerOrder)));
            
        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public VirtoCommerceOrderModuleWebModelDashboardStatisticsResult OrderModuleGetDashboardStatistics (DateTime? start = null, DateTime? end = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> localVarResponse = OrderModuleGetDashboardStatisticsWithHttpInfo(start, end);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelDashboardStatisticsResult > OrderModuleGetDashboardStatisticsWithHttpInfo (DateTime? start = null, DateTime? end = null)
        {

            var localVarPath = "/api/order/dashboardStatistics";
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
            if (start != null) localVarQueryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (end != null) localVarQueryParams.Add("end", Configuration.ApiClient.ParameterToString(end)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetDashboardStatistics: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetDashboardStatistics: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)));
            
        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> OrderModuleGetDashboardStatisticsAsync (DateTime? start = null, DateTime? end = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult> localVarResponse = await OrderModuleGetDashboardStatisticsAsyncWithHttpInfo(start, end);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get a some order statistic information for Commerce manager dashboard 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="start">start interval date (optional)</param>
        /// <param name="end">end interval date (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>> OrderModuleGetDashboardStatisticsAsyncWithHttpInfo (DateTime? start = null, DateTime? end = null)
        {

            var localVarPath = "/api/order/dashboardStatistics";
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
            if (start != null) localVarQueryParams.Add("start", Configuration.ApiClient.ParameterToString(start)); // query parameter
            if (end != null) localVarQueryParams.Add("end", Configuration.ApiClient.ParameterToString(end)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetDashboardStatistics: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetDashboardStatistics: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelDashboardStatisticsResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult)));
            
        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public VirtoCommerceOrderModuleWebModelPaymentIn OrderModuleGetNewPayment (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> localVarResponse = OrderModuleGetNewPaymentWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelPaymentIn > OrderModuleGetNewPaymentWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewPayment");

            var localVarPath = "/api/order/customerOrders/{id}/payments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewPayment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewPayment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelPaymentIn) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelPaymentIn)));
            
        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelPaymentIn</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelPaymentIn> OrderModuleGetNewPaymentAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn> localVarResponse = await OrderModuleGetNewPaymentAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new payment for specified customer order Return new payment  document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelPaymentIn)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>> OrderModuleGetNewPaymentAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewPayment");

            var localVarPath = "/api/order/customerOrders/{id}/payments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewPayment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewPayment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelPaymentIn>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelPaymentIn) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelPaymentIn)));
            
        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>VirtoCommerceOrderModuleWebModelShipment</returns>
        public VirtoCommerceOrderModuleWebModelShipment OrderModuleGetNewShipment (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelShipment> localVarResponse = OrderModuleGetNewShipmentWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelShipment</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelShipment > OrderModuleGetNewShipmentWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewShipment");

            var localVarPath = "/api/order/customerOrders/{id}/shipments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewShipment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewShipment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelShipment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelShipment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelShipment)));
            
        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelShipment</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelShipment> OrderModuleGetNewShipmentAsync (string id)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelShipment> localVarResponse = await OrderModuleGetNewShipmentAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new shipment for specified customer order Return new shipment document with populates all required properties.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">customer order id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelShipment)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelShipment>> OrderModuleGetNewShipmentAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling OrderModuleApi->OrderModuleGetNewShipment");

            var localVarPath = "/api/order/customerOrders/{id}/shipments/new";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewShipment: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleGetNewShipment: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelShipment>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelShipment) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelShipment)));
            
        }

        /// <summary>
        /// Register customer order payment in external payment system Used in storefront checkout or manual order payment registration
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public VirtoCommerceOrderModuleWebModelProcessPaymentResult OrderModuleProcessOrderPayments (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> localVarResponse = OrderModuleProcessOrderPaymentsWithHttpInfo(orderId, paymentId, bankCardInfo);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Register customer order payment in external payment system Used in storefront checkout or manual order payment registration
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelProcessPaymentResult > OrderModuleProcessOrderPaymentsWithHttpInfo (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null)
        {
            // verify the required parameter 'orderId' is set
            if (orderId == null)
                throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");
            // verify the required parameter 'paymentId' is set
            if (paymentId == null)
                throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");

            var localVarPath = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
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
            if (orderId != null) localVarPathParams.Add("orderId", Configuration.ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) localVarPathParams.Add("paymentId", Configuration.ApiClient.ParameterToString(paymentId)); // path parameter
            if (bankCardInfo != null && bankCardInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = bankCardInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleProcessOrderPayments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleProcessOrderPayments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelProcessPaymentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelProcessPaymentResult)));
            
        }

        /// <summary>
        /// Register customer order payment in external payment system Used in storefront checkout or manual order payment registration
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelProcessPaymentResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelProcessPaymentResult> OrderModuleProcessOrderPaymentsAsync (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult> localVarResponse = await OrderModuleProcessOrderPaymentsAsyncWithHttpInfo(orderId, paymentId, bankCardInfo);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Register customer order payment in external payment system Used in storefront checkout or manual order payment registration
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="orderId">customer order id</param>
        /// <param name="paymentId">payment id</param>
        /// <param name="bankCardInfo">banking card information (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelProcessPaymentResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>> OrderModuleProcessOrderPaymentsAsyncWithHttpInfo (string orderId, string paymentId, VirtoCommerceDomainPaymentModelBankCardInfo bankCardInfo = null)
        {
            // verify the required parameter 'orderId' is set
            if (orderId == null)
                throw new ApiException(400, "Missing required parameter 'orderId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");
            // verify the required parameter 'paymentId' is set
            if (paymentId == null)
                throw new ApiException(400, "Missing required parameter 'paymentId' when calling OrderModuleApi->OrderModuleProcessOrderPayments");

            var localVarPath = "/api/order/customerOrders/{orderId}/processPayment/{paymentId}";
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
            if (orderId != null) localVarPathParams.Add("orderId", Configuration.ApiClient.ParameterToString(orderId)); // path parameter
            if (paymentId != null) localVarPathParams.Add("paymentId", Configuration.ApiClient.ParameterToString(paymentId)); // path parameter
            if (bankCardInfo != null && bankCardInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(bankCardInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = bankCardInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleProcessOrderPayments: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleProcessOrderPayments: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelProcessPaymentResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelProcessPaymentResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelProcessPaymentResult)));
            
        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public VirtoCommerceOrderModuleWebModelSearchResult OrderModuleSearch (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> localVarResponse = OrderModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceOrderModuleWebModelSearchResult > OrderModuleSearchWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling OrderModuleApi->OrderModuleSearch");

            var localVarPath = "/api/order/customerOrders/search";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceOrderModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceOrderModuleWebModelSearchResult> OrderModuleSearchAsync (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult> localVarResponse = await OrderModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search customer orders by given criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceOrderModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>> OrderModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainOrderModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling OrderModuleApi->OrderModuleSearch");

            var localVarPath = "/api/order/customerOrders/search";
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
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceOrderModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceOrderModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceOrderModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns></returns>
        public void OrderModuleUpdate (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             OrderModuleUpdateWithHttpInfo(customerOrder);
        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> OrderModuleUpdateWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleUpdate");

            var localVarPath = "/api/order/customerOrders";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (customerOrder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = customerOrder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task OrderModuleUpdateAsync (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
             await OrderModuleUpdateAsyncWithHttpInfo(customerOrder);

        }

        /// <summary>
        /// Update a existing customer order 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="customerOrder">customer order</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> OrderModuleUpdateAsyncWithHttpInfo (VirtoCommerceOrderModuleWebModelCustomerOrder customerOrder)
        {
            // verify the required parameter 'customerOrder' is set
            if (customerOrder == null)
                throw new ApiException(400, "Missing required parameter 'customerOrder' when calling OrderModuleApi->OrderModuleUpdate");

            var localVarPath = "/api/order/customerOrders";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (customerOrder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(customerOrder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = customerOrder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling OrderModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
