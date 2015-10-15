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
    public interface IPricingModuleApi
    {
        
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        void PricingModuleGetProductPriceLists (string productId);
  
        /// <summary>
        /// Get pricelists for a product
        /// </summary>
        /// <remarks>
        /// Get all pricelists for given product.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleGetProductPriceListsAsync (string productId);
        
        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns></returns>
        void PricingModuleUpdateProductPriceLists (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);
  
        /// <summary>
        /// Update prices
        /// </summary>
        /// <remarks>
        /// Update prices of product for given pricelist.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleUpdateProductPriceListsAsync (string productId, VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ();
  
        /// <summary>
        /// Get pricelist assignments
        /// </summary>
        /// <remarks>
        /// Get array of all pricelist assignments for all catalogs.
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ();
        
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
  
        /// <summary>
        /// Update pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
  
        /// <summary>
        /// Create pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment);
        
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        void PricingModuleDeleteAssignments (List<string> ids);
  
        /// <summary>
        /// Delete pricelist assignments
        /// </summary>
        /// <remarks>
        /// Delete pricelist assignment by given array of ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids);
        
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ();
  
        /// <summary>
        /// Get a new pricelist assignment
        /// </summary>
        /// <remarks>
        /// Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </remarks>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ();
        
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns></returns>
        void PricingModuleGetPricelistAssignmentById (string id);
  
        /// <summary>
        /// Get pricelist assignment
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleGetPricelistAssignmentByIdAsync (string id);
        
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ();
  
        /// <summary>
        /// Get pricelists
        /// </summary>
        /// <remarks>
        /// Get all pricelists for all catalogs.
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ();
        
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns></returns>
        void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);
  
        /// <summary>
        /// Update pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList);
  
        /// <summary>
        /// Create pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList);
        
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        void PricingModuleDeletePriceLists (List<string> ids);
  
        /// <summary>
        /// Delete pricelists
        /// </summary>
        /// <remarks>
        /// Delete pricelists by given array of pricelist ids.
        /// </remarks>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids);
        
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns></returns>
        void PricingModuleGetPriceListById (string id);
  
        /// <summary>
        /// Get pricelist
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Pricelist id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleGetPriceListByIdAsync (string id);
        
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        void PricingModuleGetProductPrices (string productId);
  
        /// <summary>
        /// Get array of product prices
        /// </summary>
        /// <remarks>
        /// Get an array of valid product prices for each currency.
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task PricingModuleGetProductPricesAsync (string productId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PricingModuleApi : IPricingModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PricingModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PricingModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="PricingModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PricingModuleApi(String basePath)
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
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns></returns>            
        public void PricingModuleGetProductPriceLists (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPriceLists");
            
    
            var path = "/api/catalog/products/{productId}/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPriceLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get pricelists for a product Get all pricelists for given product.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleGetProductPriceListsAsync (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPriceLists");
            
    
            var path = "/api/catalog/products/{productId}/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPriceLists: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <param name="priceList">Pricelist with new product prices</param> 
        /// <returns></returns>            
        public void PricingModuleUpdateProductPriceLists (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleUpdateProductPriceLists");
            
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdateProductPriceLists");
            
    
            var path = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update prices Update prices of product for given pricelist.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="priceList">Pricelist with new product prices</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleUpdateProductPriceListsAsync (string productId, VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleUpdateProductPriceLists");
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdateProductPriceLists");
            
    
            var path = "/api/catalog/products/{productId}/pricelists";
    
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
            if (productId != null) pathParams.Add("productId", ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdateProductPriceLists: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetPricelistAssignments ()
        {
            
    
            var path = "/api/pricing/assignments";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>), response.Headers);
        }
    
        /// <summary>
        /// Get pricelist assignments Get array of all pricelist assignments for all catalogs.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelistAssignment>> PricingModuleGetPricelistAssignmentsAsync ()
        {
            
    
            var path = "/api/pricing/assignments";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignments: " + response.Content, response.Content);

            return (List<VirtoCommercePricingModuleWebModelPricelistAssignment>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePricingModuleWebModelPricelistAssignment>), response.Headers);
        }
        
        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns></returns>            
        public void PricingModuleUpdatePriceListAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleUpdatePriceListAssignment");
            
    
            var path = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(assignment); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleUpdatePriceListAssignment");
            
    
            var path = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(assignment); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceListAssignment: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>            
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleCreatePricelistAssignment (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleCreatePricelistAssignment");
            
    
            var path = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(assignment); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePricingModuleWebModelPricelistAssignment) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment), response.Headers);
        }
    
        /// <summary>
        /// Create pricelist assignment 
        /// </summary>
        /// <param name="assignment">PricelistAssignment</param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleCreatePricelistAssignmentAsync (VirtoCommercePricingModuleWebModelPricelistAssignment assignment)
        {
            // verify the required parameter 'assignment' is set
            if (assignment == null) throw new ApiException(400, "Missing required parameter 'assignment' when calling PricingModuleCreatePricelistAssignment");
            
    
            var path = "/api/pricing/assignments";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(assignment); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePricelistAssignment: " + response.Content, response.Content);

            return (VirtoCommercePricingModuleWebModelPricelistAssignment) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment), response.Headers);
        }
        
        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param> 
        /// <returns></returns>            
        public void PricingModuleDeleteAssignments (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeleteAssignments");
            
    
            var path = "/api/pricing/assignments";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeleteAssignments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeleteAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete pricelist assignments Delete pricelist assignment by given array of ids.
        /// </summary>
        /// <param name="ids">An array of pricelist assignment ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleDeleteAssignmentsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeleteAssignments");
            
    
            var path = "/api/pricing/assignments";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeleteAssignments: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>            
        public VirtoCommercePricingModuleWebModelPricelistAssignment PricingModuleGetNewPricelistAssignments ()
        {
            
    
            var path = "/api/pricing/assignments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePricingModuleWebModelPricelistAssignment) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment), response.Headers);
        }
    
        /// <summary>
        /// Get a new pricelist assignment Get a new pricelist assignment object. Create new pricelist assignment, but does not save one.
        /// </summary>
        /// <returns>VirtoCommercePricingModuleWebModelPricelistAssignment</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelistAssignment> PricingModuleGetNewPricelistAssignmentsAsync ()
        {
            
    
            var path = "/api/pricing/assignments/new";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetNewPricelistAssignments: " + response.Content, response.Content);

            return (VirtoCommercePricingModuleWebModelPricelistAssignment) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelistAssignment), response.Headers);
        }
        
        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param> 
        /// <returns></returns>            
        public void PricingModuleGetPricelistAssignmentById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPricelistAssignmentById");
            
    
            var path = "/api/pricing/assignments/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get pricelist assignment 
        /// </summary>
        /// <param name="id">Pricelist assignment id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleGetPricelistAssignmentByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPricelistAssignmentById");
            
    
            var path = "/api/pricing/assignments/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPricelistAssignmentById: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommercePricingModuleWebModelPricelist> PricingModuleGetPriceLists ()
        {
            
    
            var path = "/api/pricing/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommercePricingModuleWebModelPricelist>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePricingModuleWebModelPricelist>), response.Headers);
        }
    
        /// <summary>
        /// Get pricelists Get all pricelists for all catalogs.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommercePricingModuleWebModelPricelist>> PricingModuleGetPriceListsAsync ()
        {
            
    
            var path = "/api/pricing/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceLists: " + response.Content, response.Content);

            return (List<VirtoCommercePricingModuleWebModelPricelist>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommercePricingModuleWebModelPricelist>), response.Headers);
        }
        
        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns></returns>            
        public void PricingModuleUpdatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdatePriceList");
            
    
            var path = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceList: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleUpdatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleUpdatePriceList");
            
    
            var path = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleUpdatePriceList: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param> 
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>            
        public VirtoCommercePricingModuleWebModelPricelist PricingModuleCreatePriceList (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleCreatePriceList");
            
    
            var path = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePriceList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePriceList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommercePricingModuleWebModelPricelist) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelist), response.Headers);
        }
    
        /// <summary>
        /// Create pricelist 
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns>VirtoCommercePricingModuleWebModelPricelist</returns>
        public async System.Threading.Tasks.Task<VirtoCommercePricingModuleWebModelPricelist> PricingModuleCreatePriceListAsync (VirtoCommercePricingModuleWebModelPricelist priceList)
        {
            // verify the required parameter 'priceList' is set
            if (priceList == null) throw new ApiException(400, "Missing required parameter 'priceList' when calling PricingModuleCreatePriceList");
            
    
            var path = "/api/pricing/pricelists";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(priceList); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleCreatePriceList: " + response.Content, response.Content);

            return (VirtoCommercePricingModuleWebModelPricelist) ApiClient.Deserialize(response.Content, typeof(VirtoCommercePricingModuleWebModelPricelist), response.Headers);
        }
        
        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param> 
        /// <returns></returns>            
        public void PricingModuleDeletePriceLists (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeletePriceLists");
            
    
            var path = "/api/pricing/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeletePriceLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeletePriceLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete pricelists Delete pricelists by given array of pricelist ids.
        /// </summary>
        /// <param name="ids">An array of pricelist ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleDeletePriceListsAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling PricingModuleDeletePriceLists");
            
    
            var path = "/api/pricing/pricelists";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleDeletePriceLists: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param> 
        /// <returns></returns>            
        public void PricingModuleGetPriceListById (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPriceListById");
            
    
            var path = "/api/pricing/pricelists/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceListById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceListById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get pricelist 
        /// </summary>
        /// <param name="id">Pricelist id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleGetPriceListByIdAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling PricingModuleGetPriceListById");
            
    
            var path = "/api/pricing/pricelists/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetPriceListById: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <returns></returns>            
        public void PricingModuleGetProductPrices (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPrices");
            
    
            var path = "/api/products/{productId}/prices";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPrices: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPrices: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get array of product prices Get an array of valid product prices for each currency.
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PricingModuleGetProductPricesAsync (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling PricingModuleGetProductPrices");
            
    
            var path = "/api/products/{productId}/prices";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling PricingModuleGetProductPrices: " + response.Content, response.Content);

            
            return;
        }
        
    }
    
}
