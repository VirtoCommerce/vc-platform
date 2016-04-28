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
        #region Synchronous Operations
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentCreateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentFolders (List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPlaces (List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContentPublications (List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentDeleteDynamicContents (List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentGetDynamicContentById (string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById (string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById (string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById (string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo (string id);
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication ();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo ();
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        void MarketingModuleDynamicContentUpdateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionCreatePromotion (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionDeletePromotions (List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionDeletePromotionsWithHttpInfo (List<string> ids);
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        List<VirtoCommerceMarketingModuleWebModelPromotionReward> MarketingModulePromotionEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetNewDynamicPromotion ();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo ();
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetPromotionById (string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdWithHttpInfo (string id);
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns></returns>
        void MarketingModulePromotionUpdatePromotions (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MarketingModulePromotionUpdatePromotionsWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        VirtoCommerceMarketingModuleWebModelMarketingSearchResult MarketingModuleSearch (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Add new dynamic content item object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Add new dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Add new dynamic content place object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Add new dynamic content publication object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content folders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content place objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content publication objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync (List<string> ids);

        /// <summary>
        /// Delete a dynamic content item objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);

        /// <summary>
        /// Get dynamic content for given placeholders
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext);
        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync (string id);

        /// <summary>
        /// Find dynamic content item object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content item object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync (string id);

        /// <summary>
        /// Find dynamic content folder by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content folder
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync (string id);

        /// <summary>
        /// Find dynamic content place object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content place object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync (string id);

        /// <summary>
        /// Find dynamic content publication object by id
        /// </summary>
        /// <remarks>
        /// Return a single dynamic content publication object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync ();

        /// <summary>
        /// Get new dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo ();
        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);

        /// <summary>
        /// Update a existing dynamic content item object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem);
        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);

        /// <summary>
        /// Update a existing dynamic content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder);
        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);

        /// <summary>
        /// Update a existing dynamic content place object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace);
        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);

        /// <summary>
        /// Update a existing dynamic content publication object
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication);
        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Add new dynamic promotion object to marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync (List<string> ids);

        /// <summary>
        /// Delete promotions objects
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo (List<string> ids);
        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);

        /// <summary>
        /// Evaluate promotions
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context);
        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionAsync ();

        /// <summary>
        /// Get new dynamic promotion object
        /// </summary>
        /// <remarks>
        /// Return a new dynamic promotion object with populated dynamic expression tree
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo ();
        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdAsync (string id);

        /// <summary>
        /// Find promotion object by id
        /// </summary>
        /// <remarks>
        /// Return a single promotion (dynamic or custom) object
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo (string id);
        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion);

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion);
        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchAsync (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);

        /// <summary>
        /// Search marketing objects by given criteria
        /// </summary>
        /// <remarks>
        /// Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelMarketingSearchResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria);
        #endregion Asynchronous Operations
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
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentCreateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo(contentItem);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentItem > MarketingModuleDynamicContentCreateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
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
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentCreateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo(contentItem);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content item object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentCreateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
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
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentCreateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo(folder);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentFolder > MarketingModuleDynamicContentCreateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentCreateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo(folder);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be added</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentCreateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentCreateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo(contentPlace);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPlace > MarketingModuleDynamicContentCreateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
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
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentCreateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content place object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentCreateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
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
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentCreateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo(publication);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentCreateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
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
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentCreateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo(publication);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic content publication object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be added to the dynamic content system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentCreateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentCreateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
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
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentCreateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentFolders (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentFoldersWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentFolders");

            var localVarPath = "/api/marketing/contentfolders";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentFoldersAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content folders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">folders ids for delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentFoldersAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentFolders");

            var localVarPath = "/api/marketing/contentfolders";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPlaces (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPlacesWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPlaces");

            var localVarPath = "/api/marketing/contentplaces";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPlacesAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content place objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content place object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPlacesAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPlaces");

            var localVarPath = "/api/marketing/contentplaces";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPlaces: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContentPublications (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentPublicationsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPublications");

            var localVarPath = "/api/marketing/contentpublications";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content publication objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content publication object ids for delete from dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentPublicationsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContentPublications");

            var localVarPath = "/api/marketing/contentpublications";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContentPublications: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentDeleteDynamicContents (List<string> ids)
        {
             MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentDeleteDynamicContentsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContents");

            var localVarPath = "/api/marketing/contentitems";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentDeleteDynamicContentsAsync (List<string> ids)
        {
             await MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete a dynamic content item objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">content item object ids for delete in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentDeleteDynamicContentsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModuleDynamicContentDeleteDynamicContents");

            var localVarPath = "/api/marketing/contentitems";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentDeleteDynamicContents: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentEvaluateDynamicContent (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> localVarResponse = MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo(evalContext);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public ApiResponse< List<VirtoCommerceMarketingModuleWebModelDynamicContentItem> > MarketingModuleDynamicContentEvaluateDynamicContentWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling MarketingModuleApi->MarketingModuleDynamicContentEvaluateDynamicContent");

            var localVarPath = "/api/marketing/contentitems/evaluate";
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
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>)));
            
        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentEvaluateDynamicContentAsync (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> localVarResponse = await MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo(evalContext);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get dynamic content for given placeholders 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="evalContext"></param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelDynamicContentItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>> MarketingModuleDynamicContentEvaluateDynamicContentAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelDynamicContentDynamicContentEvaluationContext evalContext)
        {
            // verify the required parameter 'evalContext' is set
            if (evalContext == null)
                throw new ApiException(400, "Missing required parameter 'evalContext' when calling MarketingModuleApi->MarketingModuleDynamicContentEvaluateDynamicContent");

            var localVarPath = "/api/marketing/contentitems/evaluate";
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
            if (evalContext.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(evalContext); // http body (model) parameter
            }
            else
            {
                localVarPostBody = evalContext; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentEvaluateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceMarketingModuleWebModelDynamicContentItem>)));
            
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentItem MarketingModuleDynamicContentGetDynamicContentById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> localVarResponse = MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentItem > MarketingModuleDynamicContentGetDynamicContentByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentById");

            var localVarPath = "/api/marketing/contentitems/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentItem</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentItem> MarketingModuleDynamicContentGetDynamicContentByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content item object by id Return a single dynamic content item object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">content item id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentItem)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>> MarketingModuleDynamicContentGetDynamicContentByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentById");

            var localVarPath = "/api/marketing/contentitems/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentItem>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentItem) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentItem)));
            
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentFolder MarketingModuleDynamicContentGetDynamicContentFolderById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> localVarResponse = MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentFolder > MarketingModuleDynamicContentGetDynamicContentFolderByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentFolderById");

            var localVarPath = "/api/marketing/contentfolders/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentFolder</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content folder by id Return a single dynamic content folder
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">folder id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentFolder)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>> MarketingModuleDynamicContentGetDynamicContentFolderByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentFolderById");

            var localVarPath = "/api/marketing/contentfolders/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentFolderById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentFolder>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentFolder) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentFolder)));
            
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPlace MarketingModuleDynamicContentGetDynamicContentPlaceById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> localVarResponse = MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPlace > MarketingModuleDynamicContentGetDynamicContentPlaceByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPlaceById");

            var localVarPath = "/api/marketing/contentplaces/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPlace</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content place object by id Return a single dynamic content place object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">place id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPlace)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>> MarketingModuleDynamicContentGetDynamicContentPlaceByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPlaceById");

            var localVarPath = "/api/marketing/contentplaces/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPlaceById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPlace>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPlace) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPlace)));
            
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetDynamicContentPublicationById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentGetDynamicContentPublicationByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPublicationById");

            var localVarPath = "/api/marketing/contentpublications/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find dynamic content publication object by id Return a single dynamic content publication object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">publication id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetDynamicContentPublicationByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModuleDynamicContentGetDynamicContentPublicationById");

            var localVarPath = "/api/marketing/contentpublications/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetDynamicContentPublicationById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public VirtoCommerceMarketingModuleWebModelDynamicContentPublication MarketingModuleDynamicContentGetNewDynamicPublication ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelDynamicContentPublication > MarketingModuleDynamicContentGetNewDynamicPublicationWithHttpInfo ()
        {

            var localVarPath = "/api/marketing/contentpublications/new";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelDynamicContentPublication</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> MarketingModuleDynamicContentGetNewDynamicPublicationAsync ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication> localVarResponse = await MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelDynamicContentPublication)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>> MarketingModuleDynamicContentGetNewDynamicPublicationAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/marketing/contentpublications/new";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentGetNewDynamicPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelDynamicContentPublication>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelDynamicContentPublication) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelDynamicContentPublication)));
            
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContent (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo(contentItem);
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
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
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentAsync (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo(contentItem);

        }

        /// <summary>
        /// Update a existing dynamic content item object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentItem">dynamic content object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentItem contentItem)
        {
            // verify the required parameter 'contentItem' is set
            if (contentItem == null)
                throw new ApiException(400, "Missing required parameter 'contentItem' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContent");

            var localVarPath = "/api/marketing/contentitems";
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
            if (contentItem.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentItem); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentItem; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentFolder (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo(folder);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentFolderWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentFolderAsync (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo(folder);

        }

        /// <summary>
        /// Update a existing dynamic content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="folder">dynamic content folder that needs to be updated</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentFolderAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentFolder folder)
        {
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentFolder");

            var localVarPath = "/api/marketing/contentfolders";
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
            if (folder.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(folder); // http body (model) parameter
            }
            else
            {
                localVarPostBody = folder; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPlace (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo(contentPlace);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPlaceWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
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
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPlaceAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo(contentPlace);

        }

        /// <summary>
        /// Update a existing dynamic content place object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentPlace">dynamic content place object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPlaceAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPlace contentPlace)
        {
            // verify the required parameter 'contentPlace' is set
            if (contentPlace == null)
                throw new ApiException(400, "Missing required parameter 'contentPlace' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPlace");

            var localVarPath = "/api/marketing/contentplaces";
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
            if (contentPlace.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(contentPlace); // http body (model) parameter
            }
            else
            {
                localVarPostBody = contentPlace; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPlace: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns></returns>
        public void MarketingModuleDynamicContentUpdateDynamicContentPublication (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo(publication);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModuleDynamicContentUpdateDynamicContentPublicationWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
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
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModuleDynamicContentUpdateDynamicContentPublicationAsync (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
             await MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo(publication);

        }

        /// <summary>
        /// Update a existing dynamic content publication object 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="publication">dynamic content publication object that needs to be updated in the dynamic content system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModuleDynamicContentUpdateDynamicContentPublicationAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelDynamicContentPublication publication)
        {
            // verify the required parameter 'publication' is set
            if (publication == null)
                throw new ApiException(400, "Missing required parameter 'publication' when calling MarketingModuleApi->MarketingModuleDynamicContentUpdateDynamicContentPublication");

            var localVarPath = "/api/marketing/contentpublications";
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
            if (publication.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(publication); // http body (model) parameter
            }
            else
            {
                localVarPostBody = publication; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleDynamicContentUpdateDynamicContentPublication: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionCreatePromotion (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = MarketingModulePromotionCreatePromotionWithHttpInfo(promotion);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionCreatePromotionWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionCreatePromotion");

            var localVarPath = "/api/marketing/promotions";
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
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionCreatePromotionAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = await MarketingModulePromotionCreatePromotionAsyncWithHttpInfo(promotion);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Add new dynamic promotion object to marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">dynamic promotion object that needs to be added to the marketing system</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionCreatePromotionAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionCreatePromotion");

            var localVarPath = "/api/marketing/promotions";
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
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionCreatePromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns></returns>
        public void MarketingModulePromotionDeletePromotions (List<string> ids)
        {
             MarketingModulePromotionDeletePromotionsWithHttpInfo(ids);
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModulePromotionDeletePromotionsWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModulePromotionDeletePromotions");

            var localVarPath = "/api/marketing/promotions";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionDeletePromotionsAsync (List<string> ids)
        {
             await MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo(ids);

        }

        /// <summary>
        /// Delete promotions objects 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="ids">promotions object ids for delete in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionDeletePromotionsAsyncWithHttpInfo (List<string> ids)
        {
            // verify the required parameter 'ids' is set
            if (ids == null)
                throw new ApiException(400, "Missing required parameter 'ids' when calling MarketingModuleApi->MarketingModulePromotionDeletePromotions");

            var localVarPath = "/api/marketing/promotions";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionDeletePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public List<VirtoCommerceMarketingModuleWebModelPromotionReward> MarketingModulePromotionEvaluatePromotions (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> localVarResponse = MarketingModulePromotionEvaluatePromotionsWithHttpInfo(context);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public ApiResponse< List<VirtoCommerceMarketingModuleWebModelPromotionReward> > MarketingModulePromotionEvaluatePromotionsWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null)
                throw new ApiException(400, "Missing required parameter 'context' when calling MarketingModuleApi->MarketingModulePromotionEvaluatePromotions");

            var localVarPath = "/api/marketing/promotions/evaluate";
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
            if (context.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(context); // http body (model) parameter
            }
            else
            {
                localVarPostBody = context; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelPromotionReward>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceMarketingModuleWebModelPromotionReward>)));
            
        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> MarketingModulePromotionEvaluatePromotionsAsync (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
             ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>> localVarResponse = await MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo(context);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Evaluate promotions 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="context">Promotion evaluation context</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceMarketingModuleWebModelPromotionReward&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>> MarketingModulePromotionEvaluatePromotionsAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelPromotionEvaluationContext context)
        {
            // verify the required parameter 'context' is set
            if (context == null)
                throw new ApiException(400, "Missing required parameter 'context' when calling MarketingModuleApi->MarketingModulePromotionEvaluatePromotions");

            var localVarPath = "/api/marketing/promotions/evaluate";
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
            if (context.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(context); // http body (model) parameter
            }
            else
            {
                localVarPostBody = context; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionEvaluatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceMarketingModuleWebModelPromotionReward>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceMarketingModuleWebModelPromotionReward>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceMarketingModuleWebModelPromotionReward>)));
            
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetNewDynamicPromotion ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo();
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionGetNewDynamicPromotionWithHttpInfo ()
        {

            var localVarPath = "/api/marketing/promotions/new";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetNewDynamicPromotionAsync ()
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = await MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo();
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get new dynamic promotion object Return a new dynamic promotion object with populated dynamic expression tree
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetNewDynamicPromotionAsyncWithHttpInfo ()
        {

            var localVarPath = "/api/marketing/promotions/new";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetNewDynamicPromotion: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public VirtoCommerceMarketingModuleWebModelPromotion MarketingModulePromotionGetPromotionById (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = MarketingModulePromotionGetPromotionByIdWithHttpInfo(id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelPromotion > MarketingModulePromotionGetPromotionByIdWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModulePromotionGetPromotionById");

            var localVarPath = "/api/marketing/promotions/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelPromotion</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelPromotion> MarketingModulePromotionGetPromotionByIdAsync (string id)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion> localVarResponse = await MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo(id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Find promotion object by id Return a single promotion (dynamic or custom) object
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="id">promotion id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelPromotion)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>> MarketingModulePromotionGetPromotionByIdAsyncWithHttpInfo (string id)
        {
            // verify the required parameter 'id' is set
            if (id == null)
                throw new ApiException(400, "Missing required parameter 'id' when calling MarketingModuleApi->MarketingModulePromotionGetPromotionById");

            var localVarPath = "/api/marketing/promotions/{id}";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionGetPromotionById: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelPromotion>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelPromotion) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelPromotion)));
            
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns></returns>
        public void MarketingModulePromotionUpdatePromotions (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             MarketingModulePromotionUpdatePromotionsWithHttpInfo(promotion);
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MarketingModulePromotionUpdatePromotionsWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionUpdatePromotions");

            var localVarPath = "/api/marketing/promotions";
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
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MarketingModulePromotionUpdatePromotionsAsync (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
             await MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo(promotion);

        }

        /// <summary>
        /// Update a existing dynamic promotion object in marketing system 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="promotion">&amp;gt;dynamic promotion object that needs to be updated in the marketing system</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MarketingModulePromotionUpdatePromotionsAsyncWithHttpInfo (VirtoCommerceMarketingModuleWebModelPromotion promotion)
        {
            // verify the required parameter 'promotion' is set
            if (promotion == null)
                throw new ApiException(400, "Missing required parameter 'promotion' when calling MarketingModuleApi->MarketingModulePromotionUpdatePromotions");

            var localVarPath = "/api/marketing/promotions";
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
            if (promotion.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(promotion); // http body (model) parameter
            }
            else
            {
                localVarPostBody = promotion; // byte array
            }


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModulePromotionUpdatePromotions: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public VirtoCommerceMarketingModuleWebModelMarketingSearchResult MarketingModuleSearch (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> localVarResponse = MarketingModuleSearchWithHttpInfo(criteria);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>ApiResponse of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public ApiResponse< VirtoCommerceMarketingModuleWebModelMarketingSearchResult > MarketingModuleSearchWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling MarketingModuleApi->MarketingModuleSearch");

            var localVarPath = "/api/marketing/search";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelMarketingSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelMarketingSearchResult)));
            
        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of VirtoCommerceMarketingModuleWebModelMarketingSearchResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> MarketingModuleSearchAsync (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
             ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult> localVarResponse = await MarketingModuleSearchAsyncWithHttpInfo(criteria);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search marketing objects by given criteria Allow to find all marketing module objects (Promotions, Dynamic content objects)
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="criteria">criteria</param>
        /// <returns>Task of ApiResponse (VirtoCommerceMarketingModuleWebModelMarketingSearchResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>> MarketingModuleSearchAsyncWithHttpInfo (VirtoCommerceDomainMarketingModelMarketingSearchCriteria criteria)
        {
            // verify the required parameter 'criteria' is set
            if (criteria == null)
                throw new ApiException(400, "Missing required parameter 'criteria' when calling MarketingModuleApi->MarketingModuleSearch");

            var localVarPath = "/api/marketing/search";
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
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MarketingModuleSearch: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceMarketingModuleWebModelMarketingSearchResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceMarketingModuleWebModelMarketingSearchResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceMarketingModuleWebModelMarketingSearchResult)));
            
        }

    }
}
