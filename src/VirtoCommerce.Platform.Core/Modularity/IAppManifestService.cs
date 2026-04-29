using System.Security.Claims;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Resolves the manifest of plugins a backoffice host app should load, by
/// walking the topologically sorted module list and probing each module for
/// plugin descriptors targeting the requested app.
/// </summary>
/// <remarks>
/// Implementation lives in <c>VirtoCommerce.Platform.Modules</c>. The shape of
/// <see cref="PluginDescriptor"/> intentionally mirrors the web DTO
/// <c>PluginEntry</c> so the controller can pass through with minimal mapping.
/// </remarks>
public interface IAppManifestService
{
    /// <summary>
    /// Returns the host app's manifest (metadata + ordered plugin list) for the
    /// given app id.
    /// </summary>
    /// <param name="appId">
    /// Host app id declared by some installed module's <c>&lt;app&gt;</c> element.
    /// The reserved id <c>platform</c> targets the legacy AngularJS admin and
    /// triggers the hardcoded <c>dist/app.js</c> + <c>dist/style.css</c> probe
    /// instead of the Module Federation plugin probe.
    /// </param>
    /// <param name="user">
    /// The current user; used to filter contributions and plugins whose
    /// <c>permission</c> the user lacks. Pass <c>null</c> for unauthenticated
    /// callers — the implementation should still return non-permissioned
    /// plugins (no permission required).
    /// </param>
    /// <returns>
    /// The resolved host app + plugin list, or <c>null</c> if no installed
    /// module declares an app with the given id.
    /// </returns>
    AppManifestDescriptor GetManifest(string appId, ClaimsPrincipal user = null);
}
