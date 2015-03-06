using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public class ModuleManifestProvider : IModuleManifestProvider
    {
        private readonly CacheHelper _cacheHelper;

        public string RootPath { get; private set; }

        public string ManifestFileName { get; set; }

        public ModuleManifestProvider(string rootPath)
        {
            RootPath = rootPath;
            ManifestFileName = "module.manifest";
            //this._cacheHelper = new CacheHelper(cache);
        }

        /*
        public IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            return _cacheHelper.Get(
                CacheHelper.CreateCacheKey("Manifests", "all"),
                GetModuleManifestsInternal,
                TimeSpan.FromMinutes(10)); // TODO: add file dependecy?
        }
         * */

        public IDictionary<string, ModuleManifest> GetModuleManifests()
        {
            var result = new Dictionary<string, ModuleManifest>();

            if (Directory.Exists(RootPath))
                result = Directory.EnumerateFiles(RootPath, ManifestFileName, SearchOption.AllDirectories).ToDictionary(path => path, ManifestReader.Read);

            return result;
        }

    }
}
