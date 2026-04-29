using System.Collections.Generic;
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

/// <summary>
/// Service-layer descriptor returned by <see cref="IAppManifestService.GetManifest"/>.
/// The web layer maps this to the public <c>AppManifestResponse</c> DTO.
/// </summary>
public class AppManifestDescriptor
{
    public string AppId { get; set; }

    /// <summary>
    /// Version of the host app — the running platform version for the reserved
    /// <c>platform</c> app id, otherwise the version of the module that declares
    /// the <c>&lt;app&gt;</c> element. Surfaces directly in the JSON response.
    /// </summary>
    public string Version { get; set; }
    public string Title { get; set; }
    public string AppPermission { get; set; }
    public IList<PluginDescriptor> Plugins { get; set; } = new List<PluginDescriptor>();
}

/// <summary>
/// File-asset kinds the modularity framework recognises. Mirrors
/// the <c>ContentFileTypes</c> constants in the web layer (kept in sync as
/// plain strings to keep the JSON contract stable across renames).
/// </summary>
public static class ContentFileTypes
{
    public const string Script = "script";
    public const string Style = "style";
}

/// <summary>
/// Service-layer asset descriptor. Mirrors <c>ContentFile</c> in the web layer.
/// Carries the asset's <see cref="Type"/>, public URL <see cref="Path"/>, and
/// cache-busting <see cref="Hash"/> so client-side loaders can build correct
/// <c>&lt;script&gt;</c> / <c>&lt;link&gt;</c> tags without re-probing the file.
/// </summary>
public class ContentFileDescriptor
{
    public string Type { get; set; }
    public string Path { get; set; }
    public string Hash { get; set; }
}

/// <summary>
/// Service-layer plugin descriptor. Mirrors <c>PluginEntry</c> in the web layer.
/// </summary>
public class PluginDescriptor
{
    public string Id { get; set; }
    public string Version { get; set; }
    public ContentFileDescriptor Entry { get; set; }
    public IList<ContentFileDescriptor> ContentFiles { get; set; } = new List<ContentFileDescriptor>();
    public PluginRemoteDescriptor Remote { get; set; }
    public string Permission { get; set; }
}

public class PluginRemoteDescriptor
{
    public string Name { get; set; }
    public string Exposed { get; set; }
}
