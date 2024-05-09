using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.Extensions
{
    public static class ModuleCatalogExtensions
    {
        public static bool IsModuleInstalled(this IModuleCatalog moduleCatalog, string moduleId)
        {
            return moduleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Any(x => x.Id.EqualsInvariant(moduleId) && x.IsInstalled);
        }
    }
}
