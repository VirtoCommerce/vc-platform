using System;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly IScopedFactory<IPlatformRepository> _platformRepositoryFactory;

        [Obsolete("Use the constructor that takes IScopedFactory<T> instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        protected LicenseProvider(IOptions<PlatformOptions> platformOptions, Func<IPlatformRepository> platformRepositoryFactory)
            : this(platformOptions, new DelegateScopedFactory<IPlatformRepository>(platformRepositoryFactory))
        {
        }

        public LicenseProvider(IOptions<PlatformOptions> platformOptions, IScopedFactory<IPlatformRepository> platformRepositoryFactory)
        {
            _platformOptions = platformOptions.Value;
            _platformRepositoryFactory = platformRepositoryFactory;
        }

        public License GetLicense()
        {
            string rawLicenseData;

            using (var scopedRepository = _platformRepositoryFactory.Create())
            {
                var repository = scopedRepository.Service;
                rawLicenseData = repository.RawLicenses.OrderBy(x => x.Id).FirstOrDefault()?.Data;
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
            using (var scopedRepository = _platformRepositoryFactory.Create())
            {
                var repository = scopedRepository.Service;
                var rawLicense = repository.RawLicenses.OrderBy(x => x.Id).FirstOrDefault();
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
