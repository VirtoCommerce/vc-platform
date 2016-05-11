using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        public string FullPhysicalPath { get; private set; }
        public ICollection<ManifestBundleItem> Styles { get; private set; }
        public ICollection<ManifestBundleItem> Scripts { get; private set; }

        public ManifestModuleInfo(ModuleManifest manifest, string fullPhysicalPath)
            : base(manifest.Id, manifest.ModuleType, (manifest.Dependencies ?? new ManifestDependency[0]).Select(d => d.Id).ToArray())
        {
            InitializationMode = InitializationMode.OnDemand;
            FullPhysicalPath = fullPhysicalPath;

            Styles = new List<ManifestBundleItem>();
            Scripts = new List<ManifestBundleItem>();

            if (manifest.Styles != null)
                Styles.AddRange(manifest.Styles);

            if (manifest.Scripts != null)
                Scripts.AddRange(manifest.Scripts);
        }
    }
}
