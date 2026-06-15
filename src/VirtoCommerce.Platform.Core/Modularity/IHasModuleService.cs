namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Used to provide <see cref="IModuleService"/> access for modules during initialization.
    /// In order to query loaded modules (installed/failed modules, version checks) from a module during
    /// Initialize/PostInitialize you have to implement this interface for your IModule class.
    /// </summary>
    public interface IHasModuleService
    {
        IModuleService ModuleService { get; set; }
    }
}
