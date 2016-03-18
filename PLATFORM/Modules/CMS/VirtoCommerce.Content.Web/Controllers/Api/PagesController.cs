using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Content.Web.Security;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Web.Utilities;

namespace VirtoCommerce.Content.Web.Controllers.Api
{

    [RoutePrefix("api/cms/{storeId}/pages")]
    public class PagesController : ContentBaseController
    {
        #region Fields

        private readonly IContentStorageProvider _contentStorageProvider;
        #endregion

        #region Constructors and Destructors

        public PagesController(Func<IContentStorageProvider> contentStorageProviderFactory, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _contentStorageProvider = contentStorageProviderFactory();
        }

        #endregion

        /// <summary>
        /// Search pages
        /// </summary>
        /// <remarks>Get all pages by store and criteria</remarks>
        /// <param name="storeId">Store Id</param>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Page>))]
        [Route("")]
        public IHttpActionResult GetPages(string storeId)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });

            var result = InnerGetPages(storeId);
            var retVal = result.Pages.Concat(result.Folders.SelectMany(x => x.Pages)).ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Get pages folders by store id
        /// </summary>
        /// <param name="storeId">Store Id</param>
        [HttpGet]
        [ResponseType(typeof(GetPagesResult))]
        [Route("folders")]
        public IHttpActionResult GetFolders(string storeId)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });
            var retVal = InnerGetPages(storeId);
            return Ok(retVal);
        }

        /// <summary>
        /// Get page
        /// </summary>
        /// <remarks>Get page by store and name+language pair.</remarks>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        /// <response code="404">Page not found</response>
        /// <response code="200">Page returned OK</response>
        [HttpGet]
        [ResponseType(typeof(Page))]
        [ClientCache(Duration = 30)]
        [Route("{language}/{*pageName}")]
        public IHttpActionResult GetPage(string storeId, string language, string pageName)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });

            var blobItem = _contentStorageProvider.GetBlobInfo(GetFolderRelativeUrl(storeId, pageName));

            if (blobItem != null)
            {
                var page = blobItem.ToPageWebModel();
                page.Id = "/" + pageName;
                using (var stream = _contentStorageProvider.OpenRead(blobItem.Url))
                {
                    var data = stream.ReadFully();
                    if (ContentTypeUtility.IsImageContentType(page.ContentType))
                    {
                        page.ByteContent = data;
                    }
                    else if (ContentTypeUtility.IsTextContentType(page.ContentType))
                    {
                        page.Content = Encoding.UTF8.GetString(data);
                    }
                }
                return Ok(page);
            }

            return NotFound();

        }

        /// <summary>
        /// Check page name
        /// </summary>
        /// <remarks>Check page pair name+language for store</remarks>
        /// <param name="storeId">Store Id</param>
        /// <param name="language">Page language</param>
        /// <param name="pageName">Page name</param>
        [HttpGet]
        [ResponseType(typeof(CheckNameResult))]
        [Route("checkname")]
        public IHttpActionResult CheckName(string storeId, [FromUri]string pageName, [FromUri]string language)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });
            var response = new CheckNameResult { Result = true };
            return Ok(response);
        }

        /// <summary>
        /// Save page
        /// </summary>
        /// <param name="storeId">Store Id</param>
        /// <param name="page">Page</param>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SaveItem(string storeId, Page page)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Update, new ContentScopeObject { StoreId = storeId });

            var data = page.ByteContent;
            if (!string.IsNullOrEmpty(page.FileUrl))
            {
                using (var webClient = new WebClient())
                {
                    data = webClient.DownloadData(page.FileUrl);
                }
            }
            else if (data == null)
            {
                data = Encoding.UTF8.GetBytes(page.Content);
            }

            if (!string.IsNullOrEmpty(page.Language))
            {
                //add language to file name for new page
                var pageFileNameParts = page.Id.Split('.');
                if (pageFileNameParts.Length == 2)
                {
                    page.Id = pageFileNameParts[0] + "." + page.Language + "." + pageFileNameParts[1];
                }
            }

            using (var stream = _contentStorageProvider.OpenWrite(GetFolderRelativeUrl(storeId, page.Id)))
            using (var memStream = new MemoryStream(data))
            {
                memStream.CopyTo(stream);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete page
        /// </summary>
        /// <remarks>Delete pages with name+language pairs, that defined in pageNamesAndLanguges uri parameter</remarks>
        /// <param name="storeId">Store Id</param>
        /// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
        [HttpDelete]
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteItem(string storeId, [FromUri]string[] pageNamesAndLanguges)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Delete, new ContentScopeObject { StoreId = storeId });

            var pages = PagesUtility.GetShortPageInfoFromString(pageNamesAndLanguges);
            _contentStorageProvider.Remove(pages.Select(x => GetFolderRelativeUrl(storeId, x.Name)).ToArray());

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("blog/{blogName}")]
        public IHttpActionResult CreateBlog(string storeId, string blogName)
        {
            CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Create, new ContentScopeObject { StoreId = storeId });

            var page = GetDefaultBlog(blogName);
            SaveItem(storeId, page);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("blog/{blogName}")]
        public IHttpActionResult DeleteBlog(string storeId, string blogName)
        {
            _contentStorageProvider.Remove(new[] { GetFolderRelativeUrl(storeId, "blogs/" + blogName) });

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("blog/{blogName}/{oldBlogName}")]
        public IHttpActionResult UpdateBlog(string storeId, string blogName, string oldBlogName)
        {
            return StatusCode(HttpStatusCode.NoContent);
        }


        private Page GetDefaultBlog(string blogName)
        {
            var retVal = new Page
            {
                Id = string.Format("blogs/{0}/default.md", blogName),
                Name = string.Format("blogs/{0}/default.md", blogName),
                Content = string.Format("---title:  {0} --- ", blogName),
                ContentType = "text/html"
            };

            return retVal;
        }

        private GetPagesResult InnerGetPages(string storeId)
        {
            var retVal = new GetPagesResult();
            var result = _contentStorageProvider.Search(GetFolderRelativeUrl(storeId), null);
            foreach (var folder in result.Folders)
            {
                var pageFolder = LoadFolderRecursive(folder, folder.Name);
                retVal.Folders.Add(pageFolder);
            }
            foreach (var item in result.Items)
            {
                var page = item.ToPageWebModel();
                page.Id = item.FileName;
                retVal.Pages.Add(page);
            }
            return retVal;
        }

        private PageFolder LoadFolderRecursive(BlobFolder blobFolder, string path)
        {
            var retVal = new PageFolder
            {
                FolderName = blobFolder.Name
            };
            var result = _contentStorageProvider.Search(blobFolder.Url, null);

            foreach (var childFolder in result.Folders)
            {
                retVal.Folders.Add(LoadFolderRecursive(childFolder, path + "/" + childFolder.Name));
            }

            foreach (var item in result.Items)
            {
                var page = item.ToPageWebModel();
                page.Id = path + "/" + item.FileName;
                retVal.Pages.Add(page);
            }
            return retVal;
        }

        private static string GetFolderRelativeUrl(string storeId, string folderName = null)
        {
            if (folderName == null)
                return string.Format("/Pages/{0}", storeId);
            else
                return string.Format("/Pages/{0}/{1}", storeId, folderName);
        }
    }
}