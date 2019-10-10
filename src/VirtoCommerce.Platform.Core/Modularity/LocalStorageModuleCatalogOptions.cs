namespace VirtoCommerce.Platform.Core.Modularity
{
    public class LocalStorageModuleCatalogOptions
    {
        public string DiscoveryPath { get; set; }
        public string ProbingPath { get; set; }
        public string[] AssemblyFileExtensions { get; set; } = new[] { ".dll", ".exe", };
        public string[] AssemblyServiceFileExtensions { get; set; } = new[] { ".pdb", ".xml", ".deps.json", ".runtimeconfig.json", ".runtimeconfig.dev.json", ".dep" };
    }
}
