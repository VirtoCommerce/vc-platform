using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestApp
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("permission")]
        public string Permission { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("iconUrl")]
        public string IconUrl { get; set; }

        [XmlElement("contentPath")]
        public string ContentPath { get; set; }
    }
}
