using System.Xml.Serialization;

namespace VirtoCommerce.Framework.Web.Modularity
{
    [XmlType("permission")]
    public class ModulePermission
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
