using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        public ManifestModuleInfo()
        {
        }
        public ManifestModuleInfo(ModuleManifest manifest)
            : base(manifest.Id, manifest.ModuleType, (manifest.Dependencies ?? new ManifestDependency[0]).Select(d => d.Id).ToArray())
        {
            Id = manifest.Id;
            Version = manifest.Version;
            PlatformVersion = manifest.ProjectUrl;
            Title = manifest.Title;
            Description = manifest.Description;
            Authors = manifest.Authors;
            Owners = manifest.Owners;
            LicenseUrl = manifest.LicenseUrl;
            ProjectUrl = manifest.ProjectUrl;
            IconUrl = manifest.IconUrl;
            RequireLicenseAcceptance = manifest.RequireLicenseAcceptance;
            ReleaseNotes = manifest.ReleaseNotes;
            Copyright = manifest.Copyright;
            Tags = manifest.Tags;
            Dependencies = manifest.Dependencies;
            Ref = manifest.PackageUrl;

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
        public string Id { get; set; }
        public string Version { get; set; }
        public string PlatformVersion { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public string LicenseUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string IconUrl { get; set; }

        public bool RequireLicenseAcceptance { get; set; }
        public string ReleaseNotes { get; set; }
        public string Copyright { get; set; }
        public string Tags { get; set; }
        public IEnumerable<ManifestDependency> Dependencies { get; set; }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public string FullPhysicalPath { get; set; }
        public ICollection<ManifestBundleItem> Styles { get; set; }
        public ICollection<ManifestBundleItem> Scripts { get; set; }
        public ICollection<ModulePermissionGroup> Permissions { get; set; }
        public ICollection<ModuleSettingsGroup> Settings { get; set; }

    }
}
