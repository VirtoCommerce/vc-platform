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
    public interface IInventoryModuleApi
    {
        
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns></returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids);
  
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids);
        
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId);
  
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId);
        
        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        VirtoCommerceInventoryModuleWebModelInventoryInfo InventoryModuleUpsertProductInventory (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);
  
        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        System.Threading.Tasks.Task<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryAsync (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class InventoryModuleApi : IInventoryModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public InventoryModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public InventoryModuleApi(String basePath)
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
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param> 
        /// <returns></returns>            
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleGetProductsInventories");
            
    
            var path = "/api/inventory/products";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductsInventories: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductsInventories: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>), response.Headers);
        }
    
        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleGetProductsInventories");
            
    
            var path = "/api/inventory/products";
    
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
            
            if (ids != null) queryParams.Add("ids", ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductsInventories: " + response.Content, response.Content);

            return (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>), response.Headers);
        }
        
        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleGetProductInventories");
            
    
            var path = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductInventories: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductInventories: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>), response.Headers);
        }
    
        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleGetProductInventories");
            
    
            var path = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleGetProductInventories: " + response.Content, response.Content);

            return (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>), response.Headers);
        }
        
        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param> 
        /// <param name="productId"></param> 
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>            
        public VirtoCommerceInventoryModuleWebModelInventoryInfo InventoryModuleUpsertProductInventory (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
            
            // verify the required parameter 'inventory' is set
            if (inventory == null) throw new ApiException(400, "Missing required parameter 'inventory' when calling InventoryModuleUpsertProductInventory");
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleUpsertProductInventory");
            
    
            var path = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(inventory); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceInventoryModuleWebModelInventoryInfo) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo), response.Headers);
        }
    
        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryAsync (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
            // verify the required parameter 'inventory' is set
            if (inventory == null) throw new ApiException(400, "Missing required parameter 'inventory' when calling InventoryModuleUpsertProductInventory");
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleUpsertProductInventory");
            
    
            var path = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(inventory); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.Content, response.Content);

            return (VirtoCommerceInventoryModuleWebModelInventoryInfo) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo), response.Headers);
        }
        
    }
    
}
