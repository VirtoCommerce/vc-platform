using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Cache region for <see cref="AppManifestService"/>'s plugin-probe
    /// results. The probe walks every installed module's plugin folder
    /// and reads each <c>plugin.json</c>, so its result is cached for
    /// the process lifetime.
    /// Region tokens let an admin (or a future hot-reload feature) flush
    /// every cached app manifest in one call via
    /// <c>AppManifestCacheRegion.ExpireRegion()</c>.
    /// </summary>
    public class AppManifestCacheRegion : CancellableCacheRegion<AppManifestCacheRegion>
    {
    }
}
