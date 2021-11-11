using System;
using System.IO;
using System.Threading.Tasks;
using VirtoCommerce.AzureBlobAssets.Abstractions;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProviderBlobStub : IAzureBlobProvider
    {
        private Exception _error => new PlatformException(@"Please install Azure Blob Assets module.");

        public string GetAbsoluteUrl(string blobKey)
        {
            return null;
        }

        public bool Exists(string blobUrl)
        {
            throw _error;
        }

        public Task<bool> ExistsAsync(string blobUrl)
        {
            throw _error;
        }

        public Stream OpenRead(string blobUrl)
        {
            throw _error;
        }

        public Task<Stream> OpenReadAsync(string blobUrl)
        {
            throw _error;
        }

        public Stream OpenWrite(string blobUrl)
        {
            throw _error;
        }

        public Task<Stream> OpenWriteAsync(string blobUrl)
        {
            throw _error;
        }
    }
}
