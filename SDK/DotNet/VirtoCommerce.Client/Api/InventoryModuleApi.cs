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
    public interface IInventoryModuleApi
    {
        
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids);
  
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesWithHttpInfo (List<string> ids);

        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids);

        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductsInventoriesAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId);
  
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesWithHttpInfo (string productId);

        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId);

        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductInventoriesAsyncWithHttpInfo (string productId);
        
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
        /// <returns>ApiResponse of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);

        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        System.Threading.Tasks.Task<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryAsync (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);

        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceInventoryModuleWebModelInventoryInfo)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleUpsertProductInventoryAsyncWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class InventoryModuleApi : IInventoryModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public InventoryModuleApi(Configuration configuration)
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
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param> 
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> response = InventoryModuleGetProductsInventoriesWithHttpInfo(ids);
             return response.Data;
        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceInventoryModuleWebModelInventoryInfo> > InventoryModuleGetProductsInventoriesWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleApi->InventoryModuleGetProductsInventories");
            
    
            var path_ = "/api/inventory/products";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductsInventories: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductsInventories: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }
    
        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> response = await InventoryModuleGetProductsInventoriesAsyncWithHttpInfo(ids);
             return response.Data;

        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductsInventoriesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleGetProductsInventories");
            
    
            var path_ = "/api/inventory/products";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductsInventories: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductsInventories: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }
        
        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> response = InventoryModuleGetProductInventoriesWithHttpInfo(productId);
             return response.Data;
        }

        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceInventoryModuleWebModelInventoryInfo> > InventoryModuleGetProductInventoriesWithHttpInfo (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleApi->InventoryModuleGetProductInventories");
            
    
            var path_ = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductInventories: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductInventories: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }
    
        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> response = await InventoryModuleGetProductInventoriesAsyncWithHttpInfo(productId);
             return response.Data;

        }

        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductInventoriesAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleGetProductInventories");
            
    
            var path_ = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductInventories: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleGetProductInventories: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }
        
        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param> 
        /// <param name="productId"></param> 
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public VirtoCommerceInventoryModuleWebModelInventoryInfo InventoryModuleUpsertProductInventory (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
             ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> response = InventoryModuleUpsertProductInventoryWithHttpInfo(inventory, productId);
             return response.Data;
        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param> 
        /// <param name="productId"></param> 
        /// <returns>ApiResponse of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public ApiResponse< VirtoCommerceInventoryModuleWebModelInventoryInfo > InventoryModuleUpsertProductInventoryWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
            
            // verify the required parameter 'inventory' is set
            if (inventory == null)
                throw new ApiException(400, "Missing required parameter 'inventory' when calling InventoryModuleApi->InventoryModuleUpsertProductInventory");
            
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleApi->InventoryModuleUpsertProductInventory");
            
    
            var path_ = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            if (inventory.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(inventory); // http body (model) parameter
            }
            else
            {
                postBody = inventory; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceInventoryModuleWebModelInventoryInfo) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo)));
            
        }
    
        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryAsync (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
             ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> response = await InventoryModuleUpsertProductInventoryAsyncWithHttpInfo(inventory, productId);
             return response.Data;

        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceInventoryModuleWebModelInventoryInfo)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleUpsertProductInventoryAsyncWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
            // verify the required parameter 'inventory' is set
            if (inventory == null) throw new ApiException(400, "Missing required parameter 'inventory' when calling InventoryModuleUpsertProductInventory");
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleUpsertProductInventory");
            
    
            var path_ = "/api/inventory/products/{productId}";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(inventory); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling InventoryModuleUpsertProductInventory: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceInventoryModuleWebModelInventoryInfo) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo)));
            
        }
        
    }
    
}
