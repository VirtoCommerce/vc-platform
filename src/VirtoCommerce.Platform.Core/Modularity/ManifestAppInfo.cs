namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestAppInfo
    {
        public ManifestAppInfo()
        {
        }

        public ManifestAppInfo(ManifestApp item)
        {
            Id = item.Id;
            Title = item.Title;
            Description = item.Description;
            IconUrl = item.IconUrl;
            RelativeUrl = $"/apps/{item.Id}";
            Permission = item.Permission;
            ContentPath = item.ContentPath;
            // Placement is the canonical value. When the manifest explicitly
            // sets <placement>, honour it. Otherwise derive from the legacy
            // <supportEmbeddedMode> bool so unmodified third-party manifests
            // keep behaving identically.
            Placement = item.Placement
                        ?? (item.SupportEmbeddedMode ? AppPlacement.MainMenu : AppPlacement.AppMenu);
            PluginsDiscoveryFolder = item.PluginsDiscoveryFolder;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }

        /// <summary>
        /// Path to App content in the module. By default, Content/{moduleApp.Id}
        /// </summary>
        public string ContentPath { get; set; }

        /// <summary>
        /// Where the app surfaces in the admin navigation. Always populated;
        /// falls back to a derivation from the legacy
        /// <c>&lt;supportEmbeddedMode&gt;</c> manifest element when the new
        /// <c>&lt;placement&gt;</c> element is absent.
        /// </summary>
        public AppPlacement Placement { get; set; }

        /// <summary>
        /// Folder under each installed module that the platform probes for plugin descriptors
        /// targeting this app. Defaults to <c>plugins</c> when null/empty.
        /// </summary>
        public string PluginsDiscoveryFolder { get; set; }
    }
}
