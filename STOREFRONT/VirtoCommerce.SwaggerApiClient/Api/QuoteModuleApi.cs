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
        VirtoCommerceDomainQuoteModelQuoteRequestSearchResult QuoteModuleSearch (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, DateTime? criteriaStartDate, DateTime? criteriaEndDate, string criteriaStatus, string criteriaTag, int? criteriaStart, int? criteriaCount);
  
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
        System.Threading.Tasks.Task<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> QuoteModuleSearchAsync (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, DateTime? criteriaStartDate, DateTime? criteriaEndDate, string criteriaStatus, string criteriaTag, int? criteriaStart, int? criteriaCount);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
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
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids);
        
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
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest);
        
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
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id);
        
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns></returns>
        List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id);
  
        /// <summary>
        /// Get available shipping methods with prices for quote requests
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">RFQ id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class QuoteModuleApi : IQuoteModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public QuoteModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public QuoteModuleApi(String basePath)
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
        public VirtoCommerceDomainQuoteModelQuoteRequestSearchResult QuoteModuleSearch (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, DateTime? criteriaStartDate, DateTime? criteriaEndDate, string criteriaStatus, string criteriaTag, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/quote/requests";
    
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
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStatus != null) queryParams.Add("criteria.status", ApiClient.ParameterToString(criteriaStatus)); // query parameter
            if (criteriaTag != null) queryParams.Add("criteria.tag", ApiClient.ParameterToString(criteriaTag)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceDomainQuoteModelQuoteRequestSearchResult), response.Headers);
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
        public async System.Threading.Tasks.Task<VirtoCommerceDomainQuoteModelQuoteRequestSearchResult> QuoteModuleSearchAsync (string criteriaKeyword, string criteriaCustomerId, string criteriaStoreId, DateTime? criteriaStartDate, DateTime? criteriaEndDate, string criteriaStatus, string criteriaTag, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/quote/requests";
    
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
            if (criteriaStartDate != null) queryParams.Add("criteria.startDate", ApiClient.ParameterToString(criteriaStartDate)); // query parameter
            if (criteriaEndDate != null) queryParams.Add("criteria.endDate", ApiClient.ParameterToString(criteriaEndDate)); // query parameter
            if (criteriaStatus != null) queryParams.Add("criteria.status", ApiClient.ParameterToString(criteriaStatus)); // query parameter
            if (criteriaTag != null) queryParams.Add("criteria.tag", ApiClient.ParameterToString(criteriaTag)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleSearch: " + response.Content, response.Content);

            return (VirtoCommerceDomainQuoteModelQuoteRequestSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceDomainQuoteModelQuoteRequestSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns></returns>            
        public void QuoteModuleUpdate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleUpdate");
            
    
            var path = "/api/quote/requests";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update a existing RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task QuoteModuleUpdateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleUpdate");
            
    
            var path = "/api/quote/requests";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>            
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCreate (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCreate");
            
    
            var path = "/api/quote/requests";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
    
        /// <summary>
        /// Create new RFQ 
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCreateAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCreate");
            
    
            var path = "/api/quote/requests";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCreate: " + response.Content, response.Content);

            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
        
        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param> 
        /// <returns></returns>            
        public void QuoteModuleDelete (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleDelete");
            
    
            var path = "/api/quote/requests";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes the specified quotes by id. 
        /// </summary>
        /// <param name="ids">The quotes ids.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task QuoteModuleDeleteAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling QuoteModuleDelete");
            
    
            var path = "/api/quote/requests";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>            
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleCalculateTotals (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCalculateTotals");
            
    
            var path = "/api/quote/requests/recalculate";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCalculateTotals: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCalculateTotals: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
    
        /// <summary>
        /// Calculate totals Return totals for selected tier prices
        /// </summary>
        /// <param name="quoteRequest">RFQ</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleCalculateTotalsAsync (VirtoCommerceQuoteModuleWebModelQuoteRequest quoteRequest)
        {
            // verify the required parameter 'quoteRequest' is set
            if (quoteRequest == null) throw new ApiException(400, "Missing required parameter 'quoteRequest' when calling QuoteModuleCalculateTotals");
            
    
            var path = "/api/quote/requests/recalculate";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(quoteRequest); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleCalculateTotals: " + response.Content, response.Content);

            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
        
        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>            
        public VirtoCommerceQuoteModuleWebModelQuoteRequest QuoteModuleGetById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetById");
            
    
            var path = "/api/quote/requests/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
    
        /// <summary>
        /// Get RFQ by id Return a single RFQ
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns>VirtoCommerceQuoteModuleWebModelQuoteRequest</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceQuoteModuleWebModelQuoteRequest> QuoteModuleGetByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetById");
            
    
            var path = "/api/quote/requests/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetById: " + response.Content, response.Content);

            return (VirtoCommerceQuoteModuleWebModelQuoteRequest) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceQuoteModuleWebModelQuoteRequest), response.Headers);
        }
        
        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceQuoteModuleWebModelShipmentMethod> QuoteModuleGetShipmentMethods (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetShipmentMethods");
            
    
            var path = "/api/quote/requests/{id}/shipmentmethods";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>), response.Headers);
        }
    
        /// <summary>
        /// Get available shipping methods with prices for quote requests 
        /// </summary>
        /// <param name="id">RFQ id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceQuoteModuleWebModelShipmentMethod>> QuoteModuleGetShipmentMethodsAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling QuoteModuleGetShipmentMethods");
            
    
            var path = "/api/quote/requests/{id}/shipmentmethods";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling QuoteModuleGetShipmentMethods: " + response.Content, response.Content);

            return (List<VirtoCommerceQuoteModuleWebModelShipmentMethod>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceQuoteModuleWebModelShipmentMethod>), response.Headers);
        }
        
    }
    
}
