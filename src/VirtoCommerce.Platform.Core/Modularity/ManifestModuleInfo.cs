using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestModuleInfo : ModuleInfo
    {
        public ManifestModuleInfo()
        {
            InitializationMode = InitializationMode.OnDemand;
            DependsOn = new Collection<string>();
        }

        public ModuleIdentity Identity { get; private set; }
        public string Id { get; private set; }
        public SemanticVersion Version { get; private set; }
        public string VersionTag { get; set; }
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
        public ICollection<ModuleIdentity> Dependencies { get; } = new List<ModuleIdentity>();
        /// <summary>
        /// List of incompatible modules
        /// </summary>
        public ICollection<ModuleIdentity> Incompatibilities { get; } = new List<ModuleIdentity>();
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public ICollection<string> Groups { get; } = new List<string>();
        public string FullPhysicalPath { get; set; }
        public ICollection<string> Errors { get; } = new List<string>();
        public bool UseFullTypeNameInSwagger { get; set; }

        public virtual ManifestModuleInfo LoadFromManifest(ModuleManifest manifest)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            ModuleName = manifest.Id;         

            if (manifest.Dependencies != null)
            {
                foreach (var dependency in manifest.Dependencies)
                {
                    DependsOn.Add(dependency.Id);
                }
            }

            Id = manifest.Id;
            Version = SemanticVersion.Parse(manifest.Version);
            PlatformVersion = SemanticVersion.Parse(manifest.PlatformVersion);
            ReleaseNotes = manifest.ReleaseNotes;
            Ref = manifest.PackageUrl;
            if (manifest.Dependencies != null)
            {
                Dependencies.AddRange(manifest.Dependencies.Select(x => new ModuleIdentity(x.Id, SemanticVersion.Parse(x.Version))));
            }
            if (manifest.Incompatibilities != null)
            {
                Incompatibilities.AddRange(manifest.Incompatibilities.Select(x => new ModuleIdentity(x.Id, SemanticVersion.Parse(x.Version))));
            }

            Title = manifest.Title;
            Description = manifest.Description;
            Authors = manifest.Authors;
            Owners = manifest.Owners;
            LicenseUrl = manifest.LicenseUrl;
            ProjectUrl = manifest.ProjectUrl;
            IconUrl = manifest.IconUrl;
            RequireLicenseAcceptance = manifest.RequireLicenseAcceptance;
            Copyright = manifest.Copyright;
            Tags = manifest.Tags;
            Identity = new ModuleIdentity(Id, Version);
            if (manifest.Groups != null)
            {
                Groups.AddRange(manifest.Groups);
            }
            return this;
        }


        public virtual ManifestModuleInfo LoadFromExternalManifest(ExternalModuleManifest manifest, ExternalModuleManifestVersion version)
        {
            if(manifest == null)
            {
                throw new ArgumentNullException(nameof(manifest));
            }

            ModuleName = manifest.Id;         
            if (version.Dependencies != null)
            {
                foreach (var dependency in version.Dependencies)
                {
                    DependsOn.Add(dependency.Id);
                }
            }

            Id = manifest.Id;
            Version = version.SemanticVersion;
            VersionTag = version.VersionTag;
            PlatformVersion = version.PlatformSemanticVersion;
            ReleaseNotes = version.ReleaseNotes;
            Ref = version.PackageUrl;

            if (version.Dependencies != null)
            {
                Dependencies.AddRange(version.Dependencies.Select(x => new ModuleIdentity(x.Id, SemanticVersion.Parse(x.Version))));
            }
            if (version.Incompatibilities != null)
            {
                Incompatibilities.AddRange(version.Incompatibilities.Select(x => new ModuleIdentity(x.Id, SemanticVersion.Parse(x.Version))));
            }

            Title = manifest.Title;
            Description = manifest.Description;
            Authors = manifest.Authors;
            Owners = manifest.Owners;
            LicenseUrl = manifest.LicenseUrl;
            ProjectUrl = manifest.ProjectUrl;
            IconUrl = manifest.IconUrl;
            RequireLicenseAcceptance = manifest.RequireLicenseAcceptance;         
            Copyright = manifest.Copyright;
            Tags = manifest.Tags;
            Identity = new ModuleIdentity(Id, Version);
            if (manifest.Groups != null)
            {
                Groups.AddRange(manifest.Groups);
            }
            return this;
        }

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
            if (!(obj is ManifestModuleInfo other))
            {
                return false;
            }
            // Return true if the fields match:
            return (Id == other.Id) && (Version == other.Version) && (VersionTag == other.VersionTag);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
