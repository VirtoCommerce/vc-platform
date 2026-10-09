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
    /// The <c>contributions</c> object of <c>plugin.json</c> as compact JSON, or <c>null</c>.
    /// Opaque to the platform; its shape is defined by the host app.
    /// </summary>
    public string Contributions { get; set; }
}
