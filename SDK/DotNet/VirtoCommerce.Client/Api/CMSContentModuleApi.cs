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
        /// Copy contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        void ContentCopyContent (string srcPath, string destPath);

        /// <summary>
        /// Copy contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ContentCopyContentWithHttpInfo (string srcPath, string destPath);
        /// <summary>
        /// Create content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns></returns>
        void ContentCreateContentFolder (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder);

        /// <summary>
        /// Create content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ContentCreateContentFolderWithHttpInfo (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder);
        /// <summary>
        /// Delete content from server
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns></returns>
        void ContentDeleteContent (string contentType, string storeId, List<string> urls);

        /// <summary>
        /// Delete content from server
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ContentDeleteContentWithHttpInfo (string contentType, string storeId, List<string> urls);
        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>byte[]</returns>
        byte[] ContentGetContentItemDataStream (string contentType, string storeId, string relativeUrl);

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>ApiResponse of byte[]</returns>
        ApiResponse<byte[]> ContentGetContentItemDataStreamWithHttpInfo (string contentType, string storeId, string relativeUrl);
        /// <summary>
        /// Return summary content statistic
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>VirtoCommerceContentWebModelsContentStatistic</returns>
        VirtoCommerceContentWebModelsContentStatistic ContentGetStoreContentStats (string storeId);

        /// <summary>
        /// Return summary content statistic
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsContentStatistic</returns>
        ApiResponse<VirtoCommerceContentWebModelsContentStatistic> ContentGetStoreContentStatsWithHttpInfo (string storeId);
        /// <summary>
        /// Rename or move content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns></returns>
        void ContentMoveContent (string contentType, string storeId, string oldUrl, string newUrl);

        /// <summary>
        /// Rename or move content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ContentMoveContentWithHttpInfo (string contentType, string storeId, string oldUrl, string newUrl);
        /// <summary>
        /// Search content items in specified folder and using search keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        List<VirtoCommerceContentWebModelsContentItem> ContentSearchContent (string contentType, string storeId, string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search content items in specified folder and using search keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> ContentSearchContentWithHttpInfo (string contentType, string storeId, string folderUrl = null, string keyword = null);
        /// <summary>
        /// Unpack contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        void ContentUnpack (string contentType, string storeId, string archivePath, string destPath);

        /// <summary>
        /// Unpack contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<Object> ContentUnpackWithHttpInfo (string contentType, string storeId, string archivePath, string destPath);
        /// <summary>
        /// Upload content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        List<VirtoCommerceContentWebModelsContentItem> ContentUploadContent (string contentType, string storeId, string folderUrl, string url = null);

        /// <summary>
        /// Upload content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> ContentUploadContentWithHttpInfo (string contentType, string storeId, string folderUrl, string url = null);
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
        /// <returns>bool?</returns>
        bool? MenuCheckName (string storeId, string name, string language, string id = null);

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
        /// <returns>ApiResponse of bool?</returns>
        ApiResponse<bool?> MenuCheckNameWithHttpInfo (string storeId, string name, string language, string id = null);
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
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// Copy contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ContentCopyContentAsync (string srcPath, string destPath);

        /// <summary>
        /// Copy contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ContentCopyContentAsyncWithHttpInfo (string srcPath, string destPath);
        /// <summary>
        /// Create content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ContentCreateContentFolderAsync (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder);

        /// <summary>
        /// Create content folder
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ContentCreateContentFolderAsyncWithHttpInfo (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder);
        /// <summary>
        /// Delete content from server
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ContentDeleteContentAsync (string contentType, string storeId, List<string> urls);

        /// <summary>
        /// Delete content from server
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ContentDeleteContentAsyncWithHttpInfo (string contentType, string storeId, List<string> urls);
        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>Task of byte[]</returns>
        System.Threading.Tasks.Task<byte[]> ContentGetContentItemDataStreamAsync (string contentType, string storeId, string relativeUrl);

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager)
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>Task of ApiResponse (byte[])</returns>
        System.Threading.Tasks.Task<ApiResponse<byte[]>> ContentGetContentItemDataStreamAsyncWithHttpInfo (string contentType, string storeId, string relativeUrl);
        /// <summary>
        /// Return summary content statistic
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>Task of VirtoCommerceContentWebModelsContentStatistic</returns>
        System.Threading.Tasks.Task<VirtoCommerceContentWebModelsContentStatistic> ContentGetStoreContentStatsAsync (string storeId);

        /// <summary>
        /// Return summary content statistic
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsContentStatistic)</returns>
        System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsContentStatistic>> ContentGetStoreContentStatsAsyncWithHttpInfo (string storeId);
        /// <summary>
        /// Rename or move content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ContentMoveContentAsync (string contentType, string storeId, string oldUrl, string newUrl);

        /// <summary>
        /// Rename or move content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ContentMoveContentAsyncWithHttpInfo (string contentType, string storeId, string oldUrl, string newUrl);
        /// <summary>
        /// Search content items in specified folder and using search keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsContentItem>> ContentSearchContentAsync (string contentType, string storeId, string folderUrl = null, string keyword = null);

        /// <summary>
        /// Search content items in specified folder and using search keyword
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsContentItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>> ContentSearchContentAsyncWithHttpInfo (string contentType, string storeId, string folderUrl = null, string keyword = null);
        /// <summary>
        /// Unpack contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task ContentUnpackAsync (string contentType, string storeId, string archivePath, string destPath);

        /// <summary>
        /// Unpack contents
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> ContentUnpackAsyncWithHttpInfo (string contentType, string storeId, string archivePath, string destPath);
        /// <summary>
        /// Upload content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsContentItem>> ContentUploadContentAsync (string contentType, string storeId, string folderUrl, string url = null);

        /// <summary>
        /// Upload content item
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsContentItem&gt;)</returns>
        System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>> ContentUploadContentAsyncWithHttpInfo (string contentType, string storeId, string folderUrl, string url = null);
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
        /// <returns>Task of bool?</returns>
        System.Threading.Tasks.Task<bool?> MenuCheckNameAsync (string storeId, string name, string language, string id = null);

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
        /// <returns>Task of ApiResponse (bool?)</returns>
        System.Threading.Tasks.Task<ApiResponse<bool?>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null);
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
        /// Copy contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        public void ContentCopyContent (string srcPath, string destPath)
        {
             ContentCopyContentWithHttpInfo(srcPath, destPath);
        }

        /// <summary>
        /// Copy contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ContentCopyContentWithHttpInfo (string srcPath, string destPath)
        {
            // verify the required parameter 'srcPath' is set
            if (srcPath == null)
                throw new ApiException(400, "Missing required parameter 'srcPath' when calling CMSContentModuleApi->ContentCopyContent");
            // verify the required parameter 'destPath' is set
            if (destPath == null)
                throw new ApiException(400, "Missing required parameter 'destPath' when calling CMSContentModuleApi->ContentCopyContent");

            var localVarPath = "/api/content/copy";
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
            if (srcPath != null) localVarQueryParams.Add("srcPath", Configuration.ApiClient.ParameterToString(srcPath)); // query parameter
            if (destPath != null) localVarQueryParams.Add("destPath", Configuration.ApiClient.ParameterToString(destPath)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentCopyContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentCopyContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Copy contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ContentCopyContentAsync (string srcPath, string destPath)
        {
             await ContentCopyContentAsyncWithHttpInfo(srcPath, destPath);

        }

        /// <summary>
        /// Copy contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ContentCopyContentAsyncWithHttpInfo (string srcPath, string destPath)
        {
            // verify the required parameter 'srcPath' is set
            if (srcPath == null)
                throw new ApiException(400, "Missing required parameter 'srcPath' when calling CMSContentModuleApi->ContentCopyContent");
            // verify the required parameter 'destPath' is set
            if (destPath == null)
                throw new ApiException(400, "Missing required parameter 'destPath' when calling CMSContentModuleApi->ContentCopyContent");

            var localVarPath = "/api/content/copy";
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
            if (srcPath != null) localVarQueryParams.Add("srcPath", Configuration.ApiClient.ParameterToString(srcPath)); // query parameter
            if (destPath != null) localVarQueryParams.Add("destPath", Configuration.ApiClient.ParameterToString(destPath)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentCopyContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentCopyContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns></returns>
        public void ContentCreateContentFolder (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder)
        {
             ContentCreateContentFolderWithHttpInfo(contentType, storeId, folder);
        }

        /// <summary>
        /// Create content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ContentCreateContentFolderWithHttpInfo (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentCreateContentFolder");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentCreateContentFolder");
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling CMSContentModuleApi->ContentCreateContentFolder");

            var localVarPath = "/api/content/{contentType}/{storeId}/folder";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
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
                throw new ApiException (localVarStatusCode, "Error calling ContentCreateContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentCreateContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Create content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ContentCreateContentFolderAsync (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder)
        {
             await ContentCreateContentFolderAsyncWithHttpInfo(contentType, storeId, folder);

        }

        /// <summary>
        /// Create content folder 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ContentCreateContentFolderAsyncWithHttpInfo (string contentType, string storeId, VirtoCommerceContentWebModelsContentFolder folder)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentCreateContentFolder");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentCreateContentFolder");
            // verify the required parameter 'folder' is set
            if (folder == null)
                throw new ApiException(400, "Missing required parameter 'folder' when calling CMSContentModuleApi->ContentCreateContentFolder");

            var localVarPath = "/api/content/{contentType}/{storeId}/folder";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
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
                throw new ApiException (localVarStatusCode, "Error calling ContentCreateContentFolder: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentCreateContentFolder: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete content from server 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns></returns>
        public void ContentDeleteContent (string contentType, string storeId, List<string> urls)
        {
             ContentDeleteContentWithHttpInfo(contentType, storeId, urls);
        }

        /// <summary>
        /// Delete content from server 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ContentDeleteContentWithHttpInfo (string contentType, string storeId, List<string> urls)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentDeleteContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentDeleteContent");
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling CMSContentModuleApi->ContentDeleteContent");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (urls != null) localVarQueryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentDeleteContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentDeleteContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Delete content from server 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ContentDeleteContentAsync (string contentType, string storeId, List<string> urls)
        {
             await ContentDeleteContentAsyncWithHttpInfo(contentType, storeId, urls);

        }

        /// <summary>
        /// Delete content from server 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ContentDeleteContentAsyncWithHttpInfo (string contentType, string storeId, List<string> urls)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentDeleteContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentDeleteContent");
            // verify the required parameter 'urls' is set
            if (urls == null)
                throw new ApiException(400, "Missing required parameter 'urls' when calling CMSContentModuleApi->ContentDeleteContent");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (urls != null) localVarQueryParams.Add("urls", Configuration.ApiClient.ParameterToString(urls)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.DELETE, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentDeleteContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentDeleteContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>byte[]</returns>
        public byte[] ContentGetContentItemDataStream (string contentType, string storeId, string relativeUrl)
        {
             ApiResponse<byte[]> localVarResponse = ContentGetContentItemDataStreamWithHttpInfo(contentType, storeId, relativeUrl);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>ApiResponse of byte[]</returns>
        public ApiResponse< byte[] > ContentGetContentItemDataStreamWithHttpInfo (string contentType, string storeId, string relativeUrl)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentGetContentItemDataStream");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentGetContentItemDataStream");
            // verify the required parameter 'relativeUrl' is set
            if (relativeUrl == null)
                throw new ApiException(400, "Missing required parameter 'relativeUrl' when calling CMSContentModuleApi->ContentGetContentItemDataStream");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (relativeUrl != null) localVarQueryParams.Add("relativeUrl", Configuration.ApiClient.ParameterToString(relativeUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentGetContentItemDataStream: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentGetContentItemDataStream: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<byte[]>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (byte[]) Configuration.ApiClient.Deserialize(localVarResponse, typeof(byte[])));
            
        }

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>Task of byte[]</returns>
        public async System.Threading.Tasks.Task<byte[]> ContentGetContentItemDataStreamAsync (string contentType, string storeId, string relativeUrl)
        {
             ApiResponse<byte[]> localVarResponse = await ContentGetContentItemDataStreamAsyncWithHttpInfo(contentType, storeId, relativeUrl);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager) 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>Task of ApiResponse (byte[])</returns>
        public async System.Threading.Tasks.Task<ApiResponse<byte[]>> ContentGetContentItemDataStreamAsyncWithHttpInfo (string contentType, string storeId, string relativeUrl)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentGetContentItemDataStream");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentGetContentItemDataStream");
            // verify the required parameter 'relativeUrl' is set
            if (relativeUrl == null)
                throw new ApiException(400, "Missing required parameter 'relativeUrl' when calling CMSContentModuleApi->ContentGetContentItemDataStream");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (relativeUrl != null) localVarQueryParams.Add("relativeUrl", Configuration.ApiClient.ParameterToString(relativeUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentGetContentItemDataStream: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentGetContentItemDataStream: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<byte[]>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (byte[]) Configuration.ApiClient.Deserialize(localVarResponse, typeof(byte[])));
            
        }

        /// <summary>
        /// Return summary content statistic 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>VirtoCommerceContentWebModelsContentStatistic</returns>
        public VirtoCommerceContentWebModelsContentStatistic ContentGetStoreContentStats (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsContentStatistic> localVarResponse = ContentGetStoreContentStatsWithHttpInfo(storeId);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Return summary content statistic 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>ApiResponse of VirtoCommerceContentWebModelsContentStatistic</returns>
        public ApiResponse< VirtoCommerceContentWebModelsContentStatistic > ContentGetStoreContentStatsWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentGetStoreContentStats");

            var localVarPath = "/api/content/{storeId}/stats";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentGetStoreContentStats: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentGetStoreContentStats: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsContentStatistic>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsContentStatistic) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsContentStatistic)));
            
        }

        /// <summary>
        /// Return summary content statistic 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>Task of VirtoCommerceContentWebModelsContentStatistic</returns>
        public async System.Threading.Tasks.Task<VirtoCommerceContentWebModelsContentStatistic> ContentGetStoreContentStatsAsync (string storeId)
        {
             ApiResponse<VirtoCommerceContentWebModelsContentStatistic> localVarResponse = await ContentGetStoreContentStatsAsyncWithHttpInfo(storeId);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Return summary content statistic 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId"></param>
        /// <returns>Task of ApiResponse (VirtoCommerceContentWebModelsContentStatistic)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<VirtoCommerceContentWebModelsContentStatistic>> ContentGetStoreContentStatsAsyncWithHttpInfo (string storeId)
        {
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentGetStoreContentStats");

            var localVarPath = "/api/content/{storeId}/stats";
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
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentGetStoreContentStats: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentGetStoreContentStats: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<VirtoCommerceContentWebModelsContentStatistic>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (VirtoCommerceContentWebModelsContentStatistic) Configuration.ApiClient.Deserialize(localVarResponse, typeof(VirtoCommerceContentWebModelsContentStatistic)));
            
        }

        /// <summary>
        /// Rename or move content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns></returns>
        public void ContentMoveContent (string contentType, string storeId, string oldUrl, string newUrl)
        {
             ContentMoveContentWithHttpInfo(contentType, storeId, oldUrl, newUrl);
        }

        /// <summary>
        /// Rename or move content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ContentMoveContentWithHttpInfo (string contentType, string storeId, string oldUrl, string newUrl)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'oldUrl' is set
            if (oldUrl == null)
                throw new ApiException(400, "Missing required parameter 'oldUrl' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'newUrl' is set
            if (newUrl == null)
                throw new ApiException(400, "Missing required parameter 'newUrl' when calling CMSContentModuleApi->ContentMoveContent");

            var localVarPath = "/api/content/{contentType}/{storeId}/move";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (oldUrl != null) localVarQueryParams.Add("oldUrl", Configuration.ApiClient.ParameterToString(oldUrl)); // query parameter
            if (newUrl != null) localVarQueryParams.Add("newUrl", Configuration.ApiClient.ParameterToString(newUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentMoveContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentMoveContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Rename or move content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ContentMoveContentAsync (string contentType, string storeId, string oldUrl, string newUrl)
        {
             await ContentMoveContentAsyncWithHttpInfo(contentType, storeId, oldUrl, newUrl);

        }

        /// <summary>
        /// Rename or move content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ContentMoveContentAsyncWithHttpInfo (string contentType, string storeId, string oldUrl, string newUrl)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'oldUrl' is set
            if (oldUrl == null)
                throw new ApiException(400, "Missing required parameter 'oldUrl' when calling CMSContentModuleApi->ContentMoveContent");
            // verify the required parameter 'newUrl' is set
            if (newUrl == null)
                throw new ApiException(400, "Missing required parameter 'newUrl' when calling CMSContentModuleApi->ContentMoveContent");

            var localVarPath = "/api/content/{contentType}/{storeId}/move";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (oldUrl != null) localVarQueryParams.Add("oldUrl", Configuration.ApiClient.ParameterToString(oldUrl)); // query parameter
            if (newUrl != null) localVarQueryParams.Add("newUrl", Configuration.ApiClient.ParameterToString(newUrl)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentMoveContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentMoveContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Search content items in specified folder and using search keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public List<VirtoCommerceContentWebModelsContentItem> ContentSearchContent (string contentType, string storeId, string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> localVarResponse = ContentSearchContentWithHttpInfo(contentType, storeId, folderUrl, keyword);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Search content items in specified folder and using search keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsContentItem> > ContentSearchContentWithHttpInfo (string contentType, string storeId, string folderUrl = null, string keyword = null)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentSearchContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentSearchContent");

            var localVarPath = "/api/content/{contentType}/{storeId}/search";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentSearchContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentSearchContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsContentItem>)));
            
        }

        /// <summary>
        /// Search content items in specified folder and using search keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsContentItem>> ContentSearchContentAsync (string contentType, string storeId, string folderUrl = null, string keyword = null)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> localVarResponse = await ContentSearchContentAsyncWithHttpInfo(contentType, storeId, folderUrl, keyword);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Search content items in specified folder and using search keyword 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched (optional)</param>
        /// <param name="keyword">search keyword (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsContentItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>> ContentSearchContentAsyncWithHttpInfo (string contentType, string storeId, string folderUrl = null, string keyword = null)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentSearchContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentSearchContent");

            var localVarPath = "/api/content/{contentType}/{storeId}/search";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (keyword != null) localVarQueryParams.Add("keyword", Configuration.ApiClient.ParameterToString(keyword)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentSearchContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentSearchContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsContentItem>)));
            
        }

        /// <summary>
        /// Unpack contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        public void ContentUnpack (string contentType, string storeId, string archivePath, string destPath)
        {
             ContentUnpackWithHttpInfo(contentType, storeId, archivePath, destPath);
        }

        /// <summary>
        /// Unpack contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<Object> ContentUnpackWithHttpInfo (string contentType, string storeId, string archivePath, string destPath)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'archivePath' is set
            if (archivePath == null)
                throw new ApiException(400, "Missing required parameter 'archivePath' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'destPath' is set
            if (destPath == null)
                throw new ApiException(400, "Missing required parameter 'destPath' when calling CMSContentModuleApi->ContentUnpack");

            var localVarPath = "/api/content/{contentType}/{storeId}/unpack";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (archivePath != null) localVarQueryParams.Add("archivePath", Configuration.ApiClient.ParameterToString(archivePath)); // query parameter
            if (destPath != null) localVarQueryParams.Add("destPath", Configuration.ApiClient.ParameterToString(destPath)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentUnpack: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentUnpack: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Unpack contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task ContentUnpackAsync (string contentType, string storeId, string archivePath, string destPath)
        {
             await ContentUnpackAsyncWithHttpInfo(contentType, storeId, archivePath, destPath);

        }

        /// <summary>
        /// Unpack contents 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> ContentUnpackAsyncWithHttpInfo (string contentType, string storeId, string archivePath, string destPath)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'archivePath' is set
            if (archivePath == null)
                throw new ApiException(400, "Missing required parameter 'archivePath' when calling CMSContentModuleApi->ContentUnpack");
            // verify the required parameter 'destPath' is set
            if (destPath == null)
                throw new ApiException(400, "Missing required parameter 'destPath' when calling CMSContentModuleApi->ContentUnpack");

            var localVarPath = "/api/content/{contentType}/{storeId}/unpack";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (archivePath != null) localVarQueryParams.Add("archivePath", Configuration.ApiClient.ParameterToString(archivePath)); // query parameter
            if (destPath != null) localVarQueryParams.Add("destPath", Configuration.ApiClient.ParameterToString(destPath)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentUnpack: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentUnpack: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            
            return new ApiResponse<Object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                null);
        }

        /// <summary>
        /// Upload content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public List<VirtoCommerceContentWebModelsContentItem> ContentUploadContent (string contentType, string storeId, string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> localVarResponse = ContentUploadContentWithHttpInfo(contentType, storeId, folderUrl, url);
             return localVarResponse.Data;
        }

        /// <summary>
        /// Upload content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>ApiResponse of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public ApiResponse< List<VirtoCommerceContentWebModelsContentItem> > ContentUploadContentWithHttpInfo (string contentType, string storeId, string folderUrl, string url = null)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentUploadContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentUploadContent");
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling CMSContentModuleApi->ContentUploadContent");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentUploadContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentUploadContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsContentItem>)));
            
        }

        /// <summary>
        /// Upload content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>Task of List&lt;VirtoCommerceContentWebModelsContentItem&gt;</returns>
        public async System.Threading.Tasks.Task<List<VirtoCommerceContentWebModelsContentItem>> ContentUploadContentAsync (string contentType, string storeId, string folderUrl, string url = null)
        {
             ApiResponse<List<VirtoCommerceContentWebModelsContentItem>> localVarResponse = await ContentUploadContentAsyncWithHttpInfo(contentType, storeId, folderUrl, url);
             return localVarResponse.Data;

        }

        /// <summary>
        /// Upload content item 
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;VirtoCommerceContentWebModelsContentItem&gt;)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>> ContentUploadContentAsyncWithHttpInfo (string contentType, string storeId, string folderUrl, string url = null)
        {
            // verify the required parameter 'contentType' is set
            if (contentType == null)
                throw new ApiException(400, "Missing required parameter 'contentType' when calling CMSContentModuleApi->ContentUploadContent");
            // verify the required parameter 'storeId' is set
            if (storeId == null)
                throw new ApiException(400, "Missing required parameter 'storeId' when calling CMSContentModuleApi->ContentUploadContent");
            // verify the required parameter 'folderUrl' is set
            if (folderUrl == null)
                throw new ApiException(400, "Missing required parameter 'folderUrl' when calling CMSContentModuleApi->ContentUploadContent");

            var localVarPath = "/api/content/{contentType}/{storeId}";
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
            if (contentType != null) localVarPathParams.Add("contentType", Configuration.ApiClient.ParameterToString(contentType)); // path parameter
            if (storeId != null) localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter
            if (folderUrl != null) localVarQueryParams.Add("folderUrl", Configuration.ApiClient.ParameterToString(folderUrl)); // query parameter
            if (url != null) localVarQueryParams.Add("url", Configuration.ApiClient.ParameterToString(url)); // query parameter


            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (localVarStatusCode >= 400 && (localVarStatusCode != 404 || Configuration.ThrowExceptionWhenStatusCodeIs404))
                throw new ApiException (localVarStatusCode, "Error calling ContentUploadContent: " + localVarResponse.Content, localVarResponse.Content);
            else if (localVarStatusCode == 0)
                throw new ApiException (localVarStatusCode, "Error calling ContentUploadContent: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return new ApiResponse<List<VirtoCommerceContentWebModelsContentItem>>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (List<VirtoCommerceContentWebModelsContentItem>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<VirtoCommerceContentWebModelsContentItem>)));
            
        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>bool?</returns>
        public bool? MenuCheckName (string storeId, string name, string language, string id = null)
        {
             ApiResponse<bool?> localVarResponse = MenuCheckNameWithHttpInfo(storeId, name, language, id);
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
        /// <returns>ApiResponse of bool?</returns>
        public ApiResponse< bool? > MenuCheckNameWithHttpInfo (string storeId, string name, string language, string id = null)
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

            return new ApiResponse<bool?>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (bool?) Configuration.ApiClient.Deserialize(localVarResponse, typeof(bool?)));
            
        }

        /// <summary>
        /// Checking name of menu link list Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable
        /// </summary>
        /// <exception cref="VirtoCommerce.Client.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="storeId">Store id</param>
        /// <param name="name">Name of menu link list</param>
        /// <param name="language">Language of menu link list</param>
        /// <param name="id">Menu link list id (optional)</param>
        /// <returns>Task of bool?</returns>
        public async System.Threading.Tasks.Task<bool?> MenuCheckNameAsync (string storeId, string name, string language, string id = null)
        {
             ApiResponse<bool?> localVarResponse = await MenuCheckNameAsyncWithHttpInfo(storeId, name, language, id);
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
        /// <returns>Task of ApiResponse (bool?)</returns>
        public async System.Threading.Tasks.Task<ApiResponse<bool?>> MenuCheckNameAsyncWithHttpInfo (string storeId, string name, string language, string id = null)
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

            return new ApiResponse<bool?>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (bool?) Configuration.ApiClient.Deserialize(localVarResponse, typeof(bool?)));
            
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

    }
}
