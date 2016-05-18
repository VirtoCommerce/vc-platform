using System.IO;
using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
	internal static class ManifestReader
	{
		public static ModuleManifest Read(string filePath)
		{
            ModuleManifest retVal = null;
            using (var stream = File.OpenRead(filePath))
			{
                retVal = Read(stream);
                retVal.LocalPath = Path.GetDirectoryName(filePath);
			}
            return retVal;
		}

		public static ModuleManifest Read(Stream stream)
		{
			var serializer = new XmlSerializer(typeof(ModuleManifest));
			return serializer.Deserialize(stream) as ModuleManifest;
		}
	}
}
