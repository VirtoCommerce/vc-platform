using System.Collections.Generic;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ExternalModuleManifestVersion : ValueObject
    {
        public string Version { get; set; }
        public string VersionTag { get; set; }
        [JsonIgnore]
        public SemanticVersion SemanticVersion => SemanticVersion.Parse(Version);

        public string PlatformVersion { get; set; }
        [JsonIgnore]
        public SemanticVersion PlatformSemanticVersion => SemanticVersion.Parse(PlatformVersion);

        public string PackageUrl { get; set; }

        public ManifestDependency[] Incompatibilities { get; set; }
        public ManifestDependency[] Dependencies { get; set; }

        public string ReleaseNotes { get; set; }
               

        public static ExternalModuleManifestVersion FromManifest(ModuleManifest manifest)
        {
            var result = new ExternalModuleManifestVersion
            {
                Dependencies = manifest.Dependencies,
                Incompatibilities = manifest.Incompatibilities,
                PackageUrl = manifest.PackageUrl,
                PlatformVersion = manifest.PlatformVersion,
                ReleaseNotes = manifest.ReleaseNotes,
                Version = manifest.Version,
                VersionTag = manifest.VersionTag
            };
            return result;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Version;
            yield return VersionTag;
        }
    }
}
