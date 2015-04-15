using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using VirtoCommerce.CoreModule.Web.Assets;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Framework.Web.Asset;
using VirtoCommerce.Framework.Web.Common;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/assets")]
    public class AssetsController : ApiController
    {
        private readonly IBlobStorageProvider _blobProvider;
        private readonly string _tempPath;
        public AssetsController(IBlobStorageProvider blobProvider)
        {
            _blobProvider = blobProvider;
            _tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
        }

        [HttpPost]
        [Route("")]
        public async Task<BlobInfo[]> UploadAsset()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var blobMultipartProvider = new BlobStorageMultipartProvider(_blobProvider, _tempPath, "catalog");
            await Request.Content.ReadAsMultipartAsync(blobMultipartProvider);

            var retVal = new List<BlobInfo>();

            foreach (var blobInfo in blobMultipartProvider.BlobInfos)
            {
                retVal.Add(new BlobInfo
                {
                    Name = blobInfo.Name,
                    Size = blobInfo.Size.ToHumanReadableSize(),
                    MimeType = blobInfo.ContentType,
                    Url = blobInfo.Location
                });
            }

            return retVal.ToArray();
        }

    }


}
