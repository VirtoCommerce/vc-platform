using System.Xml.Serialization;

namespace VirtoCommerce.Framework.Web.Modularity
{
	[XmlType("directory")]
	public class ManifestBundleDirectory : ManifestBundleItem
	{
		[XmlAttribute("searchPattern")]
		public string SearchPattern { get; set; }

		[XmlAttribute("searchSubdirectories")]
		public bool SearchSubdirectories { get; set; }
	}
}
