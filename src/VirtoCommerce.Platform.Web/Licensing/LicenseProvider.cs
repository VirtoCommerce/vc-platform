using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _cdnUrl;

        public LicenseProvider(IOptions<PlatformOptions> platformOptions, IOptions<LicenceProviderBlobOptions> blobOptions)
        {
            _platformOptions = platformOptions.Value;

            _cdnUrl = blobOptions.Value.CdnUrl;
            if (!string.IsNullOrEmpty(blobOptions.Value.ConnectionString))
            {
                _blobServiceClient = new BlobServiceClient(blobOptions.Value.ConnectionString);
            }
        }

        public async Task<License> GetLicenseAsync()
        {
            License license = null;

            var licenseUrl = GetAbsoluteUrl(_platformOptions.LicenseBlobPath);
            if (licenseUrl != null && await LicenseExistsAsync(licenseUrl))
            {
                var rawLicense = await DownloadRawLicence(licenseUrl);

                license = License.Parse(rawLicense, _platformOptions.LicensePublicKeyResourceName);

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }

            // Fallback to the old file system implementation
            if (license == null)
            {
                license = GetLicenseFromFile();
            }

            return license;
        }

        public License GetLicenseFromFile()
        {
            License license = null;

            var licenseFilePath = Path.GetFullPath(_platformOptions.LicenseFilePath);
            if (File.Exists(licenseFilePath))
            {
                var rawLicense = File.ReadAllText(licenseFilePath);
                license = License.Parse(rawLicense, _platformOptions.LicensePublicKeyResourceName);

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }

            return license;
        }

        public async Task SaveLicenseAsync(License license)
        {
            var licenceUrl = GetAbsoluteUrl(_platformOptions.LicenseBlobPath);

            if (licenceUrl == null)
            {
                throw new PlatformException(@"File system not supported for licence. Use Azure Blob Storage.");
            }

            var filePath = GetFilePathFromUrl(licenceUrl);

            if (filePath == null)
            {
                throw new ArgumentException(@"Cannot get file path from URL", nameof(licenceUrl));
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(GetOutlineFromUrl(licenceUrl).First());
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(license.RawLicense));
            await blobClient.UploadAsync(stream, true);
        }

        private async Task<bool> LicenseExistsAsync(string licenseUrl)
        {
            try
            {
                var blob = GetClientByUrl(licenseUrl);
                var response = await blob.ExistsAsync();
                return response.Value;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> DownloadRawLicence(string licenseUrl)
        {
            var blob = GetClientByUrl(licenseUrl);
            var result = await blob.DownloadAsync();
            return result.Value.Content.ReadToString();
        }

        private BlobClient GetClientByUrl(string licenseUrl)
        {
            if (string.IsNullOrEmpty(licenseUrl))
            {
                throw new ArgumentNullException(nameof(licenseUrl));
            }

            var uri = licenseUrl.IsAbsoluteUrl()
                ? new Uri(licenseUrl)
                : new Uri(_blobServiceClient.Uri, licenseUrl.TrimStart('/'));

            return new BlobClient(new Uri(_blobServiceClient.Uri, uri.AbsolutePath.TrimStart('/')));
        }

        private string GetAbsoluteUrl(string blobKey)
        {
            if (_blobServiceClient == null)
            {
                return null;
            }

            if (blobKey.IsAbsoluteUrl())
            {
                return blobKey;
            }

            var baseUrl = _blobServiceClient.Uri.AbsoluteUri;

            if (!string.IsNullOrWhiteSpace(_cdnUrl))
            {
                var cdnUriBuilder = new UriBuilder(_blobServiceClient.Uri.Scheme, _cdnUrl);
                baseUrl = cdnUriBuilder.Uri.AbsoluteUri;
            }

            var result = UrlHelperExtensions.Combine(baseUrl, Uri.EscapeUriString(blobKey));
            return result;
        }

        private string[] GetOutlineFromUrl(string url)
        {
            var relativeUrl = url;
            if (url.IsAbsoluteUrl())
            {
                relativeUrl = Uri.UnescapeDataString(new Uri(url).AbsolutePath);
            }

            return relativeUrl.Split(new[] { "/", "\\" },
                StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetFilePathFromUrl(string url)
        {
            var result = string.Join("/", GetOutlineFromUrl(url).Skip(1).ToArray());
            return !string.IsNullOrEmpty(result) ? result : null;
        }
    }
}
