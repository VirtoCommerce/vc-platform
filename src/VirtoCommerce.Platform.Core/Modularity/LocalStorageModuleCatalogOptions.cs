namespace VirtoCommerce.Platform.Core.Modularity
{
    public class LocalStorageModuleCatalogOptions
    {
        /// <summary>
        /// A folder where the platform will discover installed modules
        /// Can use absolute or relative path
        /// </summary>
        public string DiscoveryPath { get; set; }
        /// <summary>
        /// A path where the platform stores installed modules. The platform loads modules assemblies directly from this folder.
        /// </summary>
        public string ProbingPath { get; set; } = "app_data/modules";
        /// <summary>
        /// Should the platform check modules assemblies existence/versions in the probing folder in comparison with found at DiscoveryPath and copy/replace them if need.
        /// Set to true by default (refresh probing folder at each startup).
        /// It can be useful to avoid refreshing in scenarios with slow storages to speed up the platform startup.
        /// In this case, set to false and deploy probing folder in some external way,
        /// the probing folder should exist and contain all the modules files consistent BEFORE the platform starts.
        /// If the probing folder is absent at platform startup, it will be forcibly refreshed regardless of RefreshProbingFolderOnStart value. 
        /// </summary>
        public bool RefreshProbingFolderOnStart { get; set; } = true;
        public string[] LocalizationFileExtensions { get; set; } = new[] { "resources.dll" };
        public string[] AssemblyFileExtensions { get; set; } = new[] { ".dll", ".exe" };
        public string[] AssemblyServiceFileExtensions { get; set; } = new[] { ".pdb", ".xml", ".deps.json", ".runtimeconfig.json", ".runtimeconfig.dev.json", ".dep" };

        /// <summary>
        /// Option for managing distributed lock access to apply migrations sequentially in multiinstance platform installations. 
        /// Total time in seconds to wait until the lock is available.
        /// </summary>
        public int DistributedLockWait { get; set; } = 180;
    }
}
