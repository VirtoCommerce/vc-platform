using System;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly Func<IPlatformRepository> _platformRepositoryFactory;

        public LicenseProvider(IOptions<PlatformOptions> platformOptions, Func<IPlatformRepository> platformRepositoryFactory)
        {
            _platformOptions = platformOptions.Value;
            _platformRepositoryFactory = platformRepositoryFactory;
        }

        public License GetLicense()
        {
            string rawLicenseData;

            using (var repository = _platformRepositoryFactory())
            {
                rawLicenseData = repository.RawLicenses?.FirstOrDefault()?.Data;
            }

            var license = License.Parse(rawLicenseData, _platformOptions.LicensePublicKeyResourceName);

            if (license != null)
            {
                license.RawLicense = null;
            }

            return license;
        }

        public void SaveLicense(License license)
        {
            using (var repository = _platformRepositoryFactory())
            {
                var rawLicense = repository.RawLicenses?.FirstOrDefault();
                if (rawLicense == null)
                {
                    rawLicense = new Data.Model.RawLicenseEntity();
                    repository.Add(rawLicense);
                }
                rawLicense.Data = license.RawLicense;                
                repository.UnitOfWork.Commit();
            }
        }
    }
}
