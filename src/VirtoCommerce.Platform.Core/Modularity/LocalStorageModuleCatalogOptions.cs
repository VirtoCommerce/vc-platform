namespace VirtoCommerce.Platform.Core.Modularity
{
    public class LocalStorageModuleCatalogOptions
    {
        /// <summary>
        /// The folder where the platform scans for installed module manifests and assemblies.
        /// Each module is expected to reside in its own subfolder with a <c>module.manifest</c> file.
        /// Supports absolute or relative paths. Default: "modules".
        /// </summary>
        public string DiscoveryPath { get; set; } = "modules";

        /// <summary>
        /// The folder where module assemblies are copied to and loaded from at runtime.
        /// During startup, the platform copies assemblies from <see cref="DiscoveryPath"/> to this folder
        /// (unless <see cref="RefreshProbingFolderOnStart"/> is <c>false</c>),
        /// then loads them directly from here.
        /// Supports absolute or relative paths. Default: "app_data/modules".
        /// </summary>
        public string ProbingPath { get; set; } = "app_data/modules";

        /// <summary>
        /// Controls whether the platform re-copies module assemblies from <see cref="DiscoveryPath"/>
        /// to <see cref="ProbingPath"/> on each startup. Default: <c>true</c>.
        /// <para>
        /// Set to <c>false</c> to skip copying and speed up startup on slow storage.
        /// In that case, ensure <see cref="ProbingPath"/> is pre-populated externally before the platform starts.
        /// If <see cref="ProbingPath"/> does not exist at startup, copying is forced regardless of this setting.
        /// </para>
        /// </summary>
        public bool RefreshProbingFolderOnStart { get; set; } = true;

        /// <summary>
        /// Module IDs that should be loaded before other modules at the same dependency level.
        /// </summary>
        public string[] ModuleSequenceBoost { get; set; } = [];

        public string[] LocalizationFileExtensions { get; set; } = ["resources.dll"];
        public string[] AssemblyFileExtensions { get; set; } = [".dll", ".exe"];
        public string[] AssemblyServiceFileExtensions { get; set; } = [".pdb", ".xml", ".deps.json", ".runtimeconfig.json", ".runtimeconfig.dev.json", ".dep", ".zip"];
        public string[] ReferenceAssemblyFolders { get; set; } = ["ref"];
    }
}
