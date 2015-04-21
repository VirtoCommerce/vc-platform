namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IModuleInitializerOptions
    {
        SampleDataLevel SampleDataLevel { get; }
        string GetModuleDirectoryPath(string moduleId);
    }
}
