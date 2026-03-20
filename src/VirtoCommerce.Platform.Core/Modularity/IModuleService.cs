using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Read-only provider for querying loaded modules.
/// Returns installed modules sorted by dependency (no-deps first).
/// </summary>
public interface IModuleService
{
    IReadOnlyList<ManifestModuleInfo> GetInstalledModules();
}
