using System;
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
    public interface ISearchModuleApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties(string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesWithHttpInfo(string storeId);
        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch(VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchWithHttpInfo(VirtoCommerceDomainCatalogModelSearchCriteria criteria);
        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns></returns>
        void SearchModuleSetFilterProperties(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SearchModuleSetFilterPropertiesWithHttpInfo(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync(string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo(string storeId);
        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync(VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo(VirtoCommerceDomainCatalogModelSearchCriteria criteria);
        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
        #endregion Asynchronous Operations
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
        /// <param name="apiClient">An instance of ApiClient.</param>
        /// <returns></returns>
        public SearchModuleApi(ApiClient apiClient)
        {
            ApiClient = apiClient;
            Configuration = apiClient.Configuration;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the API client object
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient { get; set; }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties(string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> localVarResponse = SearchModuleGetFilterPropertiesWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesWithHttpInfo(string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleGetFilterProperties");

            var localVarPath = "/api/search/storefilterproperties/{storeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleGetFilterProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleGetFilterProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>)ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync(string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> localVarResponse = await SearchModuleGetFilterPropertiesAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo(string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleGetFilterProperties");

            var localVarPath = "/api/search/storefilterproperties/{storeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleGetFilterProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleGetFilterProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>)ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }
        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch(VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> localVarResponse = SearchModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchWithHttpInfo(VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling SearchModuleApi->SearchModuleSearch");

            var localVarPath = "/api/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }

        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync(VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> localVarResponse = await SearchModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">Search parameters</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo(VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling SearchModuleApi->SearchModuleSearch");

            var localVarPath = "/api/search";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml"
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns></returns>
        public void SearchModuleSetFilterProperties(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             SearchModuleSetFilterPropertiesWithHttpInfo(storeId, filterProperties);
        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> SearchModuleSetFilterPropertiesWithHttpInfo(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleSetFilterProperties");
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null)
                throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleApi->SearchModuleSetFilterProperties");

            var localVarPath = "/api/search/storefilterproperties/{storeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (filterProperties.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(filterProperties); // http body (model) parameter
            }
            else
            {
                localVarPostBody = filterProperties; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSetFilterProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSetFilterProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             await SearchModuleSetFilterPropertiesAsyncWithHttpInfo(storeId, filterProperties);

        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo(string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleApi->SearchModuleSetFilterProperties");
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null)
                throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleApi->SearchModuleSetFilterProperties");

            var localVarPath = "/api/search/storefilterproperties/{storeId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new Dictionary<string, string>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/xml", 
                "text/xml", 
                "application/x-www-form-urlencoded"
            };
            string localVarHttpContentType = ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (filterProperties.GetType() != typeof(byte[]))
            {
                localVarPostBody = ApiClient.Serialize(filterProperties); // http body (model) parameter
            }
            else
            {
                localVarPostBody = filterProperties; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse)await ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int)localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSetFilterProperties: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException(localVarStatusCode, "Error calling SearchModuleSetFilterProperties: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    }
}
