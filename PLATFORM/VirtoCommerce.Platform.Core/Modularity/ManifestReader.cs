using System.IO;
using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
	public static class ManifestReader
	{
		public static ModuleManifest Read(string filePath)
		{
			using (var stream = File.OpenRead(filePath))
			{
				return Read(stream);
			}
		}

		public static ModuleManifest Read(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(ModuleManifest));
			return serializer.Deserialize(stream) as ModuleManifest;
		}
	}
}
