using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Framework.Web.Asset;
using VirtoCommerce.Framework.Web.Common;
using webModel = VirtoCommerce.CatalogModule.Web.Model;


namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/assets")]
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
