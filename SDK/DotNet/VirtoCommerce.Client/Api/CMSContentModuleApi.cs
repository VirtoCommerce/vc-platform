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
    public interface ICMSContentModuleApi
    {
        #region Synchronous Operations
        
        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id = null);
  
        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameWithHttpInfo (string storeId, string name, string language, string id = null);
        
        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        void MenuDelete (string listId, string storeId);
  
        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MenuDeleteWithHttpInfo (string listId, string storeId);
        
        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>
        VirtoCommerceContentWebModelsMenuLinkList MenuGetList (string storeId, string listId);
  
        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsMenuLinkList</returns>
        ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListWithHttpInfo (string storeId, string listId);
        
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId);
  
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsWithHttpInfo (string storeId);
        
        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        void MenuUpdate (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);
  
        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MenuUpdateWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);
        
        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        VirtoCommerceContentWebModelsCheckNameResult PagesCheckName (string storeId, string pageName, string language);
  
        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameWithHttpInfo (string storeId, string pageName, string language);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns></returns>
        void PagesCreateBlog (string storeId, string blogName);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesCreateBlogWithHttpInfo (string storeId, string blogName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns></returns>
        void PagesDeleteBlog (string storeId, string blogName);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesDeleteBlogWithHttpInfo (string storeId, string blogName);
        
        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns></returns>
        void PagesDeleteItem (string storeId, List<string> pageNamesAndLanguges);
  
        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesDeleteItemWithHttpInfo (string storeId, List<string> pageNamesAndLanguges);
        
        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        VirtoCommerceContentWebModelsGetPagesResult PagesGetFolders (string storeId);
  
        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsGetPagesResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersWithHttpInfo (string storeId);
        
        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>VirtoCommerceContentWebModelsPage</returns>
        VirtoCommerceContentWebModelsPage PagesGetPage (string storeId, string language, string pageName);
  
        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsPage</returns>
        ApiResponse<VirtoCommerceContentWebModelsPage> PagesGetPageWithHttpInfo (string storeId, string language, string pageName);
        
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId);
  
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesWithHttpInfo (string storeId);
        
        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns></returns>
        void PagesSaveItem (string storeId, VirtoCommerceContentWebModelsPage page);
  
        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesSaveItemWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns></returns>
        void PagesUpdateBlog (string storeId, string blogName, string oldBlogName);
  
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesUpdateBlogWithHttpInfo (string storeId, string blogName, string oldBlogName);
        
        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        void ThemeCreateDefaultTheme (string storeId);
  
        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeCreateDefaultThemeWithHttpInfo (string storeId);
        
        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns></returns>
        void ThemeCreateNewTheme (string storeId, string themeFileUrl, string themeName);
  
        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeCreateNewThemeWithHttpInfo (string storeId, string themeFileUrl, string themeName);
        
        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns></returns>
        void ThemeDeleteAssets (string storeId, string themeId, List<string> assetIds);
  
        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeDeleteAssetsWithHttpInfo (string storeId, string themeId, List<string> assetIds);
        
        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        void ThemeDeleteTheme (string storeId, string themeId);
  
        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeDeleteThemeWithHttpInfo (string storeId, string themeId);
        
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>VirtoCommerceContentWebModelsThemeAsset</returns>
        VirtoCommerceContentWebModelsThemeAsset ThemeGetThemeAsset (string assetId, string storeId, string themeId);
  
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsThemeAsset</returns>
        ApiResponse<VirtoCommerceContentWebModelsThemeAsset> ThemeGetThemeAssetWithHttpInfo (string assetId, string storeId, string themeId);
        
        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        List<VirtoCommerceContentWebModelsThemeAssetFolder> ThemeGetThemeAssets (string storeId, string themeId);
  
        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsWithHttpInfo (string storeId, string themeId);
        
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId);
  
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesWithHttpInfo (string storeId);
        
        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        void ThemeSaveItem (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);
  
        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeSaveItemWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);
        
        #endregion Synchronous Operations
        
        #region Asynchronous Operations
        
        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id = null);

        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null);
        
        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MenuDeleteAsync (string listId, string storeId);

        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MenuDeleteAsyncWithHttpInfo (string listId, string storeId);
        
        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsMenuLinkList</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListAsync (string storeId, string listId);

        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsMenuLinkList)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListAsyncWithHttpInfo (string storeId, string listId);
        
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId);

        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>> MenuGetListsAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task MenuUpdateAsync (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);

        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MenuUpdateAsyncWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);
        
        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameAsync (string storeId, string pageName, string language);

        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> PagesCheckNameAsyncWithHttpInfo (string storeId, string pageName, string language);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PagesCreateBlogAsync (string storeId, string blogName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesCreateBlogAsyncWithHttpInfo (string storeId, string blogName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PagesDeleteBlogAsync (string storeId, string blogName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteBlogAsyncWithHttpInfo (string storeId, string blogName);
        
        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PagesDeleteItemAsync (string storeId, List<string> pageNamesAndLanguges);

        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteItemAsyncWithHttpInfo (string storeId, List<string> pageNamesAndLanguges);
        
        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsGetPagesResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId);

        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsGetPagesResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>> PagesGetFoldersAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of VirtoCommerceContentWebModelsPage</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsPage> PagesGetPageAsync (string storeId, string language, string pageName);

        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsPage)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsPage>> PagesGetPageAsyncWithHttpInfo (string storeId, string language, string pageName);
        
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId);

        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsPage&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsPage>>> PagesGetPagesAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PagesSaveItemAsync (string storeId, VirtoCommerceContentWebModelsPage page);

        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesSaveItemAsyncWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task PagesUpdateBlogAsync (string storeId, string blogName, string oldBlogName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesUpdateBlogAsyncWithHttpInfo (string storeId, string blogName, string oldBlogName);
        
        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId);

        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateDefaultThemeAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeCreateNewThemeAsync (string storeId, string themeFileUrl, string themeName);

        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateNewThemeAsyncWithHttpInfo (string storeId, string themeFileUrl, string themeName);
        
        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeDeleteAssetsAsync (string storeId, string themeId, List<string> assetIds);

        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteAssetsAsyncWithHttpInfo (string storeId, string themeId, List<string> assetIds);
        
        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeDeleteThemeAsync (string storeId, string themeId);

        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteThemeAsyncWithHttpInfo (string storeId, string themeId);
        
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsThemeAsset</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsThemeAsset> ThemeGetThemeAssetAsync (string assetId, string storeId, string themeId);

        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsThemeAsset)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsThemeAsset>> ThemeGetThemeAssetAsyncWithHttpInfo (string assetId, string storeId, string themeId);
        
        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsAsync (string storeId, string themeId);

        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>> ThemeGetThemeAssetsAsyncWithHttpInfo (string storeId, string themeId);
        
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId);

        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsTheme&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsTheme>>> ThemeGetThemesAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeSaveItemAsync (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);

        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeSaveItemAsyncWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);
        
        #endregion Asynchronous Operations
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CMSContentModuleApi : ICMSContentModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CMSContentModuleApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CMSContentModuleApi(Configuration configuration)
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
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="name">Name of menu link list</param> 
        /// <param name="language">Language of menu link list</param> 
        /// <param name="id">Menu link list id (optional)</param> 
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id = null)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> localVarResponse = MenuCheckNameWithHttpInfo(storeId, name, language, id);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="name">Name of menu link list</param> 
        /// <param name="language">Language of menu link list</param> 
        /// <param name="id">Menu link list id (optional)</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public ApiResponse< VirtoCommerceContentWebModelsCheckNameResult > MenuCheckNameWithHttpInfo (string storeId, string name, string language, string id = null)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuCheckName");
            
            // verify the required parameter 'name' is set
            if (name == null)
                throw new ApiException(400, "Missing required parameter 'name' when calling CMSContentModuleApi->MenuCheckName");
            
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling CMSContentModuleApi->MenuCheckName");
            
    
            var localVarPath = "/api/cms/{storeId}/menu/checkname";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (name != null) localVarQueryParams.Add("name", Configuration.ApiClient.ParameterToString(name)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (id != null) localVarQueryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuCheckName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuCheckName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }

        
        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id = null)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> localVarResponse = await MenuCheckNameAsyncWithHttpInfo(storeId, name, language, id);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuCheckName");
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling MenuCheckName");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MenuCheckName");
            
    
            var localVarPath = "/api/cms/{storeId}/menu/checkname";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (name != null) localVarQueryParams.Add("name", Configuration.ApiClient.ParameterToString(name)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (id != null) localVarQueryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuCheckName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuCheckName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
        
        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param> 
        /// <param name="storeId"></param> 
        /// <returns></returns>
        public void MenuDelete (string listId, string storeId)
        {
             MenuDeleteWithHttpInfo(listId, storeId);
        }

        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param> 
        /// <param name="storeId"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MenuDeleteWithHttpInfo (string listId, string storeId)
        {
            
            // verify the required parameter 'listId' is set
            if (listId == null)
                throw new ApiException(400, "Missing required parameter 'listId' when calling CMSContentModuleApi->MenuDelete");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuDelete");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (listId != null) localVarQueryParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MenuDeleteAsync (string listId, string storeId)
        {
             await MenuDeleteAsyncWithHttpInfo(listId, storeId);

        }

        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MenuDeleteAsyncWithHttpInfo (string listId, string storeId)
        {
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuDelete");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuDelete");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (listId != null) localVarQueryParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuDelete: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuDelete: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="listId">List id</param> 
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>
        public VirtoCommerceContentWebModelsMenuLinkList MenuGetList (string storeId, string listId)
        {
             ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> localVarResponse = MenuGetListWithHttpInfo(storeId, listId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="listId">List id</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsMenuLinkList</returns>
        public ApiResponse< VirtoCommerceContentWebModelsMenuLinkList > MenuGetListWithHttpInfo (string storeId, string listId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuGetList");
            
            // verify the required parameter 'listId' is set
            if (listId == null)
                throw new ApiException(400, "Missing required parameter 'listId' when calling CMSContentModuleApi->MenuGetList");
            
    
            var localVarPath = "/api/cms/{storeId}/menu/{listId}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (listId != null) localVarPathParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuGetList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuGetList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsMenuLinkList) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsMenuLinkList)));
            
        }

        
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsMenuLinkList</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListAsync (string storeId, string listId)
        {
             ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> localVarResponse = await MenuGetListAsyncWithHttpInfo(storeId, listId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsMenuLinkList)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListAsyncWithHttpInfo (string storeId, string listId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetList");
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuGetList");
            
    
            var localVarPath = "/api/cms/{storeId}/menu/{listId}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (listId != null) localVarPathParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuGetList: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuGetList: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsMenuLinkList) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsMenuLinkList)));
            
        }
        
        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> localVarResponse = MenuGetListsWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsMenuLinkList> > MenuGetListsWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuGetLists");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuGetLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuGetLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsMenuLinkList>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>)));
            
        }

        
        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> localVarResponse = await MenuGetListsAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>> MenuGetListsAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetLists");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuGetLists: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuGetLists: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsMenuLinkList>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>)));
            
        }
        
        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param> 
        /// <param name="storeId"></param> 
        /// <returns></returns>
        public void MenuUpdate (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
             MenuUpdateWithHttpInfo(list, storeId);
        }

        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param> 
        /// <param name="storeId"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> MenuUpdateWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
            
            // verify the required parameter 'list' is set
            if (list == null)
                throw new ApiException(400, "Missing required parameter 'list' when calling CMSContentModuleApi->MenuUpdate");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuUpdate");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (list.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(list); // http body (model) parameter
            }
            else
            {
                localVarPostBody = list; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task MenuUpdateAsync (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
             await MenuUpdateAsyncWithHttpInfo(list, storeId);

        }

        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MenuUpdateAsyncWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
            // verify the required parameter 'list' is set
            if (list == null) throw new ApiException(400, "Missing required parameter 'list' when calling MenuUpdate");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuUpdate");
            
    
            var localVarPath = "/api/cms/{storeId}/menu";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (list.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(list); // http body (model) parameter
            }
            else
            {
                localVarPostBody = list; // byte array
            }

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling MenuUpdate: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling MenuUpdate: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageName">Page name</param> 
        /// <param name="language">Page language</param> 
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public VirtoCommerceContentWebModelsCheckNameResult PagesCheckName (string storeId, string pageName, string language)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> localVarResponse = PagesCheckNameWithHttpInfo(storeId, pageName, language);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageName">Page name</param> 
        /// <param name="language">Page language</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public ApiResponse< VirtoCommerceContentWebModelsCheckNameResult > PagesCheckNameWithHttpInfo (string storeId, string pageName, string language)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesCheckName");
            
            // verify the required parameter 'pageName' is set
            if (pageName == null)
                throw new ApiException(400, "Missing required parameter 'pageName' when calling CMSContentModuleApi->PagesCheckName");
            
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling CMSContentModuleApi->PagesCheckName");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/checkname";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageName != null) localVarQueryParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesCheckName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesCheckName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }

        
        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameAsync (string storeId, string pageName, string language)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> localVarResponse = await PagesCheckNameAsyncWithHttpInfo(storeId, pageName, language);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> PagesCheckNameAsyncWithHttpInfo (string storeId, string pageName, string language)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCheckName");
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesCheckName");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesCheckName");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/checkname";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageName != null) localVarQueryParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) localVarQueryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesCheckName: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesCheckName: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns></returns>
        public void PagesCreateBlog (string storeId, string blogName)
        {
             PagesCreateBlogWithHttpInfo(storeId, blogName);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PagesCreateBlogWithHttpInfo (string storeId, string blogName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesCreateBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null)
                throw new ApiException(400, "Missing required parameter 'blogName' when calling CMSContentModuleApi->PagesCreateBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesCreateBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesCreateBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PagesCreateBlogAsync (string storeId, string blogName)
        {
             await PagesCreateBlogAsyncWithHttpInfo(storeId, blogName);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesCreateBlogAsyncWithHttpInfo (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCreateBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesCreateBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesCreateBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesCreateBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns></returns>
        public void PagesDeleteBlog (string storeId, string blogName)
        {
             PagesDeleteBlogWithHttpInfo(storeId, blogName);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PagesDeleteBlogWithHttpInfo (string storeId, string blogName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesDeleteBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null)
                throw new ApiException(400, "Missing required parameter 'blogName' when calling CMSContentModuleApi->PagesDeleteBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PagesDeleteBlogAsync (string storeId, string blogName)
        {
             await PagesDeleteBlogAsyncWithHttpInfo(storeId, blogName);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteBlogAsyncWithHttpInfo (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesDeleteBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param> 
        /// <returns></returns>
        public void PagesDeleteItem (string storeId, List<string> pageNamesAndLanguges)
        {
             PagesDeleteItemWithHttpInfo(storeId, pageNamesAndLanguges);
        }

        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PagesDeleteItemWithHttpInfo (string storeId, List<string> pageNamesAndLanguges)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesDeleteItem");
            
            // verify the required parameter 'pageNamesAndLanguges' is set
            if (pageNamesAndLanguges == null)
                throw new ApiException(400, "Missing required parameter 'pageNamesAndLanguges' when calling CMSContentModuleApi->PagesDeleteItem");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageNamesAndLanguges != null) localVarQueryParams.Add("pageNamesAndLanguges", Configuration.ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PagesDeleteItemAsync (string storeId, List<string> pageNamesAndLanguges)
        {
             await PagesDeleteItemAsyncWithHttpInfo(storeId, pageNamesAndLanguges);

        }

        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteItemAsyncWithHttpInfo (string storeId, List<string> pageNamesAndLanguges)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteItem");
            // verify the required parameter 'pageNamesAndLanguges' is set
            if (pageNamesAndLanguges == null) throw new ApiException(400, "Missing required parameter 'pageNamesAndLanguges' when calling PagesDeleteItem");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageNamesAndLanguges != null) localVarQueryParams.Add("pageNamesAndLanguges", Configuration.ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesDeleteItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        public VirtoCommerceContentWebModelsGetPagesResult PagesGetFolders (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> localVarResponse = PagesGetFoldersWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsGetPagesResult</returns>
        public ApiResponse< VirtoCommerceContentWebModelsGetPagesResult > PagesGetFoldersWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesGetFolders");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/folders";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsGetPagesResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsGetPagesResult)));
            
        }

        
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsGetPagesResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> localVarResponse = await PagesGetFoldersAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsGetPagesResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>> PagesGetFoldersAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetFolders");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/folders";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetFolders: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetFolders: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsGetPagesResult) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsGetPagesResult)));
            
        }
        
        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="language">Page language</param> 
        /// <param name="pageName">Page name</param> 
        /// <returns>VirtoCommerceContentWebModelsPage</returns>
        public VirtoCommerceContentWebModelsPage PagesGetPage (string storeId, string language, string pageName)
        {
             ApiResponse<VirtoCommerceContentWebModelsPage> localVarResponse = PagesGetPageWithHttpInfo(storeId, language, pageName);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="language">Page language</param> 
        /// <param name="pageName">Page name</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsPage</returns>
        public ApiResponse< VirtoCommerceContentWebModelsPage > PagesGetPageWithHttpInfo (string storeId, string language, string pageName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesGetPage");
            
            // verify the required parameter 'language' is set
            if (language == null)
                throw new ApiException(400, "Missing required parameter 'language' when calling CMSContentModuleApi->PagesGetPage");
            
            // verify the required parameter 'pageName' is set
            if (pageName == null)
                throw new ApiException(400, "Missing required parameter 'pageName' when calling CMSContentModuleApi->PagesGetPage");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (language != null) localVarPathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) localVarPathParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPage: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPage: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsPage>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsPage) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsPage)));
            
        }

        
        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of VirtoCommerceContentWebModelsPage</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsPage> PagesGetPageAsync (string storeId, string language, string pageName)
        {
             ApiResponse<VirtoCommerceContentWebModelsPage> localVarResponse = await PagesGetPageAsyncWithHttpInfo(storeId, language, pageName);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsPage)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsPage>> PagesGetPageAsyncWithHttpInfo (string storeId, string language, string pageName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPage");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesGetPage");
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesGetPage");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (language != null) localVarPathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) localVarPathParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPage: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPage: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsPage>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsPage) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsPage)));
            
        }
        
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsPage>> localVarResponse = PagesGetPagesWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsPage> > PagesGetPagesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesGetPages");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPages: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPages: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsPage>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsPage>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsPage>)));
            
        }

        
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsPage>> localVarResponse = await PagesGetPagesAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsPage&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsPage>>> PagesGetPagesAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPages");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPages: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesGetPages: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsPage>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsPage>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsPage>)));
            
        }
        
        /// <summary>
        /// Save page 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="page">Page</param> 
        /// <returns></returns>
        public void PagesSaveItem (string storeId, VirtoCommerceContentWebModelsPage page)
        {
             PagesSaveItemWithHttpInfo(storeId, page);
        }

        /// <summary>
        /// Save page 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param> 
        /// <param name="page">Page</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PagesSaveItemWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesSaveItem");
            
            // verify the required parameter 'page' is set
            if (page == null)
                throw new ApiException(400, "Missing required parameter 'page' when calling CMSContentModuleApi->PagesSaveItem");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (page.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(page); // http body (model) parameter
            }
            else
            {
                localVarPostBody = page; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesSaveItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesSaveItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Save page 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PagesSaveItemAsync (string storeId, VirtoCommerceContentWebModelsPage page)
        {
             await PagesSaveItemAsyncWithHttpInfo(storeId, page);

        }

        /// <summary>
        /// Save page 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesSaveItemAsyncWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesSaveItem");
            // verify the required parameter 'page' is set
            if (page == null) throw new ApiException(400, "Missing required parameter 'page' when calling PagesSaveItem");
            
    
            var localVarPath = "/api/cms/{storeId}/pages";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (page.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(page); // http body (model) parameter
            }
            else
            {
                localVarPostBody = page; // byte array
            }

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesSaveItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesSaveItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <param name="oldBlogName"></param> 
        /// <returns></returns>
        public void PagesUpdateBlog (string storeId, string blogName, string oldBlogName)
        {
             PagesUpdateBlogWithHttpInfo(storeId, blogName, oldBlogName);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <param name="oldBlogName"></param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> PagesUpdateBlogWithHttpInfo (string storeId, string blogName, string oldBlogName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesUpdateBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null)
                throw new ApiException(400, "Missing required parameter 'blogName' when calling CMSContentModuleApi->PagesUpdateBlog");
            
            // verify the required parameter 'oldBlogName' is set
            if (oldBlogName == null)
                throw new ApiException(400, "Missing required parameter 'oldBlogName' when calling CMSContentModuleApi->PagesUpdateBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) localVarPathParams.Add("oldBlogName", Configuration.ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesUpdateBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesUpdateBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task PagesUpdateBlogAsync (string storeId, string blogName, string oldBlogName)
        {
             await PagesUpdateBlogAsyncWithHttpInfo(storeId, blogName, oldBlogName);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesUpdateBlogAsyncWithHttpInfo (string storeId, string blogName, string oldBlogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesUpdateBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesUpdateBlog");
            // verify the required parameter 'oldBlogName' is set
            if (oldBlogName == null) throw new ApiException(400, "Missing required parameter 'oldBlogName' when calling PagesUpdateBlog");
            
    
            var localVarPath = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) localVarPathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) localVarPathParams.Add("oldBlogName", Configuration.ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling PagesUpdateBlog: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling PagesUpdateBlog: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>
        public void ThemeCreateDefaultTheme (string storeId)
        {
             ThemeCreateDefaultThemeWithHttpInfo(storeId);
        }

        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeCreateDefaultThemeWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeCreateDefaultTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/createdefault";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateDefaultTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateDefaultTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId)
        {
             await ThemeCreateDefaultThemeAsyncWithHttpInfo(storeId);

        }

        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateDefaultThemeAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateDefaultTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/createdefault";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateDefaultTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateDefaultTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeFileUrl">Theme file url</param> 
        /// <param name="themeName">Theme name</param> 
        /// <returns></returns>
        public void ThemeCreateNewTheme (string storeId, string themeFileUrl, string themeName)
        {
             ThemeCreateNewThemeWithHttpInfo(storeId, themeFileUrl, themeName);
        }

        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeFileUrl">Theme file url</param> 
        /// <param name="themeName">Theme name</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeCreateNewThemeWithHttpInfo (string storeId, string themeFileUrl, string themeName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeCreateNewTheme");
            
            // verify the required parameter 'themeFileUrl' is set
            if (themeFileUrl == null)
                throw new ApiException(400, "Missing required parameter 'themeFileUrl' when calling CMSContentModuleApi->ThemeCreateNewTheme");
            
            // verify the required parameter 'themeName' is set
            if (themeName == null)
                throw new ApiException(400, "Missing required parameter 'themeName' when calling CMSContentModuleApi->ThemeCreateNewTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/file";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (themeFileUrl != null) localVarQueryParams.Add("themeFileUrl", Configuration.ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) localVarQueryParams.Add("themeName", Configuration.ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateNewTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateNewTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeCreateNewThemeAsync (string storeId, string themeFileUrl, string themeName)
        {
             await ThemeCreateNewThemeAsyncWithHttpInfo(storeId, themeFileUrl, themeName);

        }

        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateNewThemeAsyncWithHttpInfo (string storeId, string themeFileUrl, string themeName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateNewTheme");
            // verify the required parameter 'themeFileUrl' is set
            if (themeFileUrl == null) throw new ApiException(400, "Missing required parameter 'themeFileUrl' when calling ThemeCreateNewTheme");
            // verify the required parameter 'themeName' is set
            if (themeName == null) throw new ApiException(400, "Missing required parameter 'themeName' when calling ThemeCreateNewTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/file";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (themeFileUrl != null) localVarQueryParams.Add("themeFileUrl", Configuration.ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) localVarQueryParams.Add("themeName", Configuration.ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateNewTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeCreateNewTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <param name="assetIds">Deleted asset ids</param> 
        /// <returns></returns>
        public void ThemeDeleteAssets (string storeId, string themeId, List<string> assetIds)
        {
             ThemeDeleteAssetsWithHttpInfo(storeId, themeId, assetIds);
        }

        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <param name="assetIds">Deleted asset ids</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeDeleteAssetsWithHttpInfo (string storeId, string themeId, List<string> assetIds)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeDeleteAssets");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null)
                throw new ApiException(400, "Missing required parameter 'themeId' when calling CMSContentModuleApi->ThemeDeleteAssets");
            
            // verify the required parameter 'assetIds' is set
            if (assetIds == null)
                throw new ApiException(400, "Missing required parameter 'assetIds' when calling CMSContentModuleApi->ThemeDeleteAssets");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) localVarQueryParams.Add("assetIds", Configuration.ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteAssets: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteAssets: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeDeleteAssetsAsync (string storeId, string themeId, List<string> assetIds)
        {
             await ThemeDeleteAssetsAsyncWithHttpInfo(storeId, themeId, assetIds);

        }

        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteAssetsAsyncWithHttpInfo (string storeId, string themeId, List<string> assetIds)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteAssets");
            // verify the required parameter 'assetIds' is set
            if (assetIds == null) throw new ApiException(400, "Missing required parameter 'assetIds' when calling ThemeDeleteAssets");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) localVarQueryParams.Add("assetIds", Configuration.ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteAssets: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteAssets: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns></returns>
        public void ThemeDeleteTheme (string storeId, string themeId)
        {
             ThemeDeleteThemeWithHttpInfo(storeId, themeId);
        }

        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeDeleteThemeWithHttpInfo (string storeId, string themeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeDeleteTheme");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null)
                throw new ApiException(400, "Missing required parameter 'themeId' when calling CMSContentModuleApi->ThemeDeleteTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeDeleteThemeAsync (string storeId, string themeId)
        {
             await ThemeDeleteThemeAsyncWithHttpInfo(storeId, themeId);

        }

        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteThemeAsyncWithHttpInfo (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteTheme");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteTheme");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteTheme: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeDeleteTheme: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>VirtoCommerceContentWebModelsThemeAsset</returns>
        public VirtoCommerceContentWebModelsThemeAsset ThemeGetThemeAsset (string assetId, string storeId, string themeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsThemeAsset> localVarResponse = ThemeGetThemeAssetWithHttpInfo(assetId, storeId, themeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsThemeAsset</returns>
        public ApiResponse< VirtoCommerceContentWebModelsThemeAsset > ThemeGetThemeAssetWithHttpInfo (string assetId, string storeId, string themeId)
        {
            
            // verify the required parameter 'assetId' is set
            if (assetId == null)
                throw new ApiException(400, "Missing required parameter 'assetId' when calling CMSContentModuleApi->ThemeGetThemeAsset");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeGetThemeAsset");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null)
                throw new ApiException(400, "Missing required parameter 'themeId' when calling CMSContentModuleApi->ThemeGetThemeAsset");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (assetId != null) localVarPathParams.Add("assetId", Configuration.ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsThemeAsset>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsThemeAsset) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsThemeAsset)));
            
        }

        
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsThemeAsset</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsThemeAsset> ThemeGetThemeAssetAsync (string assetId, string storeId, string themeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsThemeAsset> localVarResponse = await ThemeGetThemeAssetAsyncWithHttpInfo(assetId, storeId, themeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsThemeAsset)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsThemeAsset>> ThemeGetThemeAssetAsyncWithHttpInfo (string assetId, string storeId, string themeId)
        {
            // verify the required parameter 'assetId' is set
            if (assetId == null) throw new ApiException(400, "Missing required parameter 'assetId' when calling ThemeGetThemeAsset");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAsset");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAsset");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (assetId != null) localVarPathParams.Add("assetId", Configuration.ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAsset: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAsset: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsThemeAsset>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsThemeAsset) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsThemeAsset)));
            
        }
        
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        public List<VirtoCommerceContentWebModelsThemeAssetFolder> ThemeGetThemeAssets (string storeId, string themeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> localVarResponse = ThemeGetThemeAssetsWithHttpInfo(storeId, themeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsThemeAssetFolder> > ThemeGetThemeAssetsWithHttpInfo (string storeId, string themeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeGetThemeAssets");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null)
                throw new ApiException(400, "Missing required parameter 'themeId' when calling CMSContentModuleApi->ThemeGetThemeAssets");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAssets: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAssets: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsThemeAssetFolder>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>)));
            
        }

        
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsAsync (string storeId, string themeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> localVarResponse = await ThemeGetThemeAssetsAsyncWithHttpInfo(storeId, themeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>> ThemeGetThemeAssetsAsyncWithHttpInfo (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAssets");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAssets: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemeAssets: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsThemeAssetFolder>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>)));
            
        }
        
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsTheme>> localVarResponse = ThemeGetThemesWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsTheme> > ThemeGetThemesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeGetThemes");
            
    
            var localVarPath = "/api/cms/{storeId}/themes";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsTheme>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsTheme>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsTheme>)));
            
        }

        
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsTheme>> localVarResponse = await ThemeGetThemesAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsTheme&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsTheme>>> ThemeGetThemesAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemes");
            
    
            var localVarPath = "/api/cms/{storeId}/themes";
    
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
                "application/json", "text/json"
            };
            String localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            // set "format" to json by default
            // e.g. /pet/{petId}.{format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemes: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeGetThemes: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsTheme>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsTheme>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsTheme>)));
            
        }
        
        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns></returns>
        public void ThemeSaveItem (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId)
        {
             ThemeSaveItemWithHttpInfo(asset, storeId, themeId);
        }

        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeSaveItemWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId)
        {
            
            // verify the required parameter 'asset' is set
            if (asset == null)
                throw new ApiException(400, "Missing required parameter 'asset' when calling CMSContentModuleApi->ThemeSaveItem");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeSaveItem");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null)
                throw new ApiException(400, "Missing required parameter 'themeId' when calling CMSContentModuleApi->ThemeSaveItem");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            if (asset.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(asset); // http body (model) parameter
            }
            else
            {
                localVarPostBody = asset; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
    
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeSaveItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeSaveItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
    
            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        
        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeSaveItemAsync (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId)
        {
             await ThemeSaveItemAsyncWithHttpInfo(asset, storeId, themeId);

        }

        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeSaveItemAsyncWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId)
        {
            // verify the required parameter 'asset' is set
            if (asset == null) throw new ApiException(400, "Missing required parameter 'asset' when calling ThemeSaveItem");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeSaveItem");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeSaveItem");
            
    
            var localVarPath = "/api/cms/{storeId}/themes/{themeId}/assets";
    
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;

            // to determine the Content-Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json", "text/json", "application/x-www-form-urlencoded"
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) localVarPathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            if (asset.GetType() != typeof(byte[]))
            {
                localVarPostBody = Configuration.ApiClient.Serialize(asset); // http body (model) parameter
            }
            else
            {
                localVarPostBody = asset; // byte array
            }

            

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, 
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, 
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;
 
            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ThemeSaveItem: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ThemeSaveItem: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
    }
    
}
