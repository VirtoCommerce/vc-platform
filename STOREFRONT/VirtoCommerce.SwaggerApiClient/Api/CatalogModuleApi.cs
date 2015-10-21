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
    public interface ICatalogModuleApi
    {
        
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns></returns>
        List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ();
  
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ();
        
        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        void CatalogModuleCatalogsUpdate (VirtoCommerceCatalogModuleWebModelCatalog catalog);
  
        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        
        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsCreate (VirtoCommerceCatalogModuleWebModelCatalog catalog);
  
        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        
        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewCatalog ();
  
        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ();
        
        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewVirtualCatalog ();
  
        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ();
        
        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <param name="id">The Catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGet (string id);
  
        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <param name="id">The Catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id);
        
        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        void CatalogModuleCatalogsDelete (string id);
  
        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id);
        
        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        void CatalogModuleCategoriesCreateOrUpdateCategory (VirtoCommerceCatalogModuleWebModelCategory category);
  
        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category);
        
        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The categories ids.</param>
        /// <returns></returns>
        void CatalogModuleCategoriesDelete (List<string> ids);
  
        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The categories ids.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids);
        
        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCategoryProperty (string categoryId);
  
        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId);
        
        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGet (string id);
  
        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id);
        
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification</returns>
        VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
  
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
        
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification</returns>
        VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
  
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
        
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter);
  
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter);
        
        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaResponseGroup">Gets or sets the response group to define which types of entries to search for.</param>
        /// <param name="criteriaKeyword">Gets or sets the keyword to search for.</param>
        /// <param name="criteriaCategoryId">Gets or sets the category identifier.</param>
        /// <param name="criteriaCatalogId">Gets or sets the catalog identifier.</param>
        /// <param name="criteriaStart">Gets or sets the start index of total results from which entries should be returned.</param>
        /// <param name="criteriaCount">Gets or sets the maximum count of results to return.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (string criteriaResponseGroup, string criteriaKeyword, string criteriaCategoryId, string criteriaCatalogId, int? criteriaStart, int? criteriaCount);
  
        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="criteriaResponseGroup">Gets or sets the response group to define which types of entries to search for.</param>
        /// <param name="criteriaKeyword">Gets or sets the keyword to search for.</param>
        /// <param name="criteriaCategoryId">Gets or sets the category identifier.</param>
        /// <param name="criteriaCatalogId">Gets or sets the catalog identifier.</param>
        /// <param name="criteriaStart">Gets or sets the start index of total results from which entries should be returned.</param>
        /// <param name="criteriaCount">Gets or sets the maximum count of results to return.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (string criteriaResponseGroup, string criteriaKeyword, string criteriaCategoryId, string criteriaCatalogId, int? criteriaStart, int? criteriaCount);
        
        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns></returns>
        void CatalogModuleListEntryMove (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);
  
        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);
        
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        void CatalogModuleListEntryCreateLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
  
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        void CatalogModuleListEntryDeleteLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
  
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        
        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        void CatalogModuleProductsUpdate (VirtoCommerceCatalogModuleWebModelProduct product);
  
        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product);
        
        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The items ids.</param>
        /// <returns></returns>
        void CatalogModuleProductsDelete (List<string> ids);
  
        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The items ids.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids);
        
        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGet (string id);
  
        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetAsync (string id);
        
        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">The parent product id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewVariation (string productId);
  
        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">The parent product id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId);
        
        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        void CatalogModulePropertiesCreateOrUpdateProperty (VirtoCommerceCatalogModuleWebModelProperty property);
  
        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property);
        
        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
        void CatalogModulePropertiesDelete (string id);
  
        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
        System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id);
        
        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGet (string propertyId);
  
        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId);
        
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns></returns>
        List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword);
  
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword);
        
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId);
  
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId);
        
        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalogAndCategory (string catalogId, string categoryId);
  
        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsync (string catalogId, string categoryId);
        
        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalog (string catalogId);
  
        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId);
        
        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCatalogProperty (string catalogId);
  
        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CatalogModuleApi : ICatalogModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CatalogModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CatalogModuleApi(String basePath)
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
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns></returns>            
        public List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ()
        {
            
    
            var path = "/api/catalog/catalogs";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCatalogModuleWebModelCatalog>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>), response.Headers);
        }
    
        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ()
        {
            
    
            var path = "/api/catalog/catalogs";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.Content, response.Content);

            return (List<VirtoCommerceCatalogModuleWebModelCatalog>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>), response.Headers);
        }
        
        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param> 
        /// <returns></returns>            
        public void CatalogModuleCatalogsUpdate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsUpdate");
            
    
            var path = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(catalog); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsUpdate");
            
    
            var path = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(catalog); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>            
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsCreate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsCreate");
            
    
            var path = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(catalog); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsCreate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
    
        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsCreate");
            
    
            var path = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(catalog); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsCreate: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>            
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewCatalog ()
        {
            
    
            var path = "/api/catalog/catalogs/getnew";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ()
        {
            
    
            var path = "/api/catalog/catalogs/getnew";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>            
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewVirtualCatalog ()
        {
            
    
            var path = "/api/catalog/catalogs/getnewvirtual";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ()
        {
            
    
            var path = "/api/catalog/catalogs/getnewvirtual";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
        
        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>            
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGet (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsGet");
            
    
            var path = "/api/catalog/catalogs/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
    
        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsGet");
            
    
            var path = "/api/catalog/catalogs/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsGet: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCatalog) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCatalog), response.Headers);
        }
        
        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param> 
        /// <returns></returns>            
        public void CatalogModuleCatalogsDelete (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsDelete");
            
    
            var path = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsDelete");
            
    
            var path = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCatalogsDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param> 
        /// <returns></returns>            
        public void CatalogModuleCategoriesCreateOrUpdateCategory (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleCategoriesCreateOrUpdateCategory");
            
    
            var path = "/api/catalog/categories";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(category); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleCategoriesCreateOrUpdateCategory");
            
    
            var path = "/api/catalog/categories";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(category); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param> 
        /// <returns></returns>            
        public void CatalogModuleCategoriesDelete (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleCategoriesDelete");
            
    
            var path = "/api/catalog/categories";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleCategoriesDelete");
            
    
            var path = "/api/catalog/categories";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>            
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCategoryProperty (string categoryId)
        {
            
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModulePropertiesGetNewCategoryProperty");
            
    
            var path = "/api/catalog/categories/{categoryId}/properties/getnew";
    
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
            if (categoryId != null) pathParams.Add("categoryId", ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModulePropertiesGetNewCategoryProperty");
            
    
            var path = "/api/catalog/categories/{categoryId}/properties/getnew";
    
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
            if (categoryId != null) pathParams.Add("categoryId", ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
        
        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>            
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGet (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCategoriesGet");
            
    
            var path = "/api/catalog/categories/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCategory) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCategory), response.Headers);
        }
    
        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCategoriesGet");
            
    
            var path = "/api/catalog/categories/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGet: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCategory) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCategory), response.Headers);
        }
        
        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification</returns>            
        public VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null) throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleExportImportDoExport");
            
    
            var path = "/api/catalog/export";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(exportInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoExport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoExport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification), response.Headers);
        }
    
        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null) throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleExportImportDoExport");
            
    
            var path = "/api/catalog/export";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(exportInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoExport: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelEventNotificationsExportNotification), response.Headers);
        }
        
        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification</returns>            
        public VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            
            // verify the required parameter 'importInfo' is set
            if (importInfo == null) throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleExportImportDoImport");
            
    
            var path = "/api/catalog/import";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(importInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoImport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoImport: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification), response.Headers);
        }
    
        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            // verify the required parameter 'importInfo' is set
            if (importInfo == null) throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleExportImportDoImport");
            
    
            var path = "/api/catalog/import";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(importInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportDoImport: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelEventNotificationsImportNotification), response.Headers);
        }
        
        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param> 
        /// <param name="delimiter">The CSV delimiter.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>            
        public VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter)
        {
            
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null) throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleExportImportGetMappingConfiguration");
            
    
            var path = "/api/catalog/import/mappingconfiguration";
    
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
            
            if (fileUrl != null) queryParams.Add("fileUrl", ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) queryParams.Add("delimiter", ApiClient.ParameterToString(delimiter)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration), response.Headers);
        }
    
        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter)
        {
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null) throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleExportImportGetMappingConfiguration");
            
    
            var path = "/api/catalog/import/mappingconfiguration";
    
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
            
            if (fileUrl != null) queryParams.Add("fileUrl", ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) queryParams.Add("delimiter", ApiClient.ParameterToString(delimiter)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration), response.Headers);
        }
        
        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <param name="criteriaResponseGroup">Gets or sets the response group to define which types of entries to search for.</param> 
        /// <param name="criteriaKeyword">Gets or sets the keyword to search for.</param> 
        /// <param name="criteriaCategoryId">Gets or sets the category identifier.</param> 
        /// <param name="criteriaCatalogId">Gets or sets the catalog identifier.</param> 
        /// <param name="criteriaStart">Gets or sets the start index of total results from which entries should be returned.</param> 
        /// <param name="criteriaCount">Gets or sets the maximum count of results to return.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>            
        public VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (string criteriaResponseGroup, string criteriaKeyword, string criteriaCategoryId, string criteriaCatalogId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/catalog/listentries";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult), response.Headers);
        }
    
        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <param name="criteriaResponseGroup">Gets or sets the response group to define which types of entries to search for.</param>
        /// <param name="criteriaKeyword">Gets or sets the keyword to search for.</param>
        /// <param name="criteriaCategoryId">Gets or sets the category identifier.</param>
        /// <param name="criteriaCatalogId">Gets or sets the catalog identifier.</param>
        /// <param name="criteriaStart">Gets or sets the start index of total results from which entries should be returned.</param>
        /// <param name="criteriaCount">Gets or sets the maximum count of results to return.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (string criteriaResponseGroup, string criteriaKeyword, string criteriaCategoryId, string criteriaCatalogId, int? criteriaStart, int? criteriaCount)
        {
            
    
            var path = "/api/catalog/listentries";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult), response.Headers);
        }
        
        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param> 
        /// <returns></returns>            
        public void CatalogModuleListEntryMove (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null) throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleListEntryMove");
            
    
            var path = "/api/catalog/listentries/move";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(moveInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryMove: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryMove: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null) throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleListEntryMove");
            
    
            var path = "/api/catalog/listentries/move";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(moveInfo); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryMove: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns></returns>            
        public void CatalogModuleListEntryCreateLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryCreateLinks");
            
    
            var path = "/api/catalog/listentrylinks";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(links); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryCreateLinks");
            
    
            var path = "/api/catalog/listentrylinks";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(links); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns></returns>            
        public void CatalogModuleListEntryDeleteLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryDeleteLinks");
            
    
            var path = "/api/catalog/listentrylinks/delete";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(links); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryDeleteLinks");
            
    
            var path = "/api/catalog/listentrylinks/delete";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(links); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param> 
        /// <returns></returns>            
        public void CatalogModuleProductsUpdate (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleProductsUpdate");
            
    
            var path = "/api/catalog/products";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(product); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleProductsUpdate");
            
    
            var path = "/api/catalog/products";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(product); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param> 
        /// <returns></returns>            
        public void CatalogModuleProductsDelete (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleProductsDelete");
            
    
            var path = "/api/catalog/products";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleProductsDelete");
            
    
            var path = "/api/catalog/products";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>            
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGet (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleProductsGet");
            
    
            var path = "/api/catalog/products/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
    
        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleProductsGet");
            
    
            var path = "/api/catalog/products/{id}";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGet: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>            
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewVariation (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleProductsGetNewVariation");
            
    
            var path = "/api/catalog/products/{productId}/getnewvariation";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleProductsGetNewVariation");
            
    
            var path = "/api/catalog/products/{productId}/getnewvariation";
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
        
        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param> 
        /// <returns></returns>            
        public void CatalogModulePropertiesCreateOrUpdateProperty (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModulePropertiesCreateOrUpdateProperty");
            
    
            var path = "/api/catalog/properties";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModulePropertiesCreateOrUpdateProperty");
            
    
            var path = "/api/catalog/properties";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(property); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param> 
        /// <returns></returns>            
        public void CatalogModulePropertiesDelete (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModulePropertiesDelete");
            
    
            var path = "/api/catalog/properties";
    
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
            
            if (id != null) queryParams.Add("id", ApiClient.ParameterToString(id)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModulePropertiesDelete");
            
    
            var path = "/api/catalog/properties";
    
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
            
            if (id != null) queryParams.Add("id", ApiClient.ParameterToString(id)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>            
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGet (string propertyId)
        {
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGet");
            
    
            var path = "/api/catalog/properties/{propertyId}";
    
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
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGet: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
    
        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGet");
            
    
            var path = "/api/catalog/properties/{propertyId}";
    
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
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGet: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
        
        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <param name="keyword">The keyword. (Optional)</param> 
        /// <returns></returns>            
        public List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword)
        {
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGetPropertyValues");
            
    
            var path = "/api/catalog/properties/{propertyId}/values";
    
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
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (keyword != null) queryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>), response.Headers);
        }
    
        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGetPropertyValues");
            
    
            var path = "/api/catalog/properties/{propertyId}/values";
    
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
            if (propertyId != null) pathParams.Add("propertyId", ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (keyword != null) queryParams.Add("keyword", ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.Content, response.Content);

            return (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="parentCategoryId">The parent category id. (Optional)</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>            
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleCategoriesGetNewCategory");
            
    
            var path = "/api/catalog/{catalogId}/categories/newcategory";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            if (parentCategoryId != null) queryParams.Add("parentCategoryId", ApiClient.ParameterToString(parentCategoryId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelCategory) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCategory), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleCategoriesGetNewCategory");
            
    
            var path = "/api/catalog/{catalogId}/categories/newcategory";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            if (parentCategoryId != null) queryParams.Add("parentCategoryId", ApiClient.ParameterToString(parentCategoryId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelCategory) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelCategory), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="categoryId">The category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>            
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalogAndCategory (string catalogId, string categoryId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
    
            var path = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) pathParams.Add("categoryId", ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsync (string catalogId, string categoryId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
    
            var path = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) pathParams.Add("categoryId", ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>            
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalog (string catalogId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalog");
            
    
            var path = "/api/catalog/{catalogId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalog");
            
    
            var path = "/api/catalog/{catalogId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProduct) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProduct), response.Headers);
        }
        
        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>            
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCatalogProperty (string catalogId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModulePropertiesGetNewCatalogProperty");
            
    
            var path = "/api/catalog/{catalogId}/properties/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
    
        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModulePropertiesGetNewCatalogProperty");
            
    
            var path = "/api/catalog/{catalogId}/properties/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.Content, response.Content);

            return (VirtoCommerceCatalogModuleWebModelProperty) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceCatalogModuleWebModelProperty), response.Headers);
        }
        
    }
    
}
