using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestApp
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("permission")]
        public string Permission { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("iconUrl")]
        public string IconUrl { get; set; }

        [XmlElement("contentPath")]
        public string ContentPath { get; set; }

        /// <summary>
        /// Legacy boolean. Use <see cref="Placement"/> instead. Kept for
        /// backwards compatibility: when <see cref="Placement"/> is not
        /// declared, <c>true</c> maps to <see cref="AppPlacement.MainMenu"/>
        /// and <c>false</c> maps to <see cref="AppPlacement.AppMenu"/>.
        /// </summary>
        [XmlElement("supportEmbeddedMode")]
        public bool SupportEmbeddedMode { get; set; }

        /// <summary>
        /// Where the app surfaces in the admin navigation. When omitted from
        /// <c>module.manifest</c>, the value is derived from
        /// <see cref="SupportEmbeddedMode"/> for backwards compatibility.
        /// Nullable so deserialization can distinguish "element omitted"
        /// from "element present and equal to AppMenu (default)".
        /// </summary>
        [XmlElement("placement")]
        public AppPlacement? Placement { get; set; }

        /// <summary>
        /// Folder under each installed module that the platform probes for plugin descriptors
        /// targeting this app. Default: <c>plugins</c>. Each plugin lives at
        /// <c>{moduleRoot}/{PluginsDiscoveryFolder}/{appId}/</c>.
        /// </summary>
        [XmlElement("pluginsDiscoveryFolder")]
        public string PluginsDiscoveryFolder { get; set; } = "plugins";
    }
}
