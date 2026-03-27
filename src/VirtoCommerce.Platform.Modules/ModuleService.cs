using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// DI-friendly provider that delegates to static ModuleRegistry + ModuleRunner.
/// Always returns modules sorted by dependency (no-deps first).
/// </summary>
public class ModuleService : IModuleService
{
    public IList<ManifestModuleInfo> GetInstalledModules()
    {
        return ModuleRegistry.GetInstalledModules();
    }
}
