using System.Xml.Serialization;

namespace VirtoCommerce.Framework.Web.Modularity
{
	[XmlType("dependency")]
	public class ModuleDependency
	{
		[XmlAttribute("moduleName")]
		public string ModuleName { get; set; }
	}
}
