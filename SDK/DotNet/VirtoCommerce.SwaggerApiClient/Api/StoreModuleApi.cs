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
    public interface IStoreModuleApi
    {
        
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ();
  
        /// <summary>
        /// Get all stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ();
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store);
        
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
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids);
        
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
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class StoreModuleApi : IStoreModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public StoreModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public StoreModuleApi(String basePath)
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
        /// Get all stores 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStores ()
        {
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStores: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStores: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceStoreModuleWebModelStore>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceStoreModuleWebModelStore>), response.Headers);
        }
    
        /// <summary>
        /// Get all stores 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceStoreModuleWebModelStore>> StoreModuleGetStoresAsync ()
        {
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStores: " + response.Content, response.Content);

            return (List<VirtoCommerceStoreModuleWebModelStore>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceStoreModuleWebModelStore>), response.Headers);
        }
        
        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns></returns>            
        public void StoreModuleUpdate (VirtoCommerceStoreModuleWebModelStore store)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleUpdate");
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(store); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task StoreModuleUpdateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleUpdate");
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(store); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param> 
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>            
        public VirtoCommerceStoreModuleWebModelStore StoreModuleCreate (VirtoCommerceStoreModuleWebModelStore store)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleCreate");
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(store); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceStoreModuleWebModelStore) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceStoreModuleWebModelStore), response.Headers);
        }
    
        /// <summary>
        /// Create store 
        /// </summary>
        /// <param name="store">Store</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleCreateAsync (VirtoCommerceStoreModuleWebModelStore store)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling StoreModuleCreate");
            
    
            var path = "/api/stores";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(store); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleCreate: " + response.Content, response.Content);

            return (VirtoCommerceStoreModuleWebModelStore) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceStoreModuleWebModelStore), response.Headers);
        }
        
        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param> 
        /// <returns></returns>            
        public void StoreModuleDelete (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleDelete");
            
    
            var path = "/api/stores";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete stores 
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task StoreModuleDeleteAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling StoreModuleDelete");
            
    
            var path = "/api/stores";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param> 
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>            
        public VirtoCommerceStoreModuleWebModelStore StoreModuleGetStoreById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleGetStoreById");
            
    
            var path = "/api/stores/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStoreById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStoreById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceStoreModuleWebModelStore) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceStoreModuleWebModelStore), response.Headers);
        }
    
        /// <summary>
        /// Get store by id 
        /// </summary>
        /// <param name="id">Store id</param>
        /// <returns>VirtoCommerceStoreModuleWebModelStore</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceStoreModuleWebModelStore> StoreModuleGetStoreByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling StoreModuleGetStoreById");
            
    
            var path = "/api/stores/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling StoreModuleGetStoreById: " + response.Content, response.Content);

            return (VirtoCommerceStoreModuleWebModelStore) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceStoreModuleWebModelStore), response.Headers);
        }
        
    }
    
}
