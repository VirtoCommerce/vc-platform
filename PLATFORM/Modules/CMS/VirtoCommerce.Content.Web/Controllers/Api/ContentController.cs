using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Content.Web.Security;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Asset;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
    [RoutePrefix("api/content/{contentType}/{storeId}")]
    public class ContentController : ContentBaseController
    {
        private readonly Func<string, IContentBlobStorageProvider> _contentStorageProviderFactory;
        private readonly IBlobUrlResolver _urlResolver;
        private readonly IStoreService _storeService;
        public ContentController(Func<string, IContentBlobStorageProvider> contentStorageProviderFactory, IBlobUrlResolver urlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService, IStoreService storeService)
            : base(securityService, permissionScopeService)
        {
            _storeService = storeService;
            _contentStorageProviderFactory = contentStorageProviderFactory;
            _urlResolver = urlResolver;
        }

        /// <summary>
        /// Return summary content statistic 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>Object contains counters with main content types</returns>
        [HttpGet]
        [Route("~/api/content/{storeId}/stats")]
        [ResponseType(typeof(ContentStatistic))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Read)]
        public IHttpActionResult GetStoreContentStats(string storeId)
        {
            var contentStorageProvider = _contentStorageProviderFactory("");
            var store = _storeService.GetById(storeId);
            var retVal = new ContentStatistic
            {
                ActiveThemeName = store.GetDynamicPropertyValue("DefaultThemeName", "not set"),
                ThemesCount = contentStorageProvider.Search("Themes/" + storeId, null).Folders.Count(),
                BlogsCount = contentStorageProvider.Search("Pages/" + storeId + "/blogs", null).Folders.Count(),
                PagesCount = contentStorageProvider.Search("Pages/" + storeId, null).Items.Count()
            };
            return Ok(retVal);
        }


        /// <summary>
        /// Delete content from server
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id </param>
        /// <param name="urls">relative content urls to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Delete)]
        public IHttpActionResult DeleteContent(string contentType, string storeId, [FromUri] string[] urls)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            storageProvider.Remove(urls);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Return streamed data for requested by relativeUrl content (Used to prevent Cross domain requests in manager)
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="relativeUrl">content relative url</param>
        /// <returns>stream</returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(byte[]))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Read)]
        public HttpResponseMessage GetContentItemDataStream(string contentType, string storeId, string relativeUrl)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));
            var stream = storageProvider.OpenRead(relativeUrl);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(relativeUrl));
            return result;
        }


        /// <summary>
        /// Search content items in specified folder and using search keyword
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">relative path for folder where content items will be searched</param>
        /// <param name="keyword">search keyword</param>
        /// <returns>content items</returns>
        [HttpGet]
        [Route("search")]
        [ResponseType(typeof(ContentItem[]))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Read)]
        public IHttpActionResult SearchContent(string contentType, string storeId, string folderUrl = null, string keyword = null)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            var result = storageProvider.Search(folderUrl, keyword);
            var retVal = result.Folders.Select(x => x.ToContentModel())
                               .OfType<ContentItem>()
                               .Concat(result.Items.Select(x => x.ToContentModel()))
                               .ToArray();
            return Ok(retVal);
        }

        /// <summary>
        /// Rename or move content item
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="oldUrl">old content item relative or absolute url</param>
        /// <param name="newUrl">new content item relative or absolute url</param>
        /// <returns></returns>
        [HttpGet]
        [Route("move")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Update)]
        public IHttpActionResult MoveContent(string contentType, string storeId, string oldUrl, string newUrl)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            storageProvider.MoveContent(oldUrl, newUrl);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copy contents
        /// </summary>
        /// <param name="srcPath">source content  relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        [HttpGet]
        [Route("~/api/content/copy")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Update)]
        public IHttpActionResult CopyContent(string srcPath, string destPath)
        {
            //This method used only for default themes copying that we use string.Empty instead storeId because default themes placed only in root content folder
            var storageProvider = _contentStorageProviderFactory(string.Empty);

            storageProvider.CopyContent(srcPath, destPath);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Unpack contents
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="archivePath">archive file relative path</param>
        /// <param name="destPath">destination content relative path</param>
        /// <returns></returns>
        [HttpGet]
        [Route("unpack")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Update)]
        public IHttpActionResult Unpack(string contentType, string storeId, string archivePath, string destPath)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            using (var stream = storageProvider.OpenRead(archivePath))
            using (ZipArchive archive = new ZipArchive(stream))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (!entry.FullName.EndsWith("/"))
                    {
                        var fileName = String.Join("/", entry.FullName.Split('/').Skip(1));
                        using (var entryStream = entry.Open())
                        using (var targetStream = storageProvider.OpenWrite(destPath + "/" + fileName))
                        {
                            entryStream.CopyTo(targetStream);
                        }
                    }
                }
                //remove archive after unpack
                storageProvider.Remove(new[] { archivePath });
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Create content folder 
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folder">content folder</param>
        /// <returns></returns>
        [HttpPost]
        [Route("folder")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Create)]
        public IHttpActionResult CreateContentFolder(string contentType, string storeId, ContentFolder folder)
        {
            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            storageProvider.CreateFolder(folder.ToBlobModel());
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Upload content item 
        /// </summary>
        /// <param name="contentType">possible values Themes or Pages</param>
        /// <param name="storeId">Store id</param>
        /// <param name="folderUrl">folder relative url where content will be uploaded</param>
        /// <param name="url">external url which will be used to download content item data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(ContentItem[]))]
        [CheckPermission(Permission = ContentPredefinedPermissions.Create)]
        public async Task<IHttpActionResult> UploadContent(string contentType, string storeId, [FromUri] string folderUrl, [FromUri]string url = null)
        {
            if (url == null && !Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var storageProvider = _contentStorageProviderFactory(GetContentBasePath(contentType, storeId));

            if (url != null)
            {
                var fileName = HttpUtility.UrlDecode(System.IO.Path.GetFileName(url));
                var fileUrl = folderUrl + "/" + fileName;
                using (var client = new WebClient())
                using (var blobStream = storageProvider.OpenWrite(fileUrl))
                using (var remoteStream = client.OpenRead(url))
                {
                    remoteStream.CopyTo(blobStream);

                    var retVal = new ContentFile
                    {
                        Name = fileName,
                        Url = _urlResolver.GetAbsoluteUrl(fileUrl)
                    };
                    return Ok(retVal);
                }
            }
            else
            {
                var blobMultipartProvider = new BlobStorageMultipartProvider(storageProvider, _urlResolver, folderUrl);
                await Request.Content.ReadAsMultipartAsync(blobMultipartProvider);

                var retVal = new List<ContentFile>();

                foreach (var blobInfo in blobMultipartProvider.BlobInfos)
                {
                    retVal.Add(new ContentFile
                    {
                        Name = blobInfo.FileName,
                        Url = _urlResolver.GetAbsoluteUrl(blobInfo.Key)
                    });
                }

                return Ok(retVal.ToArray());
            }
        }

        private string GetContentBasePath(string contentType, string storeId)
        {
            var retVal = string.Empty;
            if (contentType.EqualsInvariant("themes"))
            {
                retVal = "Themes/" + storeId;
            }
            else if (contentType.EqualsInvariant("pages"))
            {
                retVal = "Pages/" + storeId;
            }
            else if (contentType.EqualsInvariant("blogs"))
            {
                retVal = "Pages/" + storeId + "/blogs";
            }
            return retVal;
        }
    }
}
