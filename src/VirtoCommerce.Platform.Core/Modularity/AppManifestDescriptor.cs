using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity;

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
