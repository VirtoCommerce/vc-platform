using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public class ModuleManifestProvider : IModuleManifestProvider
    {
        public string RootPath { get; private set; }
        public string ManifestFileName { get; set; }

        public ModuleManifestProvider(string rootPath)
        {
            RootPath = rootPath;
            ManifestFileName = "module.manifest";
        }

        public IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            // TODO: Add caching
            var result = new Dictionary<string, ModuleManifest>();

            if (Directory.Exists(RootPath))
                result = Directory.EnumerateFiles(RootPath, ManifestFileName, SearchOption.AllDirectories).ToDictionary(path => path, ManifestReader.Read);

            return result;
        }
    }
}
