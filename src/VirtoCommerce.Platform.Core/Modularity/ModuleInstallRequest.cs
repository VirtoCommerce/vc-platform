namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Lightweight request to install or update a module.
/// Version is optional: null or empty means "latest available".
/// </summary>
public class ModuleInstallRequest
{
    public ModuleInstallRequest()
    {
    }

    public ModuleInstallRequest(string id, string version = null)
    {
        Id = id;
        Version = version;
    }

    /// <summary>
    /// ModuleId
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Version is optional: null or empty means "latest available".
    /// </summary>
    public string Version { get; set; }
}
