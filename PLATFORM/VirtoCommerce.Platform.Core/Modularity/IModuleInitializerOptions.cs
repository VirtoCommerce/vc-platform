namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IModuleInitializerOptions
    {
        string RoutPrefix { get; }
        string VirtualRoot { get; }
        string GetModuleDirectoryPath(string moduleId);
    }
}
