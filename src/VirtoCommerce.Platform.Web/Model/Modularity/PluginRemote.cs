namespace VirtoCommerce.Platform.Web.Model.Modularity;

/// <summary>
/// Module Federation remote coordinates for a plugin. Mirrors the shape
/// expected by <c>@module-federation/runtime</c>.
/// </summary>
public class PluginRemote
{
    public string Name { get; set; }
    public string Exposed { get; set; }
}
