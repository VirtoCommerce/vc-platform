using System;
using System.IO;
using System.Collections.Generic;
using RestSharp;
using VirtoCommerce.Client.Client;
using VirtoCommerce.Client.Model;


namespace VirtoCommerce.Client.Api
{
    
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMerchandisingModuleApi
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        void MerchandisingNotificationSendNotification (VirtoCommerceMerchandisingModuleWebModelNotificaitonsSendDynamicMerchandisingNotificationRequest request);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task MerchandisingNotificationSendNotificationAsync (VirtoCommerceMerchandisingModuleWebModelNotificaitonsSendDynamicMerchandisingNotificationRequest request);
        
        /// <summary>
        /// Get store products collection by their ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="ids">Product ids</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param>
        /// <returns></returns>
        List<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductsByIds (string store, List<string> ids, string responseGroup = null);
  
        /// <summary>
        /// Get store products collection by their ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="ids">Product ids</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>> MerchandisingModuleProductGetProductsByIdsAsync (string store, List<string> ids, string responseGroup = null);
        
        /// <summary>
        /// Search for store products
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestStore">Store ID</param>
        /// <param name="requestPricelists">Array of pricelist IDs</param>
        /// <param name="requestResponseGroup">Response detalization scale (default value is ItemMedium)</param>
        /// <param name="requestOutline">Product category outline</param>
        /// <param name="requestLanguage">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <param name="requestCurrency">Currency (default value is \&quot;USD\&quot;)</param>
        /// <param name="requestSearchPhrase">Gets or sets the search phrase</param>
        /// <param name="requestSort">Gets or sets the sort</param>
        /// <param name="requestSortOrder">Gets or sets the sort order ascending or descending</param>
        /// <param name="requestStartDateFrom">Gets or sets the start date</param>
        /// <param name="requestSkip">Gets or sets the number of items to skip</param>
        /// <param name="requestTake">Gets or sets the number of items to return</param>
        /// <param name="requestTerms">Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</param>
        /// <param name="requestFacets">Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelProductSearchResult</returns>
        VirtoCommerceMerchandisingModuleWebModelProductSearchResult MerchandisingModuleProductSearch (string requestStore = null, List<string> requestPricelists = null, string requestResponseGroup = null, string requestOutline = null, string requestLanguage = null, string requestCurrency = null, string requestSearchPhrase = null, string requestSort = null, string requestSortOrder = null, DateTime? requestStartDateFrom = null, int? requestSkip = null, int? requestTake = null, List<string> requestTerms = null, List<string> requestFacets = null);
  
        /// <summary>
        /// Search for store products
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="requestStore">Store ID</param>
        /// <param name="requestPricelists">Array of pricelist IDs</param>
        /// <param name="requestResponseGroup">Response detalization scale (default value is ItemMedium)</param>
        /// <param name="requestOutline">Product category outline</param>
        /// <param name="requestLanguage">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <param name="requestCurrency">Currency (default value is \&quot;USD\&quot;)</param>
        /// <param name="requestSearchPhrase">Gets or sets the search phrase</param>
        /// <param name="requestSort">Gets or sets the sort</param>
        /// <param name="requestSortOrder">Gets or sets the sort order ascending or descending</param>
        /// <param name="requestStartDateFrom">Gets or sets the start date</param>
        /// <param name="requestSkip">Gets or sets the number of items to skip</param>
        /// <param name="requestTake">Gets or sets the number of items to return</param>
        /// <param name="requestTerms">Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</param>
        /// <param name="requestFacets">Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelProductSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelProductSearchResult> MerchandisingModuleProductSearchAsync (string requestStore = null, List<string> requestPricelists = null, string requestResponseGroup = null, string requestOutline = null, string requestLanguage = null, string requestCurrency = null, string requestSearchPhrase = null, string requestSort = null, string requestSortOrder = null, DateTime? requestStartDateFrom = null, int? requestSkip = null, int? requestTake = null, List<string> requestTerms = null, List<string> requestFacets = null);
        
        /// <summary>
        /// Get store product by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="product">Product id</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelCatalogItem</returns>
        VirtoCommerceMerchandisingModuleWebModelCatalogItem MerchandisingModuleProductGetProduct (string store, string product, string responseGroup = null, string language = null);
  
        /// <summary>
        /// Get store product by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="product">Product id</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelCatalogItem</returns>
        System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductAsync (string store, string product, string responseGroup = null, string language = null);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MerchandisingModuleApi : IMerchandisingModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MerchandisingModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient</param>
        /// <returns></returns>
        public MerchandisingModuleApi(ApiClient apiClient)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
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
        ///  
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>            
        public void MerchandisingNotificationSendNotification (VirtoCommerceMerchandisingModuleWebModelNotificaitonsSendDynamicMerchandisingNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling MerchandisingNotificationSendNotification");
            
    
            var path_ = "/api/mp/notification";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingNotificationSendNotification: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingNotificationSendNotification: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MerchandisingNotificationSendNotificationAsync (VirtoCommerceMerchandisingModuleWebModelNotificaitonsSendDynamicMerchandisingNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling MerchandisingNotificationSendNotification");
            
    
            var path_ = "/api/mp/notification";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(request); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingNotificationSendNotification: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get store products collection by their ids 
        /// </summary>
        /// <param name="store">Store id</param> 
        /// <param name="ids">Product ids</param> 
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param> 
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductsByIds (string store, List<string> ids, string responseGroup = null)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProductsByIds");
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MerchandisingModuleProductGetProductsByIds");
            
    
            var path_ = "/api/mp/products";
    
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
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            if (responseGroup != null) queryParams.Add("responseGroup", ApiClient.ParameterToString(responseGroup)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>));
        }
    
