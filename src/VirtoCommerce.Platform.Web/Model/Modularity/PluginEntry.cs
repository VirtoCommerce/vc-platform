using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model.Modularity;

/// <summary>
/// One plugin contribution to a host app, returned by
/// <c>GET /api/apps/{appId}/manifest</c>.
/// </summary>
public class PluginEntry
{
    /// <summary>
    /// Unique plugin id within the (host app, request) tuple.
    /// Defaults to the owning .NET module id (e.g. <c>VirtoCommerce.MarketplaceReviews</c>).
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Plugin version. Defaults to the parent module version.
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// The plugin's primary file. For Module Federation plugins this is the
    /// federation entry (<c>remoteEntry.js</c>). For the legacy AngularJS host
    /// it is the module bundle (<c>dist/app.js</c>). Always a script.
    /// </summary>
    public ContentFile Entry { get; set; }

    /// <summary>
    /// Additional assets the host should preload alongside <see cref="Entry"/>
    /// (typically stylesheets). Each carries its own type so a generic loader
    /// can dispatch on it.
    /// </summary>
    public IList<ContentFile> ContentFiles { get; set; } = new List<ContentFile>();

    /// <summary>
    /// Module Federation coordinates. <c>null</c> for the legacy AngularJS host.
    /// </summary>
    public PluginRemote Remote { get; set; }
}
