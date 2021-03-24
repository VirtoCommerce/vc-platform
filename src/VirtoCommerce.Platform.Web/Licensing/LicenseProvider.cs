using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly IBlobStorageProvider _blobStorageProvider;
        private readonly IBlobUrlResolver _blobUrlResolver;

        public LicenseProvider(IOptions<PlatformOptions> platformOptions, IBlobStorageProvider blobStorageProvider, IBlobUrlResolver blobUrlResolver)
        {
            _platformOptions = platformOptions.Value;
            _blobStorageProvider = blobStorageProvider;
            _blobUrlResolver = blobUrlResolver;
        }

        public async Task<License> GetLicenseAsync()
        {
            License license = null;

            var licenseUrl = _blobUrlResolver.GetAbsoluteUrl(_platformOptions.LicenseBlobPath);
            if (await LicenseExistsAsync(licenseUrl))
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

        public void SaveLicense(License license)
        {
            using (var stream = _blobStorageProvider.OpenWrite(_blobUrlResolver.GetAbsoluteUrl(_platformOptions.LicenseBlobPath)))
            {
                var streamWriter = new StreamWriter(stream);
                streamWriter.Write(license.RawLicense);
                streamWriter.Flush();
            }
        }

        private async Task<bool> LicenseExistsAsync(string licenseUrl)
        {
            var blobInfo = await _blobStorageProvider.GetBlobInfoAsync(licenseUrl);
            return blobInfo != null;
        }
    }
}
