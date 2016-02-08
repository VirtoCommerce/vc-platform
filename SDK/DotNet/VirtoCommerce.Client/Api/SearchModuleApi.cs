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
    public interface ISearchModuleApi
    {
        
        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (VirtoCommerceDomainCatalogModelSearchCriteria criteria);
  
        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria);
        
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId);
  
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesWithHttpInfo (string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns></returns>
        void SearchModuleSetFilterProperties (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
  
        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SearchModuleSetFilterPropertiesWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SearchModuleApi : ISearchModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public SearchModuleApi(Configuration configuration)
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
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteria">Search parameters</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = SearchModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteria">Search parameters</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalogSearchResult > SearchModuleSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling SearchModuleApi->SearchModuleSearch");
            
    
            var path_ = "/api/search";
    
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
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
    
        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = await SearchModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling SearchModuleSearch");
            
    
            var path_ = "/api/search";
    
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
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
        
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> response = SearchModuleGetFilterPropertiesWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public ApiResponse< List<VirtoCommerceSearchModuleWebModelFilterProperty> > SearchModuleGetFilterPropertiesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleGetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }
    
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> response = await SearchModuleGetFilterPropertiesAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleGetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }
        
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <param name="filterProperties"></param> 
        /// <returns></returns>
        public void SearchModuleSetFilterProperties (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             SearchModuleSetFilterPropertiesWithHttpInfo(storeId, filterProperties);
        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <param name="filterProperties"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SearchModuleSetFilterPropertiesWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleSetFilterProperties");
            
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null)
                throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleApi->SearchModuleSetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (filterProperties.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(filterProperties); // http body (model) parameter
            }
            else
            {
                postBody = filterProperties; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             await SearchModuleSetFilterPropertiesAsyncWithHttpInfo(storeId, filterProperties);

        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleSetFilterProperties");
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null) throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleSetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(filterProperties); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
    }
    
}
