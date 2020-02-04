namespace VirtoCommerce.Platform.Core.Modularity
{
    public class LocalStorageModuleCatalogOptions
    {
        //Folder where platform will discover installed modules
        //Can use absolute or relative path
        public string DiscoveryPath { get; set; }
        public string ProbingPath { get; set; } = "App_Data/Modules";
        public string[] LocalizationFileExtensions { get; set; } = new[] { "resources.dll"};
        public string[] AssemblyFileExtensions { get; set; } = new[] { ".dll", ".exe"};
        public string[] AssemblyServiceFileExtensions { get; set; } = new[] { ".pdb", ".xml", ".deps.json", ".runtimeconfig.json", ".runtimeconfig.dev.json", ".dep" };
    }
}
