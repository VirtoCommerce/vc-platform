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
    public interface IStoreModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        VirtoCommerceStoreModuleWebModelStore StoreModuleCreate (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns></returns>
        void StoreModuleDelete (List<string> ids);

        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleDeleteWithHttpInfo (List<string> ids);
        /// <summary>
        /// Check if given contact has login on behalf permission
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo StoreModuleGetLoginOnBehalfInfo (string storeId, string id);

        /// <summary>
        /// Check if given contact has login on behalf permission
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo> StoreModuleGetLoginOnBehalfInfoWithHttpInfo (string storeId, string id);
        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        VirtoCommerceStoreModuleWebModelStore StoreModuleGetStoreById (string id);

        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdWithHttpInfo (string id);
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ();

        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresWithHttpInfo ();
        /// <summary>
        /// Returns list of stores which user can sign in
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetUserAllowedStores (string userId);

        /// <summary>
        /// Returns list of stores which user can sign in
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetUserAllowedStoresWithHttpInfo (string userId);
        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>VirtoCommerceStoreModuleWebModelSearchResult</returns>
        VirtoCommerceStoreModuleWebModelSearchResult StoreModuleSearchStores (VirtoCommerceDomainStoreModelSearchCriteria criteria);

        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria);
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns></returns>
        void StoreModuleSendDynamicNotificationAnStoreEmail (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);
        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns></returns>
        void StoreModuleUpdate (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleUpdateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleCreateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids);

        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleDeleteAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Check if given contact has login on behalf permission
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo> StoreModuleGetLoginOnBehalfInfoAsync (string storeId, string id);

        /// <summary>
        /// Check if given contact has login on behalf permission
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo>> StoreModuleGetLoginOnBehalfInfoAsyncWithHttpInfo (string storeId, string id);
        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id);

        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoreByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ();

        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetStoresAsyncWithHttpInfo ();
        /// <summary>
        /// Returns list of stores which user can sign in
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetUserAllowedStoresAsync (string userId);

        /// <summary>
        /// Returns list of stores which user can sign in
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetUserAllowedStoresAsyncWithHttpInfo (string userId);
        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresAsync (VirtoCommerceDomainStoreModelSearchCriteria criteria);

        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>> StoreModuleSearchStoresAsyncWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria);
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleSendDynamicNotificationAnStoreEmailAsync (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);
        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleUpdateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class StoreModuleApi : IStoreModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public StoreModuleApi(Configuration configuration)
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
        /// Create store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public VirtoCommerceStoreModuleWebModelStore StoreModuleCreate (VirtoCommerceStoreModuleWebModelStore store)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> localVarResponse = StoreModuleCreateWithHttpInfo(store);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Create store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelStore > StoreModuleCreateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleCreate");

            var localVarPath = "/api/stores";
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
            if (store.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                localVarPostBody = store; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }

        /// <summary>
        /// Create store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> localVarResponse = await StoreModuleCreateAsyncWithHttpInfo(store);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Create store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleCreateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleCreate");

            var localVarPath = "/api/stores";
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
            if (store.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                localVarPostBody = store; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns></returns>
        public void StoreModuleDelete (List<string> ids)
        {
             StoreModuleDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleDeleteWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleApi->StoreModuleDelete");

            var localVarPath = "/api/stores";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids)
        {
             await StoreModuleDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleApi->StoreModuleDelete");

            var localVarPath = "/api/stores";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Check if given contact has login on behalf permission 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        public VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo StoreModuleGetLoginOnBehalfInfo (string storeId, string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo> localVarResponse = StoreModuleGetLoginOnBehalfInfoWithHttpInfo(storeId, id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Check if given contact has login on behalf permission 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo > StoreModuleGetLoginOnBehalfInfoWithHttpInfo (string storeId, string id)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling StoreModuleApi->StoreModuleGetLoginOnBehalfInfo");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleApi->StoreModuleGetLoginOnBehalfInfo");

            var localVarPath = "/api/stores/{storeId}/accounts/{id}/loginonbehalf";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetLoginOnBehalfInfo: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetLoginOnBehalfInfo: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo)));
            
        }

        /// <summary>
        /// Check if given contact has login on behalf permission 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo> StoreModuleGetLoginOnBehalfInfoAsync (string storeId, string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo> localVarResponse = await StoreModuleGetLoginOnBehalfInfoAsyncWithHttpInfo(storeId, id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Check if given contact has login on behalf permission 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo>> StoreModuleGetLoginOnBehalfInfoAsyncWithHttpInfo (string storeId, string id)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling StoreModuleApi->StoreModuleGetLoginOnBehalfInfo");
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleApi->StoreModuleGetLoginOnBehalfInfo");

            var localVarPath = "/api/stores/{storeId}/accounts/{id}/loginonbehalf";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetLoginOnBehalfInfo: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetLoginOnBehalfInfo: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelLoginOnBehalfInfo)));
            
        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public VirtoCommerceStoreModuleWebModelStore StoreModuleGetStoreById (string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> localVarResponse = StoreModuleGetStoreByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelStore > StoreModuleGetStoreByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleApi->StoreModuleGetStoreById");

            var localVarPath = "/api/stores/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStoreById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStoreById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> localVarResponse = await StoreModuleGetStoreByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Store id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoreByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleApi->StoreModuleGetStoreById");

            var localVarPath = "/api/stores/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStoreById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStoreById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ()
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> localVarResponse = StoreModuleGetStoresWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public ApiResponse< List<VirtoCommerceStoreModuleWebModelStore> > StoreModuleGetStoresWithHttpInfo ()
        {

            var localVarPath = "/api/stores";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ()
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> localVarResponse = await StoreModuleGetStoresAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetStoresAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/stores";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }

        /// <summary>
        /// Returns list of stores which user can sign in 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetUserAllowedStores (string userId)
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> localVarResponse = StoreModuleGetUserAllowedStoresWithHttpInfo(userId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Returns list of stores which user can sign in 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public ApiResponse< List<VirtoCommerceStoreModuleWebModelStore> > StoreModuleGetUserAllowedStoresWithHttpInfo (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling StoreModuleApi->StoreModuleGetUserAllowedStores");

            var localVarPath = "/api/stores/allowed/{userId}";
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
            if (userId != null) localVarPathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetUserAllowedStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetUserAllowedStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }

        /// <summary>
        /// Returns list of stores which user can sign in 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetUserAllowedStoresAsync (string userId)
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> localVarResponse = await StoreModuleGetUserAllowedStoresAsyncWithHttpInfo(userId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Returns list of stores which user can sign in 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="userId"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetUserAllowedStoresAsyncWithHttpInfo (string userId)
        {
            // verify the required parameter 'userId' is set
            if (userId == null)
                throw new ApiException(400, "Missing required parameter 'userId' when calling StoreModuleApi->StoreModuleGetUserAllowedStores");

            var localVarPath = "/api/stores/allowed/{userId}";
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
            if (userId != null) localVarPathParams.Add("userId", Configuration.ApiClient.ParameterToString(userId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetUserAllowedStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleGetUserAllowedStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public VirtoCommerceStoreModuleWebModelSearchResult StoreModuleSearchStores (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> localVarResponse = StoreModuleSearchStoresWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelSearchResult > StoreModuleSearchStoresWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling StoreModuleApi->StoreModuleSearchStores");

            var localVarPath = "/api/stores/search";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSearchStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSearchStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresAsync (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> localVarResponse = await StoreModuleSearchStoresAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>> StoreModuleSearchStoresAsyncWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling StoreModuleApi->StoreModuleSearchStores");

            var localVarPath = "/api/stores/search";
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
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSearchStores: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSearchStores: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceStoreModuleWebModelSearchResult)));
            
        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns></returns>
        public void StoreModuleSendDynamicNotificationAnStoreEmail (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
             StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo(request);
        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling StoreModuleApi->StoreModuleSendDynamicNotificationAnStoreEmail");

            var localVarPath = "/api/stores/send/dynamicnotification";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleSendDynamicNotificationAnStoreEmailAsync (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
             await StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo(request);

        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="request"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling StoreModuleApi->StoreModuleSendDynamicNotificationAnStoreEmail");

            var localVarPath = "/api/stores/send/dynamicnotification";
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
            if (request.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                localVarPostBody = request; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns></returns>
        public void StoreModuleUpdate (VirtoCommerceStoreModuleWebModelStore store)
        {
             StoreModuleUpdateWithHttpInfo(store);
        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleUpdateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleUpdate");

            var localVarPath = "/api/stores";
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
            if (store.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                localVarPostBody = store; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
             await StoreModuleUpdateAsyncWithHttpInfo(store);

        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleUpdateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleUpdate");

            var localVarPath = "/api/stores";
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
            if (store.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                localVarPostBody = store; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling StoreModuleUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

    }
}
