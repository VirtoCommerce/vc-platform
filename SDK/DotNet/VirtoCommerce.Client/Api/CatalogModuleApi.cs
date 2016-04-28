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
    public interface ICatalogModuleApi
    {
        #region Synchronous Operations
        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsCreate (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        void CatalogModuleCatalogsDelete (string id);

        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCatalogsDeleteWithHttpInfo (string id);
        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGet (string id);

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetWithHttpInfo (string id);
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ();

        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsWithHttpInfo ();
        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewCatalog ();

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogWithHttpInfo ();
        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewVirtualCatalog ();

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo ();
        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        void CatalogModuleCatalogsUpdate (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCatalogsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        void CatalogModuleCategoriesCreateOrUpdateCategory (VirtoCommerceCatalogModuleWebModelCategory category);

        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category);
        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns></returns>
        void CatalogModuleCategoriesDelete (List<string> ids);

        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleCategoriesDeleteWithHttpInfo (List<string> ids);
        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGet (string id);

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetWithHttpInfo (string id);
        /// <summary>
        /// Gets categories by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetCategoriesByIds (List<string> ids, string respGroup = null);

        /// <summary>
        /// Gets categories by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetCategoriesByIdsWithHttpInfo (List<string> ids, string respGroup = null);
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId = null);

        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryWithHttpInfo (string catalogId, string parentCategoryId = null);
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);

        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);

        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter = null);

        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationWithHttpInfo (string fileUrl, string delimiter = null);
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        void CatalogModuleListEntryCreateLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryCreateLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        void CatalogModuleListEntryDeleteLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryDeleteLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria);

        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria);
        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns></returns>
        void CatalogModuleListEntryMove (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);

        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleListEntryMoveWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsCloneProduct (string productId);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsCloneProductWithHttpInfo (string productId);
        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns></returns>
        void CatalogModuleProductsDelete (List<string> ids);

        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleProductsDeleteWithHttpInfo (List<string> ids);
        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalog (string catalogId);

        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogWithHttpInfo (string catalogId);
        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo (string catalogId, string categoryId);
        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewVariation (string productId);

        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationWithHttpInfo (string productId);
        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetProductById (string id, string respGroup = null);

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdWithHttpInfo (string id, string respGroup = null);
        /// <summary>
        /// Gets products by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIds (List<string> ids, string respGroup = null);

        /// <summary>
        /// Gets products by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdsWithHttpInfo (List<string> ids, string respGroup = null);
        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        void CatalogModuleProductsUpdate (VirtoCommerceCatalogModuleWebModelProduct product);

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModuleProductsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product);
        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        void CatalogModulePropertiesCreateOrUpdateProperty (VirtoCommerceCatalogModuleWebModelProperty property);

        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property);
        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
        void CatalogModulePropertiesDelete (string id);

        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> CatalogModulePropertiesDeleteWithHttpInfo (string id);
        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGet (string propertyId);

        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetWithHttpInfo (string propertyId);
        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCatalogProperty (string catalogId);

        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo (string catalogId);
        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCategoryProperty (string categoryId);

        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo (string categoryId);
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword = null);

        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesWithHttpInfo (string propertyId, string keyword = null);
        /// <summary>
        /// Searches for the items by complex criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        VirtoCommerceCatalogModuleWebModelCatalogSearchResult CatalogModuleSearchSearch (VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Searches for the items by complex criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Creates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Creates the specified catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsCreateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id);

        /// <summary>
        /// Deletes catalog by id.
        /// </summary>
        /// <remarks>
        /// Deletes catalog by id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsDeleteAsyncWithHttpInfo (string id);
        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id);

        /// <summary>
        /// Gets Catalog by id.
        /// </summary>
        /// <remarks>
        /// Gets Catalog by id with full information loaded
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ();

        /// <summary>
        /// Get Catalogs list
        /// </summary>
        /// <remarks>
        /// Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>> CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo ();
        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ();

        /// <summary>
        /// Gets the template for a new catalog.
        /// </summary>
        /// <remarks>
        /// Gets the template for a new common catalog
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo ();
        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ();

        /// <summary>
        /// Gets the template for a new virtual catalog.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo ();
        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog);

        /// <summary>
        /// Updates the specified catalog.
        /// </summary>
        /// <remarks>
        /// Updates the specified catalog.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog);
        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category);

        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>
        /// If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category);
        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesDeleteAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id);

        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetAsyncWithHttpInfo (string id);
        /// <summary>
        /// Gets categories by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetCategoriesByIdsAsync (List<string> ids, string respGroup = null);

        /// <summary>
        /// Gets categories by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>>> CatalogModuleCategoriesGetCategoriesByIdsAsyncWithHttpInfo (List<string> ids, string respGroup = null);
        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId = null);

        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo (string catalogId, string parentCategoryId = null);
        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);

        /// <summary>
        /// Start catalog data export process.
        /// </summary>
        /// <remarks>
        /// Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>> CatalogModuleExportImportDoExportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo);
        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);

        /// <summary>
        /// Start catalog data import process.
        /// </summary>
        /// <remarks>
        /// Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>> CatalogModuleExportImportDoImportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo);
        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter = null);

        /// <summary>
        /// Gets the CSV mapping configuration.
        /// </summary>
        /// <remarks>
        /// Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>> CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo (string fileUrl, string delimiter = null);
        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryCreateLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links);
        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria);

        /// <summary>
        /// Searches for the items by complex criteria.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelListEntrySearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>> CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria);
        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);

        /// <summary>
        /// Move categories or products to another location.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryMoveAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo);
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsCloneProductAsync (string productId);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsCloneProductAsyncWithHttpInfo (string productId);
        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids);

        /// <summary>
        /// Deletes the specified items by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsDeleteAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId);

        /// <summary>
        /// Gets the template for a new product (outside of category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog directly.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo (string catalogId);
        /// <summary>
        /// Gets the template for a new product (inside category).
        /// </summary>
        /// <remarks>
        /// Use when need to create item belonging to catalog category.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
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
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo (string catalogId, string categoryId);
        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId);

        /// <summary>
        /// Gets the template for a new variation.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewVariationAsyncWithHttpInfo (string productId);
        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdAsync (string id, string respGroup = null);

        /// <summary>
        /// Gets product by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdAsyncWithHttpInfo (string id, string respGroup = null);
        /// <summary>
        /// Gets products by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdsAsync (List<string> ids, string respGroup = null);

        /// <summary>
        /// Gets products by ids
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>>> CatalogModuleProductsGetProductByIdsAsyncWithHttpInfo (List<string> ids, string respGroup = null);
        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product);

        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product);
        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property);

        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>
        /// If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property);
        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id);

        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesDeleteAsyncWithHttpInfo (string id);
        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId);

        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetAsyncWithHttpInfo (string propertyId);
        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId);

        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo (string catalogId);
        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId);

        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo (string categoryId);
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword = null);

        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>> CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo (string propertyId, string keyword = null);
        /// <summary>
        /// Searches for the items by complex criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria criteria);

        /// <summary>
        /// Searches for the items by complex criteria
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> CatalogModuleSearchSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria);
        #endregion Asynchronous Operations
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
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsCreate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = CatalogModuleCatalogsCreateWithHttpInfo(catalog);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsCreateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null)
                throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleApi->CatalogModuleCatalogsCreate");

            var localVarPath = "/api/catalog/catalogs";
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
            if (catalog.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            }
            else
            {
                localVarPostBody = catalog; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsCreateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = await CatalogModuleCatalogsCreateAsyncWithHttpInfo(catalog);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Creates the specified catalog. Creates the specified catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog to create</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsCreateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null)
                throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleApi->CatalogModuleCatalogsCreate");

            var localVarPath = "/api/catalog/catalogs";
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
            if (catalog.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            }
            else
            {
                localVarPostBody = catalog; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsCreate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsCreate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns></returns>
        public void CatalogModuleCatalogsDelete (string id)
        {
             CatalogModuleCatalogsDeleteWithHttpInfo(id);
        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCatalogsDeleteWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCatalogsDelete");

            var localVarPath = "/api/catalog/catalogs/{id}";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsDeleteAsync (string id)
        {
             await CatalogModuleCatalogsDeleteAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Deletes catalog by id. Deletes catalog by id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Catalog id.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsDeleteAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCatalogsDelete");

            var localVarPath = "/api/catalog/catalogs/{id}";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGet (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = CatalogModuleCatalogsGetWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCatalogsGet");

            var localVarPath = "/api/catalog/catalogs/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetAsync (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = await CatalogModuleCatalogsGetAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets Catalog by id. Gets Catalog by id with full information loaded
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The Catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCatalogsGet");

            var localVarPath = "/api/catalog/catalogs/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetCatalogs ()
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> localVarResponse = CatalogModuleCatalogsGetCatalogsWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelCatalog> > CatalogModuleCatalogsGetCatalogsWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCatalog>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>)));
            
        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetCatalogsAsync ()
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>> localVarResponse = await CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get Catalogs list Get common and virtual Catalogs list with minimal information included. Returns array of Catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCatalog&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>> CatalogModuleCatalogsGetCatalogsAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetCatalogs: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCatalog>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCatalog>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelCatalog>)));
            
        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewCatalog ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = CatalogModuleCatalogsGetNewCatalogWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetNewCatalogWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs/getnew";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewCatalogAsync ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = await CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new catalog. Gets the template for a new common catalog
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewCatalogAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs/getnew";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public VirtoCommerceCatalogModuleWebModelCatalog CatalogModuleCatalogsGetNewVirtualCatalog ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalog > CatalogModuleCatalogsGetNewVirtualCatalogWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs/getnewvirtual";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalog</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalog> CatalogModuleCatalogsGetNewVirtualCatalogAsync ()
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog> localVarResponse = await CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new virtual catalog. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalog)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>> CatalogModuleCatalogsGetNewVirtualCatalogAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/catalog/catalogs/getnewvirtual";
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


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsGetNewVirtualCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalog>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalog) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalog)));
            
        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns></returns>
        public void CatalogModuleCatalogsUpdate (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             CatalogModuleCatalogsUpdateWithHttpInfo(catalog);
        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCatalogsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null)
                throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleApi->CatalogModuleCatalogsUpdate");

            var localVarPath = "/api/catalog/catalogs";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (catalog.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            }
            else
            {
                localVarPostBody = catalog; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCatalogsUpdateAsync (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
             await CatalogModuleCatalogsUpdateAsyncWithHttpInfo(catalog);

        }

        /// <summary>
        /// Updates the specified catalog. Updates the specified catalog.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalog">The catalog.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCatalogsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCatalog catalog)
        {
            // verify the required parameter 'catalog' is set
            if (catalog == null)
                throw new ApiException(400, "Missing required parameter 'catalog' when calling CatalogModuleApi->CatalogModuleCatalogsUpdate");

            var localVarPath = "/api/catalog/catalogs";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (catalog.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(catalog); // http body (model) parameter
            }
            else
            {
                localVarPostBody = catalog; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCatalogsUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public void CatalogModuleCategoriesCreateOrUpdateCategory (VirtoCommerceCatalogModuleWebModelCategory category)
        {
             CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo(category);
        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCategoriesCreateOrUpdateCategoryWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            // verify the required parameter 'category' is set
            if (category == null)
                throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleApi->CatalogModuleCategoriesCreateOrUpdateCategory");

            var localVarPath = "/api/catalog/categories";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (category.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(category); // http body (model) parameter
            }
            else
            {
                localVarPostBody = category; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesCreateOrUpdateCategoryAsync (VirtoCommerceCatalogModuleWebModelCategory category)
        {
             await CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo(category);

        }

        /// <summary>
        /// Creates or updates the specified category. If category.id is null, a new category is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="category">The category.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesCreateOrUpdateCategoryAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelCategory category)
        {
            // verify the required parameter 'category' is set
            if (category == null)
                throw new ApiException(400, "Missing required parameter 'category' when calling CatalogModuleApi->CatalogModuleCategoriesCreateOrUpdateCategory");

            var localVarPath = "/api/catalog/categories";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (category.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(category); // http body (model) parameter
            }
            else
            {
                localVarPostBody = category; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesCreateOrUpdateCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns></returns>
        public void CatalogModuleCategoriesDelete (List<string> ids)
        {
             CatalogModuleCategoriesDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleCategoriesDeleteWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleCategoriesDelete");

            var localVarPath = "/api/catalog/categories";
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleCategoriesDeleteAsync (List<string> ids)
        {
             await CatalogModuleCategoriesDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified categories by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The categories ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleCategoriesDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleCategoriesDelete");

            var localVarPath = "/api/catalog/categories";
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGet (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> localVarResponse = CatalogModuleCategoriesGetWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCategory > CatalogModuleCategoriesGetWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCategoriesGet");

            var localVarPath = "/api/catalog/categories/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetAsync (string id)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> localVarResponse = await CatalogModuleCategoriesGetAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets category by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleCategoriesGet");

            var localVarPath = "/api/catalog/categories/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }

        /// <summary>
        /// Gets categories by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetCategoriesByIds (List<string> ids, string respGroup = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>> localVarResponse = CatalogModuleCategoriesGetCategoriesByIdsWithHttpInfo(ids, respGroup);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets categories by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelCategory> > CatalogModuleCategoriesGetCategoriesByIdsWithHttpInfo (List<string> ids, string respGroup = null)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleCategoriesGetCategoriesByIds");

            var localVarPath = "/api/catalog/categories";
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
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetCategoriesByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetCategoriesByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCategory>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelCategory>)));
            
        }

        /// <summary>
        /// Gets categories by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetCategoriesByIdsAsync (List<string> ids, string respGroup = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>> localVarResponse = await CatalogModuleCategoriesGetCategoriesByIdsAsyncWithHttpInfo(ids, respGroup);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets categories by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Categories ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelCategory&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>>> CatalogModuleCategoriesGetCategoriesByIdsAsyncWithHttpInfo (List<string> ids, string respGroup = null)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleCategoriesGetCategoriesByIds");

            var localVarPath = "/api/catalog/categories";
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
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetCategoriesByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetCategoriesByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelCategory>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelCategory>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelCategory>)));
            
        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCategory</returns>
        public VirtoCommerceCatalogModuleWebModelCategory CatalogModuleCategoriesGetNewCategory (string catalogId, string parentCategoryId = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> localVarResponse = CatalogModuleCategoriesGetNewCategoryWithHttpInfo(catalogId, parentCategoryId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCategory > CatalogModuleCategoriesGetNewCategoryWithHttpInfo (string catalogId, string parentCategoryId = null)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleCategoriesGetNewCategory");

            var localVarPath = "/api/catalog/{catalogId}/categories/newcategory";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (parentCategoryId != null) localVarQueryParams.Add("parentCategoryId", Configuration.ApiClient.ParameterToString(parentCategoryId)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCategory</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCategory> CatalogModuleCategoriesGetNewCategoryAsync (string catalogId, string parentCategoryId = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCategory> localVarResponse = await CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo(catalogId, parentCategoryId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new category. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional) (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCategory)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>> CatalogModuleCategoriesGetNewCategoryAsyncWithHttpInfo (string catalogId, string parentCategoryId = null)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleCategoriesGetNewCategory");

            var localVarPath = "/api/catalog/{catalogId}/categories/newcategory";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (parentCategoryId != null) localVarQueryParams.Add("parentCategoryId", Configuration.ApiClient.ParameterToString(parentCategoryId)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleCategoriesGetNewCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCategory>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCategory) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCategory)));
            
        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification CatalogModuleExportImportDoExport (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> localVarResponse = CatalogModuleExportImportDoExportWithHttpInfo(exportInfo);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification > CatalogModuleExportImportDoExportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null)
                throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleApi->CatalogModuleExportImportDoExport");

            var localVarPath = "/api/catalog/export";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (exportInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(exportInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = exportInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoExport: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoExport: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)));
            
        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> CatalogModuleExportImportDoExportAsync (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification> localVarResponse = await CatalogModuleExportImportDoExportAsyncWithHttpInfo(exportInfo);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Start catalog data export process. Data export is an async process. An ExportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="exportInfo">The export configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>> CatalogModuleExportImportDoExportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvExportInfo exportInfo)
        {
            // verify the required parameter 'exportInfo' is set
            if (exportInfo == null)
                throw new ApiException(400, "Missing required parameter 'exportInfo' when calling CatalogModuleApi->CatalogModuleExportImportDoExport");

            var localVarPath = "/api/catalog/export";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (exportInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(exportInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = exportInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoExport: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoExport: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsExportNotification)));
            
        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification CatalogModuleExportImportDoImport (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> localVarResponse = CatalogModuleExportImportDoImportWithHttpInfo(importInfo);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification > CatalogModuleExportImportDoImportWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            // verify the required parameter 'importInfo' is set
            if (importInfo == null)
                throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleApi->CatalogModuleExportImportDoImport");

            var localVarPath = "/api/catalog/import";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (importInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(importInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = importInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoImport: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoImport: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)));
            
        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> CatalogModuleExportImportDoImportAsync (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification> localVarResponse = await CatalogModuleExportImportDoImportAsyncWithHttpInfo(importInfo);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Start catalog data import process. Data import is an async process. An ImportNotification is returned for progress reporting.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="importInfo">The import data configuration.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>> CatalogModuleExportImportDoImportAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebExportImportCsvImportInfo importInfo)
        {
            // verify the required parameter 'importInfo' is set
            if (importInfo == null)
                throw new ApiException(400, "Missing required parameter 'importInfo' when calling CatalogModuleApi->CatalogModuleExportImportDoImport");

            var localVarPath = "/api/catalog/import";
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
                "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (importInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(importInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = importInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoImport: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportDoImport: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelPushNotificationsImportNotification)));
            
        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration CatalogModuleExportImportGetMappingConfiguration (string fileUrl, string delimiter = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> localVarResponse = CatalogModuleExportImportGetMappingConfigurationWithHttpInfo(fileUrl, delimiter);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration > CatalogModuleExportImportGetMappingConfigurationWithHttpInfo (string fileUrl, string delimiter = null)
        {
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null)
                throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleApi->CatalogModuleExportImportGetMappingConfiguration");

            var localVarPath = "/api/catalog/import/mappingconfiguration";
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
            if (fileUrl != null) localVarQueryParams.Add("fileUrl", Configuration.ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) localVarQueryParams.Add("delimiter", Configuration.ApiClient.ParameterToString(delimiter)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)));
            
        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> CatalogModuleExportImportGetMappingConfigurationAsync (string fileUrl, string delimiter = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration> localVarResponse = await CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo(fileUrl, delimiter);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the CSV mapping configuration. Analyses the supplied file&#39;s structure and returns automatic column mapping.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="fileUrl">The file URL.</param>
        /// <param name="delimiter">The CSV delimiter. (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>> CatalogModuleExportImportGetMappingConfigurationAsyncWithHttpInfo (string fileUrl, string delimiter = null)
        {
            // verify the required parameter 'fileUrl' is set
            if (fileUrl == null)
                throw new ApiException(400, "Missing required parameter 'fileUrl' when calling CatalogModuleApi->CatalogModuleExportImportGetMappingConfiguration");

            var localVarPath = "/api/catalog/import/mappingconfiguration";
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
            if (fileUrl != null) localVarQueryParams.Add("fileUrl", Configuration.ApiClient.ParameterToString(fileUrl)); // query parameter
            if (delimiter != null) localVarQueryParams.Add("delimiter", Configuration.ApiClient.ParameterToString(delimiter)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleExportImportGetMappingConfiguration: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebExportImportCsvProductMappingConfiguration)));
            
        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        public void CatalogModuleListEntryCreateLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             CatalogModuleListEntryCreateLinksWithHttpInfo(links);
        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryCreateLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null)
                throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleApi->CatalogModuleListEntryCreateLinks");

            var localVarPath = "/api/catalog/listentrylinks";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (links.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            }
            else
            {
                localVarPostBody = links; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryCreateLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             await CatalogModuleListEntryCreateLinksAsyncWithHttpInfo(links);

        }

        /// <summary>
        /// Creates links for categories or items to parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryCreateLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null)
                throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleApi->CatalogModuleListEntryCreateLinks");

            var localVarPath = "/api/catalog/listentrylinks";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (links.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            }
            else
            {
                localVarPostBody = links; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryCreateLinks: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns></returns>
        public void CatalogModuleListEntryDeleteLinks (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             CatalogModuleListEntryDeleteLinksWithHttpInfo(links);
        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryDeleteLinksWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null)
                throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleApi->CatalogModuleListEntryDeleteLinks");

            var localVarPath = "/api/catalog/listentrylinks/delete";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (links.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            }
            else
            {
                localVarPostBody = links; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryDeleteLinksAsync (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
             await CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo(links);

        }

        /// <summary>
        /// Unlinks the linked categories or items from parent categories and catalogs. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="links">The links.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryDeleteLinksAsyncWithHttpInfo (List<VirtoCommerceCatalogModuleWebModelListEntryLink> links)
        {
            // verify the required parameter 'links' is set
            if (links == null)
                throw new ApiException(400, "Missing required parameter 'links' when calling CatalogModuleApi->CatalogModuleListEntryDeleteLinks");

            var localVarPath = "/api/catalog/listentrylinks/delete";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (links.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(links); // http body (model) parameter
            }
            else
            {
                localVarPostBody = links; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryDeleteLinks: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public VirtoCommerceCatalogModuleWebModelListEntrySearchResult CatalogModuleListEntryListItemsSearch (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> localVarResponse = CatalogModuleListEntryListItemsSearchWithHttpInfo(searchCriteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelListEntrySearchResult > CatalogModuleListEntryListItemsSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria)
        {
            // verify the required parameter 'searchCriteria' is set
            if (searchCriteria == null)
                throw new ApiException(400, "Missing required parameter 'searchCriteria' when calling CatalogModuleApi->CatalogModuleListEntryListItemsSearch");

            var localVarPath = "/api/catalog/listentries";
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
            if (searchCriteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(searchCriteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = searchCriteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult)));
            
        }

        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelListEntrySearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> CatalogModuleListEntryListItemsSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult> localVarResponse = await CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo(searchCriteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Searches for the items by complex criteria. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelListEntrySearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>> CatalogModuleListEntryListItemsSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria searchCriteria)
        {
            // verify the required parameter 'searchCriteria' is set
            if (searchCriteria == null)
                throw new ApiException(400, "Missing required parameter 'searchCriteria' when calling CatalogModuleApi->CatalogModuleListEntryListItemsSearch");

            var localVarPath = "/api/catalog/listentries";
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
            if (searchCriteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(searchCriteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = searchCriteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryListItemsSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelListEntrySearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelListEntrySearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelListEntrySearchResult)));
            
        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns></returns>
        public void CatalogModuleListEntryMove (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
             CatalogModuleListEntryMoveWithHttpInfo(moveInfo);
        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleListEntryMoveWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null)
                throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleApi->CatalogModuleListEntryMove");

            var localVarPath = "/api/catalog/listentries/move";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moveInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(moveInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moveInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryMove: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryMove: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleListEntryMoveAsync (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
             await CatalogModuleListEntryMoveAsyncWithHttpInfo(moveInfo);

        }

        /// <summary>
        /// Move categories or products to another location. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="moveInfo">Move operation details</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleListEntryMoveAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelMoveInfo moveInfo)
        {
            // verify the required parameter 'moveInfo' is set
            if (moveInfo == null)
                throw new ApiException(400, "Missing required parameter 'moveInfo' when calling CatalogModuleApi->CatalogModuleListEntryMove");

            var localVarPath = "/api/catalog/listentries/move";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (moveInfo.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(moveInfo); // http body (model) parameter
            }
            else
            {
                localVarPostBody = moveInfo; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryMove: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleListEntryMove: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsCloneProduct (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = CatalogModuleProductsCloneProductWithHttpInfo(productId);
             return localVarResponse.Data;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsCloneProductWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleApi->CatalogModuleProductsCloneProduct");

            var localVarPath = "/api/catalog/products/{productId}/clone";
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
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsCloneProduct: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsCloneProduct: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsCloneProductAsync (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = await CatalogModuleProductsCloneProductAsyncWithHttpInfo(productId);
             return localVarResponse.Data;

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsCloneProductAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleApi->CatalogModuleProductsCloneProduct");

            var localVarPath = "/api/catalog/products/{productId}/clone";
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
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsCloneProduct: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsCloneProduct: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns></returns>
        public void CatalogModuleProductsDelete (List<string> ids)
        {
             CatalogModuleProductsDeleteWithHttpInfo(ids);
        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleProductsDeleteWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleProductsDelete");

            var localVarPath = "/api/catalog/products";
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsDeleteAsync (List<string> ids)
        {
             await CatalogModuleProductsDeleteAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Deletes the specified items by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">The items ids.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsDeleteAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleProductsDelete");

            var localVarPath = "/api/catalog/products";
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
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalog (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = CatalogModuleProductsGetNewProductByCatalogWithHttpInfo(catalogId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewProductByCatalogWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalog");

            var localVarPath = "/api/catalog/{catalogId}/products/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAsync (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = await CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo(catalogId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new product (outside of category). Use when need to create item belonging to catalog directly.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAsyncWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalog");

            var localVarPath = "/api/catalog/{catalogId}/products/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewProductByCatalogAndCategory (string catalogId, string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo(catalogId, categoryId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewProductByCatalogAndCategoryWithHttpInfo (string catalogId, string categoryId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalogAndCategory");
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalogAndCategory");

            var localVarPath = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsync (string catalogId, string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = await CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo(catalogId, categoryId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new product (inside category). Use when need to create item belonging to catalog category.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewProductByCatalogAndCategoryAsyncWithHttpInfo (string catalogId, string categoryId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalogAndCategory");
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleApi->CatalogModuleProductsGetNewProductByCatalogAndCategory");

            var localVarPath = "/api/catalog/{catalogId}/categories/{categoryId}/products/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter
            if (categoryId != null) localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewProductByCatalogAndCategory: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetNewVariation (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = CatalogModuleProductsGetNewVariationWithHttpInfo(productId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetNewVariationWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleApi->CatalogModuleProductsGetNewVariation");

            var localVarPath = "/api/catalog/products/{productId}/getnewvariation";
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
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetNewVariationAsync (string productId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = await CatalogModuleProductsGetNewVariationAsyncWithHttpInfo(productId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new variation. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="productId">The parent product id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetNewVariationAsyncWithHttpInfo (string productId)
        {
            // verify the required parameter 'productId' is set
            if (productId == null)
                throw new ApiException(400, "Missing required parameter 'productId' when calling CatalogModuleApi->CatalogModuleProductsGetNewVariation");

            var localVarPath = "/api/catalog/products/{productId}/getnewvariation";
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
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetNewVariation: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets product by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProduct</returns>
        public VirtoCommerceCatalogModuleWebModelProduct CatalogModuleProductsGetProductById (string id, string respGroup = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = CatalogModuleProductsGetProductByIdWithHttpInfo(id, respGroup);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets product by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProduct > CatalogModuleProductsGetProductByIdWithHttpInfo (string id, string respGroup = null)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleProductsGetProductById");

            var localVarPath = "/api/catalog/products/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets product by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProduct</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIdAsync (string id, string respGroup = null)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProduct> localVarResponse = await CatalogModuleProductsGetProductByIdAsyncWithHttpInfo(id, respGroup);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets product by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">Item id.</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProduct)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdAsyncWithHttpInfo (string id, string respGroup = null)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModuleProductsGetProductById");

            var localVarPath = "/api/catalog/products/{id}";
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
            if (id != null) localVarPathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProduct>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProduct) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProduct)));
            
        }

        /// <summary>
        /// Gets products by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelProduct> CatalogModuleProductsGetProductByIds (List<string> ids, string respGroup = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>> localVarResponse = CatalogModuleProductsGetProductByIdsWithHttpInfo(ids, respGroup);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets products by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelProduct> > CatalogModuleProductsGetProductByIdsWithHttpInfo (List<string> ids, string respGroup = null)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleProductsGetProductByIds");

            var localVarPath = "/api/catalog/products";
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
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelProduct>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelProduct>)));
            
        }

        /// <summary>
        /// Gets products by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelProduct>> CatalogModuleProductsGetProductByIdsAsync (List<string> ids, string respGroup = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>> localVarResponse = await CatalogModuleProductsGetProductByIdsAsyncWithHttpInfo(ids, respGroup);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets products by ids 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">Item ids</param>
        /// <param name="respGroup">Response group. (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelProduct&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>>> CatalogModuleProductsGetProductByIdsAsyncWithHttpInfo (List<string> ids, string respGroup = null)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling CatalogModuleApi->CatalogModuleProductsGetProductByIds");

            var localVarPath = "/api/catalog/products";
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
            if (respGroup != null) localVarQueryParams.Add("respGroup", Configuration.ApiClient.ParameterToString(respGroup)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductByIds: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsGetProductByIds: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelProduct>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelProduct>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelProduct>)));
            
        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public void CatalogModuleProductsUpdate (VirtoCommerceCatalogModuleWebModelProduct product)
        {
             CatalogModuleProductsUpdateWithHttpInfo(product);
        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModuleProductsUpdateWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            // verify the required parameter 'product' is set
            if (product == null)
                throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleApi->CatalogModuleProductsUpdate");

            var localVarPath = "/api/catalog/products";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (product.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(product); // http body (model) parameter
            }
            else
            {
                localVarPostBody = product; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModuleProductsUpdateAsync (VirtoCommerceCatalogModuleWebModelProduct product)
        {
             await CatalogModuleProductsUpdateAsyncWithHttpInfo(product);

        }

        /// <summary>
        /// Updates the specified product. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="product">The product.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModuleProductsUpdateAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProduct product)
        {
            // verify the required parameter 'product' is set
            if (product == null)
                throw new ApiException(400, "Missing required parameter 'product' when calling CatalogModuleApi->CatalogModuleProductsUpdate");

            var localVarPath = "/api/catalog/products";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (product.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(product); // http body (model) parameter
            }
            else
            {
                localVarPostBody = product; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleProductsUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public void CatalogModulePropertiesCreateOrUpdateProperty (VirtoCommerceCatalogModuleWebModelProperty property)
        {
             CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo(property);
        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModulePropertiesCreateOrUpdatePropertyWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModuleApi->CatalogModulePropertiesCreateOrUpdateProperty");

            var localVarPath = "/api/catalog/properties";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesCreateOrUpdatePropertyAsync (VirtoCommerceCatalogModuleWebModelProperty property)
        {
             await CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo(property);

        }

        /// <summary>
        /// Creates or updates the specified property. If property.IsNew &#x3D;&#x3D; True, a new property is created. It&#39;s updated otherwise
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="property">The property.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesCreateOrUpdatePropertyAsyncWithHttpInfo (VirtoCommerceCatalogModuleWebModelProperty property)
        {
            // verify the required parameter 'property' is set
            if (property == null)
                throw new ApiException(400, "Missing required parameter 'property' when calling CatalogModuleApi->CatalogModulePropertiesCreateOrUpdateProperty");

            var localVarPath = "/api/catalog/properties";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (property.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(property); // http body (model) parameter
            }
            else
            {
                localVarPostBody = property; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesCreateOrUpdateProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
        public void CatalogModulePropertiesDelete (string id)
        {
             CatalogModulePropertiesDeleteWithHttpInfo(id);
        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> CatalogModulePropertiesDeleteWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModulePropertiesDelete");

            var localVarPath = "/api/catalog/properties";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarQueryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task CatalogModulePropertiesDeleteAsync (string id)
        {
             await CatalogModulePropertiesDeleteAsyncWithHttpInfo(id);

        }

        /// <summary>
        /// Deletes property by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">The property id.</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> CatalogModulePropertiesDeleteAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling CatalogModuleApi->CatalogModulePropertiesDelete");

            var localVarPath = "/api/catalog/properties";
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
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (id != null) localVarQueryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGet (string propertyId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = CatalogModulePropertiesGetWithHttpInfo(propertyId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetWithHttpInfo (string propertyId)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModuleApi->CatalogModulePropertiesGet");

            var localVarPath = "/api/catalog/properties/{propertyId}";
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
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetAsync (string propertyId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = await CatalogModulePropertiesGetAsyncWithHttpInfo(propertyId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets property metainformation by id. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetAsyncWithHttpInfo (string propertyId)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModuleApi->CatalogModulePropertiesGet");

            var localVarPath = "/api/catalog/properties/{propertyId}";
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
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGet: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGet: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCatalogProperty (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo(catalogId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetNewCatalogPropertyWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModulePropertiesGetNewCatalogProperty");

            var localVarPath = "/api/catalog/{catalogId}/properties/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCatalogPropertyAsync (string catalogId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = await CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo(catalogId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new catalog property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="catalogId">The catalog id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCatalogPropertyAsyncWithHttpInfo (string catalogId)
        {
            // verify the required parameter 'catalogId' is set
            if (catalogId == null)
                throw new ApiException(400, "Missing required parameter 'catalogId' when calling CatalogModuleApi->CatalogModulePropertiesGetNewCatalogProperty");

            var localVarPath = "/api/catalog/{catalogId}/properties/getnew";
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
            if (catalogId != null) localVarPathParams.Add("catalogId", Configuration.ApiClient.ParameterToString(catalogId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCatalogProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelProperty</returns>
        public VirtoCommerceCatalogModuleWebModelProperty CatalogModulePropertiesGetNewCategoryProperty (string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo(categoryId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelProperty > CatalogModulePropertiesGetNewCategoryPropertyWithHttpInfo (string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleApi->CatalogModulePropertiesGetNewCategoryProperty");

            var localVarPath = "/api/catalog/categories/{categoryId}/properties/getnew";
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
            if (categoryId != null) localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelProperty</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelProperty> CatalogModulePropertiesGetNewCategoryPropertyAsync (string categoryId)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelProperty> localVarResponse = await CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo(categoryId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets the template for a new category property. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId">The category id.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelProperty)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>> CatalogModulePropertiesGetNewCategoryPropertyAsyncWithHttpInfo (string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CatalogModuleApi->CatalogModulePropertiesGetNewCategoryProperty");

            var localVarPath = "/api/catalog/categories/{categoryId}/properties/getnew";
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
            if (categoryId != null) localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetNewCategoryProperty: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelProperty>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelProperty) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelProperty)));
            
        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public List<VirtoCommerceCatalogModuleWebModelPropertyValue> CatalogModulePropertiesGetPropertyValues (string propertyId, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> localVarResponse = CatalogModulePropertiesGetPropertyValuesWithHttpInfo(propertyId, keyword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public ApiResponse< List<VirtoCommerceCatalogModuleWebModelPropertyValue> > CatalogModulePropertiesGetPropertyValuesWithHttpInfo (string propertyId, string keyword = null)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModuleApi->CatalogModulePropertiesGetPropertyValues");

            var localVarPath = "/api/catalog/properties/{propertyId}/values";
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
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>)));
            
        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> CatalogModulePropertiesGetPropertyValuesAsync (string propertyId, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>> localVarResponse = await CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo(propertyId, keyword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Gets all dictionary values that specified property can have. 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional) (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceCatalogModuleWebModelPropertyValue&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>> CatalogModulePropertiesGetPropertyValuesAsyncWithHttpInfo (string propertyId, string keyword = null)
        {
            // verify the required parameter 'propertyId' is set
            if (propertyId == null)
                throw new ApiException(400, "Missing required parameter 'propertyId' when calling CatalogModuleApi->CatalogModulePropertiesGetPropertyValues");

            var localVarPath = "/api/catalog/properties/{propertyId}/values";
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
            if (propertyId != null) localVarPathParams.Add("propertyId", Configuration.ApiClient.ParameterToString(propertyId)); // path parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModulePropertiesGetPropertyValues: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceCatalogModuleWebModelPropertyValue>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceCatalogModuleWebModelPropertyValue>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceCatalogModuleWebModelPropertyValue>)));
            
        }

        /// <summary>
        /// Searches for the items by complex criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public VirtoCommerceCatalogModuleWebModelCatalogSearchResult CatalogModuleSearchSearch (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> localVarResponse = CatalogModuleSearchSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Searches for the items by complex criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>ApiResponse of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public ApiResponse< VirtoCommerceCatalogModuleWebModelCatalogSearchResult > CatalogModuleSearchSearchWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CatalogModuleApi->CatalogModuleSearchSearch");

            var localVarPath = "/api/catalog/search";
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
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleSearchSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleSearchSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }

        /// <summary>
        /// Searches for the items by complex criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>Task of VirtoCommerceCatalogModuleWebModelCatalogSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> CatalogModuleSearchSearchAsync (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult> localVarResponse = await CatalogModuleSearchSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Searches for the items by complex criteria 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>Task of ApiResponse (VirtoCommerceCatalogModuleWebModelCatalogSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>> CatalogModuleSearchSearchAsyncWithHttpInfo (VirtoCommerceDomainCatalogModelSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling CatalogModuleApi->CatalogModuleSearchSearch");

            var localVarPath = "/api/catalog/search";
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
            if (criteria.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                localVarPostBody = criteria; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleSearchSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling CatalogModuleSearchSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceCatalogModuleWebModelCatalogSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceCatalogModuleWebModelCatalogSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceCatalogModuleWebModelCatalogSearchResult)));
            
        }

    }
}
