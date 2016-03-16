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
        
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId);
  
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsWithHttpInfo (string storeId);

        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId);

        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>> MenuGetListsAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MenuUpdateWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);

        /// <summary>
        /// Update menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MenuUpdateAsyncWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);
        
        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> MenuDeleteWithHttpInfo (string listId, string storeId);

        /// <summary>
        /// Delete menu link list
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> MenuDeleteAsyncWithHttpInfo (string listId, string storeId);
        
        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id = null);
  
        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameWithHttpInfo (string storeId, string name, string language, string id = null);

        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id = null);

        /// <summary>
        /// Checking name of menu link list
        /// </summary>
        /// <remarks>
        /// Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null);
        
        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsMenuLinkList</returns>
        ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListWithHttpInfo (string storeId, string listId);

        /// <summary>
        /// Get menu link list by id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsMenuLinkList)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListAsyncWithHttpInfo (string storeId, string listId);
        
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId);
  
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesWithHttpInfo (string storeId);

        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId);

        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsPage&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsPage>>> PagesGetPagesAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesSaveItemWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page);

        /// <summary>
        /// Save page
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesSaveItemAsyncWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page);
        
        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesDeleteItemWithHttpInfo (string storeId, List<string> pageNamesAndLanguges);

        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>
        /// Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteItemAsyncWithHttpInfo (string storeId, List<string> pageNamesAndLanguges);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesDeleteBlogWithHttpInfo (string storeId, string blogName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteBlogAsyncWithHttpInfo (string storeId, string blogName);
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> PagesUpdateBlogWithHttpInfo (string storeId, string blogName, string oldBlogName);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> PagesUpdateBlogAsyncWithHttpInfo (string storeId, string blogName, string oldBlogName);
        
        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsCheckNameResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameWithHttpInfo (string storeId, string pageName, string language);

        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>
        /// Check page pair name+language for store
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> PagesCheckNameAsyncWithHttpInfo (string storeId, string pageName, string language);
        
        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        VirtoCommerceContentWebModelsGetPagesResult PagesGetFolders (string storeId);
  
        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsGetPagesResult</returns>
        ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersWithHttpInfo (string storeId);

        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsGetPagesResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId);

        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsGetPagesResult)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>> PagesGetFoldersAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsPage</returns>
        ApiResponse<VirtoCommerceContentWebModelsPage> PagesGetPageWithHttpInfo (string storeId, string language, string pageName);

        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>
        /// Get page by store and name+language pair.
        /// </remarks>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsPage)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsPage>> PagesGetPageAsyncWithHttpInfo (string storeId, string language, string pageName);
        
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId);
  
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesWithHttpInfo (string storeId);

        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId);

        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsTheme&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsTheme>>> ThemeGetThemesAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        void ThemeCreateDefaultTheme (string storeId);
  
        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeCreateDefaultThemeWithHttpInfo (string storeId);

        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId);

        /// <summary>
        /// Create default theme by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateDefaultThemeAsyncWithHttpInfo (string storeId);
        
        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeCreateNewThemeWithHttpInfo (string storeId, string themeFileUrl, string themeName);

        /// <summary>
        /// Create new theme
        /// </summary>
        /// <remarks>
        /// Create new theme considering store id, theme file url and theme name
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateNewThemeAsyncWithHttpInfo (string storeId, string themeFileUrl, string themeName);
        
        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeDeleteThemeWithHttpInfo (string storeId, string themeId);

        /// <summary>
        /// Delete theme
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id and theme id
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteThemeAsyncWithHttpInfo (string storeId, string themeId);
        
        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
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
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeSaveItemWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);

        /// <summary>
        /// Save theme asset
        /// </summary>
        /// <remarks>
        /// Save theme asset considering store id and theme id
        /// </remarks>
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
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeSaveItemAsyncWithHttpInfo (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);
        
        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ThemeDeleteAssetsWithHttpInfo (string storeId, string themeId, List<string> assetIds);

        /// <summary>
        /// Delete theme assets by assetIds
        /// </summary>
        /// <remarks>
        /// Delete theme assets considering store id, theme id and assetIds
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteAssetsAsyncWithHttpInfo (string storeId, string themeId, List<string> assetIds);
        
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
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
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsThemeAsset</returns>
        ApiResponse<VirtoCommerceContentWebModelsThemeAsset> ThemeGetThemeAssetWithHttpInfo (string assetId, string storeId, string themeId);

        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsWithHttpInfo (string storeId, string themeId);

        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>> ThemeGetThemeAssetsAsyncWithHttpInfo (string storeId, string themeId);
        
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
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> response = MenuGetListsWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsMenuLinkList> > MenuGetListsWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->MenuGetLists");
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuGetLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuGetLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsMenuLinkList>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>)));
            
        }
    
        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>> response = await MenuGetListsAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsMenuLinkList&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>> MenuGetListsAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetLists");
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuGetLists: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuGetLists: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsMenuLinkList>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsMenuLinkList>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>)));
            
        }
        
        /// <summary>
        /// Update menu link list 
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (list.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(list); // http body (model) parameter
            }
            else
            {
                postBody = list; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Update menu link list 
        /// </summary>
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
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MenuUpdateAsyncWithHttpInfo (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
            // verify the required parameter 'list' is set
            if (list == null) throw new ApiException(400, "Missing required parameter 'list' when calling MenuUpdate");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuUpdate");
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(list); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuUpdate: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuUpdate: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete menu link list 
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (listId != null) queryParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete menu link list 
        /// </summary>
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
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> MenuDeleteAsyncWithHttpInfo (string listId, string storeId)
        {
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuDelete");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuDelete");
            
    
            var path_ = "/api/cms/{storeId}/menu";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (listId != null) queryParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuDelete: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuDelete: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="name">Name of menu link list</param> 
        /// <param name="language">Language of menu link list</param> 
        /// <param name="id">Menu link list id</param> 
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id = null)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> response = MenuCheckNameWithHttpInfo(storeId, name, language, id);
             return response.Data;
        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="name">Name of menu link list</param> 
        /// <param name="language">Language of menu link list</param> 
        /// <param name="id">Menu link list id</param> 
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
            
    
            var path_ = "/api/cms/{storeId}/menu/checkname";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (name != null) queryParams.Add("name", Configuration.ApiClient.ParameterToString(name)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (id != null) queryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuCheckName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuCheckName: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
    
        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id = null)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> response = await MenuCheckNameAsyncWithHttpInfo(storeId, name, language, id);
             return response.Data;

        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsCheckNameResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuCheckName");
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling MenuCheckName");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MenuCheckName");
            
    
            var path_ = "/api/cms/{storeId}/menu/checkname";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (name != null) queryParams.Add("name", Configuration.ApiClient.ParameterToString(name)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            if (id != null) queryParams.Add("id", Configuration.ApiClient.ParameterToString(id)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuCheckName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuCheckName: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
        
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="listId">List id</param> 
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>
        public VirtoCommerceContentWebModelsMenuLinkList MenuGetList (string storeId, string listId)
        {
             ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> response = MenuGetListWithHttpInfo(storeId, listId);
             return response.Data;
        }

        /// <summary>
        /// Get menu link list by id 
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/menu/{listId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (listId != null) pathParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuGetList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsMenuLinkList) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsMenuLinkList)));
            
        }
    
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsMenuLinkList</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListAsync (string storeId, string listId)
        {
             ApiResponse<VirtoCommerceContentWebModelsMenuLinkList> response = await MenuGetListAsyncWithHttpInfo(storeId, listId);
             return response.Data;

        }

        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsMenuLinkList)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListAsyncWithHttpInfo (string storeId, string listId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetList");
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuGetList");
            
    
            var path_ = "/api/cms/{storeId}/menu/{listId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (listId != null) pathParams.Add("listId", Configuration.ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling MenuGetList: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling MenuGetList: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsMenuLinkList>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsMenuLinkList) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsMenuLinkList)));
            
        }
        
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsPage>> response = PagesGetPagesWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsPage> > PagesGetPagesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesGetPages");
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetPages: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetPages: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsPage>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsPage>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsPage>)));
            
        }
    
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsPage&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsPage>> response = await PagesGetPagesAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsPage&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsPage>>> PagesGetPagesAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPages");
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetPages: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetPages: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsPage>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsPage>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsPage>)));
            
        }
        
        /// <summary>
        /// Save page 
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            if (page.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(page); // http body (model) parameter
            }
            else
            {
                postBody = page; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesSaveItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesSaveItem: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Save page 
        /// </summary>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesSaveItemAsyncWithHttpInfo (string storeId, VirtoCommerceContentWebModelsPage page)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesSaveItem");
            // verify the required parameter 'page' is set
            if (page == null) throw new ApiException(400, "Missing required parameter 'page' when calling PagesSaveItem");
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(page); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesSaveItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesSaveItem: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageNamesAndLanguges != null) queryParams.Add("pageNamesAndLanguges", Configuration.ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesDeleteItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesDeleteItem: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
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
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteItemAsyncWithHttpInfo (string storeId, List<string> pageNamesAndLanguges)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteItem");
            // verify the required parameter 'pageNamesAndLanguges' is set
            if (pageNamesAndLanguges == null) throw new ApiException(400, "Missing required parameter 'pageNamesAndLanguges' when calling PagesDeleteItem");
            
    
            var path_ = "/api/cms/{storeId}/pages";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageNamesAndLanguges != null) queryParams.Add("pageNamesAndLanguges", Configuration.ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesDeleteItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesDeleteItem: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        ///  
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesCreateBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesCreateBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        ///  
        /// </summary>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesCreateBlogAsyncWithHttpInfo (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCreateBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesCreateBlog");
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesCreateBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesCreateBlog: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        ///  
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesDeleteBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesDeleteBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        ///  
        /// </summary>
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
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> PagesDeleteBlogAsyncWithHttpInfo (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesDeleteBlog");
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesDeleteBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesDeleteBlog: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        ///  
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) pathParams.Add("oldBlogName", Configuration.ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesUpdateBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesUpdateBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        ///  
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (blogName != null) pathParams.Add("blogName", Configuration.ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) pathParams.Add("oldBlogName", Configuration.ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesUpdateBlog: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesUpdateBlog: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageName">Page name</param> 
        /// <param name="language">Page language</param> 
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public VirtoCommerceContentWebModelsCheckNameResult PagesCheckName (string storeId, string pageName, string language)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> response = PagesCheckNameWithHttpInfo(storeId, pageName, language);
             return response.Data;
        }

        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/checkname";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageName != null) queryParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesCheckName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesCheckName: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
    
        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>Task of VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameAsync (string storeId, string pageName, string language)
        {
             ApiResponse<VirtoCommerceContentWebModelsCheckNameResult> response = await PagesCheckNameAsyncWithHttpInfo(storeId, pageName, language);
             return response.Data;

        }

        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/checkname";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (pageName != null) queryParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) queryParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesCheckName: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesCheckName: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsCheckNameResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsCheckNameResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsCheckNameResult)));
            
        }
        
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        public VirtoCommerceContentWebModelsGetPagesResult PagesGetFolders (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> response = PagesGetFoldersWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsGetPagesResult</returns>
        public ApiResponse< VirtoCommerceContentWebModelsGetPagesResult > PagesGetFoldersWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->PagesGetFolders");
            
    
            var path_ = "/api/cms/{storeId}/pages/folders";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetFolders: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetFolders: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsGetPagesResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsGetPagesResult)));
            
        }
    
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsGetPagesResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsGetPagesResult> response = await PagesGetFoldersAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsGetPagesResult)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>> PagesGetFoldersAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetFolders");
            
    
            var path_ = "/api/cms/{storeId}/pages/folders";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetFolders: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetFolders: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsGetPagesResult>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsGetPagesResult) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsGetPagesResult)));
            
        }
        
        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <param name="language">Page language</param> 
        /// <param name="pageName">Page name</param> 
        /// <returns>VirtoCommerceContentWebModelsPage</returns>
        public VirtoCommerceContentWebModelsPage PagesGetPage (string storeId, string language, string pageName)
        {
             ApiResponse<VirtoCommerceContentWebModelsPage> response = PagesGetPageWithHttpInfo(storeId, language, pageName);
             return response.Data;
        }

        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (language != null) pathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) pathParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetPage: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetPage: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsPage>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsPage) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsPage)));
            
        }
    
        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>Task of VirtoCommerceContentWebModelsPage</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsPage> PagesGetPageAsync (string storeId, string language, string pageName)
        {
             ApiResponse<VirtoCommerceContentWebModelsPage> response = await PagesGetPageAsyncWithHttpInfo(storeId, language, pageName);
             return response.Data;

        }

        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (language != null) pathParams.Add("language", Configuration.ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) pathParams.Add("pageName", Configuration.ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling PagesGetPage: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling PagesGetPage: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsPage>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsPage) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsPage)));
            
        }
        
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsTheme>> response = ThemeGetThemesWithHttpInfo(storeId);
             return response.Data;
        }

        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsTheme> > ThemeGetThemesWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeGetThemes");
            
    
            var path_ = "/api/cms/{storeId}/themes";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemes: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsTheme>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsTheme>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsTheme>)));
            
        }
    
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsTheme&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsTheme>> response = await ThemeGetThemesAsyncWithHttpInfo(storeId);
             return response.Data;

        }

        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsTheme&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsTheme>>> ThemeGetThemesAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemes");
            
    
            var path_ = "/api/cms/{storeId}/themes";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemes: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemes: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsTheme>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsTheme>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsTheme>)));
            
        }
        
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>
        public void ThemeCreateDefaultTheme (string storeId)
        {
             ThemeCreateDefaultThemeWithHttpInfo(storeId);
        }

        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ThemeCreateDefaultThemeWithHttpInfo (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ThemeCreateDefaultTheme");
            
    
            var path_ = "/api/cms/{storeId}/themes/createdefault";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeCreateDefaultTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeCreateDefaultTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId)
        {
             await ThemeCreateDefaultThemeAsyncWithHttpInfo(storeId);

        }

        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeCreateDefaultThemeAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateDefaultTheme");
            
    
            var path_ = "/api/cms/{storeId}/themes/createdefault";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeCreateDefaultTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeCreateDefaultTheme: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/file";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (themeFileUrl != null) queryParams.Add("themeFileUrl", Configuration.ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) queryParams.Add("themeName", Configuration.ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeCreateNewTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeCreateNewTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/file";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            
            if (themeFileUrl != null) queryParams.Add("themeFileUrl", Configuration.ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) queryParams.Add("themeName", Configuration.ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeCreateNewTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeCreateNewTheme: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeDeleteTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeDeleteTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
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
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ThemeDeleteThemeAsyncWithHttpInfo (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteTheme");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteTheme");
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeDeleteTheme: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeDeleteTheme: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            if (asset.GetType() != typeof(byte[]))
            {
                postBody = Configuration.ApiClient.Serialize(asset); // http body (model) parameter
            }
            else
            {
                postBody = asset; // byte array
            }

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeSaveItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeSaveItem: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            postBody = Configuration.ApiClient.Serialize(asset); // http body (model) parameter
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.POST, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeSaveItem: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeSaveItem: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) queryParams.Add("assetIds", Configuration.ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeDeleteAssets: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeDeleteAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
    
        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) queryParams.Add("assetIds", Configuration.ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeDeleteAssets: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeDeleteAssets: " + response.ErrorMessage, response.ErrorMessage);

            
            return new ApiResponse<Object>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }
        
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <param name="assetId">Theme asset id</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>VirtoCommerceContentWebModelsThemeAsset</returns>
        public VirtoCommerceContentWebModelsThemeAsset ThemeGetThemeAsset (string assetId, string storeId, string themeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsThemeAsset> response = ThemeGetThemeAssetWithHttpInfo(assetId, storeId, themeId);
             return response.Data;
        }

        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
            if (assetId != null) pathParams.Add("assetId", Configuration.ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAsset: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAsset: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<VirtoCommerceContentWebModelsThemeAsset>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsThemeAsset) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsThemeAsset)));
            
        }
    
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of VirtoCommerceContentWebModelsThemeAsset</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsThemeAsset> ThemeGetThemeAssetAsync (string assetId, string storeId, string themeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsThemeAsset> response = await ThemeGetThemeAssetAsyncWithHttpInfo(assetId, storeId, themeId);
             return response.Data;

        }

        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
            if (assetId != null) pathParams.Add("assetId", Configuration.ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAsset: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAsset: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsThemeAsset>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsThemeAsset) Configuration.ApiClient.Deserialize(response, typeof(VirtoCommerceContentWebModelsThemeAsset)));
            
        }
        
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns>List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        public List<VirtoCommerceContentWebModelsThemeAssetFolder> ThemeGetThemeAssets (string storeId, string themeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> response = ThemeGetThemeAssetsWithHttpInfo(storeId, themeId);
             return response.Data;
        }

        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
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
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) Configuration.ApiClient.CallApi(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams,
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
    
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAssets: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            return new ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsThemeAssetFolder>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>)));
            
        }
    
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsAsync (string storeId, string themeId)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>> response = await ThemeGetThemeAssetsAsyncWithHttpInfo(storeId, themeId);
             return response.Data;

        }

        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsThemeAssetFolder&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>> ThemeGetThemeAssetsAsyncWithHttpInfo (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAssets");
            
    
            var path_ = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
            if (storeId != null) pathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", Configuration.ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            

            

            // make the HTTP request
            IRestResponse response = (IRestResponse) await Configuration.ApiClient.CallApiAsync(path_, 
                Method.GET, queryParams, postBody, headerParams, formParams, fileParams, 
                pathParams, httpContentType);

            int statusCode = (int) response.StatusCode;
 
            if (statusCode >= 400 && (statusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAssets: " + response.Content, response.Content);
            else if (statusCode == 0)
                throw new ApiException (statusCode, "Error calling ThemeGetThemeAssets: " + response.ErrorMessage, response.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsThemeAssetFolder>>(statusCode,
                response.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsThemeAssetFolder>) Configuration.ApiClient.Deserialize(response, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>)));
            
        }
        
    }
    
}
