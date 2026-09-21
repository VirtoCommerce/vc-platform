using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public class LicenseProvider
    {
        private readonly PlatformOptions _platformOptions;
        private readonly IScopedServiceFactory<IPlatformRepository> _platformRepositoryFactory;

        [Obsolete("Use the constructor that takes IScopedServiceFactory<T> instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public LicenseProvider(IOptions<PlatformOptions> platformOptions, Func<IPlatformRepository> platformRepositoryFactory)
            : this(platformOptions, new DelegateScopedServiceFactory<IPlatformRepository>(platformRepositoryFactory))
        {
        }

        [ActivatorUtilitiesConstructor]
        public LicenseProvider(IOptions<PlatformOptions> platformOptions, IScopedServiceFactory<IPlatformRepository> platformRepositoryFactory)
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
