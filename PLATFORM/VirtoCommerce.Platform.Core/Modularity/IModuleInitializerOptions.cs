namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IModuleInitializerOptions
    {
       string GetModuleDirectoryPath(string moduleId);
    }
}
