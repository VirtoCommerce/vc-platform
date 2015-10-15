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
    public interface ICMSContentModuleApi
    {
        
        /// <summary>
        /// Sync assets elements
        /// </summary>
        /// <remarks>
        /// Method allows synchronize asset elements(theme assets and pages). For synchronization used store id, theme id and last theme and pages update date.\r\n            If last update dates = null, returns all pages or theme assets for that store and theme.
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="theme">Theme name</param>
        /// <param name="themeUpdated">Last theme updated date</param>
        /// <param name="pagesUpdated">Last pages updated date</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsSyncAssetGroup> SyncSyncAssets (string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated);
  
        /// <summary>
        /// Sync assets elements
        /// </summary>
        /// <remarks>
        /// Method allows synchronize asset elements(theme assets and pages). For synchronization used store id, theme id and last theme and pages update date.\r\n            If last update dates = null, returns all pages or theme assets for that store and theme.
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="theme">Theme name</param>
        /// <param name="themeUpdated">Last theme updated date</param>
        /// <param name="pagesUpdated">Last pages updated date</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsSyncAssetGroup>> SyncSyncAssetsAsync (string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated);
        
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId);
  
        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task MenuUpdateAsync (VirtoCommerceContentWebModelsMenuLinkList list, string storeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task MenuDeleteAsync (string listId, string storeId);
        
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
        VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id);
  
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
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id);
        
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
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListAsync (string storeId, string listId);
        
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId, DateTime? criteriaLastUpdateDate);
  
        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>
        /// Get all pages by store and criteria
        /// </remarks>
        /// <param name="storeId">Store Id</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId, DateTime? criteriaLastUpdateDate);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task PagesSaveItemAsync (string storeId, VirtoCommerceContentWebModelsPage page);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task PagesDeleteItemAsync (string storeId, List<string> pageNamesAndLanguges);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task PagesCreateBlogAsync (string storeId, string blogName);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task PagesDeleteBlogAsync (string storeId, string blogName);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task PagesUpdateBlogAsync (string storeId, string blogName, string oldBlogName);
        
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
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameAsync (string storeId, string pageName, string language);
        
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
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId);
        
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
        /// <returns>VirtoCommerceContentWebModelsPage</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsPage> PagesGetPageAsync (string storeId, string language, string pageName);
        
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId);
  
        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeCreateNewThemeAsync (string storeId, string themeFileUrl, string themeName);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeDeleteThemeAsync (string storeId, string themeId);
        
        /// <summary>
        /// Search theme assets
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id, theme id and criteria
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="criteriaLoadContent">If true - returns array of theme assets including binary or text content, if false - returns array of theme assets without content</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsThemeAsset> ThemeSearchThemeAssets (string storeId, string themeId, bool? criteriaLoadContent, DateTime? criteriaLastUpdateDate);
  
        /// <summary>
        /// Search theme assets
        /// </summary>
        /// <remarks>
        /// Search theme assets by store id, theme id and criteria
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="criteriaLoadContent">If true - returns array of theme assets including binary or text content, if false - returns array of theme assets without content</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAsset>> ThemeSearchThemeAssetsAsync (string storeId, string themeId, bool? criteriaLoadContent, DateTime? criteriaLastUpdateDate);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeSaveItemAsync (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId);
        
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
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeDeleteAssetsAsync (string storeId, string themeId, List<string> assetIds);
        
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        void ThemeGetThemeAsset (string assetId, string storeId, string themeId);
  
        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>
        /// Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </remarks>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task ThemeGetThemeAssetAsync (string assetId, string storeId, string themeId);
        
        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        List<VirtoCommerceContentWebModelsThemeAssetFolder> ThemeGetThemeAssets (string storeId, string themeId);
  
        /// <summary>
        /// Get theme assets folders
        /// </summary>
        /// <remarks>
        /// Get theme assets folders by store id and theme id
        /// </remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsAsync (string storeId, string themeId);
        
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CMSContentModuleApi : ICMSContentModuleApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CMSContentModuleApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CMSContentModuleApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CMSContentModuleApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CMSContentModuleApi(String basePath)
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
        /// Sync assets elements Method allows synchronize asset elements(theme assets and pages). For synchronization used store id, theme id and last theme and pages update date.\r\n            If last update dates = null, returns all pages or theme assets for that store and theme.
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="theme">Theme name</param> 
        /// <param name="themeUpdated">Last theme updated date</param> 
        /// <param name="pagesUpdated">Last pages updated date</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsSyncAssetGroup> SyncSyncAssets (string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SyncSyncAssets");
            
            // verify the required parameter 'theme' is set
            if (theme == null) throw new ApiException(400, "Missing required parameter 'theme' when calling SyncSyncAssets");
            
            // verify the required parameter 'themeUpdated' is set
            if (themeUpdated == null) throw new ApiException(400, "Missing required parameter 'themeUpdated' when calling SyncSyncAssets");
            
            // verify the required parameter 'pagesUpdated' is set
            if (pagesUpdated == null) throw new ApiException(400, "Missing required parameter 'pagesUpdated' when calling SyncSyncAssets");
            
    
            var path = "/api/cms/sync/stores/{storeId}/assets";
    
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
            
            if (theme != null) queryParams.Add("theme", ApiClient.ParameterToString(theme)); // query parameter
            if (themeUpdated != null) queryParams.Add("themeUpdated", ApiClient.ParameterToString(themeUpdated)); // query parameter
            if (pagesUpdated != null) queryParams.Add("pagesUpdated", ApiClient.ParameterToString(pagesUpdated)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SyncSyncAssets: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SyncSyncAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsSyncAssetGroup>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsSyncAssetGroup>), response.Headers);
        }
    
        /// <summary>
        /// Sync assets elements Method allows synchronize asset elements(theme assets and pages). For synchronization used store id, theme id and last theme and pages update date.\r\n            If last update dates = null, returns all pages or theme assets for that store and theme.
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="theme">Theme name</param>
        /// <param name="themeUpdated">Last theme updated date</param>
        /// <param name="pagesUpdated">Last pages updated date</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsSyncAssetGroup>> SyncSyncAssetsAsync (string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling SyncSyncAssets");
            // verify the required parameter 'theme' is set
            if (theme == null) throw new ApiException(400, "Missing required parameter 'theme' when calling SyncSyncAssets");
            // verify the required parameter 'themeUpdated' is set
            if (themeUpdated == null) throw new ApiException(400, "Missing required parameter 'themeUpdated' when calling SyncSyncAssets");
            // verify the required parameter 'pagesUpdated' is set
            if (pagesUpdated == null) throw new ApiException(400, "Missing required parameter 'pagesUpdated' when calling SyncSyncAssets");
            
    
            var path = "/api/cms/sync/stores/{storeId}/assets";
    
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
            
            if (theme != null) queryParams.Add("theme", ApiClient.ParameterToString(theme)); // query parameter
            if (themeUpdated != null) queryParams.Add("themeUpdated", ApiClient.ParameterToString(themeUpdated)); // query parameter
            if (pagesUpdated != null) queryParams.Add("pagesUpdated", ApiClient.ParameterToString(pagesUpdated)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SyncSyncAssets: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsSyncAssetGroup>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsSyncAssetGroup>), response.Headers);
        }
        
        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsMenuLinkList> MenuGetLists (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetLists");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsMenuLinkList>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>), response.Headers);
        }
    
        /// <summary>
        /// Get menu link lists 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsMenuLinkList>> MenuGetListsAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetLists");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetLists: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsMenuLinkList>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsMenuLinkList>), response.Headers);
        }
        
        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <param name="list">Menu link list</param> 
        /// <param name="storeId"></param> 
        /// <returns></returns>            
        public void MenuUpdate (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
            
            // verify the required parameter 'list' is set
            if (list == null) throw new ApiException(400, "Missing required parameter 'list' when calling MenuUpdate");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuUpdate");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(list); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuUpdate: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuUpdate: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Update menu link list 
        /// </summary>
        /// <param name="list">Menu link list</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MenuUpdateAsync (VirtoCommerceContentWebModelsMenuLinkList list, string storeId)
        {
            // verify the required parameter 'list' is set
            if (list == null) throw new ApiException(400, "Missing required parameter 'list' when calling MenuUpdate");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuUpdate");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(list); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuUpdate: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <param name="listId">Menu link list id</param> 
        /// <param name="storeId"></param> 
        /// <returns></returns>            
        public void MenuDelete (string listId, string storeId)
        {
            
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuDelete");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuDelete");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            
            if (listId != null) queryParams.Add("listId", ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete menu link list 
        /// </summary>
        /// <param name="listId">Menu link list id</param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task MenuDeleteAsync (string listId, string storeId)
        {
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuDelete");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuDelete");
            
    
            var path = "/api/cms/{storeId}/menu";
    
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
            
            if (listId != null) queryParams.Add("listId", ApiClient.ParameterToString(listId)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuDelete: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="name">Name of menu link list</param> 
        /// <param name="language">Language of menu link list</param> 
        /// <param name="id">Menu link list id</param> 
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>            
        public VirtoCommerceContentWebModelsCheckNameResult MenuCheckName (string storeId, string name, string language, string id)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuCheckName");
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling MenuCheckName");
            
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MenuCheckName");
            
    
            var path = "/api/cms/{storeId}/menu/checkname";
    
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
            
            if (name != null) queryParams.Add("name", ApiClient.ParameterToString(name)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            if (id != null) queryParams.Add("id", ApiClient.ParameterToString(id)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuCheckName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuCheckName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceContentWebModelsCheckNameResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsCheckNameResult), response.Headers);
        }
    
        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id</param>
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> MenuCheckNameAsync (string storeId, string name, string language, string id)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuCheckName");
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling MenuCheckName");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling MenuCheckName");
            
    
            var path = "/api/cms/{storeId}/menu/checkname";
    
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
            
            if (name != null) queryParams.Add("name", ApiClient.ParameterToString(name)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            if (id != null) queryParams.Add("id", ApiClient.ParameterToString(id)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuCheckName: " + response.Content, response.Content);

            return (VirtoCommerceContentWebModelsCheckNameResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsCheckNameResult), response.Headers);
        }
        
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="listId">List id</param> 
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>            
        public VirtoCommerceContentWebModelsMenuLinkList MenuGetList (string storeId, string listId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetList");
            
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuGetList");
            
    
            var path = "/api/cms/{storeId}/menu/{listId}";
    
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
            if (listId != null) pathParams.Add("listId", ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceContentWebModelsMenuLinkList) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsMenuLinkList), response.Headers);
        }
    
        /// <summary>
        /// Get menu link list by id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="listId">List id</param>
        /// <returns>VirtoCommerceContentWebModelsMenuLinkList</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsMenuLinkList> MenuGetListAsync (string storeId, string listId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling MenuGetList");
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling MenuGetList");
            
    
            var path = "/api/cms/{storeId}/menu/{listId}";
    
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
            if (listId != null) pathParams.Add("listId", ApiClient.ParameterToString(listId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MenuGetList: " + response.Content, response.Content);

            return (VirtoCommerceContentWebModelsMenuLinkList) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsMenuLinkList), response.Headers);
        }
        
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsPage> PagesGetPages (string storeId, DateTime? criteriaLastUpdateDate)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPages");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            if (criteriaLastUpdateDate != null) queryParams.Add("criteria.lastUpdateDate", ApiClient.ParameterToString(criteriaLastUpdateDate)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPages: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPages: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsPage>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsPage>), response.Headers);
        }
    
        /// <summary>
        /// Search pages Get all pages by store and criteria
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsPage>> PagesGetPagesAsync (string storeId, DateTime? criteriaLastUpdateDate)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPages");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            if (criteriaLastUpdateDate != null) queryParams.Add("criteria.lastUpdateDate", ApiClient.ParameterToString(criteriaLastUpdateDate)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPages: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsPage>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsPage>), response.Headers);
        }
        
        /// <summary>
        /// Save page 
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <param name="page">Page</param> 
        /// <returns></returns>            
        public void PagesSaveItem (string storeId, VirtoCommerceContentWebModelsPage page)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesSaveItem");
            
            // verify the required parameter 'page' is set
            if (page == null) throw new ApiException(400, "Missing required parameter 'page' when calling PagesSaveItem");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(page); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesSaveItem: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesSaveItem: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Save page 
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PagesSaveItemAsync (string storeId, VirtoCommerceContentWebModelsPage page)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesSaveItem");
            // verify the required parameter 'page' is set
            if (page == null) throw new ApiException(400, "Missing required parameter 'page' when calling PagesSaveItem");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            
            
            
            postBody = ApiClient.Serialize(page); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesSaveItem: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param> 
        /// <returns></returns>            
        public void PagesDeleteItem (string storeId, List<string> pageNamesAndLanguges)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteItem");
            
            // verify the required parameter 'pageNamesAndLanguges' is set
            if (pageNamesAndLanguges == null) throw new ApiException(400, "Missing required parameter 'pageNamesAndLanguges' when calling PagesDeleteItem");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            if (pageNamesAndLanguges != null) queryParams.Add("pageNamesAndLanguges", ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteItem: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteItem: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete page Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PagesDeleteItemAsync (string storeId, List<string> pageNamesAndLanguges)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteItem");
            // verify the required parameter 'pageNamesAndLanguges' is set
            if (pageNamesAndLanguges == null) throw new ApiException(400, "Missing required parameter 'pageNamesAndLanguges' when calling PagesDeleteItem");
            
    
            var path = "/api/cms/{storeId}/pages";
    
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
            
            if (pageNamesAndLanguges != null) queryParams.Add("pageNamesAndLanguges", ApiClient.ParameterToString(pageNamesAndLanguges)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteItem: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns></returns>            
        public void PagesCreateBlog (string storeId, string blogName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCreateBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesCreateBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCreateBlog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCreateBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PagesCreateBlogAsync (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCreateBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesCreateBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCreateBlog: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <param name="storeId"></param> 
        /// <param name="blogName"></param> 
        /// <returns></returns>            
        public void PagesDeleteBlog (string storeId, string blogName)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesDeleteBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteBlog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PagesDeleteBlogAsync (string storeId, string blogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesDeleteBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesDeleteBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesDeleteBlog: " + response.Content, response.Content);

            
            return;
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
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesUpdateBlog");
            
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesUpdateBlog");
            
            // verify the required parameter 'oldBlogName' is set
            if (oldBlogName == null) throw new ApiException(400, "Missing required parameter 'oldBlogName' when calling PagesUpdateBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) pathParams.Add("oldBlogName", ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesUpdateBlog: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesUpdateBlog: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="blogName"></param>
        /// <param name="oldBlogName"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task PagesUpdateBlogAsync (string storeId, string blogName, string oldBlogName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesUpdateBlog");
            // verify the required parameter 'blogName' is set
            if (blogName == null) throw new ApiException(400, "Missing required parameter 'blogName' when calling PagesUpdateBlog");
            // verify the required parameter 'oldBlogName' is set
            if (oldBlogName == null) throw new ApiException(400, "Missing required parameter 'oldBlogName' when calling PagesUpdateBlog");
            
    
            var path = "/api/cms/{storeId}/pages/blog/{blogName}/{oldBlogName}";
    
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
            if (blogName != null) pathParams.Add("blogName", ApiClient.ParameterToString(blogName)); // path parameter
            if (oldBlogName != null) pathParams.Add("oldBlogName", ApiClient.ParameterToString(oldBlogName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesUpdateBlog: " + response.Content, response.Content);

            
            return;
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
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCheckName");
            
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesCheckName");
            
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesCheckName");
            
    
            var path = "/api/cms/{storeId}/pages/checkname";
    
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
            
            if (pageName != null) queryParams.Add("pageName", ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCheckName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCheckName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceContentWebModelsCheckNameResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsCheckNameResult), response.Headers);
        }
    
        /// <summary>
        /// Check page name Check page pair name+language for store
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageName">Page name</param>
        /// <param name="language">Page language</param>
        /// <returns>VirtoCommerceContentWebModelsCheckNameResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsCheckNameResult> PagesCheckNameAsync (string storeId, string pageName, string language)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesCheckName");
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesCheckName");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesCheckName");
            
    
            var path = "/api/cms/{storeId}/pages/checkname";
    
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
            
            if (pageName != null) queryParams.Add("pageName", ApiClient.ParameterToString(pageName)); // query parameter
            if (language != null) queryParams.Add("language", ApiClient.ParameterToString(language)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesCheckName: " + response.Content, response.Content);

            return (VirtoCommerceContentWebModelsCheckNameResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsCheckNameResult), response.Headers);
        }
        
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param> 
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>            
        public VirtoCommerceContentWebModelsGetPagesResult PagesGetFolders (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetFolders");
            
    
            var path = "/api/cms/{storeId}/pages/folders";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetFolders: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetFolders: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceContentWebModelsGetPagesResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsGetPagesResult), response.Headers);
        }
    
        /// <summary>
        /// Get pages folders by store id 
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <returns>VirtoCommerceContentWebModelsGetPagesResult</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsGetPagesResult> PagesGetFoldersAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetFolders");
            
    
            var path = "/api/cms/{storeId}/pages/folders";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetFolders: " + response.Content, response.Content);

            return (VirtoCommerceContentWebModelsGetPagesResult) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsGetPagesResult), response.Headers);
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
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPage");
            
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesGetPage");
            
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesGetPage");
            
    
            var path = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) pathParams.Add("pageName", ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (VirtoCommerceContentWebModelsPage) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsPage), response.Headers);
        }
    
        /// <summary>
        /// Get page Get page by store and name+language pair.
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <returns>VirtoCommerceContentWebModelsPage</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsPage> PagesGetPageAsync (string storeId, string language, string pageName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling PagesGetPage");
            // verify the required parameter 'language' is set
            if (language == null) throw new ApiException(400, "Missing required parameter 'language' when calling PagesGetPage");
            // verify the required parameter 'pageName' is set
            if (pageName == null) throw new ApiException(400, "Missing required parameter 'pageName' when calling PagesGetPage");
            
    
            var path = "/api/cms/{storeId}/pages/{language}/{pageName}";
    
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
            if (language != null) pathParams.Add("language", ApiClient.ParameterToString(language)); // path parameter
            if (pageName != null) pathParams.Add("pageName", ApiClient.ParameterToString(pageName)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PagesGetPage: " + response.Content, response.Content);

            return (VirtoCommerceContentWebModelsPage) ApiClient.Deserialize(response.Content, typeof(VirtoCommerceContentWebModelsPage), response.Headers);
        }
        
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsTheme> ThemeGetThemes (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemes");
            
    
            var path = "/api/cms/{storeId}/themes";
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemes: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemes: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsTheme>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsTheme>), response.Headers);
        }
    
        /// <summary>
        /// Get themes by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsTheme>> ThemeGetThemesAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemes");
            
    
            var path = "/api/cms/{storeId}/themes";
    
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
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemes: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsTheme>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsTheme>), response.Headers);
        }
        
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <returns></returns>            
        public void ThemeCreateDefaultTheme (string storeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateDefaultTheme");
            
    
            var path = "/api/cms/{storeId}/themes/createdefault";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateDefaultTheme: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateDefaultTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create default theme by store id 
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeCreateDefaultThemeAsync (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateDefaultTheme");
            
    
            var path = "/api/cms/{storeId}/themes/createdefault";
    
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
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateDefaultTheme: " + response.Content, response.Content);

            
            return;
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
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateNewTheme");
            
            // verify the required parameter 'themeFileUrl' is set
            if (themeFileUrl == null) throw new ApiException(400, "Missing required parameter 'themeFileUrl' when calling ThemeCreateNewTheme");
            
            // verify the required parameter 'themeName' is set
            if (themeName == null) throw new ApiException(400, "Missing required parameter 'themeName' when calling ThemeCreateNewTheme");
            
    
            var path = "/api/cms/{storeId}/themes/file";
    
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
            
            if (themeFileUrl != null) queryParams.Add("themeFileUrl", ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) queryParams.Add("themeName", ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateNewTheme: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateNewTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create new theme Create new theme considering store id, theme file url and theme name
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeFileUrl">Theme file url</param>
        /// <param name="themeName">Theme name</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeCreateNewThemeAsync (string storeId, string themeFileUrl, string themeName)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeCreateNewTheme");
            // verify the required parameter 'themeFileUrl' is set
            if (themeFileUrl == null) throw new ApiException(400, "Missing required parameter 'themeFileUrl' when calling ThemeCreateNewTheme");
            // verify the required parameter 'themeName' is set
            if (themeName == null) throw new ApiException(400, "Missing required parameter 'themeName' when calling ThemeCreateNewTheme");
            
    
            var path = "/api/cms/{storeId}/themes/file";
    
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
            
            if (themeFileUrl != null) queryParams.Add("themeFileUrl", ApiClient.ParameterToString(themeFileUrl)); // query parameter
            if (themeName != null) queryParams.Add("themeName", ApiClient.ParameterToString(themeName)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeCreateNewTheme: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns></returns>            
        public void ThemeDeleteTheme (string storeId, string themeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteTheme");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteTheme");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteTheme: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteTheme: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete theme Search theme assets by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeDeleteThemeAsync (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteTheme");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteTheme");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteTheme: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Search theme assets Search theme assets by store id, theme id and criteria
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <param name="criteriaLoadContent">If true - returns array of theme assets including binary or text content, if false - returns array of theme assets without content</param> 
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsThemeAsset> ThemeSearchThemeAssets (string storeId, string themeId, bool? criteriaLoadContent, DateTime? criteriaLastUpdateDate)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeSearchThemeAssets");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeSearchThemeAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            if (criteriaLoadContent != null) queryParams.Add("criteria.loadContent", ApiClient.ParameterToString(criteriaLoadContent)); // query parameter
            if (criteriaLastUpdateDate != null) queryParams.Add("criteria.lastUpdateDate", ApiClient.ParameterToString(criteriaLastUpdateDate)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSearchThemeAssets: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSearchThemeAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsThemeAsset>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsThemeAsset>), response.Headers);
        }
    
        /// <summary>
        /// Search theme assets Search theme assets by store id, theme id and criteria
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="criteriaLoadContent">If true - returns array of theme assets including binary or text content, if false - returns array of theme assets without content</param>
        /// <param name="criteriaLastUpdateDate">Max value of last updated date, if it&#39;s null returns all pages for store</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAsset>> ThemeSearchThemeAssetsAsync (string storeId, string themeId, bool? criteriaLoadContent, DateTime? criteriaLastUpdateDate)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeSearchThemeAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeSearchThemeAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            if (criteriaLoadContent != null) queryParams.Add("criteria.loadContent", ApiClient.ParameterToString(criteriaLoadContent)); // query parameter
            if (criteriaLastUpdateDate != null) queryParams.Add("criteria.lastUpdateDate", ApiClient.ParameterToString(criteriaLastUpdateDate)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSearchThemeAssets: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsThemeAsset>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsThemeAsset>), response.Headers);
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
            
            // verify the required parameter 'asset' is set
            if (asset == null) throw new ApiException(400, "Missing required parameter 'asset' when calling ThemeSaveItem");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeSaveItem");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeSaveItem");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(asset); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSaveItem: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSaveItem: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Save theme asset Save theme asset considering store id and theme id
        /// </summary>
        /// <param name="asset">Theme asset</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeSaveItemAsync (VirtoCommerceContentWebModelsThemeAsset asset, string storeId, string themeId)
        {
            // verify the required parameter 'asset' is set
            if (asset == null) throw new ApiException(400, "Missing required parameter 'asset' when calling ThemeSaveItem");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeSaveItem");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeSaveItem");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            postBody = ApiClient.Serialize(asset); // http body (model) parameter
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeSaveItem: " + response.Content, response.Content);

            
            return;
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
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteAssets");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteAssets");
            
            // verify the required parameter 'assetIds' is set
            if (assetIds == null) throw new ApiException(400, "Missing required parameter 'assetIds' when calling ThemeDeleteAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) queryParams.Add("assetIds", ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteAssets: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete theme assets by assetIds Delete theme assets considering store id, theme id and assetIds
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetIds">Deleted asset ids</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeDeleteAssetsAsync (string storeId, string themeId, List<string> assetIds)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeDeleteAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeDeleteAssets");
            // verify the required parameter 'assetIds' is set
            if (assetIds == null) throw new ApiException(400, "Missing required parameter 'assetIds' when calling ThemeDeleteAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            if (assetIds != null) queryParams.Add("assetIds", ApiClient.ParameterToString(assetIds)); // query parameter
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeDeleteAssets: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <param name="assetId">Theme asset id</param> 
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns></returns>            
        public void ThemeGetThemeAsset (string assetId, string storeId, string themeId)
        {
            
            // verify the required parameter 'assetId' is set
            if (assetId == null) throw new ApiException(400, "Missing required parameter 'assetId' when calling ThemeGetThemeAsset");
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAsset");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAsset");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
            if (assetId != null) pathParams.Add("assetId", ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAsset: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAsset: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get theme asset Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path
        /// </summary>
        /// <param name="assetId">Theme asset id</param>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ThemeGetThemeAssetAsync (string assetId, string storeId, string themeId)
        {
            // verify the required parameter 'assetId' is set
            if (assetId == null) throw new ApiException(400, "Missing required parameter 'assetId' when calling ThemeGetThemeAsset");
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAsset");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAsset");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/assets/{assetId}";
    
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
            if (assetId != null) pathParams.Add("assetId", ApiClient.ParameterToString(assetId)); // path parameter
            if (storeId != null) pathParams.Add("storeId", ApiClient.ParameterToString(storeId)); // path parameter
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAsset: " + response.Content, response.Content);

            
            return;
        }
        
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param> 
        /// <param name="themeId">Theme id</param> 
        /// <returns></returns>            
        public List<VirtoCommerceContentWebModelsThemeAssetFolder> ThemeGetThemeAssets (string storeId, string themeId)
        {
            
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAssets");
            
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAssets: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAssets: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<VirtoCommerceContentWebModelsThemeAssetFolder>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>), response.Headers);
        }
    
        /// <summary>
        /// Get theme assets folders Get theme assets folders by store id and theme id
        /// </summary>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsThemeAssetFolder>> ThemeGetThemeAssetsAsync (string storeId, string themeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null) throw new ApiException(400, "Missing required parameter 'storeId' when calling ThemeGetThemeAssets");
            // verify the required parameter 'themeId' is set
            if (themeId == null) throw new ApiException(400, "Missing required parameter 'themeId' when calling ThemeGetThemeAssets");
            
    
            var path = "/api/cms/{storeId}/themes/{themeId}/folders";
    
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
            if (themeId != null) pathParams.Add("themeId", ApiClient.ParameterToString(themeId)); // path parameter
            
            
            
            
            
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) await ApiClient.CallApiAsync(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, pathParams, authSettings);
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ThemeGetThemeAssets: " + response.Content, response.Content);

            return (List<VirtoCommerceContentWebModelsThemeAssetFolder>) ApiClient.Deserialize(response.Content, typeof(List<VirtoCommerceContentWebModelsThemeAssetFolder>), response.Headers);
        }
        
    }
    
}
