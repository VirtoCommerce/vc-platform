using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Asset;
using VirtoCommerce.Platform.Web.Converters.Asset;
using webModel = VirtoCommerce.Platform.Web.Model.Asset;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/assets")]
    public class AssetsController : ApiController
    {
        private readonly IBlobStorageProvider _blobProvider;
        private readonly IBlobUrlResolver _urlResolver;
        private readonly string _tempPath;
        public AssetsController(IBlobStorageProvider blobProvider, IBlobUrlResolver urlResolver)
        {
            _blobProvider = blobProvider;
            _urlResolver = urlResolver;
            _tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
        }


        /// <summary>
        /// Upload assets to the folder
        /// </summary>
        /// <remarks>
        /// Request body can contain multiple files.
        /// </remarks>
        /// <param name="folderUrl">Parent folder url (relative or absolute).</param>
        /// <param name="url">Url for uploaded remote resource (optional)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(webModel.BlobInfo[]))]
        [CheckPermission(Permission = PredefinedPermissions.AssetCreate)]
        public async Task<IHttpActionResult> UploadAsset([FromUri] string folderUrl, [FromUri]string url = null)
        {
            if (url == null && !Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            if (url != null)
            {
                using (var client = new WebClient())
                {
                    var uploadInfo = new UploadStreamInfo
                    {
                        FileByteStream = client.OpenRead(url),
                        FolderName = folderUrl,
                        FileName = HttpUtility.UrlDecode(System.IO.Path.GetFileName(url))
                    };

                    var key = _blobProvider.Upload(uploadInfo);
                    var retVal = new webModel.BlobInfo
                    {

                        Name = uploadInfo.FileName,
                        RelativeUrl = key,
                        Url = _urlResolver.GetAbsoluteUrl(key)
                    };
                    return Ok(retVal);
                }
            }
            else
            {
                var blobMultipartProvider = new BlobStorageMultipartProvider(_blobProvider, _tempPath, folderUrl);
                await Request.Content.ReadAsMultipartAsync(blobMultipartProvider);

                var retVal = new List<webModel.BlobInfo>();

                foreach (var blobInfo in blobMultipartProvider.BlobInfos)
                {
                    retVal.Add(new webModel.BlobInfo
                    {
                        Name = blobInfo.FileName,
                        Size = blobInfo.Size.ToString(),
                        MimeType = blobInfo.ContentType,
                        RelativeUrl = blobInfo.Key,
                        Url = _urlResolver.GetAbsoluteUrl(blobInfo.Key)
                    });
                }

                return Ok(retVal.ToArray());
            }
        }

        /// <summary>
        /// Delete blobs by urls
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.AssetDelete)]
        public IHttpActionResult DeleteBlobs([FromUri] string[] urls)
        {
            _blobProvider.Remove(urls);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Search asset folders and blobs
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.AssetListItem[]))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.AssetRead)]
        public IHttpActionResult SearchAssetItems(string folderUrl = null, string keyword = null)
        {
            var result = _blobProvider.Search(folderUrl, keyword);
            return Ok(result.ToWebModel());
        }

        /// <summary>
        /// Create new blob folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("folder")]
        [CheckPermission(Permission = PredefinedPermissions.AssetCreate)]
        public IHttpActionResult CreateBlobFolder(BlobFolder folder)
        {
            _blobProvider.CreateFolder(folder);
            return StatusCode(HttpStatusCode.NoContent);
        }

    }


}
