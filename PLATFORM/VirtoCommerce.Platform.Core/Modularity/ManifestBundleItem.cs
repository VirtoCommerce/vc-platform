using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
	public class ManifestBundleItem
	{
		[XmlAttribute("virtualPath")]
		public string VirtualPath { get; set; }
	}
}
