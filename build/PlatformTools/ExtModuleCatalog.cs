using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;

namespace PlatformTools
{
    class ExtModuleCatalog
    {
        private static ExternalModuleCatalog _catalog;

        public static ExternalModuleCatalog GetCatalog(string authToken, LocalStorageModuleCatalog localCatalog, IEnumerable<string> manifestUrls)
        {
            if (_catalog == null)
            {
                var moduleCatalogOptions = new ExternalModuleCatalogOptions()
                {
                    ModulesManifestUrl = new Uri(manifestUrls.First()),
                    AuthorizationToken = authToken,
                    IncludePrerelease = false,
                    AutoInstallModuleBundles = new string[] { },
                    ExtraModulesManifestUrls = manifestUrls.Select(m => new Uri(m)).ToArray()
                };
                var options = Microsoft.Extensions.Options.Options.Create<ExternalModuleCatalogOptions>(moduleCatalogOptions);
                var client = new ExternalModulesClient(options);
                var logger = new LoggerFactory().CreateLogger<ExternalModuleCatalog>();
                _catalog = new ExternalModuleCatalog(localCatalog, client, options, logger);
                _catalog.Load();
            }
            _catalog.Reload();
            return _catalog;
        }
    }
}
