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
        private readonly TranslationOptions _options;
        private readonly IModuleService _moduleService;

        public ModulesTranslationDataProvider(IOptions<TranslationOptions> options, IModuleService moduleService)
        {
            _options = options.Value;
            _moduleService = moduleService;
        }

        protected override IEnumerable<string> DiscoveryFolders
        {
            get
            {
                // Get modules localization files ordered by dependency.
                return _moduleService.GetInstalledModules()
                    .Where(x => x.State == ModuleState.Initialized && !string.IsNullOrEmpty(x.FullPhysicalPath))
                    .Select(x => Path.Combine(x.FullPhysicalPath, _options.ModuleTranslationFolderName));
            }
        }
    }
}
