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
        
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ();
  
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresWithHttpInfo ();

        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ();

        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetStoresAsyncWithHttpInfo ();
        
        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns></returns>
        void StoreModuleUpdate (VirtoCommerceStoreModuleWebModelStore store);
  
        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleUpdateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Update store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleUpdateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        
        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        VirtoCommerceStoreModuleWebModelStore StoreModuleCreate (VirtoCommerceStoreModuleWebModelStore store);
  
        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store);

        /// <summary>
        /// Create store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleCreateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store);
        
        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns></returns>
        void StoreModuleDelete (List<string> ids);
  
        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleDeleteWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids);

        /// <summary>
        /// Delete stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleDeleteAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria"></param>
        /// <returns>VirtoCommerceStoreModuleWebModelSearchResult</returns>
        VirtoCommerceStoreModuleWebModelSearchResult StoreModuleSearchStores (VirtoCommerceDomainStoreModelSearchCriteria criteria);
  
        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria"></param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria);

        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria"></param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresAsync (VirtoCommerceDomainStoreModelSearchCriteria criteria);

        /// <summary>
        /// Search stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteria"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>> StoreModuleSearchStoresAsyncWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria);
        
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        void StoreModuleSendDynamicNotificationAnStoreEmail (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);
  
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task StoreModuleSendDynamicNotificationAnStoreEmailAsync (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request);
        
        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Store id</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        VirtoCommerceStoreModuleWebModelStore StoreModuleGetStoreById (string id);
  
        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Store id</param>
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        ApiResponse<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdWithHttpInfo (string id);

        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Store id</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id);

        /// <summary>
        /// Get store by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Store id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoreByIdAsyncWithHttpInfo (string id);
        
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
        /// Get all stores 
        /// </summary>
        /// <returns>List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ()
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> response = StoreModuleGetStoresWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public ApiResponse< List<VirtoCommerceStoreModuleWebModelStore> > StoreModuleGetStoresWithHttpInfo ()
        {
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStores: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStores: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }
    
        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceStoreModuleWebModelStore&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ()
        {
             ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>> response = await StoreModuleGetStoresAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceStoreModuleWebModelStore&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>> StoreModuleGetStoresAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStores: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStores: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceStoreModuleWebModelStore>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceStoreModuleWebModelStore>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceStoreModuleWebModelStore>)));
            
        }
        
        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns></returns>
        public void StoreModuleUpdate (VirtoCommerceStoreModuleWebModelStore store)
        {
             StoreModuleUpdateWithHttpInfo(store);
        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleUpdateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleUpdate");
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            if (store.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                postBody = store; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
             await StoreModuleUpdateAsyncWithHttpInfo(store);

        }

        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleUpdateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleUpdate");
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public VirtoCommerceStoreModuleWebModelStore StoreModuleCreate (VirtoCommerceStoreModuleWebModelStore store)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> response = StoreModuleCreateWithHttpInfo(store);
             return response.Data;
        }

        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelStore > StoreModuleCreateWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            
            // verify the required parameter 'store' is set
            if (store == null)
                throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleApi->StoreModuleCreate");
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            if (store.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            }
            else
            {
                postBody = store; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }
    
        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> response = await StoreModuleCreateAsyncWithHttpInfo(store);
             return response.Data;

        }

        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleCreateAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleCreate");
            
    
            var path_ = "/api/stores";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(store); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleCreate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }
        
        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param> 
        /// <returns></returns>
        public void StoreModuleDelete (List<string> ids)
        {
             StoreModuleDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleDeleteWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleApi->StoreModuleDelete");
            
    
            var path_ = "/api/stores";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids)
        {
             await StoreModuleDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleDelete");
            
    
            var path_ = "/api/stores";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Search stores 
        /// </summary>
        /// <param name="criteria"></param> 
        /// <returns>VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public VirtoCommerceStoreModuleWebModelSearchResult StoreModuleSearchStores (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> response = StoreModuleSearchStoresWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <param name="criteria"></param> 
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelSearchResult > StoreModuleSearchStoresWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling StoreModuleApi->StoreModuleSearchStores");
            
    
            var path_ = "/api/stores/search";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleSearchStores: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleSearchStores: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelSearchResult)));
            
        }
    
        /// <summary>
        /// Search stores 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelSearchResult> StoreModuleSearchStoresAsync (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult> response = await StoreModuleSearchStoresAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search stores 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>> StoreModuleSearchStoresAsyncWithHttpInfo (VirtoCommerceDomainStoreModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling StoreModuleSearchStores");
            
    
            var path_ = "/api/stores/search";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleSearchStores: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleSearchStores: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelSearchResult)));
            
        }
        
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>
        public void StoreModuleSendDynamicNotificationAnStoreEmail (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
             StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo(request);
        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <param name="request"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> StoreModuleSendDynamicNotificationAnStoreEmailWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null)
                throw new ApiException(400, "Missing required parameter 'request' when calling StoreModuleApi->StoreModuleSendDynamicNotificationAnStoreEmail");
            
    
            var path_ = "/api/stores/send/dynamicnotification";
    
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
            
            
            
            
            if (request.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            }
            else
            {
                postBody = request; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task StoreModuleSendDynamicNotificationAnStoreEmailAsync (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
             await StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo(request);

        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> StoreModuleSendDynamicNotificationAnStoreEmailAsyncWithHttpInfo (VirtoCommerceStoreModuleWebModelSendDynamicNotificationRequest request)
        {
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling StoreModuleSendDynamicNotificationAnStoreEmail");
            
    
            var path_ = "/api/stores/send/dynamicnotification";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(request); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleSendDynamicNotificationAnStoreEmail: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param> 
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public VirtoCommerceStoreModuleWebModelStore StoreModuleGetStoreById (string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> response = StoreModuleGetStoreByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param> 
        /// <returns>ApiResponse of VirtoCommerceStoreModuleWebModelStore</returns>
        public ApiResponse< VirtoCommerceStoreModuleWebModelStore > StoreModuleGetStoreByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleApi->StoreModuleGetStoreById");
            
    
            var path_ = "/api/stores/{id}";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleGetStoreById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStoreById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }
    
        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param>
        /// <returns>Task of VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceStoreModuleWebModelStore> response = await StoreModuleGetStoreByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceStoreModuleWebModelStore)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoreByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleGetStoreById");
            
    
            var path_ = "/api/stores/{id}";
    
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
                throw new ApiException (statusCode, "Error calling StoreModuleGetStoreById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling StoreModuleGetStoreById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceStoreModuleWebModelStore>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceStoreModuleWebModelStore) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceStoreModuleWebModelStore)));
            
        }
        
    }
    
}
