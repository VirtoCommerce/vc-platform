using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Modularity;

namespace PlatformTools
{
    class PackageManifest
    {
        public string PlatformVersion { get; set; }
        public string PlatformAssetUrl { get; set; }
        public List<string> ModuleSources { get; set; }
        public List<ModuleItem> Modules { get; set; }

    }
}
