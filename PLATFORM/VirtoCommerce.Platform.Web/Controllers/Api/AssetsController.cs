using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Asset;
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
		/// <param name="folder">Folder name.</param>
		/// <returns></returns>
		[HttpPost]
		[Route("{folder}")]
		[ResponseType(typeof(webModel.BlobInfo[]))]
		public async Task<IHttpActionResult> UploadAsset(string folder)
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var blobMultipartProvider = new BlobStorageMultipartProvider(_blobProvider, _tempPath, folder);
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

        /// <summary>
        /// Deletes blobs by they key.
        /// </summary>
        /// <remarks>Delete blob by key</remarks>
        /// <param name="blobKey">blob key.</param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Delete([FromUri] string blobKey)
        {
             _blobProvider.Remove(blobKey);

            return StatusCode(HttpStatusCode.NoContent);
        }

    }


}
