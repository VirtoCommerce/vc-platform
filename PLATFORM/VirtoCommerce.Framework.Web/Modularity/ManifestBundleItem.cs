using System.Xml.Serialization;

namespace VirtoCommerce.Framework.Web.Modularity
{
	public class ManifestBundleItem
	{
		[XmlAttribute("virtualPath")]
		public string VirtualPath { get; set; }
	}
}
