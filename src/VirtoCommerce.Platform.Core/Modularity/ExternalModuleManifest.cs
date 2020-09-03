using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    // Represents the module information with all of it historical versions and is used to download from an external source for installation and update operations.
    public class ExternalModuleManifest : ValueObject
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Authors { get; set; }
        public string[] Owners { get; set; }

        public string LicenseUrl { get; set; }

        public string ProjectUrl { get; set; }

        public string IconUrl { get; set; }
        public bool RequireLicenseAcceptance { get; set; }

        public string Copyright { get; set; }

        public string Tags { get; set; }

        public string[] Groups { get; set; }

        public IList<ExternalModuleManifestVersion> Versions { get; set; } = new List<ExternalModuleManifestVersion>();

        public static ExternalModuleManifest FromManifest(ModuleManifest manifest)
        {
            var result = new ExternalModuleManifest
            {
                Title = manifest.Title,
                Description = manifest.Description,
                Authors = manifest.Authors,
                Copyright = manifest.Copyright,
                Groups = manifest.Groups,
                IconUrl = manifest.IconUrl,
                Id = manifest.Id,
                LicenseUrl = manifest.LicenseUrl,
                Owners = manifest.Owners,
                ProjectUrl = manifest.ProjectUrl,
                RequireLicenseAcceptance = manifest.RequireLicenseAcceptance,
                Tags = manifest.Tags
            };

            result.Versions.Add(ExternalModuleManifestVersion.FromManifest(manifest));
            return result;
        }

        public void PublishNewVersion(ModuleManifest manifest)
        {
            var version = ExternalModuleManifestVersion.FromManifest(manifest);
            Versions.Add(version);
            var byPlatformMajorGroups = Versions.GroupBy(x => x.PlatformSemanticVersion.Major).OrderByDescending(x => x.Key).ToList();
            Versions.Clear();
            foreach (var byPlatformGroup in byPlatformMajorGroups)
            {
                var latestReleaseVersion = byPlatformGroup.Where(x => string.IsNullOrEmpty(x.VersionTag))
                                                          .OrderByDescending(x => x.SemanticVersion).FirstOrDefault();
                if (latestReleaseVersion != null)
                {
                    Versions.Add(latestReleaseVersion);
                }

                var latestPreReleaseVersion = byPlatformGroup.Where(x => !string.IsNullOrEmpty(x.VersionTag))
                                                             .OrderByDescending(x => x.SemanticVersion)
                                                             .ThenByDescending(x => x.VersionTag).FirstOrDefault();

                var addPreRelease = latestPreReleaseVersion != null;
                if(addPreRelease)
                {
                    addPreRelease = latestReleaseVersion == null || (!latestPreReleaseVersion.SemanticVersion.Equals(latestReleaseVersion.SemanticVersion)
                                                                     && latestReleaseVersion.SemanticVersion.IsCompatibleWithBySemVer(latestPreReleaseVersion.SemanticVersion));
                }
                if (addPreRelease)
                {
                    Versions.Add(latestPreReleaseVersion);
                }
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
