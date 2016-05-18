using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        private readonly ModuleManifest _manifest;
        public ManifestModuleInfo(ModuleManifest manifest)
            : base(manifest.Id, manifest.ModuleType, (manifest.Dependencies ?? new ManifestDependency[0]).Select(d => d.Id).ToArray())
        {
            _manifest = manifest;
            Styles = new List<ManifestBundleItem>();
            if (manifest.Styles != null)
            {
                Styles.AddRange(manifest.Styles);
            }
            Scripts = new List<ManifestBundleItem>();
            if (manifest.Scripts != null)
            {
                Scripts.AddRange(manifest.Scripts);
            }
            Permissions = new List<ModulePermissionGroup>();
            if(manifest.Permissions != null)
            {
                Permissions.AddRange(manifest.Permissions);
            }
            Settings = new List<ModuleSettingsGroup>();
            if (manifest.Settings != null)
            {
                Settings.AddRange(manifest.Settings);
            }
            InitializationMode = InitializationMode.OnDemand;         
        }
        public string Id { get { return _manifest.Id; } }
        public string Version { get { return _manifest.Version; } }
        public string PlatformVersion { get { return _manifest.PlatformVersion; } }
        public string Title { get { return _manifest.Title; } }
        public string Description { get { return _manifest.Description; } }
        public IEnumerable<string> Authors { get { return _manifest.Authors; } }
        public IEnumerable<string> Owners { get { return _manifest.Owners; } }
        public string LicenseUrl { get { return _manifest.LicenseUrl; } }
        public string ProjectUrl { get { return _manifest.ProjectUrl; } }
        public string IconUrl { get { return _manifest.IconUrl; } }

        public bool RequireLicenseAcceptance { get { return _manifest.RequireLicenseAcceptance; } }
        public string ReleaseNotes { get { return _manifest.ReleaseNotes; } }
        public string Copyright { get { return _manifest.Copyright; } }
        public string Tags { get { return _manifest.Tags; } }
        public IEnumerable<ManifestDependency> Dependencies { get { return _manifest.Dependencies; } }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public string FullPhysicalPath { get; set; }
        public ICollection<ManifestBundleItem> Styles { get; private set; }
        public ICollection<ManifestBundleItem> Scripts { get; private set; }
        public ICollection<ModulePermissionGroup> Permissions { get; private set; }
        public ICollection<ModuleSettingsGroup> Settings { get; private set; }

    }
}
