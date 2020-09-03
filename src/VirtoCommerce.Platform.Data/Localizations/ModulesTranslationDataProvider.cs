using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Data.Localizations
{
    public class ModulesTranslationDataProvider : FileSystemTranslationDataProvider
    {
        private readonly ILocalModuleCatalog _moduleCatalog;
        private readonly TranslationOptions _options;

        public ModulesTranslationDataProvider(ILocalModuleCatalog moduleCatalog, IOptions<TranslationOptions> options)
        {
            _moduleCatalog = moduleCatalog;
            _options = options.Value;
        }

        protected override IEnumerable<string> DiscoveryFolders
        {
            get
            {
                // Get modules localization files ordered by dependency.
                var allModules = _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.State == ModuleState.Initialized && !x.Errors.Any()).ToArray();
                var manifestModules = _moduleCatalog.CompleteListWithDependencies(allModules).Where(x => x.State == ModuleState.Initialized)
                    .OfType<ManifestModuleInfo>();

                foreach (var manifest in manifestModules.Where(x => !string.IsNullOrEmpty(x.FullPhysicalPath)))
                {
                   yield return Path.Combine(manifest.FullPhysicalPath, _options.ModuleTranslationFolderName);
                }
            }
        }
    }
}
