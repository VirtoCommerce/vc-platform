using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;


namespace VirtoCommerce.Client.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IQuoteModuleApi
    {
        
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        VirtoCommerceDomainQuoteModelQuoteRequestSearchResult QuoteModuleSearch (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null);
  
        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>ApiResponse of VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> QuoteModuleSearchWithHttpInfo (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>Task of VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> QuoteModuleSearchAsync (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null);

        /// <summary>
        /// Search RFQ by given criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null);
        
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        void QuoteModuleUpdate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
  
        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleUpdateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Update a existing RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleUpdateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCreate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
  
        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Create new RFQ
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        void QuoteModuleDelete (List<string> ids);
  
        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> QuoteModuleDeleteWithHttpInfo (List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified quotes by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleDeleteAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCalculateTotals (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
  
        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);

        /// <summary>
        /// Calculate totals
        /// </summary>
        /// <remarks>
        /// Return totals for selected tier prices
        /// </remarks>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleGetById (string id);
  
        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdWithHttpInfo (string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id);

        /// <summary>
        /// Get RFQ by id
        /// </summary>
        /// <remarks>
        /// Return a single RFQ
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id);
  
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsWithHttpInfo (string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id);

        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class QuoteModuleApi : IQuoteModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public QuoteModuleApi(Configuration configuration)
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
        /// Search RFQ by given criteria 
        /// </summary>
        /// <param name="criteriaKeyword"></param> 
        /// <param name="criteriaCustomerId"></param> 
        /// <param name="criteriaStoreId"></param> 
        /// <param name="criteriaStartDate"></param> 
        /// <param name="criteriaEndDate"></param> 
        /// <param name="criteriaStatus"></param> 
        /// <param name="criteriaTag"></param> 
        /// <param name="criteriaStart"></param> 
        /// <param name="criteriaCount"></param> 
        /// <returns>VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        public VirtoCommerceDomainQuoteModelQuoteRequestSearchResult QuoteModuleSearch (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null)
        {
             ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> response = QuoteModuleSearchWithHttpInfo(criteriaKeyword, criteriaCustomerId, criteriaStoreId, criteriaStartDate, criteriaEndDate, criteriaStatus, criteriaTag, criteriaStart, criteriaCount);
             return response.Data;
        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <param name="criteriaKeyword"></param> 
        /// <param name="criteriaCustomerId"></param> 
        /// <param name="criteriaStoreId"></param> 
        /// <param name="criteriaStartDate"></param> 
        /// <param name="criteriaEndDate"></param> 
        /// <param name="criteriaStatus"></param> 
        /// <param name="criteriaTag"></param> 
        /// <param name="criteriaStart"></param> 
        /// <param name="criteriaCount"></param> 
        /// <returns>ApiResponse of VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        public ApiResponse< VirtoCommerceDomainQuoteModelQuoteRequestSearchResult > QuoteModuleSearchWithHttpInfo (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null)
        {
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", Configuration.ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", Configuration.ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", Configuration.ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", Configuration.ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStatus != null) queryParams.Add("criteria.status", Configuration.ApiClient.ParameterToString(criteriaStatus)); // query parameter
            if (criteriaTag != null) queryParams.Add("criteria.tag", Configuration.ApiClient.ParameterToString(criteriaTag)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", Configuration.ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", Configuration.ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainQuoteModelQuoteRequestSearchResult)));
            
        }
    
        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>Task of VirtoCommerceDomainQuoteModelQuoteRequestSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> QuoteModuleSearchAsync (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null)
        {
             ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> response = await QuoteModuleSearchAsyncWithHttpInfo(criteriaKeyword, criteriaCustomerId, criteriaStoreId, criteriaStartDate, criteriaEndDate, criteriaStatus, criteriaTag, criteriaStart, criteriaCount);
             return response.Data;

        }

        /// <summary>
        /// Search RFQ by given criteria 
        /// </summary>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaCustomerId"></param>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaStartDate"></param>
        /// <param name="criteriaEndDate"></param>
        /// <param name="criteriaStatus"></param>
        /// <param name="criteriaTag"></param>
        /// <param name="criteriaStart"></param>
        /// <param name="criteriaCount"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult>> QuoteModuleSearchAsyncWithHttpInfo (string criteriaKeyword = null, string criteriaCustomerId = null, string criteriaStoreId = null, DateTime? criteriaStartDate = null, DateTime? criteriaEndDate = null, string criteriaStatus = null, string criteriaTag = null, int? criteriaStart = null, int? criteriaCount = null)
        {
            
    
            var path_ = "/api/quote/requests";
    
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCustomerId != null) queryParams.Add("criteria.customerId", Configuration.ApiClient.ParameterToString(criteriaCustomerId)); // query parameter
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", Configuration.ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", Configuration.ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", Configuration.ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStatus != null) queryParams.Add("criteria.status", Configuration.ApiClient.ParameterToString(criteriaStatus)); // query parameter
            if (criteriaTag != null) queryParams.Add("criteria.tag", Configuration.ApiClient.ParameterToString(criteriaTag)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", Configuration.ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", Configuration.ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceDomainQuoteModelQuoteRequestSearchResult)));
            
        }
        
        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns></returns>
        public void QuoteModuleUpdate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             QuoteModuleUpdateWithHttpInfo(quoteRequest);
        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> QuoteModuleUpdateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleUpdate");
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             await QuoteModuleUpdateAsyncWithHttpInfo(quoteRequest);

        }

        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleUpdateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleUpdate");
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCreate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = QuoteModuleCreateWithHttpInfo(quoteRequest);
             return response.Data;
        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleCreateWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCreate");
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
    
        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = await QuoteModuleCreateAsyncWithHttpInfo(quoteRequest);
             return response.Data;

        }

        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCreateAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCreate");
            
    
            var path_ = "/api/quote/requests";
    
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleCreate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
        
        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param> 
        /// <returns></returns>
        public void QuoteModuleDelete (List<string> ids)
        {
             QuoteModuleDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> QuoteModuleDeleteWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleDelete");
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids)
        {
             await QuoteModuleDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> QuoteModuleDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleDelete");
            
    
            var path_ = "/api/quote/requests";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCalculateTotals (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = QuoteModuleCalculateTotalsWithHttpInfo(quoteRequest);
             return response.Data;
        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleCalculateTotalsWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCalculateTotals");
            
    
            var path_ = "/api/quote/requests/recalculate";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleCalculateTotals: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleCalculateTotals: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
    
        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = await QuoteModuleCalculateTotalsAsyncWithHttpInfo(quoteRequest);
             return response.Data;

        }

        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleCalculateTotalsAsyncWithHttpInfo (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCalculateTotals");
            
    
            var path_ = "/api/quote/requests/recalculate";
    
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(quoteRequest); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleCalculateTotals: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleCalculateTotals: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
        
        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleGetById (string id)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = QuoteModuleGetByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns>ApiResponse of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public ApiResponse< VirtoCommerceQuoteModuleWebModelQuoteRequest > QuoteModuleGetByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetById");
            
    
            var path_ = "/api/quote/requests/{id}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
    
        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest> response = await QuoteModuleGetByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceQuoteModuleWebModelQuoteRequest)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>> QuoteModuleGetByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetById");
            
    
            var path_ = "/api/quote/requests/{id}";
    
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceQuoteModuleWebModelQuoteRequest>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceQuoteModuleWebModelQuoteRequest) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest)));
            
        }
        
        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns>List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id)
        {
             ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> response = QuoteModuleGetShipmentMethodsWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public ApiResponse< List<VirtoCommerceQuoteModuleWebModelShipmentMethod> > QuoteModuleGetShipmentMethodsWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetShipmentMethods");
            
    
            var path_ = "/api/quote/requests/{id}/shipmentmethods";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>)));
            
        }
    
        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id)
        {
             ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> response = await QuoteModuleGetShipmentMethodsAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceQuoteModuleWebModelShipmentMethod&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>> QuoteModuleGetShipmentMethodsAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetShipmentMethods");
            
    
            var path_ = "/api/quote/requests/{id}/shipmentmethods";
    
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>)));
            
        }
        
    }
    
}
