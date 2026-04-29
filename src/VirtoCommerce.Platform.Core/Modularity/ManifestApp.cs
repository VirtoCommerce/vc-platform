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

        [XmlElement("supportEmbeddedMode")]
        public bool SupportEmbeddedMode { get; set; }

        /// <summary>
        /// Folder under each installed module that the platform probes for plugin descriptors
        /// targeting this app. Default: <c>plugins</c>. Each plugin lives at
        /// <c>{moduleRoot}/{PluginsDiscoveryFolder}/{appId}/</c>.
        /// </summary>
        [XmlElement("pluginsDiscoveryFolder")]
        public string PluginsDiscoveryFolder { get; set; } = "plugins";
    }
}
