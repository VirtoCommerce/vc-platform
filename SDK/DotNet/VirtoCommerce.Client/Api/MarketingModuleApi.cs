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
    public interface IMarketingModuleApi
    {
        
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
  
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
  
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">folders ids for delete</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentFolders (List<string> ids);
  
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <param name="id">folder id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById (string id);
  
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <param name="id">folder id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo (string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <param name="id">folder id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync (string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
  
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentCreateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
  
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContents (List<string> ids);
  
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);
  
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);
        
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <param name="id">content item id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentGetDynamicContentById (string id);
  
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <param name="id">content item id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo (string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <param name="id">content item id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync (string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
  
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
  
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPlaces (List<string> ids);
  
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <param name="id">place id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById (string id);
  
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <param name="id">place id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo (string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <param name="id">place id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync (string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
  
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
  
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPublications (List<string> ids);
  
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication ();
  
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo ();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync ();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo ();
        
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <param name="id">publication id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById (string id);
  
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <param name="id">publication id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo (string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <param name="id">publication id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync (string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionUpdatePromotions (VirtoCommerceMarketingModuleWebModelPromotion promotion);
  
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionUpdatePromotionsWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionCreatePromotion (VirtoCommerceMarketingModuleWebModelPromotion promotion);
  
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionDeletePromotions (List<string> ids);
  
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionDeletePromotionsWithHttpInfo (List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync (List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo (List<string> ids);
        
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        List<VirtoCommerceMarketingModuleWebModelPromotionReward> MarketingModulePromotionEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
  
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
        
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetNewDynamicPromotion ();
  
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo ();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionAsync ();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo ();
        
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <param name="id">promotion id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetPromotionById (string id);
  
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <param name="id">promotion id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdWithHttpInfo (string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <param name="id">promotion id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdAsync (string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo (string id);
        
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        VirtoCommerceMarketingModuleWebModelMarketingSearchResult MarketingModuleSearch (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);
  
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchAsync (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelMarketingSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MarketingModuleApi : IMarketingModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public MarketingModuleApi(Configuration configuration)
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
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentFolder");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (folder.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                postBody = folder; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleDynamicContentUpdateDynamicContentFolder");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> response = MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo(folder);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentFolder > MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentFolder");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
            
            
            
            
            if (folder.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                postBody = folder; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }
    
        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> response = await MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo(folder);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null) throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleDynamicContentCreateDynamicContentFolder");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }
        
        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <param name="ids">folders ids for delete</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentFolders (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <param name="ids">folders ids for delete</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentFolders");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleDynamicContentDeleteDynamicContentFolders");
            
    
            var path_ = "/api/marketing/contentfolders";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <param name="id">folder id</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> response = MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <param name="id">folder id</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentFolder > MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentFolderById");
            
    
            var path_ = "/api/marketing/contentfolders/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }
    
        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <param name="id">folder id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> response = await MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleDynamicContentGetDynamicContentFolderById");
            
    
            var path_ = "/api/marketing/contentfolders/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }
        
        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo(contentItem);
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (contentItem.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                postBody = contentItem; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo(contentItem);

        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null) throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleDynamicContentUpdateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentCreateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> response = MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo(contentItem);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentItem > MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
            
            
            
            
            if (contentItem.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                postBody = contentItem; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }
    
        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> response = await MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo(contentItem);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null) throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleDynamicContentCreateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }
        
        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContents (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContents");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleDynamicContentDeleteDynamicContents");
            
    
            var path_ = "/api/marketing/contentitems";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="evalContext"></param> 
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> response = MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo(evalContext);
             return response.Data;
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="evalContext"></param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public ApiResponse< List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> > MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
            
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling MarketingModuleApi->MarketingModuleDynamicContentEvaluateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems/evaluate";
    
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
            
            
            
            
            if (evalContext.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                postBody = evalContext; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>)));
            
        }
    
        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> response = await MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo(evalContext);
             return response.Data;

        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null) throw new ApiException(400, "Missing required parameter 'evalContext' when calling MarketingModuleDynamicContentEvaluateDynamicContent");
            
    
            var path_ = "/api/marketing/contentitems/evaluate";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>)));
            
        }
        
        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <param name="id">content item id</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentGetDynamicContentById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> response = MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <param name="id">content item id</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentItem > MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentById");
            
    
            var path_ = "/api/marketing/contentitems/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }
    
        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <param name="id">content item id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> response = await MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleDynamicContentGetDynamicContentById");
            
    
            var path_ = "/api/marketing/contentitems/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }
        
        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo(contentPlace);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPlace");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (contentPlace.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                postBody = contentPlace; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);

        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null) throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleDynamicContentUpdateDynamicContentPlace");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> response = MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo(contentPlace);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPlace > MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPlace");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
            
            
            
            
            if (contentPlace.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                postBody = contentPlace; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }
    
        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> response = await MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null) throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleDynamicContentCreateDynamicContentPlace");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }
        
        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPlaces (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPlaces");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleDynamicContentDeleteDynamicContentPlaces");
            
    
            var path_ = "/api/marketing/contentplaces";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <param name="id">place id</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> response = MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <param name="id">place id</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPlace > MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPlaceById");
            
    
            var path_ = "/api/marketing/contentplaces/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }
    
        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <param name="id">place id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> response = await MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleDynamicContentGetDynamicContentPlaceById");
            
    
            var path_ = "/api/marketing/contentplaces/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }
        
        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo(publication);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPublication");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (publication.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                postBody = publication; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo(publication);

        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null) throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleDynamicContentUpdateDynamicContentPublication");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo(publication);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPublication");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
            
            
            
            
            if (publication.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                postBody = publication; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
    
        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = await MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo(publication);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null) throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleDynamicContentCreateDynamicContentPublication");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
        
        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param> 
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPublications (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPublications");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleDynamicContentDeleteDynamicContentPublications");
            
    
            var path_ = "/api/marketing/contentpublications";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo ()
        {
            
    
            var path_ = "/api/marketing/contentpublications/new";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
    
        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = await MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/marketing/contentpublications/new";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
        
        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <param name="id">publication id</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <param name="id">publication id</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPublicationById");
            
    
            var path_ = "/api/marketing/contentpublications/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
    
        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <param name="id">publication id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> response = await MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleDynamicContentGetDynamicContentPublicationById");
            
    
            var path_ = "/api/marketing/contentpublications/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }
        
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param> 
        /// <returns></returns>
        public void MarketingModulePromotionUpdatePromotions (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             MarketingModulePromotionUpdatePromotionsWithHttpInfo(promotion);
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModulePromotionUpdatePromotionsWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionUpdatePromotions");
            
    
            var path_ = "/api/marketing/promotions";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            if (promotion.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                postBody = promotion; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             await MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo(promotion);

        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null) throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModulePromotionUpdatePromotions");
            
    
            var path_ = "/api/marketing/promotions";
    
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
                
            };
            String httpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(httpHeaderAccepts);
            if (httpHeaderAccept != null)
                headerParams.Add("Accept", httpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            pathParams.Add("format", "json");
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionCreatePromotion (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = MarketingModulePromotionCreatePromotionWithHttpInfo(promotion);
             return response.Data;
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionCreatePromotionWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionCreatePromotion");
            
    
            var path_ = "/api/marketing/promotions";
    
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
            
            
            
            
            if (promotion.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                postBody = promotion; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionCreatePromotion: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionCreatePromotion: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
    
        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = await MarketingModulePromotionCreatePromotionAsyncWithHttpInfo(promotion);
             return response.Data;

        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null) throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModulePromotionCreatePromotion");
            
    
            var path_ = "/api/marketing/promotions";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionCreatePromotion: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionCreatePromotion: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
        
        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param> 
        /// <returns></returns>
        public void MarketingModulePromotionDeletePromotions (List<string> ids)
        {
             MarketingModulePromotionDeletePromotionsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModulePromotionDeletePromotionsWithHttpInfo (List<string> ids)
        {
            
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModulePromotionDeletePromotions");
            
    
            var path_ = "/api/marketing/promotions";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionDeletePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionDeletePromotions: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync (List<string> ids)
        {
             await MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null) throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModulePromotionDeletePromotions");
            
    
            var path_ = "/api/marketing/promotions";
    
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
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionDeletePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionDeletePromotions: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param> 
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public List<VirtoCommerceMarketingModuleWebModelPromotionReward> MarketingModulePromotionEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> response = MarketingModulePromotionEvaluatePromotionsWithHttpInfo(context);
             return response.Data;
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public ApiResponse< List<VirtoCommerceMarketingModuleWebModelPromotionReward> > MarketingModulePromotionEvaluatePromotionsWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            
            // verify the required parameter 'context' is set
            if (context == null)
                throw new ApiException(400, "Missing required parameter 'context' when calling MarketingModuleApi->MarketingModulePromotionEvaluatePromotions");
            
    
            var path_ = "/api/marketing/promotions/evaluate";
    
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
            
            
            
            
            if (context.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(context); // http body (model) parameter
            }
            else
            {
                postBody = context; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelPromotionReward>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceMarketingModuleWebModelPromotionReward>)));
            
        }
    
        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> response = await MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo(context);
             return response.Data;

        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null) throw new ApiException(400, "Missing required parameter 'context' when calling MarketingModulePromotionEvaluatePromotions");
            
    
            var path_ = "/api/marketing/promotions/evaluate";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(context); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelPromotionReward>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceMarketingModuleWebModelPromotionReward>)));
            
        }
        
        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetNewDynamicPromotion ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo();
             return response.Data;
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo ()
        {
            
    
            var path_ = "/api/marketing/promotions/new";
    
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
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
    
        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionAsync ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = await MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo();
             return response.Data;

        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo ()
        {
            
    
            var path_ = "/api/marketing/promotions/new";
    
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
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
        
        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <param name="id">promotion id</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetPromotionById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = MarketingModulePromotionGetPromotionByIdWithHttpInfo(id);
             return response.Data;
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <param name="id">promotion id</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionGetPromotionByIdWithHttpInfo (string id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModulePromotionGetPromotionById");
            
    
            var path_ = "/api/marketing/promotions/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetPromotionById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetPromotionById: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
    
        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <param name="id">promotion id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> response = await MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo(id);
             return response.Data;

        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModulePromotionGetPromotionById");
            
    
            var path_ = "/api/marketing/promotions/{id}";
    
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
            if (id != null) pathParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetPromotionById: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModulePromotionGetPromotionById: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }
        
        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <param name="criteria">criteria</param> 
        /// <returns>VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public VirtoCommerceMarketingModuleWebModelMarketingSearchResult MarketingModuleSearch (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> response = MarketingModuleSearchWithHttpInfo(criteria);
             return response.Data;
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <param name="criteria">criteria</param> 
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelMarketingSearchResult > MarketingModuleSearchWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
            
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling MarketingModuleApi->MarketingModuleSearch");
            
    
            var path_ = "/api/marketing/search";
    
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
            
            
            
            
            if (criteria.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            }
            else
            {
                postBody = criteria; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleSearch: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelMarketingSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelMarketingSearchResult)));
            
        }
    
        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchAsync (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> response = await MarketingModuleSearchAsyncWithHttpInfo(criteria);
             return response.Data;

        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelMarketingSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null) throw new ApiException(400, "Missing required parameter 'criteria' when calling MarketingModuleSearch");
            
    
            var path_ = "/api/marketing/search";
    
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
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(criteria); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400)
                throw new ApiException (statusCode, "Error calling MarketingModuleSearch: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MarketingModuleSearch: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelMarketingSearchResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceMarketingModuleWebModelMarketingSearchResult)));
            
        }
        
    }
    
}
