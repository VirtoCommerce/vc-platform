using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Web.Assets
{
    public class BlobStorageMultipartProvider : MultipartStreamProvider
    {
        private readonly IBlobStorageProvider _blobProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly string _rootPath;
        public BlobStorageMultipartProvider(IBlobStorageProvider blobProvider, IBlobUrlResolver blobUrlResolver, string rootPath)
        {
            _rootPath = rootPath;
            _blobProvider = blobProvider;
            _blobUrlResolver = blobUrlResolver;
            BlobInfos = new List<BlobInfo>();
        }

        public List<BlobInfo> BlobInfos { get; set; }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            var fileName = Path.GetFileName((headers.ContentDisposition.FileName ?? headers.ContentDisposition.Name).Replace("\"", string.Empty));
            var relativeUrl = _rootPath + "/" + fileName;
            var absoluteUrl = _blobUrlResolver.GetAbsoluteUrl(relativeUrl);

            BlobInfos.Add(new BlobInfo
            {
                ContentType = MimeTypeResolver.ResolveContentType(fileName),
                FileName = fileName,
                Key = relativeUrl,
                Url = absoluteUrl
            });

            return _blobProvider.OpenWrite(_rootPath + "/" + fileName);
        }
    }
}
