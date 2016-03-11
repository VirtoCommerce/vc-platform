namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IModuleInitializerOptions
    {
        string RoutePrefix { get; }
        string VirtualRoot { get; }
        string GetModuleDirectoryPath(string moduleId);
    }
}
