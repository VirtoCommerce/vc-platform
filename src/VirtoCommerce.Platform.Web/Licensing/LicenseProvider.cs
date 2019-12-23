using System.IO;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;

        public LicenseProvider(IOptions<PlatformOptions> platformOptions)
        {
            _platformOptions = platformOptions.Value;
        }
        public License GetLicense()
        {
            License license = null;

            var licenseFilePath = Path.GetFullPath(_platformOptions.LicenseFilePath);
            if (System.IO.File.Exists(licenseFilePath))
            {
                var rawLicense = System.IO.File.ReadAllText(licenseFilePath);
                license = License.Parse(rawLicense, Path.GetFullPath(_platformOptions.LicensePublicKeyPath));

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }

            return license;
        }
    }
}
