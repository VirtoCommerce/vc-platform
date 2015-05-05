using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ModuleManifestProvider : IModuleManifestProvider
    {
        private IDictionary<string, ModuleManifest> _Modules;

        public string RootPath { get; private set; }
        public string ManifestFileName { get; private set; }

        public ModuleManifestProvider(string rootPath)
            : this(rootPath, "module.manifest")
        {
        }

        public ModuleManifestProvider(string rootPath, string manifestFileName)
        {
            RootPath = rootPath;
            ManifestFileName = manifestFileName;
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
