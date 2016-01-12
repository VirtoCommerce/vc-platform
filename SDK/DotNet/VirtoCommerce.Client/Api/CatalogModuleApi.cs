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
    public interface ICatalogModuleApi
    {
        
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ();
  
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsWithHttpInfo ();

        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ();

        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>> CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo ();
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCatalogsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsCreateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogWithHttpInfo ();

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ();

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo ();
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo ();

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ();

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo ();
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetWithHttpInfo (string id);

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id);

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCatalogsDeleteWithHttpInfo (string id);

        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id);

        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsDeleteAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category);

        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="category">The category.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category);

        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="category">The category.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCategoriesDeleteWithHttpInfo (List<string> ids);

        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesDeleteAsyncWithHttpInfo (List<string> ids);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo (string categoryId);

        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId);

        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo (string categoryId);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetWithHttpInfo (string id);

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id);

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
  
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);

        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);

        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>> CatalogModuleExportImportDoExportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
        
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
  
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);

        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);

        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>> CatalogModuleExportImportDoImportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
        
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter = null);
  
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationWithHttpInfo (string fileUrl, string delimiter = null);

        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter = null);

        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>> CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo (string fileUrl, string delimiter = null);
        
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
        VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null);
  
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchWithHttpInfo (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null);

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
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null);

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
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelListEntrySearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>> CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryMoveWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);

        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);

        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryMoveAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryCreateLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryCreateLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryDeleteLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleProductsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product);

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="product">The product.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product);

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="product">The product.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleProductsDeleteWithHttpInfo (List<string> ids);

        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsDeleteAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetProductById (string id, string respGroup = null);
  
        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdWithHttpInfo (string id, string respGroup = null);

        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdAsync (string id, string respGroup = null);

        /// <summary>
        /// Gets item by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdAsyncWithHttpInfo (string id, string respGroup = null);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationWithHttpInfo (string productId);

        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId);

        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewVariationAsyncWithHttpInfo (string productId);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property);

        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="property">The property.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property);

        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <param name="property">The property.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property);
        
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
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModulePropertiesDeleteWithHttpInfo (string id);

        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">The property id.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id);

        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">The property id.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesDeleteAsyncWithHttpInfo (string id);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetWithHttpInfo (string propertyId);

        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId);

        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetAsyncWithHttpInfo (string propertyId);
        
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword = null);
  
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesWithHttpInfo (string propertyId, string keyword = null);

        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword = null);

        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>> CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo (string propertyId, string keyword = null);
        
        /// <summary>
        /// Searches for the items by complex criteria
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
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult CatalogModuleSearchSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);
  
        /// <summary>
        /// Searches for the items by complex criteria
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
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);

        /// <summary>
        /// Searches for the items by complex criteria
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
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);

        /// <summary>
        /// Searches for the items by complex criteria
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
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> CatalogModuleSearchSearchAsyncWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null);
        
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId = null);
  
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryWithHttpInfo (string catalogId, string parentCategoryId = null);

        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId = null);

        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo (string catalogId, string parentCategoryId = null);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo (string catalogId, string categoryId);

        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsync (string catalogId, string categoryId);

        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo (string catalogId, string categoryId);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogWithHttpInfo (string catalogId);

        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId);

        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo (string catalogId);
        
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo (string catalogId);

        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId);

        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo (string catalogId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CatalogModuleApi : ICatalogModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CatalogModuleApi(Configuration configuration)
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
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ()
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> response = CatalogModuleCatalogsGetCatalogsWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelCatalog> > CatalogModuleCatalogsGetCatalogsWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCatalog>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>)));
            
        }
    
        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ()
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> response = await CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>> CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCatalog>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>)));
            
        }
        
        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param> 
        /// <returns></returns>
        public void CatalogModuleCatalogsUpdate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             CatalogModuleCatalogsUpdateWithHttpInfo(catalog);
        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCatalogsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsUpdate");
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             await CatalogModuleCatalogsUpdateAsyncWithHttpInfo(catalog);

        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsUpdate");
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsCreate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = CatalogModuleCatalogsCreateWithHttpInfo(catalog);
             return response.Data;
        }

        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsCreateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsCreate");
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsCreate: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
    
        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = await CatalogModuleCatalogsCreateAsyncWithHttpInfo(catalog);
             return response.Data;

        }

        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsCreateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null) throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleCatalogsCreate");
            
    
            var path_ = "/api/catalog/catalogs";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsCreate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsCreate: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
        
        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewCatalog ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = CatalogModuleCatalogsGetNewCatalogWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetNewCatalogWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs/getnew";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
    
        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = await CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs/getnew";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
        
        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewVirtualCatalog ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs/getnewvirtual";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
    
        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = await CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/catalog/catalogs/getnewvirtual";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
        
        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGet (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = CatalogModuleCatalogsGetWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsGet");
            
    
            var path_ = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
    
        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> response = await CatalogModuleCatalogsGetAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsGet");
            
    
            var path_ = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsGet: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }
        
        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param> 
        /// <returns></returns>
        public void CatalogModuleCatalogsDelete (string id)
        {
             CatalogModuleCatalogsDeleteWithHttpInfo(id);
        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCatalogsDeleteWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsDelete");
            
    
            var path_ = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id)
        {
             await CatalogModuleCatalogsDeleteAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsDeleteAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCatalogsDelete");
            
    
            var path_ = "/api/catalog/catalogs/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCatalogsDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param> 
        /// <returns></returns>
        public void CatalogModuleCategoriesCreateOrUpdateCategory (VirtoCommerceCatalogModuleWebModelCategory category)
        {
             CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo(category);
        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleCategoriesCreateOrUpdateCategory");
            
    
            var path_ = "/api/catalog/categories";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(category); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category)
        {
             await CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo(category);

        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            // verify the required parameter 'category' is set
            if (category == null) throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleCategoriesCreateOrUpdateCategory");
            
    
            var path_ = "/api/catalog/categories";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(category); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param> 
        /// <returns></returns>
        public void CatalogModuleCategoriesDelete (List<string> ids)
        {
             CatalogModuleCategoriesDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCategoriesDeleteWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleCategoriesDelete");
            
    
            var path_ = "/api/catalog/categories";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids)
        {
             await CatalogModuleCategoriesDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleCategoriesDelete");
            
    
            var path_ = "/api/catalog/categories";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCategoryProperty (string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo(categoryId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo (string categoryId)
        {
            
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModulePropertiesGetNewCategoryProperty");
            
    
            var path_ = "/api/catalog/categories/{categoryId}/properties/getnew";
    
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
            if (categoryId != null) pathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
    
        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = await CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo(categoryId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo (string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModulePropertiesGetNewCategoryProperty");
            
    
            var path_ = "/api/catalog/categories/{categoryId}/properties/getnew";
    
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
            if (categoryId != null) pathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
        
        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGet (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> response = CatalogModuleCategoriesGetWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCategory > CatalogModuleCategoriesGetWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCategoriesGet");
            
    
            var path_ = "/api/catalog/categories/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }
    
        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> response = await CatalogModuleCategoriesGetAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleCategoriesGet");
            
    
            var path_ = "/api/catalog/categories/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGet: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }
        
        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> response = CatalogModuleExportImportDoExportWithHttpInfo(exportInfo);
             return response.Data;
        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification > CatalogModuleExportImportDoExportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null) throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleExportImportDoExport");
            
    
            var path_ = "/api/catalog/export";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(exportInfo); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoExport: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoExport: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)));
            
        }
    
        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> response = await CatalogModuleExportImportDoExportAsyncWithHttpInfo(exportInfo);
             return response.Data;

        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>> CatalogModuleExportImportDoExportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null) throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleExportImportDoExport");
            
    
            var path_ = "/api/catalog/export";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(exportInfo); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoExport: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoExport: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)));
            
        }
        
        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> response = CatalogModuleExportImportDoImportWithHttpInfo(importInfo);
             return response.Data;
        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification > CatalogModuleExportImportDoImportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            
            // verify the required parameter 'importInfo' is set
            if (importInfo == null) throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleExportImportDoImport");
            
    
            var path_ = "/api/catalog/import";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(importInfo); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoImport: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoImport: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)));
            
        }
    
        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> response = await CatalogModuleExportImportDoImportAsyncWithHttpInfo(importInfo);
             return response.Data;

        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>> CatalogModuleExportImportDoImportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            // verify the required parameter 'importInfo' is set
            if (importInfo == null) throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleExportImportDoImport");
            
    
            var path_ = "/api/catalog/import";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(importInfo); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoImport: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportDoImport: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)));
            
        }
        
        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param> 
        /// <param name="delimiter">The CSV delimiter.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> response = CatalogModuleExportImportGetMappingConfigurationWithHttpInfo(fileUrl, delimiter);
             return response.Data;
        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param> 
        /// <param name="delimiter">The CSV delimiter.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration > CatalogModuleExportImportGetMappingConfigurationWithHttpInfo (string fileUrl, string delimiter = null)
        {
            
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null) throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleExportImportGetMappingConfiguration");
            
    
            var path_ = "/api/catalog/import/mappingconfiguration";
    
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
            
            if (fileUrl != null) queryParams.Add("fileUrl", Configuration.ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) queryParams.Add("delimiter", Configuration.ApiClient.ParameterToString(delimiter)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)));
            
        }
    
        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> response = await CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo(fileUrl, delimiter);
             return response.Data;

        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>> CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo (string fileUrl, string delimiter = null)
        {
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null) throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleExportImportGetMappingConfiguration");
            
    
            var path_ = "/api/catalog/import/mappingconfiguration";
    
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
            
            if (fileUrl != null) queryParams.Add("fileUrl", Configuration.ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) queryParams.Add("delimiter", Configuration.ApiClient.ParameterToString(delimiter)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)));
            
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
        public VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> response = CatalogModuleListEntryListItemsSearchWithHttpInfo(criteriaResponseGroup, criteriaKeyword, criteriaCategoryId, criteriaCatalogId, criteriaStart, criteriaCount);
             return response.Data;
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
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelListEntrySearchResult > CatalogModuleListEntryListItemsSearchWithHttpInfo (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null)
        {
            
    
            var path_ = "/api/catalog/listentries";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", Configuration.ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", Configuration.ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", Configuration.ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", Configuration.ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", Configuration.ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult)));
            
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
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> response = await CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo(criteriaResponseGroup, criteriaKeyword, criteriaCategoryId, criteriaCatalogId, criteriaStart, criteriaCount);
             return response.Data;

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
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelListEntrySearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>> CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo (string criteriaResponseGroup = null, string criteriaKeyword = null, string criteriaCategoryId = null, string criteriaCatalogId = null, int? criteriaStart = null, int? criteriaCount = null)
        {
            
    
            var path_ = "/api/catalog/listentries";
    
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
            
            if (criteriaResponseGroup != null) queryParams.Add("criteria.responseGroup", Configuration.ApiClient.ParameterToString(criteriaResponseGroup)); // query parameter
            if (criteriaKeyword != null) queryParams.Add("criteria.keyword", Configuration.ApiClient.ParameterToString(criteriaKeyword)); // query parameter
            if (criteriaCategoryId != null) queryParams.Add("criteria.categoryId", Configuration.ApiClient.ParameterToString(criteriaCategoryId)); // query parameter
            if (criteriaCatalogId != null) queryParams.Add("criteria.catalogId", Configuration.ApiClient.ParameterToString(criteriaCatalogId)); // query parameter
            if (criteriaStart != null) queryParams.Add("criteria.start", Configuration.ApiClient.ParameterToString(criteriaStart)); // query parameter
            if (criteriaCount != null) queryParams.Add("criteria.count", Configuration.ApiClient.ParameterToString(criteriaCount)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult)));
            
        }
        
        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param> 
        /// <returns></returns>
        public void CatalogModuleListEntryMove (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
             CatalogModuleListEntryMoveWithHttpInfo(moveInfo);
        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryMoveWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null) throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleListEntryMove");
            
    
            var path_ = "/api/catalog/listentries/move";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(moveInfo); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryMove: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryMove: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
             await CatalogModuleListEntryMoveAsyncWithHttpInfo(moveInfo);

        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryMoveAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null) throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleListEntryMove");
            
    
            var path_ = "/api/catalog/listentries/move";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(moveInfo); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryMove: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryMove: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns></returns>
        public void CatalogModuleListEntryCreateLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             CatalogModuleListEntryCreateLinksWithHttpInfo(links);
        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryCreateLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryCreateLinks");
            
    
            var path_ = "/api/catalog/listentrylinks";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             await CatalogModuleListEntryCreateLinksAsyncWithHttpInfo(links);

        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryCreateLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryCreateLinks");
            
    
            var path_ = "/api/catalog/listentrylinks";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryCreateLinks: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns></returns>
        public void CatalogModuleListEntryDeleteLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             CatalogModuleListEntryDeleteLinksWithHttpInfo(links);
        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryDeleteLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryDeleteLinks");
            
    
            var path_ = "/api/catalog/listentrylinks/delete";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             await CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo(links);

        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null) throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleListEntryDeleteLinks");
            
    
            var path_ = "/api/catalog/listentrylinks/delete";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param> 
        /// <returns></returns>
        public void CatalogModuleProductsUpdate (VirtoCommerceCatalogModuleWebModelProduct product)
        {
             CatalogModuleProductsUpdateWithHttpInfo(product);
        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleProductsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleProductsUpdate");
            
    
            var path_ = "/api/catalog/products";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(product); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product)
        {
             await CatalogModuleProductsUpdateAsyncWithHttpInfo(product);

        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            // verify the required parameter 'product' is set
            if (product == null) throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleProductsUpdate");
            
    
            var path_ = "/api/catalog/products";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(product); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param> 
        /// <returns></returns>
        public void CatalogModuleProductsDelete (List<string> ids)
        {
             CatalogModuleProductsDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleProductsDeleteWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleProductsDelete");
            
    
            var path_ = "/api/catalog/products";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids)
        {
             await CatalogModuleProductsDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleProductsDelete");
            
    
            var path_ = "/api/catalog/products";
    
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
            
            if (ids != null) queryParams.Add("ids", Configuration.ApiClient.ParameterToString(ids)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param> 
        /// <param name="respGroup">Response group.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetProductById (string id, string respGroup = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = CatalogModuleProductsGetProductByIdWithHttpInfo(id, respGroup);
             return response.Data;
        }

        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param> 
        /// <param name="respGroup">Response group.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetProductByIdWithHttpInfo (string id, string respGroup = null)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleProductsGetProductById");
            
    
            var path_ = "/api/catalog/products/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            if (respGroup != null) queryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetProductById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetProductById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
    
        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdAsync (string id, string respGroup = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = await CatalogModuleProductsGetProductByIdAsyncWithHttpInfo(id, respGroup);
             return response.Data;

        }

        /// <summary>
        /// Gets item by id. 
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdAsyncWithHttpInfo (string id, string respGroup = null)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleProductsGetProductById");
            
    
            var path_ = "/api/catalog/products/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            if (respGroup != null) queryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetProductById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetProductById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
        
        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewVariation (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = CatalogModuleProductsGetNewVariationWithHttpInfo(productId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewVariationWithHttpInfo (string productId)
        {
            
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleProductsGetNewVariation");
            
    
            var path_ = "/api/catalog/products/{productId}/getnewvariation";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
    
        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = await CatalogModuleProductsGetNewVariationAsyncWithHttpInfo(productId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewVariationAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null) throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleProductsGetNewVariation");
            
    
            var path_ = "/api/catalog/products/{productId}/getnewvariation";
    
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
            if (productId != null) pathParams.Add("productId", Configuration.ApiClient.ParameterToString(productId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewVariation: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
        
        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param> 
        /// <returns></returns>
        public void CatalogModulePropertiesCreateOrUpdateProperty (VirtoCommerceCatalogModuleWebModelProperty property)
        {
             CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo(property);
        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModulePropertiesCreateOrUpdateProperty");
            
    
            var path_ = "/api/catalog/properties";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property)
        {
             await CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo(property);

        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew == True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            // verify the required parameter 'property' is set
            if (property == null) throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModulePropertiesCreateOrUpdateProperty");
            
    
            var path_ = "/api/catalog/properties";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param> 
        /// <returns></returns>
        public void CatalogModulePropertiesDelete (string id)
        {
             CatalogModulePropertiesDeleteWithHttpInfo(id);
        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModulePropertiesDeleteWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModulePropertiesDelete");
            
    
            var path_ = "/api/catalog/properties";
    
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
            
            if (id != null) queryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id)
        {
             await CatalogModulePropertiesDeleteAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesDeleteAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModulePropertiesDelete");
            
    
            var path_ = "/api/catalog/properties";
    
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
            
            if (id != null) queryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGet (string propertyId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = CatalogModulePropertiesGetWithHttpInfo(propertyId);
             return response.Data;
        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetWithHttpInfo (string propertyId)
        {
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGet");
            
    
            var path_ = "/api/catalog/properties/{propertyId}";
    
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
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGet: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
    
        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = await CatalogModulePropertiesGetAsyncWithHttpInfo(propertyId);
             return response.Data;

        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetAsyncWithHttpInfo (string propertyId)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGet");
            
    
            var path_ = "/api/catalog/properties/{propertyId}";
    
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
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGet: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGet: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
        
        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <param name="keyword">The keyword. (Optional)</param> 
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> response = CatalogModulePropertiesGetPropertyValuesWithHttpInfo(propertyId, keyword);
             return response.Data;
        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param> 
        /// <param name="keyword">The keyword. (Optional)</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelPropertyValue> > CatalogModulePropertiesGetPropertyValuesWithHttpInfo (string propertyId, string keyword = null)
        {
            
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGetPropertyValues");
            
    
            var path_ = "/api/catalog/properties/{propertyId}/values";
    
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
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (keyword != null) queryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>)));
            
        }
    
        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> response = await CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo(propertyId, keyword);
             return response.Data;

        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>> CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo (string propertyId, string keyword = null)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null) throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModulePropertiesGetPropertyValues");
            
    
            var path_ = "/api/catalog/properties/{propertyId}/values";
    
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
            if (propertyId != null) pathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            
            if (keyword != null) queryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>)));
            
        }
        
        /// <summary>
        /// Searches for the items by complex criteria 
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
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult CatalogModuleSearchSearch (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = CatalogModuleSearchSearchWithHttpInfo(criteriaStoreId, criteriaResponseGroup, criteriaKeyword, criteriaSearchInChildren, criteriaCategoryId, criteriaCategoryIds, criteriaCatalogId, criteriaCatalogIds, criteriaLanguageCode, criteriaCode, criteriaSort, criteriaSortOrder, criteriaHideDirectLinkedCategories, criteriaPropertyValues, criteriaCurrency, criteriaStartPrice, criteriaEndPrice, criteriaSkip, criteriaTake, criteriaIndexDate, criteriaPricelistId, criteriaPricelistIds, criteriaTerms, criteriaFacets, criteriaOutline, criteriaWithHidden, criteriaStartDateFrom);
             return response.Data;
        }

        /// <summary>
        /// Searches for the items by complex criteria 
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
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalogSearchResult > CatalogModuleSearchSearchWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
            
    
            var path_ = "/api/catalog/search";
    
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
                throw new ApiException (statusCode, "Error calling CatalogModuleSearchSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleSearchSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
    
        /// <summary>
        /// Searches for the items by complex criteria 
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
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchAsync (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> response = await CatalogModuleSearchSearchAsyncWithHttpInfo(criteriaStoreId, criteriaResponseGroup, criteriaKeyword, criteriaSearchInChildren, criteriaCategoryId, criteriaCategoryIds, criteriaCatalogId, criteriaCatalogIds, criteriaLanguageCode, criteriaCode, criteriaSort, criteriaSortOrder, criteriaHideDirectLinkedCategories, criteriaPropertyValues, criteriaCurrency, criteriaStartPrice, criteriaEndPrice, criteriaSkip, criteriaTake, criteriaIndexDate, criteriaPricelistId, criteriaPricelistIds, criteriaTerms, criteriaFacets, criteriaOutline, criteriaWithHidden, criteriaStartDateFrom);
             return response.Data;

        }

        /// <summary>
        /// Searches for the items by complex criteria 
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
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> CatalogModuleSearchSearchAsyncWithHttpInfo (string criteriaStoreId = null, string criteriaResponseGroup = null, string criteriaKeyword = null, bool? criteriaSearchInChildren = null, string criteriaCategoryId = null, List<string> criteriaCategoryIds = null, string criteriaCatalogId = null, List<string> criteriaCatalogIds = null, string criteriaLanguageCode = null, string criteriaCode = null, string criteriaSort = null, string criteriaSortOrder = null, bool? criteriaHideDirectLinkedCategories = null, List<string> criteriaPropertyValues = null, string criteriaCurrency = null, double? criteriaStartPrice = null, double? criteriaEndPrice = null, int? criteriaSkip = null, int? criteriaTake = null, DateTime? criteriaIndexDate = null, string criteriaPricelistId = null, List<string> criteriaPricelistIds = null, List<string> criteriaTerms = null, List<string> criteriaFacets = null, string criteriaOutline = null, bool? criteriaWithHidden = null, DateTime? criteriaStartDateFrom = null)
        {
            
    
            var path_ = "/api/catalog/search";
    
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
                throw new ApiException (statusCode, "Error calling CatalogModuleSearchSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleSearchSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }
        
        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="parentCategoryId">The parent category id. (Optional)</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> response = CatalogModuleCategoriesGetNewCategoryWithHttpInfo(catalogId, parentCategoryId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="parentCategoryId">The parent category id. (Optional)</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCategory > CatalogModuleCategoriesGetNewCategoryWithHttpInfo (string catalogId, string parentCategoryId = null)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleCategoriesGetNewCategory");
            
    
            var path_ = "/api/catalog/{catalogId}/categories/newcategory";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            if (parentCategoryId != null) queryParams.Add("parentCategoryId", Configuration.ApiClient.ParameterToString(parentCategoryId)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }
    
        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> response = await CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo(catalogId, parentCategoryId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo (string catalogId, string parentCategoryId = null)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleCategoriesGetNewCategory");
            
    
            var path_ = "/api/catalog/{catalogId}/categories/newcategory";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            if (parentCategoryId != null) queryParams.Add("parentCategoryId", Configuration.ApiClient.ParameterToString(parentCategoryId)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }
        
        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="categoryId">The category id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalogAndCategory (string catalogId, string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo(catalogId, categoryId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <param name="categoryId">The category id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo (string catalogId, string categoryId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
    
            var path_ = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) pathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
    
        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsync (string catalogId, string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = await CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo(catalogId, categoryId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo (string catalogId, string categoryId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            // verify the required parameter 'categoryId' is set
            if (categoryId == null) throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleProductsGetNewProductByCatalogAndCategory");
            
    
            var path_ = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) pathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
        
        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalog (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = CatalogModuleProductsGetNewProductByCatalogWithHttpInfo(catalogId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewProductByCatalogWithHttpInfo (string catalogId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalog");
            
    
            var path_ = "/api/catalog/{catalogId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
    
        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> response = await CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo(catalogId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleProductsGetNewProductByCatalog");
            
    
            var path_ = "/api/catalog/{catalogId}/products/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }
        
        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCatalogProperty (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo(catalogId);
             return response.Data;
        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param> 
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo (string catalogId)
        {
            
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModulePropertiesGetNewCatalogProperty");
            
    
            var path_ = "/api/catalog/{catalogId}/properties/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
    
        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> response = await CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo(catalogId);
             return response.Data;

        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null) throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModulePropertiesGetNewCatalogProperty");
            
    
            var path_ = "/api/catalog/{catalogId}/properties/getnew";
    
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
            if (catalogId != null) pathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }
        
    }
    
}
