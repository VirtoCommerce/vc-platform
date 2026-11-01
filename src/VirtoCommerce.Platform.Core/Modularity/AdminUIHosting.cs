namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// How the platform serves an admin UI declared with <c>&lt;adminUI&gt;</c> on an <c>&lt;app&gt;</c> in <c>module.manifest</c>.
/// </summary>
public enum AdminUIHosting
{
    /// <summary>
    /// A static single-page app in the app's content folder, served at <c>/apps/{appId}/</c>.
    /// As the root admin UI, the platform answers <c>/</c> with the app's index page; the address stays <c>/</c>.
    /// The app's index page must load its files by absolute URLs (<c>/apps/{appId}/...</c>).
    /// </summary>
    App = 0,

    /// <summary>
    /// The module maps the UI page at <c>/</c> itself, marked with <see cref="AdminUIPageAttribute"/>.
    /// The platform also mounts the UI at <c>/apps/{appId}/</c> as a path base, so the page and every
    /// URL relative to it (API calls, bundles, SignalR) work there too.
    /// </summary>
    Endpoint,
}
