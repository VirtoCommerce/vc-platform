using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public class ModuleManifestProvider : IModuleManifestProvider
    {
        private IDictionary<string, ModuleManifest> _Modules;

        public string RootPath { get; private set; }

        public string ManifestFileName { get; set; }

        public ModuleManifestProvider(string rootPath)
        {
            RootPath = rootPath;
            ManifestFileName = "module.manifest";
        }

        public IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            if (_Modules != null)
                return _Modules;

            var result = new Dictionary<string, ModuleManifest>();

            if (Directory.Exists(RootPath))
                result = Directory.EnumerateFiles(RootPath, ManifestFileName, SearchOption.AllDirectories).ToDictionary(path => path, ManifestReader.Read);

            _Modules = result;
            return _Modules;
        }
    }
}
