using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        public string FullPhysicalPath { get; private set; }
        public ICollection<ManifestBundleItem> Styles { get; private set; }
        public ICollection<ManifestBundleItem> Scripts { get; private set; }

        public ManifestModuleInfo(ModuleManifest manifest, string fullPhysicalPath)
            : base(manifest.Id, manifest.ModuleType, manifest.Dependencies)
        {
            InitializationMode = InitializationMode.OnDemand;
            FullPhysicalPath = fullPhysicalPath;

            Styles = new List<ManifestBundleItem>();
            Scripts = new List<ManifestBundleItem>();

            if (manifest.Styles != null)
                manifest.Styles.ForEach(s => Styles.Add(s));

            if (manifest.Scripts != null)
                manifest.Scripts.ForEach(s => Scripts.Add(s));
        }
    }
}
