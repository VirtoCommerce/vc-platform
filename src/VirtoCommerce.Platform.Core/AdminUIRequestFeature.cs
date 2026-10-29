namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Set by the platform on a request for an admin UI's page, and on every request that came through an admin UI's
/// <c>/apps/{appId}/</c> path (<see cref="Modularity.AdminUIHosting.Endpoint"/> admin UIs). The platform handles such a
/// request as if it came to the site root, so its request path no longer shows the app path: an admin UI page that
/// relies on relative URLs puts <c>PathBase + MountPath + "/"</c> into its <c>&lt;base href&gt;</c>.
/// </summary>
public sealed class AdminUIRequestFeature(string appId, string mountPath)
{
    /// <summary>
    /// The <c>&lt;app&gt;</c> id of the admin UI chosen for the request, or null for the platform's own page.
    /// </summary>
    public string AppId { get; } = appId;

    /// <summary>
    /// <c>/apps/{appId}</c> when the request came through the admin UI's app path, empty at the site root.
    /// </summary>
    public string MountPath { get; } = mountPath ?? string.Empty;
}
