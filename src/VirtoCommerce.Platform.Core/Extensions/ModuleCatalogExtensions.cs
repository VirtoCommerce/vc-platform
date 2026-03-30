using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.Extensions
{
    public static class ModuleCatalogExtensions
    {
        [Obsolete("Use ModuleRegistry.IsInstalled(moduleId) instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public static bool IsModuleInstalled(this IModuleCatalog moduleCatalog, string moduleId)
        {
            return moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Any(x => x.Id.EqualsIgnoreCase(moduleId) && x.IsInstalled);
        }
    }
}
