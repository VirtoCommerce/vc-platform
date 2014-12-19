using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VirtoCommerce.Framework.Web.Common;
using webModel = VirtoCommerce.CatalogModule.Web.Model;


namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/assets")]
    public class AssetsController : ApiController
    {
        private readonly string _relativeDir = "Content/Uploads/";
		public AssetsController()
		{

		}
		[HttpPost]
		[Route("")]
        public async Task<webModel.BlobInfo[]> UploadAsset()
        {
			var streamProvider = await HttpRequestUploader.ReadDataAsync(Request, _relativeDir);

			var retVal = new List<webModel.BlobInfo>();

            foreach (var file in streamProvider.FileData)
            {
                var fInfo = new FileInfo(file.LocalFileName);

				retVal.Add(new webModel.BlobInfo
                {
                    Name = fInfo.Name,
                    Size = fInfo.Length.ToHumanReadableSize(),
                    MimeType = "application/octet-stream",
                    Url = string.Concat(_relativeDir, fInfo.Name)
                });
            }

            return retVal.ToArray();
        }
    
    }

  
}