        /// <summary>
        /// Get store products collection by their ids 
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="ids">Product ids</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>> MerchandisingModuleProductGetProductsByIdsAsync (string store, List<string> ids, string responseGroup = null)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProductsByIds");
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MerchandisingModuleProductGetProductsByIds");
            
    
            var path_ = "/api/mp/products";
    
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
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            if (responseGroup != null) queryParams.Add("responseGroup", ApiClient.ParameterToString(responseGroup)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>));
        }
        
        /// <summary>
        /// Search for store products 
        /// </summary>
        /// <param name="requestStore">Store ID</param> 
        /// <param name="requestPricelists">Array of pricelist IDs</param> 
        /// <param name="requestResponseGroup">Response detalization scale (default value is ItemMedium)</param> 
        /// <param name="requestOutline">Product category outline</param> 
        /// <param name="requestLanguage">Culture name (default value is \&quot;en-us\&quot;)</param> 
        /// <param name="requestCurrency">Currency (default value is \&quot;USD\&quot;)</param> 
        /// <param name="requestSearchPhrase">Gets or sets the search phrase</param> 
        /// <param name="requestSort">Gets or sets the sort</param> 
        /// <param name="requestSortOrder">Gets or sets the sort order ascending or descending</param> 
        /// <param name="requestStartDateFrom">Gets or sets the start date</param> 
        /// <param name="requestSkip">Gets or sets the number of items to skip</param> 
        /// <param name="requestTake">Gets or sets the number of items to return</param> 
        /// <param name="requestTerms">Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</param> 
        /// <param name="requestFacets">Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</param> 
        /// <returns>VirtoCommerceMerchandisingModuleWebModelProductSearchResult</returns>            
        public VirtoCommerceMerchandisingModuleWebModelProductSearchResult MerchandisingModuleProductSearch (string requestStore = null, List<string> requestPricelists = null, string requestResponseGroup = null, string requestOutline = null, string requestLanguage = null, string requestCurrency = null, string requestSearchPhrase = null, string requestSort = null, string requestSortOrder = null, DateTime? requestStartDateFrom = null, int? requestSkip = null, int? requestTake = null, List<string> requestTerms = null, List<string> requestFacets = null)
        {
            
    
            var path_ = "/api/mp/products/search";
    
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
            
            if (requestStore != null) queryParams.Add("request.store", ApiClient.ParameterToString(requestStore)); // query parameter
            if (requestPricelists != null) queryParams.Add("request.pricelists", ApiClient.ParameterToString(requestPricelists)); // query parameter
            if (requestResponseGroup != null) queryParams.Add("request.responseGroup", ApiClient.ParameterToString(requestResponseGroup)); // query parameter
            if (requestOutline != null) queryParams.Add("request.outline", ApiClient.ParameterToString(requestOutline)); // query parameter
            if (requestLanguage != null) queryParams.Add("request.language", ApiClient.ParameterToString(requestLanguage)); // query parameter
            if (requestCurrency != null) queryParams.Add("request.currency", ApiClient.ParameterToString(requestCurrency)); // query parameter
            if (requestSearchPhrase != null) queryParams.Add("request.searchPhrase", ApiClient.ParameterToString(requestSearchPhrase)); // query parameter
            if (requestSort != null) queryParams.Add("request.sort", ApiClient.ParameterToString(requestSort)); // query parameter
            if (requestSortOrder != null) queryParams.Add("request.sortOrder", ApiClient.ParameterToString(requestSortOrder)); // query parameter
            if (requestStartDateFrom != null) queryParams.Add("request.startDateFrom", ApiClient.ParameterToString(requestStartDateFrom)); // query parameter
            if (requestSkip != null) queryParams.Add("request.skip", ApiClient.ParameterToString(requestSkip)); // query parameter
            if (requestTake != null) queryParams.Add("request.take", ApiClient.ParameterToString(requestTake)); // query parameter
            if (requestTerms != null) queryParams.Add("request.terms", ApiClient.ParameterToString(requestTerms)); // query parameter
            if (requestFacets != null) queryParams.Add("request.facets", ApiClient.ParameterToString(requestFacets)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceMerchandisingModuleWebModelProductSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommerceMerchandisingModuleWebModelProductSearchResult));
        }
    
        /// <summary>
        /// Search for store products 
        /// </summary>
        /// <param name="requestStore">Store ID</param>
        /// <param name="requestPricelists">Array of pricelist IDs</param>
        /// <param name="requestResponseGroup">Response detalization scale (default value is ItemMedium)</param>
        /// <param name="requestOutline">Product category outline</param>
        /// <param name="requestLanguage">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <param name="requestCurrency">Currency (default value is \&quot;USD\&quot;)</param>
        /// <param name="requestSearchPhrase">Gets or sets the search phrase</param>
        /// <param name="requestSort">Gets or sets the sort</param>
        /// <param name="requestSortOrder">Gets or sets the sort order ascending or descending</param>
        /// <param name="requestStartDateFrom">Gets or sets the start date</param>
        /// <param name="requestSkip">Gets or sets the number of items to skip</param>
        /// <param name="requestTake">Gets or sets the number of items to return</param>
        /// <param name="requestTerms">Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</param>
        /// <param name="requestFacets">Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelProductSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelProductSearchResult> MerchandisingModuleProductSearchAsync (string requestStore = null, List<string> requestPricelists = null, string requestResponseGroup = null, string requestOutline = null, string requestLanguage = null, string requestCurrency = null, string requestSearchPhrase = null, string requestSort = null, string requestSortOrder = null, DateTime? requestStartDateFrom = null, int? requestSkip = null, int? requestTake = null, List<string> requestTerms = null, List<string> requestFacets = null)
        {
            
    
            var path_ = "/api/mp/products/search";
    
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
            
            if (requestStore != null) queryParams.Add("request.store", ApiClient.ParameterToString(requestStore)); // query parameter
            if (requestPricelists != null) queryParams.Add("request.pricelists", ApiClient.ParameterToString(requestPricelists)); // query parameter
            if (requestResponseGroup != null) queryParams.Add("request.responseGroup", ApiClient.ParameterToString(requestResponseGroup)); // query parameter
            if (requestOutline != null) queryParams.Add("request.outline", ApiClient.ParameterToString(requestOutline)); // query parameter
            if (requestLanguage != null) queryParams.Add("request.language", ApiClient.ParameterToString(requestLanguage)); // query parameter
            if (requestCurrency != null) queryParams.Add("request.currency", ApiClient.ParameterToString(requestCurrency)); // query parameter
            if (requestSearchPhrase != null) queryParams.Add("request.searchPhrase", ApiClient.ParameterToString(requestSearchPhrase)); // query parameter
            if (requestSort != null) queryParams.Add("request.sort", ApiClient.ParameterToString(requestSort)); // query parameter
            if (requestSortOrder != null) queryParams.Add("request.sortOrder", ApiClient.ParameterToString(requestSortOrder)); // query parameter
            if (requestStartDateFrom != null) queryParams.Add("request.startDateFrom", ApiClient.ParameterToString(requestStartDateFrom)); // query parameter
            if (requestSkip != null) queryParams.Add("request.skip", ApiClient.ParameterToString(requestSkip)); // query parameter
            if (requestTake != null) queryParams.Add("request.take", ApiClient.ParameterToString(requestTake)); // query parameter
            if (requestTerms != null) queryParams.Add("request.terms", ApiClient.ParameterToString(requestTerms)); // query parameter
            if (requestFacets != null) queryParams.Add("request.facets", ApiClient.ParameterToString(requestFacets)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.Content, response.Content);

            return (VirtoCommerceMerchandisingModuleWebModelProductSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommerceMerchandisingModuleWebModelProductSearchResult));
        }
        
        /// <summary>
        /// Get store product by id 
        /// </summary>
        /// <param name="store">Store id</param> 
        /// <param name="product">Product id</param> 
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param> 
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param> 
        /// <returns>VirtoCommerceMerchandisingModuleWebModelCatalogItem</returns>            
        public VirtoCommerceMerchandisingModuleWebModelCatalogItem MerchandisingModuleProductGetProduct (string store, string product, string responseGroup = null, string language = null)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProduct");
            
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling MerchandisingModuleProductGetProduct");
            
    
            var path_ = "/api/mp/products/{product}";
    
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
            if (product != null) pathParams.Add("product", ApiClient.ParameterToString(product)); // path parameter
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (responseGroup != null) queryParams.Add("responseGroup", ApiClient.ParameterToString(responseGroup)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceMerchandisingModuleWebModelCatalogItem) ApiClient.Deserialize(response, typeof(VirtoCommerceMerchandisingModuleWebModelCatalogItem));
        }
    
        /// <summary>
        /// Get store product by id 
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="product">Product id</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelCatalogItem</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductAsync (string store, string product, string responseGroup = null, string language = null)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProduct");
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling MerchandisingModuleProductGetProduct");
            
    
            var path_ = "/api/mp/products/{product}";
    
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
            if (product != null) pathParams.Add("product", ApiClient.ParameterToString(product)); // path parameter
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (responseGroup != null) queryParams.Add("responseGroup", ApiClient.ParameterToString(responseGroup)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.Content, response.Content);

            return (VirtoCommerceMerchandisingModuleWebModelCatalogItem) ApiClient.Deserialize(response, typeof(VirtoCommerceMerchandisingModuleWebModelCatalogItem));
        }
        
    }
    
}
