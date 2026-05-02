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
}
