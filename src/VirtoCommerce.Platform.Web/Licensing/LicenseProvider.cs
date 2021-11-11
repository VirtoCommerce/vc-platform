using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.AzureBlobAssets.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly IAzureBlobProvider _blobStorageProvider;

        public LicenseProvider(IOptions<PlatformOptions> platformOptions, IAzureBlobProvider blobStorageProvider)
        {
            _platformOptions = platformOptions.Value;
            _blobStorageProvider = blobStorageProvider;
        }

        public async Task<License> GetLicenseAsync()
        {
            License license = null;

            var licenseUrl = _blobStorageProvider.GetAbsoluteUrl(_platformOptions.LicenseBlobPath);
            if (licenseUrl != null && await LicenseExistsAsync(licenseUrl))
            {
                var rawLicense = string.Empty;
                using (var stream = _blobStorageProvider.OpenRead(licenseUrl))
                {
                    rawLicense = stream.ReadToString();
                }

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
            var licenseUrl = _blobStorageProvider.GetAbsoluteUrl(_platformOptions.LicenseBlobPath);

            if (licenseUrl == null)
            {
                throw new PlatformException(@"File system not supported for licence. Use Azure Blob Storage.");
            }

            using (var stream = await _blobStorageProvider.OpenWriteAsync(licenseUrl))
            {
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(license.RawLicense);
                streamWriter.Flush();
            }
        }

        private async Task<bool> LicenseExistsAsync(string licenseUrl)
        {
            var blobInfo = await _blobStorageProvider.ExistsAsync(licenseUrl);
            return blobInfo;
        }
    }
}
