using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using VirtoCommerce.Assets.Abstractions;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Web.Extensions;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public sealed class LicenseProviderBlobStub : ICommonBlobProvider
    {
        private readonly string _storagePath;
        private readonly string _rootPath = "~/assets";

        public LicenseProviderBlobStub(IWebHostEnvironment hostingEnvironment)
        {
            var path = hostingEnvironment.MapPath(_rootPath);
            _storagePath = path.TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public string GetAbsoluteUrl(string blobKey)
        {
            if (blobKey == null)
            {
                throw new ArgumentNullException(nameof(blobKey));
            }

            return new Uri(blobKey, UriKind.Relative).ToString();
        }

        public bool Exists(string blobUrl)
        {
            return ExistsAsync(blobUrl).GetAwaiter().GetResult();
        }

        public Task<bool> ExistsAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
            {
                throw new ArgumentNullException(nameof(blobUrl));
            }

            var filePath = GetStoragePathFromUrl(blobUrl);

            ValidatePath(filePath);

            var exists = File.Exists(filePath);

            return Task.FromResult(exists);
        }

        public Stream OpenRead(string blobUrl)
        {
            var filePath = GetStoragePathFromUrl(blobUrl);

            ValidatePath(filePath);

            return File.Open(filePath, FileMode.Open, FileAccess.Read);
        }

        public Task<Stream> OpenReadAsync(string blobUrl)
        {
            return Task.FromResult(OpenRead(blobUrl));
        }

        public Stream OpenWrite(string blobUrl)
        {
            var filePath = GetStoragePathFromUrl(blobUrl);
            var folderPath = Path.GetDirectoryName(filePath);

            ValidatePath(filePath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return File.Open(filePath, FileMode.Create);
        }

        public Task<Stream> OpenWriteAsync(string blobUrl)
        {
            return Task.FromResult(OpenWrite(blobUrl));
        }

        private string GetStoragePathFromUrl(string url)
        {
            var result = _storagePath;
            if (url != null)
            {
                result = _storagePath + Path.DirectorySeparatorChar;
                result += url;
                result = result.Replace('/', Path.DirectorySeparatorChar)
                               .Replace($"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}");
            }
            return Uri.UnescapeDataString(result);
        }

        private void ValidatePath(string path)
        {
            path = Path.GetFullPath(path);
            if (!path.StartsWith(_storagePath))
            {
                throw new PlatformException($"Invalid path {path}");
            }
        }
    }
}
