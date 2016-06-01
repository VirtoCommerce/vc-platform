using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        public ManifestModuleInfo(ModuleManifest manifest)
            : base(manifest.Id, manifest.ModuleType, (manifest.Dependencies ?? new ManifestDependency[0]).Select(d => d.Id).ToArray())
        {
            Id = manifest.Id;
            Version = new SemanticVersion(new System.Version(manifest.Version));
            PlatformVersion = new SemanticVersion(new System.Version(manifest.PlatformVersion));
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
            UseFullTypeNameInSwagger = manifest.UseFullTypeNameInSwagger;

            Ref = manifest.PackageUrl;
            Identity = new ModuleIdentity(Id, Version);

            Errors = new List<string>();


            Dependencies = new List<ModuleIdentity>();
            if (manifest.Dependencies != null)
            {
                Dependencies.AddRange(manifest.Dependencies.Select(x => new ModuleIdentity(x.Id, x.Version)));
            }
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
            if (manifest.Permissions != null)
            {
                Permissions.AddRange(manifest.Permissions);
            }
            Settings = new List<ModuleSettingsGroup>();
            if (manifest.Settings != null)
            {
                Settings.AddRange(manifest.Settings);
            }
            Groups = new List<string>();
            if (manifest.Groups != null)
            {
                Groups.AddRange(manifest.Groups);
            }
            InitializationMode = InitializationMode.OnDemand;
        }

        public ModuleIdentity Identity { get; private set; }
        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }
        public SemanticVersion PlatformVersion { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public IEnumerable<string> Authors { get; private set; }
        public IEnumerable<string> Owners { get; private set; }
        public string LicenseUrl { get; private set; }
        public string ProjectUrl { get; private set; }
        public string IconUrl { get; private set; }

        public bool RequireLicenseAcceptance { get; private set; }
        public string ReleaseNotes { get; private set; }
        public string Copyright { get; private set; }
        public string Tags { get; private set; }
        public ICollection<ModuleIdentity> Dependencies { get; private set; }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public ICollection<string> Groups { get; private set; }
        public string FullPhysicalPath { get; set; }
        public ICollection<ManifestBundleItem> Styles { get; private set; }
        public ICollection<ManifestBundleItem> Scripts { get; private set; }
        public ICollection<ModulePermissionGroup> Permissions { get; private set; }
        public ICollection<ModuleSettingsGroup> Settings { get; private set; }
        public ICollection<string> Errors { get; set; }
        public bool UseFullTypeNameInSwagger { get; set; }

        public override string ToString()
        {
            return Identity.ToString();
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to ModuleIdentity return false.
            ManifestModuleInfo other = obj as ManifestModuleInfo;
            if (other == null)
            {
                return false;
            }
            // Return true if the fields match:
            return (Id == other.Id) && (Version == other.Version);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
