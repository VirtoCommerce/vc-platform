using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;

namespace PlatformTools
{
    class LocalModuleCatalog
    {
        private static LocalStorageModuleCatalog _catalog;

        public static LocalStorageModuleCatalog GetCatalog(string discoveryPath, string probingPath)
        {
            if(_catalog == null)
            {
                var moduleCatalogOptions = new LocalStorageModuleCatalogOptions()
                {
                    RefreshProbingFolderOnStart = true,
                    DiscoveryPath = discoveryPath,
                    ProbingPath = probingPath,
                };
                var options = Microsoft.Extensions.Options.Options.Create<LocalStorageModuleCatalogOptions>(moduleCatalogOptions);
                var logger = new LoggerFactory().CreateLogger<LocalStorageModuleCatalog>();
                _catalog = new LocalStorageModuleCatalog(options, logger);
                _catalog.Load();
            }
            _catalog.Reload();
            return _catalog;
        }
    }
}
