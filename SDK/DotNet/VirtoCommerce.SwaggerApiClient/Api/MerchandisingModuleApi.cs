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
    public interface IMerchandisingModuleApi
    {
        
        /// <summary>
        /// Get store category by code
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="code">Category code</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        void MerchandisingModuleCategoryGetCategoryByCode (string store, string code, string language);
  
        /// <summary>
        /// Get store category by code
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store id</param>
        /// <param name="code">Category code</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        System.Threading.Tasks.Task MerchandisingModuleCategoryGetCategoryByCodeAsync (string store, string code, string language);
        
        /// <summary>
        /// Get store category by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="category">Category id</param>
        /// <param name="store">Store id</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        void MerchandisingModuleCategoryGetCategoryById (string category, string store, string language);
  
        /// <summary>
        /// Get store category by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="category">Category id</param>
        /// <param name="store">Store id</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        System.Threading.Tasks.Task MerchandisingModuleCategoryGetCategoryByIdAsync (string category, string store, string language);
        
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store ID</param>
        /// <param name="placeHolders">Array of placeholder ids</param>
        /// <param name="tags">Array of tags</param>
        /// <param name="language">Culture name (devault value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection</returns>
        VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection MerchandisingModuleMarketingGetDynamicContent (string store, List<string> placeHolders, List<string> tags, string language);
  
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="store">Store ID</param>
        /// <param name="placeHolders">Array of placeholder ids</param>
        /// <param name="tags">Array of tags</param>
        /// <param name="language">Culture name (devault value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection</returns>
        System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection> MerchandisingModuleMarketingGetDynamicContentAsync (string store, List<string> placeHolders, List<string> tags, string language);
        
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns></returns>
        List<VirtoCommerceMerchandisingModuleWebModelPromotionReward> MerchandisingModuleMarketingEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
  
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>> MerchandisingModuleMarketingEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
        
        /// <summary>
        /// Process marketing event
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="marketingEvent">Marketing event</param>
        /// <returns></returns>
        List<VirtoCommerceMerchandisingModuleWebModelPromotionReward> MerchandisingModuleMarketingProcessMarketingEvent (VirtoCommerceMerchandisingModuleWebModelMarketingEvent marketingEvent);
  
        /// <summary>
        /// Process marketing event
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="marketingEvent">Marketing event</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>> MerchandisingModuleMarketingProcessMarketingEventAsync (VirtoCommerceMerchandisingModuleWebModelMarketingEvent marketingEvent);
        
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
        /// Get collection of pricelists for given catalog
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalog">Catalog id</param>
        /// <param name="currency">Currency code</param>
        /// <param name="tags">Array of tags</param>
        /// <returns></returns>
        List<string> MerchandisingModulePriceGetPriceListStack (string catalog, string currency, List<string> tags);
  
        /// <summary>
        /// Get collection of pricelists for given catalog
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalog">Catalog id</param>
        /// <param name="currency">Currency code</param>
        /// <param name="tags">Array of tags</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<string>> MerchandisingModulePriceGetPriceListStackAsync (string catalog, string currency, List<string> tags);
        
        /// <summary>
        /// Get prices collection by product ids and pricelist ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceLists">Array of product ids</param>
        /// <param name="products">Array of pricelists ids</param>
        /// <returns></returns>
        List<VirtoCommerceMerchandisingModuleWebModelPrice> MerchandisingModulePriceGetProductPrices (List<string> priceLists, List<string> products);
  
        /// <summary>
        /// Get prices collection by product ids and pricelist ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="priceLists">Array of product ids</param>
        /// <param name="products">Array of pricelists ids</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPrice>> MerchandisingModulePriceGetProductPricesAsync (List<string> priceLists, List<string> products);
        
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
        List<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductsByIds (string store, List<string> ids, string responseGroup);
  
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
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>> MerchandisingModuleProductGetProductsByIdsAsync (string store, List<string> ids, string responseGroup);
        
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
        VirtoCommerceMerchandisingModuleWebModelProductSearchResult MerchandisingModuleProductSearch (string requestStore, List<string> requestPricelists, string requestResponseGroup, string requestOutline, string requestLanguage, string requestCurrency, string requestSearchPhrase, string requestSort, string requestSortOrder, DateTime? requestStartDateFrom, int? requestSkip, int? requestTake, List<string> requestTerms, List<string> requestFacets);
  
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
        System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelProductSearchResult> MerchandisingModuleProductSearchAsync (string requestStore, List<string> requestPricelists, string requestResponseGroup, string requestOutline, string requestLanguage, string requestCurrency, string requestSearchPhrase, string requestSort, string requestSortOrder, DateTime? requestStartDateFrom, int? requestSkip, int? requestTake, List<string> requestTerms, List<string> requestFacets);
        
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
        /// <returns></returns>
        void MerchandisingModuleProductGetProduct (string store, string product, string responseGroup, string language);
  
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
        /// <returns></returns>
        System.Threading.Tasks.Task MerchandisingModuleProductGetProductAsync (string store, string product, string responseGroup, string language);
        
        /// <summary>
        /// Get stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommerceMerchandisingModuleWebModelStore> MerchandisingModuleStoreGetStores ();
  
        /// <summary>
        /// Get stores
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelStore>> MerchandisingModuleStoreGetStoresAsync ();
        
        /// <summary>
        /// Get product reviews
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <param name="language">Culture name</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelReview</returns>
        VirtoCommerceMerchandisingModuleWebModelReview MerchandisingModuleReviewGetProductReviews (string productId, string language);
  
        /// <summary>
        /// Get product reviews
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">Product id</param>
        /// <param name="language">Culture name</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelReview</returns>
        System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelReview> MerchandisingModuleReviewGetProductReviewsAsync (string productId, string language);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MerchandisingModuleApi : IMerchandisingModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MerchandisingModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MerchandisingModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="MerchandisingModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MerchandisingModuleApi(String basePath)
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
        /// Get store category by code 
        /// </summary>
        /// <param name="store">Store id</param> 
        /// <param name="code">Category code</param> 
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param> 
        /// <returns></returns>            
        public void MerchandisingModuleCategoryGetCategoryByCode (string store, string code, string language)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleCategoryGetCategoryByCode");
            
            // verify the required parameter 'code' is set
            if (code == null) throw new ApiException(400, "Missing required parameter 'code' when calling MerchandisingModuleCategoryGetCategoryByCode");
            
    
            var path = "/api/mp/categories";
    
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
            if (code != null) queryParams.Add("code", ApiClient.ParameterToString(code)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryByCode: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryByCode: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get store category by code 
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="code">Category code</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MerchandisingModuleCategoryGetCategoryByCodeAsync (string store, string code, string language)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleCategoryGetCategoryByCode");
            // verify the required parameter 'code' is set
            if (code == null) throw new ApiException(400, "Missing required parameter 'code' when calling MerchandisingModuleCategoryGetCategoryByCode");
            
    
            var path = "/api/mp/categories";
    
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
            if (code != null) queryParams.Add("code", ApiClient.ParameterToString(code)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryByCode: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get store category by id 
        /// </summary>
        /// <param name="category">Category id</param> 
        /// <param name="store">Store id</param> 
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param> 
        /// <returns></returns>            
        public void MerchandisingModuleCategoryGetCategoryById (string category, string store, string language)
        {
            
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling MerchandisingModuleCategoryGetCategoryById");
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleCategoryGetCategoryById");
            
    
            var path = "/api/mp/categories/{category}";
    
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
            if (category != null) pathParams.Add("category", ApiClient.ParameterToString(category)); // path parameter
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryById: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get store category by id 
        /// </summary>
        /// <param name="category">Category id</param>
        /// <param name="store">Store id</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MerchandisingModuleCategoryGetCategoryByIdAsync (string category, string store, string language)
        {
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling MerchandisingModuleCategoryGetCategoryById");
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleCategoryGetCategoryById");
            
    
            var path = "/api/mp/categories/{category}";
    
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
            if (category != null) pathParams.Add("category", ApiClient.ParameterToString(category)); // path parameter
            
            if (store != null) queryParams.Add("store", ApiClient.ParameterToString(store)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleCategoryGetCategoryById: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="store">Store ID</param> 
        /// <param name="placeHolders">Array of placeholder ids</param> 
        /// <param name="tags">Array of tags</param> 
        /// <param name="language">Culture name (devault value is \&quot;en-us\&quot;)</param> 
        /// <returns>VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection</returns>            
        public VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection MerchandisingModuleMarketingGetDynamicContent (string store, List<string> placeHolders, List<string> tags, string language)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleMarketingGetDynamicContent");
            
            // verify the required parameter 'placeHolders' is set
            if (placeHolders == null) throw new ApiException(400, "Missing required parameter 'placeHolders' when calling MerchandisingModuleMarketingGetDynamicContent");
            
            // verify the required parameter 'tags' is set
            if (tags == null) throw new ApiException(400, "Missing required parameter 'tags' when calling MerchandisingModuleMarketingGetDynamicContent");
            
    
            var path = "/api/mp/marketing/contentitems";
    
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
            if (tags != null) queryParams.Add("tags", ApiClient.ParameterToString(tags)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            postBody = ApiClient.Serialize(placeHolders); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingGetDynamicContent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingGetDynamicContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection), response.Headers);
        }
    
        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="store">Store ID</param>
        /// <param name="placeHolders">Array of placeholder ids</param>
        /// <param name="tags">Array of tags</param>
        /// <param name="language">Culture name (devault value is \&quot;en-us\&quot;)</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection> MerchandisingModuleMarketingGetDynamicContentAsync (string store, List<string> placeHolders, List<string> tags, string language)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleMarketingGetDynamicContent");
            // verify the required parameter 'placeHolders' is set
            if (placeHolders == null) throw new ApiException(400, "Missing required parameter 'placeHolders' when calling MerchandisingModuleMarketingGetDynamicContent");
            // verify the required parameter 'tags' is set
            if (tags == null) throw new ApiException(400, "Missing required parameter 'tags' when calling MerchandisingModuleMarketingGetDynamicContent");
            
    
            var path = "/api/mp/marketing/contentitems";
    
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
            if (tags != null) queryParams.Add("tags", ApiClient.ParameterToString(tags)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            postBody = ApiClient.Serialize(placeHolders); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingGetDynamicContent: " + response.Content, response.Content);

            return (VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelDynamicContentItemGroupResponseCollection), response.Headers);
        }
        
        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param> 
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelPromotionReward> MerchandisingModuleMarketingEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            
            // verify the required parameter 'context' is set
            if (context == null) throw new ApiException(400, "Missing required parameter 'context' when calling MerchandisingModuleMarketingEvaluatePromotions");
            
    
            var path = "/api/mp/marketing/promotions/evaluate";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(context); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingEvaluatePromotions: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingEvaluatePromotions: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>), response.Headers);
        }
    
        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>> MerchandisingModuleMarketingEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null) throw new ApiException(400, "Missing required parameter 'context' when calling MerchandisingModuleMarketingEvaluatePromotions");
            
    
            var path = "/api/mp/marketing/promotions/evaluate";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(context); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingEvaluatePromotions: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>), response.Headers);
        }
        
        /// <summary>
        /// Process marketing event 
        /// </summary>
        /// <param name="marketingEvent">Marketing event</param> 
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelPromotionReward> MerchandisingModuleMarketingProcessMarketingEvent (VirtoCommerceMerchandisingModuleWebModelMarketingEvent marketingEvent)
        {
            
            // verify the required parameter 'marketingEvent' is set
            if (marketingEvent == null) throw new ApiException(400, "Missing required parameter 'marketingEvent' when calling MerchandisingModuleMarketingProcessMarketingEvent");
            
    
            var path = "/api/mp/marketing/promotions/processevent";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(marketingEvent); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingProcessMarketingEvent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingProcessMarketingEvent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>), response.Headers);
        }
    
        /// <summary>
        /// Process marketing event 
        /// </summary>
        /// <param name="marketingEvent">Marketing event</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>> MerchandisingModuleMarketingProcessMarketingEventAsync (VirtoCommerceMerchandisingModuleWebModelMarketingEvent marketingEvent)
        {
            // verify the required parameter 'marketingEvent' is set
            if (marketingEvent == null) throw new ApiException(400, "Missing required parameter 'marketingEvent' when calling MerchandisingModuleMarketingProcessMarketingEvent");
            
    
            var path = "/api/mp/marketing/promotions/processevent";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(marketingEvent); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleMarketingProcessMarketingEvent: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPromotionReward>), response.Headers);
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="request"></param> 
        /// <returns></returns>            
        public void MerchandisingNotificationSendNotification (VirtoCommerceMerchandisingModuleWebModelNotificaitonsSendDynamicMerchandisingNotificationRequest request)
        {
            
            // verify the required parameter 'request' is set
            if (request == null) throw new ApiException(400, "Missing required parameter 'request' when calling MerchandisingNotificationSendNotification");
            
    
            var path = "/api/mp/notification";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
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
            
    
            var path = "/api/mp/notification";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingNotificationSendNotification: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get collection of pricelists for given catalog 
        /// </summary>
        /// <param name="catalog">Catalog id</param> 
        /// <param name="currency">Currency code</param> 
        /// <param name="tags">Array of tags</param> 
        /// <returns></returns>            
        public List<string> MerchandisingModulePriceGetPriceListStack (string catalog, string currency, List<string> tags)
        {
            
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling MerchandisingModulePriceGetPriceListStack");
            
    
            var path = "/api/mp/pricelists";
    
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
            
            if (catalog != null) queryParams.Add("catalog", ApiClient.ParameterToString(catalog)); // query parameter
            if (currency != null) queryParams.Add("currency", ApiClient.ParameterToString(currency)); // query parameter
            if (tags != null) queryParams.Add("tags", ApiClient.ParameterToString(tags)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetPriceListStack: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetPriceListStack: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Get collection of pricelists for given catalog 
        /// </summary>
        /// <param name="catalog">Catalog id</param>
        /// <param name="currency">Currency code</param>
        /// <param name="tags">Array of tags</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<string>> MerchandisingModulePriceGetPriceListStackAsync (string catalog, string currency, List<string> tags)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling MerchandisingModulePriceGetPriceListStack");
            
    
            var path = "/api/mp/pricelists";
    
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
            
            if (catalog != null) queryParams.Add("catalog", ApiClient.ParameterToString(catalog)); // query parameter
            if (currency != null) queryParams.Add("currency", ApiClient.ParameterToString(currency)); // query parameter
            if (tags != null) queryParams.Add("tags", ApiClient.ParameterToString(tags)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetPriceListStack: " + response.Content, response.Content);

            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
        
        /// <summary>
        /// Get prices collection by product ids and pricelist ids 
        /// </summary>
        /// <param name="priceLists">Array of product ids</param> 
        /// <param name="products">Array of pricelists ids</param> 
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelPrice> MerchandisingModulePriceGetProductPrices (List<string> priceLists, List<string> products)
        {
            
            // verify the required parameter 'priceLists' is set
            if (priceLists == null) throw new ApiException(400, "Missing required parameter 'priceLists' when calling MerchandisingModulePriceGetProductPrices");
            
            // verify the required parameter 'products' is set
            if (products == null) throw new ApiException(400, "Missing required parameter 'products' when calling MerchandisingModulePriceGetProductPrices");
            
    
            var path = "/api/mp/prices";
    
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
            
            if (priceLists != null) queryParams.Add("priceLists", ApiClient.ParameterToString(priceLists)); // query parameter
            if (products != null) queryParams.Add("products", ApiClient.ParameterToString(products)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetProductPrices: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetProductPrices: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelPrice>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPrice>), response.Headers);
        }
    
        /// <summary>
        /// Get prices collection by product ids and pricelist ids 
        /// </summary>
        /// <param name="priceLists">Array of product ids</param>
        /// <param name="products">Array of pricelists ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelPrice>> MerchandisingModulePriceGetProductPricesAsync (List<string> priceLists, List<string> products)
        {
            // verify the required parameter 'priceLists' is set
            if (priceLists == null) throw new ApiException(400, "Missing required parameter 'priceLists' when calling MerchandisingModulePriceGetProductPrices");
            // verify the required parameter 'products' is set
            if (products == null) throw new ApiException(400, "Missing required parameter 'products' when calling MerchandisingModulePriceGetProductPrices");
            
    
            var path = "/api/mp/prices";
    
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
            
            if (priceLists != null) queryParams.Add("priceLists", ApiClient.ParameterToString(priceLists)); // query parameter
            if (products != null) queryParams.Add("products", ApiClient.ParameterToString(products)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModulePriceGetProductPrices: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelPrice>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelPrice>), response.Headers);
        }
        
        /// <summary>
        /// Get store products collection by their ids 
        /// </summary>
        /// <param name="store">Store id</param> 
        /// <param name="ids">Product ids</param> 
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param> 
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelCatalogItem> MerchandisingModuleProductGetProductsByIds (string store, List<string> ids, string responseGroup)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProductsByIds");
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MerchandisingModuleProductGetProductsByIds");
            
    
            var path = "/api/mp/products";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>), response.Headers);
        }
    
        /// <summary>
        /// Get store products collection by their ids 
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="ids">Product ids</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemInfo)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>> MerchandisingModuleProductGetProductsByIdsAsync (string store, List<string> ids, string responseGroup)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProductsByIds");
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MerchandisingModuleProductGetProductsByIds");
            
    
            var path = "/api/mp/products";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProductsByIds: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelCatalogItem>), response.Headers);
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
        public VirtoCommerceMerchandisingModuleWebModelProductSearchResult MerchandisingModuleProductSearch (string requestStore, List<string> requestPricelists, string requestResponseGroup, string requestOutline, string requestLanguage, string requestCurrency, string requestSearchPhrase, string requestSort, string requestSortOrder, DateTime? requestStartDateFrom, int? requestSkip, int? requestTake, List<string> requestTerms, List<string> requestFacets)
        {
            
    
            var path = "/api/mp/products/search";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceMerchandisingModuleWebModelProductSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelProductSearchResult), response.Headers);
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
        public async System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelProductSearchResult> MerchandisingModuleProductSearchAsync (string requestStore, List<string> requestPricelists, string requestResponseGroup, string requestOutline, string requestLanguage, string requestCurrency, string requestSearchPhrase, string requestSort, string requestSortOrder, DateTime? requestStartDateFrom, int? requestSkip, int? requestTake, List<string> requestTerms, List<string> requestFacets)
        {
            
    
            var path = "/api/mp/products/search";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductSearch: " + response.Content, response.Content);

            return (VirtoCommerceMerchandisingModuleWebModelProductSearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelProductSearchResult), response.Headers);
        }
        
        /// <summary>
        /// Get store product by id 
        /// </summary>
        /// <param name="store">Store id</param> 
        /// <param name="product">Product id</param> 
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param> 
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param> 
        /// <returns></returns>            
        public void MerchandisingModuleProductGetProduct (string store, string product, string responseGroup, string language)
        {
            
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProduct");
            
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling MerchandisingModuleProductGetProduct");
            
    
            var path = "/api/mp/products/{product}";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get store product by id 
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="product">Product id</param>
        /// <param name="responseGroup">Response detalization scale (default value is ItemLarge)</param>
        /// <param name="language">Culture name (default value is \&quot;en-us\&quot;)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MerchandisingModuleProductGetProductAsync (string store, string product, string responseGroup, string language)
        {
            // verify the required parameter 'store' is set
            if (store == null) throw new ApiException(400, "Missing required parameter 'store' when calling MerchandisingModuleProductGetProduct");
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling MerchandisingModuleProductGetProduct");
            
    
            var path = "/api/mp/products/{product}";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleProductGetProduct: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get stores 
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommerceMerchandisingModuleWebModelStore> MerchandisingModuleStoreGetStores ()
        {
            
    
            var path = "/api/mp/stores";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleStoreGetStores: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleStoreGetStores: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceMerchandisingModuleWebModelStore>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelStore>), response.Headers);
        }
    
        /// <summary>
        /// Get stores 
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMerchandisingModuleWebModelStore>> MerchandisingModuleStoreGetStoresAsync ()
        {
            
    
            var path = "/api/mp/stores";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleStoreGetStores: " + response.Content, response.Content);

            return (List<VirtoCommerceMerchandisingModuleWebModelStore>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceMerchandisingModuleWebModelStore>), response.Headers);
        }
        
        /// <summary>
        /// Get product reviews 
        /// </summary>
        /// <param name="productId">Product id</param> 
        /// <param name="language">Culture name</param> 
        /// <returns>VirtoCommerceMerchandisingModuleWebModelReview</returns>            
        public VirtoCommerceMerchandisingModuleWebModelReview MerchandisingModuleReviewGetProductReviews (string productId, string language)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling MerchandisingModuleReviewGetProductReviews");
            
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MerchandisingModuleReviewGetProductReviews");
            
    
            var path = "/api/mp/{language}/products/{productId}/reviews";
    
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
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleReviewGetProductReviews: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleReviewGetProductReviews: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceMerchandisingModuleWebModelReview) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelReview), response.Headers);
        }
    
        /// <summary>
        /// Get product reviews 
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="language">Culture name</param>
        /// <returns>VirtoCommerceMerchandisingModuleWebModelReview</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMerchandisingModuleWebModelReview> MerchandisingModuleReviewGetProductReviewsAsync (string productId, string language)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling MerchandisingModuleReviewGetProductReviews");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MerchandisingModuleReviewGetProductReviews");
            
    
            var path = "/api/mp/{language}/products/{productId}/reviews";
    
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
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MerchandisingModuleReviewGetProductReviews: " + response.Content, response.Content);

            return (VirtoCommerceMerchandisingModuleWebModelReview) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceMerchandisingModuleWebModelReview), response.Headers);
        }
        
    }
    
}
