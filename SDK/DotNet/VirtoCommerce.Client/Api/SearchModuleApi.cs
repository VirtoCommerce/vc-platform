using System;
using System.IO;
using System.Collections.Generic;
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
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaSearchInChildren"></param>
        /// <param name="criteriaCategoryId"></param>
        /// <param name="criteriaCategoryIds"></param>
        /// <param name="criteriaCatalogId"></param>
        /// <param name="criteriaCatalogIds"></param>
        /// <param name="criteriaLanguageCode"></param>
        /// <param name="criteriaCode"></param>
        /// <param name="criteriaSort"></param>
        /// <param name="criteriaSortOrder"></param>
        /// <param name="criteriaHideDirectLinkedCategories"></param>
        /// <param name="criteriaPropertyValues"></param>
        /// <param name="criteriaCurrency"></param>
        /// <param name="criteriaStartPrice"></param>
        /// <param name="criteriaEndPrice"></param>
        /// <param name="criteriaSkip"></param>
        /// <param name="criteriaTake"></param>
        /// <param name="criteriaIndexDate"></param>
        /// <param name="criteriaPricelistId"></param>
        /// <param name="criteriaPricelistIds"></param>
        /// <param name="criteriaTerms"></param>
        /// <param name="criteriaFacets"></param>
        /// <param name="criteriaOutline"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, DateTime? criteriaStartDateFrom = null);
  
        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaSearchInChildren"></param>
        /// <param name="criteriaCategoryId"></param>
        /// <param name="criteriaCategoryIds"></param>
        /// <param name="criteriaCatalogId"></param>
        /// <param name="criteriaCatalogIds"></param>
        /// <param name="criteriaLanguageCode"></param>
        /// <param name="criteriaCode"></param>
        /// <param name="criteriaSort"></param>
        /// <param name="criteriaSortOrder"></param>
        /// <param name="criteriaHideDirectLinkedCategories"></param>
        /// <param name="criteriaPropertyValues"></param>
        /// <param name="criteriaCurrency"></param>
        /// <param name="criteriaStartPrice"></param>
        /// <param name="criteriaEndPrice"></param>
        /// <param name="criteriaSkip"></param>
        /// <param name="criteriaTake"></param>
        /// <param name="criteriaIndexDate"></param>
        /// <param name="criteriaPricelistId"></param>
        /// <param name="criteriaPricelistIds"></param>
        /// <param name="criteriaTerms"></param>
        /// <param name="criteriaFacets"></param>
        /// <param name="criteriaOutline"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, DateTime? criteriaStartDateFrom = null);
        
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns></returns>
        List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId);
  
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SearchModuleApi : ISearchModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient</param>
        /// <returns></returns>
        public SearchModuleApi(ApiClient apiClient)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
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
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteriaStoreId"></param> 
        /// <param name="criteriaResponseGroup"></param> 
        /// <param name="criteriaKeyword"></param> 
        /// <param name="criteriaSearchInChildren"></param> 
        /// <param name="criteriaCategoryId"></param> 
        /// <param name="criteriaCategoryIds"></param> 
        /// <param name="criteriaCatalogId"></param> 
        /// <param name="criteriaCatalogIds"></param> 
        /// <param name="criteriaLanguageCode"></param> 
        /// <param name="criteriaCode"></param> 
        /// <param name="criteriaSort"></param> 
        /// <param name="criteriaSortOrder"></param> 
        /// <param name="criteriaHideDirectLinkedCategories"></param> 
        /// <param name="criteriaPropertyValues"></param> 
        /// <param name="criteriaCurrency"></param> 
        /// <param name="criteriaStartPrice"></param> 
        /// <param name="criteriaEndPrice"></param> 
        /// <param name="criteriaSkip"></param> 
        /// <param name="criteriaTake"></param> 
        /// <param name="criteriaIndexDate"></param> 
        /// <param name="criteriaPricelistId"></param> 
        /// <param name="criteriaPricelistIds"></param> 
        /// <param name="criteriaTerms"></param> 
        /// <param name="criteriaFacets"></param> 
        /// <param name="criteriaOutline"></param> 
        /// <param name="criteriaStartDateFrom"></param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>            
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, DateTime? criteriaStartDateFrom = null)
        {
            
    
            var path_ = "/api/search";
    
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
            
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaSearchInChildren != null) queryParams.Add("criteria.searchInChildren", ApiClient.ParameterToString(criteriaSearchInChildren)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCategoryIds != null) queryParams.Add("criteria.categoryIds", ApiClient.ParameterToString(criteriaCategoryIds)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaCatalogIds != null) queryParams.Add("criteria.catalogIds", ApiClient.ParameterToString(criteriaCatalogIds)); // query parameter
            if (criteriaLanguageCode != null) queryParams.Add("criteria.languageCode", ApiClient.ParameterToString(criteriaLanguageCode)); // query parameter
            if (criteriaCode != null) queryParams.Add("criteria.code", ApiClient.ParameterToString(criteriaCode)); // query parameter
            if (criteriaSort != null) queryParams.Add("criteria.sort", ApiClient.ParameterToString(criteriaSort)); // query parameter
            if (criteriaSortOrder != null) queryParams.Add("criteria.sortOrder", ApiClient.ParameterToString(criteriaSortOrder)); // query parameter
            if (criteriaHideDirectLinkedCategories != null) queryParams.Add("criteria.hideDirectLinkedCategories", ApiClient.ParameterToString(criteriaHideDirectLinkedCategories)); // query parameter
            if (criteriaPropertyValues != null) queryParams.Add("criteria.propertyValues", ApiClient.ParameterToString(criteriaPropertyValues)); // query parameter
            if (criteriaCurrency != null) queryParams.Add("criteria.currency", ApiClient.ParameterToString(criteriaCurrency)); // query parameter
            if (criteriaStartPrice != null) queryParams.Add("criteria.startPrice", ApiClient.ParameterToString(criteriaStartPrice)); // query parameter
            if (criteriaEndPrice != null) queryParams.Add("criteria.endPrice", ApiClient.ParameterToString(criteriaEndPrice)); // query parameter
            if (criteriaSkip != null) queryParams.Add("criteria.skip", ApiClient.ParameterToString(criteriaSkip)); // query parameter
            if (criteriaTake != null) queryParams.Add("criteria.take", ApiClient.ParameterToString(criteriaTake)); // query parameter
            if (criteriaIndexDate != null) queryParams.Add("criteria.indexDate", ApiClient.ParameterToString(criteriaIndexDate)); // query parameter
            if (criteriaPricelistId != null) queryParams.Add("criteria.pricelistId", ApiClient.ParameterToString(criteriaPricelistId)); // query parameter
            if (criteriaPricelistIds != null) queryParams.Add("criteria.pricelistIds", ApiClient.ParameterToString(criteriaPricelistIds)); // query parameter
            if (criteriaTerms != null) queryParams.Add("criteria.terms", ApiClient.ParameterToString(criteriaTerms)); // query parameter
            if (criteriaFacets != null) queryParams.Add("criteria.facets", ApiClient.ParameterToString(criteriaFacets)); // query parameter
            if (criteriaOutline != null) queryParams.Add("criteria.outline", ApiClient.ParameterToString(criteriaOutline)); // query parameter
            if (criteriaStartDateFrom != null) queryParams.Add("criteria.startDateFrom", ApiClient.ParameterToString(criteriaStartDateFrom)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult));
        }
    
        /// <summary>
        /// Search for products and categories 
        /// </summary>
        /// <param name="criteriaStoreId"></param>
        /// <param name="criteriaResponseGroup"></param>
        /// <param name="criteriaKeyword"></param>
        /// <param name="criteriaSearchInChildren"></param>
        /// <param name="criteriaCategoryId"></param>
        /// <param name="criteriaCategoryIds"></param>
        /// <param name="criteriaCatalogId"></param>
        /// <param name="criteriaCatalogIds"></param>
        /// <param name="criteriaLanguageCode"></param>
        /// <param name="criteriaCode"></param>
        /// <param name="criteriaSort"></param>
        /// <param name="criteriaSortOrder"></param>
        /// <param name="criteriaHideDirectLinkedCategories"></param>
        /// <param name="criteriaPropertyValues"></param>
        /// <param name="criteriaCurrency"></param>
        /// <param name="criteriaStartPrice"></param>
        /// <param name="criteriaEndPrice"></param>
        /// <param name="criteriaSkip"></param>
        /// <param name="criteriaTake"></param>
        /// <param name="criteriaIndexDate"></param>
        /// <param name="criteriaPricelistId"></param>
        /// <param name="criteriaPricelistIds"></param>
        /// <param name="criteriaTerms"></param>
        /// <param name="criteriaFacets"></param>
        /// <param name="criteriaOutline"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, DateTime? criteriaStartDateFrom = null)
        {
            
    
            var path_ = "/api/search";
    
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
            
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaSearchInChildren != null) queryParams.Add("criteria.searchInChildren", ApiClient.ParameterToString(criteriaSearchInChildren)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCategoryIds != null) queryParams.Add("criteria.categoryIds", ApiClient.ParameterToString(criteriaCategoryIds)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaCatalogIds != null) queryParams.Add("criteria.catalogIds", ApiClient.ParameterToString(criteriaCatalogIds)); // query parameter
            if (criteriaLanguageCode != null) queryParams.Add("criteria.languageCode", ApiClient.ParameterToString(criteriaLanguageCode)); // query parameter
            if (criteriaCode != null) queryParams.Add("criteria.code", ApiClient.ParameterToString(criteriaCode)); // query parameter
            if (criteriaSort != null) queryParams.Add("criteria.sort", ApiClient.ParameterToString(criteriaSort)); // query parameter
            if (criteriaSortOrder != null) queryParams.Add("criteria.sortOrder", ApiClient.ParameterToString(criteriaSortOrder)); // query parameter
            if (criteriaHideDirectLinkedCategories != null) queryParams.Add("criteria.hideDirectLinkedCategories", ApiClient.ParameterToString(criteriaHideDirectLinkedCategories)); // query parameter
            if (criteriaPropertyValues != null) queryParams.Add("criteria.propertyValues", ApiClient.ParameterToString(criteriaPropertyValues)); // query parameter
            if (criteriaCurrency != null) queryParams.Add("criteria.currency", ApiClient.ParameterToString(criteriaCurrency)); // query parameter
            if (criteriaStartPrice != null) queryParams.Add("criteria.startPrice", ApiClient.ParameterToString(criteriaStartPrice)); // query parameter
            if (criteriaEndPrice != null) queryParams.Add("criteria.endPrice", ApiClient.ParameterToString(criteriaEndPrice)); // query parameter
            if (criteriaSkip != null) queryParams.Add("criteria.skip", ApiClient.ParameterToString(criteriaSkip)); // query parameter
            if (criteriaTake != null) queryParams.Add("criteria.take", ApiClient.ParameterToString(criteriaTake)); // query parameter
            if (criteriaIndexDate != null) queryParams.Add("criteria.indexDate", ApiClient.ParameterToString(criteriaIndexDate)); // query parameter
            if (criteriaPricelistId != null) queryParams.Add("criteria.pricelistId", ApiClient.ParameterToString(criteriaPricelistId)); // query parameter
            if (criteriaPricelistIds != null) queryParams.Add("criteria.pricelistIds", ApiClient.ParameterToString(criteriaPricelistIds)); // query parameter
            if (criteriaTerms != null) queryParams.Add("criteria.terms", ApiClient.ParameterToString(criteriaTerms)); // query parameter
            if (criteriaFacets != null) queryParams.Add("criteria.facets", ApiClient.ParameterToString(criteriaFacets)); // query parameter
            if (criteriaOutline != null) queryParams.Add("criteria.outline", ApiClient.ParameterToString(criteriaOutline)); // query parameter
            if (criteriaStartDateFrom != null) queryParams.Add("criteria.startDateFrom", ApiClient.ParameterToString(criteriaStartDateFrom)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult));
        }
        
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <returns></returns>            
        public List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleGetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleGetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceSearchModuleWebModelFilterProperty>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>));
        }
    
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleGetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);

            return (List<VirtoCommerceSearchModuleWebModelFilterProperty>) ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>));
        }
        
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <param name="filterProperties"></param> 
        /// <returns></returns>            
        public void SearchModuleSetFilterProperties (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleSetFilterProperties");
            
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null) throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleSetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(filterProperties); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleSetFilterProperties");
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null) throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleSetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(filterProperties); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);

            
            return;
        }
        
    }
    
}
