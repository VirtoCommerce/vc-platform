using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestBundleDirectory : ManifestBundleItem
    {
        [XmlAttribute("searchPattern")]
        public string SearchPattern { get; set; }

        [XmlAttribute("searchSubdirectories")]
        public bool SearchSubdirectories { get; set; }
    }
}
