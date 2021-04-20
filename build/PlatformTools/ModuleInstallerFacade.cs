using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.TransactionFileManager;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Modules.External;
using VirtoCommerce.Platform.Data.TransactionFileManager;
using VirtoCommerce.Platform.Data.ZipFile;

namespace PlatformTools
{
    class ModuleInstallerFacade
    {
        private static ModuleInstaller _moduleInstaller;

        public static ModuleInstaller GetModuleInstaller(string discoveryPath, string probingPath, string authToken, IEnumerable<string> manifestUrls)
        {
            if(_moduleInstaller == null)
            {
                var fileManager = new TransactionFileManager();
                var fileSystem = new System.IO.Abstractions.FileSystem();
                var zipFileWrapper = new ZipFileWrapper(fileSystem, fileManager);
                var localCatalogOptions = LocalModuleCatalog.GetOptions(discoveryPath, probingPath);
                var extCatalogOptions = ExtModuleCatalog.GetOptions(authToken, manifestUrls);
                var localModuleCatalog = LocalModuleCatalog.GetCatalog(localCatalogOptions);
                var externalModuleCatalog = ExtModuleCatalog.GetCatalog(extCatalogOptions, localModuleCatalog);
                var modulesClient = new ExternalModulesClient(extCatalogOptions);
                _moduleInstaller = new ModuleInstaller(externalModuleCatalog, modulesClient, fileManager, localCatalogOptions, fileSystem, zipFileWrapper);
            }
            return _moduleInstaller;
        }
    }
}
