using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity;

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

    /// <summary>
    /// The <c>contributions</c> object of the plugin's <c>plugin.json</c>, as compact JSON text,
    /// or <c>null</c> when it declares none. Its shape belongs to the host app: the platform does
    /// not interpret it, only serves it, so a host learns what a plugin contributes without
    /// fetching anything of the plugin first.
    /// </summary>
    public string Contributions { get; set; }
}
