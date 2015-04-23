using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Asset;
using webModel = VirtoCommerce.Platform.Web.Model.Asset;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/assets")]
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

        [HttpPost]
        [Route("")]
        public async Task<webModel.BlobInfo[]> UploadAsset()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var blobMultipartProvider = new BlobStorageMultipartProvider(_blobProvider, _tempPath, "catalog");
            await Request.Content.ReadAsMultipartAsync(blobMultipartProvider);

            var retVal = new List<webModel.BlobInfo>();

            foreach (var blobInfo in blobMultipartProvider.BlobInfos)
            {
                retVal.Add(new webModel.BlobInfo
                {
                    Name = blobInfo.FileName,
                    Size = blobInfo.Size.ToHumanReadableSize(),
                    MimeType = blobInfo.ContentType,
                    Url = _urlResolver.GetAbsoluteUrl(blobInfo.Key)
                });
            }

            return retVal.ToArray();
        }

    }


}
