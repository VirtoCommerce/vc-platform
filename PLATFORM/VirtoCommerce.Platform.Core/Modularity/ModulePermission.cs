using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    [XmlType("permission")]
    public class ModulePermission
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}
