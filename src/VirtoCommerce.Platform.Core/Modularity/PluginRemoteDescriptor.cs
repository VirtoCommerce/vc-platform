namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Module Federation remote coordinates for a plugin. Mirrors the web DTO
/// <c>PluginRemote</c> shape expected by <c>@module-federation/runtime</c>.
/// </summary>
public class PluginRemoteDescriptor
{
    public string Name { get; set; }
    public string Exposed { get; set; }
}
