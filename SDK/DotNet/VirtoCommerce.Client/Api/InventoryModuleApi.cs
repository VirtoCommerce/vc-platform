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
        #region Synchronous Operations
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId);

        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesWithHttpInfo (string productId);
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids);

        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesWithHttpInfo (List<string> ids);
        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>ApiResponse of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId);

        /// <summary>
        /// Get inventories of product
        /// </summary>
        /// <remarks>
        /// Get inventories of product for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductInventoriesAsyncWithHttpInfo (string productId);
        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids);

        /// <summary>
        /// Get inventories of products
        /// </summary>
        /// <remarks>
        /// Get inventory of products for each fulfillment center.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductsInventoriesAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Upsert inventory
        /// </summary>
        /// <remarks>
        /// Upsert (add or update) given inventory of product.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceInventoryModuleWebModelInventoryInfo)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleUpsertProductInventoryAsyncWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId);
        #endregion Asynchronous Operations
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
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductInventories (string productId)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> localVarResponse = InventoryModuleGetProductInventoriesWithHttpInfo(productId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceInventoryModuleWebModelInventoryInfo> > InventoryModuleGetProductInventoriesWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleApi->InventoryModuleGetProductInventories");

            var localVarPath = "/api/inventory/products/{productId}";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductInventories: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductInventories: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }

        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductInventoriesAsync (string productId)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> localVarResponse = await InventoryModuleGetProductInventoriesAsyncWithHttpInfo(productId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get inventories of product Get inventories of product for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">Product id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductInventoriesAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleApi->InventoryModuleGetProductInventories");

            var localVarPath = "/api/inventory/products/{productId}";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductInventories: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductInventories: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public List<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleGetProductsInventories (List<string> ids)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> localVarResponse = InventoryModuleGetProductsInventoriesWithHttpInfo(ids);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public ApiResponse< List<VirtoCommerceInventoryModuleWebModelInventoryInfo> > InventoryModuleGetProductsInventoriesWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleApi->InventoryModuleGetProductsInventories");

            var localVarPath = "/api/inventory/products";
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
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductsInventories: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductsInventories: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleGetProductsInventoriesAsync (List<string> ids)
        {
             ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>> localVarResponse = await InventoryModuleGetProductsInventoriesAsyncWithHttpInfo(ids);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get inventories of products Get inventory of products for each fulfillment center.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Products ids</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceInventoryModuleWebModelInventoryInfo&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>> InventoryModuleGetProductsInventoriesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling InventoryModuleApi->InventoryModuleGetProductsInventories");

            var localVarPath = "/api/inventory/products";
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
            if (ids != null) localVarQueryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductsInventories: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleGetProductsInventories: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceInventoryModuleWebModelInventoryInfo>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceInventoryModuleWebModelInventoryInfo>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceInventoryModuleWebModelInventoryInfo>)));
            
        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public VirtoCommerceInventoryModuleWebModelInventoryInfo InventoryModuleUpsertProductInventory (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
             ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> localVarResponse = InventoryModuleUpsertProductInventoryWithHttpInfo(inventory, productId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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

            var localVarPath = "/api/inventory/products/{productId}";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            if (inventory.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(inventory); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventory; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleUpsertProductInventory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleUpsertProductInventory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceInventoryModuleWebModelInventoryInfo) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo)));
            
        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of VirtoCommerceInventoryModuleWebModelInventoryInfo</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceInventoryModuleWebModelInventoryInfo> InventoryModuleUpsertProductInventoryAsync (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
             ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo> localVarResponse = await InventoryModuleUpsertProductInventoryAsyncWithHttpInfo(inventory, productId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upsert inventory Upsert (add or update) given inventory of product.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="inventory">Inventory to upsert</param>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceInventoryModuleWebModelInventoryInfo)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>> InventoryModuleUpsertProductInventoryAsyncWithHttpInfo (VirtoCommerceInventoryModuleWebModelInventoryInfo inventory, string productId)
        {
            // verify the required parameter 'inventory' is set
            if (inventory == null)
                throw new ApiException(400, "Missing required parameter 'inventory' when calling InventoryModuleApi->InventoryModuleUpsertProductInventory");
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling InventoryModuleApi->InventoryModuleUpsertProductInventory");

            var localVarPath = "/api/inventory/products/{productId}";
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
            if (productId != null) localVarPathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            if (inventory.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(inventory); // http body (model) parameter
            }
            else
            {
                localVarPostBody = inventory; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleUpsertProductInventory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling InventoryModuleUpsertProductInventory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceInventoryModuleWebModelInventoryInfo>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceInventoryModuleWebModelInventoryInfo) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceInventoryModuleWebModelInventoryInfo)));
            
        }

    }
}
