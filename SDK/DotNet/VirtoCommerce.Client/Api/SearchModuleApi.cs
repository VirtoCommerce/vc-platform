using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);
  
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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);

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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);

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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);
        
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId);
  
        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesWithHttpInfo (string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId);

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo (string storeId);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> SearchModuleSetFilterPropertiesWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties);
        
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
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public SearchModuleApi(Configuration configuration)
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
        /// <param name="criteriaWithHidden"></param> 
        /// <param name="criteriaStartDateFrom"></param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult SearchModuleSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = SearchModuleSearchWithHttpInfo(criteriaStoreId, criteriaResponseGroup, criteriaKeyword, criteriaSearchInChildren, criteriaCategoryId, criteriaCategoryIds, criteriaCatalogId, criteriaCatalogIds, criteriaLanguageCode, criteriaCode, criteriaSort, criteriaSortOrder, criteriaHideDirectLinkedCategories, criteriaPropertyValues, criteriaCurrency, criteriaStartPrice, criteriaEndPrice, criteriaSkip, criteriaTake, criteriaIndexDate, criteriaPricelistId, criteriaPricelistIds, criteriaTerms, criteriaFacets, criteriaOutline, criteriaWithHidden, criteriaStartDateFrom);
             return response.Data;
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
        /// <param name="criteriaWithHidden"></param> 
        /// <param name="criteriaStartDateFrom"></param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalogSearchResult > SearchModuleSearchWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
            
    
            var path_ = "/api/search";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", Configuration.ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", Configuration.ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaSearchInChildren != null) queryParams.Add("criteria.searchInChildren", Configuration.ApiClient.ParameterToString(criteriaSearchInChildren)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", Configuration.ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCategoryIds != null) queryParams.Add("criteria.categoryIds", Configuration.ApiClient.ParameterToString(criteriaCategoryIds)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", Configuration.ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaCatalogIds != null) queryParams.Add("criteria.catalogIds", Configuration.ApiClient.ParameterToString(criteriaCatalogIds)); // query parameter
            if (criteriaLanguageCode != null) queryParams.Add("criteria.languageCode", Configuration.ApiClient.ParameterToString(criteriaLanguageCode)); // query parameter
            if (criteriaCode != null) queryParams.Add("criteria.code", Configuration.ApiClient.ParameterToString(criteriaCode)); // query parameter
            if (criteriaSort != null) queryParams.Add("criteria.sort", Configuration.ApiClient.ParameterToString(criteriaSort)); // query parameter
            if (criteriaSortOrder != null) queryParams.Add("criteria.sortOrder", Configuration.ApiClient.ParameterToString(criteriaSortOrder)); // query parameter
            if (criteriaHideDirectLinkedCategories != null) queryParams.Add("criteria.hideDirectLinkedCategories", Configuration.ApiClient.ParameterToString(criteriaHideDirectLinkedCategories)); // query parameter
            if (criteriaPropertyValues != null) queryParams.Add("criteria.propertyValues", Configuration.ApiClient.ParameterToString(criteriaPropertyValues)); // query parameter
            if (criteriaCurrency != null) queryParams.Add("criteria.currency", Configuration.ApiClient.ParameterToString(criteriaCurrency)); // query parameter
            if (criteriaStartPrice != null) queryParams.Add("criteria.startPrice", Configuration.ApiClient.ParameterToString(criteriaStartPrice)); // query parameter
            if (criteriaEndPrice != null) queryParams.Add("criteria.endPrice", Configuration.ApiClient.ParameterToString(criteriaEndPrice)); // query parameter
            if (criteriaSkip != null) queryParams.Add("criteria.skip", Configuration.ApiClient.ParameterToString(criteriaSkip)); // query parameter
            if (criteriaTake != null) queryParams.Add("criteria.take", Configuration.ApiClient.ParameterToString(criteriaTake)); // query parameter
            if (criteriaIndexDate != null) queryParams.Add("criteria.indexDate", Configuration.ApiClient.ParameterToString(criteriaIndexDate)); // query parameter
            if (criteriaPricelistId != null) queryParams.Add("criteria.pricelistId", Configuration.ApiClient.ParameterToString(criteriaPricelistId)); // query parameter
            if (criteriaPricelistIds != null) queryParams.Add("criteria.pricelistIds", Configuration.ApiClient.ParameterToString(criteriaPricelistIds)); // query parameter
            if (criteriaTerms != null) queryParams.Add("criteria.terms", Configuration.ApiClient.ParameterToString(criteriaTerms)); // query parameter
            if (criteriaFacets != null) queryParams.Add("criteria.facets", Configuration.ApiClient.ParameterToString(criteriaFacets)); // query parameter
            if (criteriaOutline != null) queryParams.Add("criteria.outline", Configuration.ApiClient.ParameterToString(criteriaOutline)); // query parameter
            if (criteriaWithHidden != null) queryParams.Add("criteria.withHidden", Configuration.ApiClient.ParameterToString(criteriaWithHidden)); // query parameter
            if (criteriaStartDateFrom != null) queryParams.Add("criteria.startDateFrom", Configuration.ApiClient.ParameterToString(criteriaStartDateFrom)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> SearchModuleSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = await SearchModuleSearchAsyncWithHttpInfo(criteriaStoreId, criteriaResponseGroup, criteriaKeyword, criteriaSearchInChildren, criteriaCategoryId, criteriaCategoryIds, criteriaCatalogId, criteriaCatalogIds, criteriaLanguageCode, criteriaCode, criteriaSort, criteriaSortOrder, criteriaHideDirectLinkedCategories, criteriaPropertyValues, criteriaCurrency, criteriaStartPrice, criteriaEndPrice, criteriaSkip, criteriaTake, criteriaIndexDate, criteriaPricelistId, criteriaPricelistIds, criteriaTerms, criteriaFacets, criteriaOutline, criteriaWithHidden, criteriaStartDateFrom);
             return response.Data;

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
        /// <param name="criteriaWithHidden"></param>
        /// <param name="criteriaStartDateFrom"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> SearchModuleSearchAsyncWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            if (criteriaStoreId != null) queryParams.Add("criteria.storeId", Configuration.ApiClient.ParameterToString(criteriaStoreId)); // query parameter
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", Configuration.ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaSearchInChildren != null) queryParams.Add("criteria.searchInChildren", Configuration.ApiClient.ParameterToString(criteriaSearchInChildren)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", Configuration.ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCategoryIds != null) queryParams.Add("criteria.categoryIds", Configuration.ApiClient.ParameterToString(criteriaCategoryIds)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", Configuration.ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaCatalogIds != null) queryParams.Add("criteria.catalogIds", Configuration.ApiClient.ParameterToString(criteriaCatalogIds)); // query parameter
            if (criteriaLanguageCode != null) queryParams.Add("criteria.languageCode", Configuration.ApiClient.ParameterToString(criteriaLanguageCode)); // query parameter
            if (criteriaCode != null) queryParams.Add("criteria.code", Configuration.ApiClient.ParameterToString(criteriaCode)); // query parameter
            if (criteriaSort != null) queryParams.Add("criteria.sort", Configuration.ApiClient.ParameterToString(criteriaSort)); // query parameter
            if (criteriaSortOrder != null) queryParams.Add("criteria.sortOrder", Configuration.ApiClient.ParameterToString(criteriaSortOrder)); // query parameter
            if (criteriaHideDirectLinkedCategories != null) queryParams.Add("criteria.hideDirectLinkedCategories", Configuration.ApiClient.ParameterToString(criteriaHideDirectLinkedCategories)); // query parameter
            if (criteriaPropertyValues != null) queryParams.Add("criteria.propertyValues", Configuration.ApiClient.ParameterToString(criteriaPropertyValues)); // query parameter
            if (criteriaCurrency != null) queryParams.Add("criteria.currency", Configuration.ApiClient.ParameterToString(criteriaCurrency)); // query parameter
            if (criteriaStartPrice != null) queryParams.Add("criteria.startPrice", Configuration.ApiClient.ParameterToString(criteriaStartPrice)); // query parameter
            if (criteriaEndPrice != null) queryParams.Add("criteria.endPrice", Configuration.ApiClient.ParameterToString(criteriaEndPrice)); // query parameter
            if (criteriaSkip != null) queryParams.Add("criteria.skip", Configuration.ApiClient.ParameterToString(criteriaSkip)); // query parameter
            if (criteriaTake != null) queryParams.Add("criteria.take", Configuration.ApiClient.ParameterToString(criteriaTake)); // query parameter
            if (criteriaIndexDate != null) queryParams.Add("criteria.indexDate", Configuration.ApiClient.ParameterToString(criteriaIndexDate)); // query parameter
            if (criteriaPricelistId != null) queryParams.Add("criteria.pricelistId", Configuration.ApiClient.ParameterToString(criteriaPricelistId)); // query parameter
            if (criteriaPricelistIds != null) queryParams.Add("criteria.pricelistIds", Configuration.ApiClient.ParameterToString(criteriaPricelistIds)); // query parameter
            if (criteriaTerms != null) queryParams.Add("criteria.terms", Configuration.ApiClient.ParameterToString(criteriaTerms)); // query parameter
            if (criteriaFacets != null) queryParams.Add("criteria.facets", Configuration.ApiClient.ParameterToString(criteriaFacets)); // query parameter
            if (criteriaOutline != null) queryParams.Add("criteria.outline", Configuration.ApiClient.ParameterToString(criteriaOutline)); // query parameter
            if (criteriaWithHidden != null) queryParams.Add("criteria.withHidden", Configuration.ApiClient.ParameterToString(criteriaWithHidden)); // query parameter
            if (criteriaStartDateFrom != null) queryParams.Add("criteria.startDateFrom", Configuration.ApiClient.ParameterToString(criteriaStartDateFrom)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
        
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <returns>List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public List<VirtoCommerceSearchModuleWebModelFilterProperty> SearchModuleGetFilterProperties (string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> response = SearchModuleGetFilterPropertiesWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public ApiResponse< List<VirtoCommerceSearchModuleWebModelFilterProperty> > SearchModuleGetFilterPropertiesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleGetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                "application/json", "text/json"
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }
    
        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceSearchModuleWebModelFilterProperty>> SearchModuleGetFilterPropertiesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>> response = await SearchModuleGetFilterPropertiesAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get filter properties for store Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceSearchModuleWebModelFilterProperty&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>> SearchModuleGetFilterPropertiesAsyncWithHttpInfo (string storeId)
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleGetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceSearchModuleWebModelFilterProperty>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceSearchModuleWebModelFilterProperty>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceSearchModuleWebModelFilterProperty>)));
            
        }
        
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <param name="filterProperties"></param> 
        /// <returns></returns>
        public void SearchModuleSetFilterProperties (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             SearchModuleSetFilterPropertiesWithHttpInfo(storeId, filterProperties);
        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param> 
        /// <param name="filterProperties"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> SearchModuleSetFilterPropertiesWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SearchModuleSetFilterProperties");
            
            // verify the required parameter 'filterProperties' is set
            if (filterProperties == null) throw new ApiException(400, "Missing required parameter 'filterProperties' when calling SearchModuleSetFilterProperties");
            
    
            var path_ = "/api/search/storefilterproperties/{storeId}";
    
            var pathParams = new Dictionary<String, String>();
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;

            // to determine the Accept header
            String[] http_header_accepts = new String[] {
                
            };
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(filterProperties); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task SearchModuleSetFilterPropertiesAsync (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
        {
             await SearchModuleSetFilterPropertiesAsyncWithHttpInfo(storeId, filterProperties);

        }

        /// <summary>
        /// Set filter properties for store 
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> SearchModuleSetFilterPropertiesAsyncWithHttpInfo (string storeId, List<VirtoCommerceSearchModuleWebModelFilterProperty> filterProperties)
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
            String http_header_accept = Configuration.ApiClient.SelectHeaderAccept(http_header_accepts);
            if (http_header_accept != null)
                headerParams.Add("Accept", Configuration.ApiClient.SelectHeaderAccept(http_header_accepts));

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(filterProperties); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling SearchModuleSetFilterProperties: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
    }
    
}
