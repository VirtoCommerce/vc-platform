using System.Xml.Serialization;

namespace VirtoCommerce.Framework.Web.Modularity
{
	[XmlType("module")]
	public class ModuleManifest
	{
		public ModuleManifest()
		{
			Dependencies = new string[] { };
			Styles = new ManifestBundleItem[] { };
			Scripts = new ManifestBundleItem[] { };
		}

		[XmlAttribute("moduleName")]
		public string ModuleName { get; set; }

		[XmlAttribute("assemblyFile")]
		public string AssemblyFile { get; set; }

		[XmlAttribute("moduleType")]
		public string ModuleType { get; set; }

		[XmlArray("dependencies")]
		[XmlArrayItem("dependency")]
		public string[] Dependencies { get; set; }

		[XmlArray("styles")]
		[XmlArrayItem("file", Type = typeof(ManifestBundleFile))]
		[XmlArrayItem("directory", Type = typeof(ManifestBundleDirectory))]
		public ManifestBundleItem[] Styles { get; set; }

		[XmlArray("scripts")]
		[XmlArrayItem(typeof(ManifestBundleFile))]
		[XmlArrayItem(typeof(ManifestBundleDirectory))]
		public ManifestBundleItem[] Scripts { get; set; }
	}
}
