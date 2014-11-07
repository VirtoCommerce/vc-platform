using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using webModel = VirtoCommerce.CatalogModule.Web.Model;


namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class AssetsController : ApiController
    {
        private readonly string _relativeDir = "Content/Uploads/";


        [HttpPost]
        public async Task<webModel.ProductImage[]> Upload()
        {
            var streamProvider = await ReadDataAsync();

            var retVal = new List<webModel.ProductImage>();

            foreach (var file in streamProvider.FileData)
            {
                retVal.Add(new webModel.ProductImage
                {
                    Id = Guid.NewGuid().ToString(),
                    //ItemId = int.Parse(streamProvider.FormData["itemId"]),
                    Url = string.Concat(_relativeDir, Path.GetFileName(file.LocalFileName))
                });
            }

            return retVal.ToArray();
        }

        [HttpPut]
        public async Task<webModel.ProductAsset[]> UploadAsset()
        {
            var streamProvider = await ReadDataAsync();

            var retVal = new List<webModel.ProductAsset>();

            foreach (var file in streamProvider.FileData)
            {
                var fInfo = new FileInfo(file.LocalFileName);

                retVal.Add(new webModel.ProductAsset
                {
                    Id = Guid.NewGuid().ToString(),
                    //ItemId = int.Parse(streamProvider.FormData["itemId"]),
                    Name = fInfo.Name,
                    Size = FormatInformationSize(fInfo.Length),
                    MimeType = "application/octet-stream",

                    Url = string.Concat(_relativeDir, fInfo.Name)
                });
            }

            return retVal.ToArray();
        }

        private async Task<CustomMultipartFormDataStreamProvider> ReadDataAsync()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var localPath = HttpContext.Current.Server.MapPath("~/" + _relativeDir);

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            var streamProvider = new CustomMultipartFormDataStreamProvider(localPath);
            await Request.Content.ReadAsMultipartAsync(streamProvider);
            return streamProvider;
        }

        public static string FormatInformationSize(long len)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }

    // We implement MultipartFormDataStreamProvider to override the filename of File which 
    // will be stored on server, or else the default name will be of the format like Body- 
    // Part_{GUID}. In the following implementation we simply get the FileName from 
    // ContentDisposition Header of the Request Body.
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }

}
