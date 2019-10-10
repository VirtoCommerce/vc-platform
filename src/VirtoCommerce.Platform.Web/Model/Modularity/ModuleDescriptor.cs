using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Modularity
{
    public class ModuleDescriptor : Entity
    {
        public ModuleDescriptor(ManifestModuleInfo moduleInfo)
            : this()
        {
            Id = moduleInfo.Id;
            Title = moduleInfo.Title;
            Description = moduleInfo.Description;
            Authors = moduleInfo.Authors;
            Owners = moduleInfo.Owners;
            LicenseUrl = moduleInfo.LicenseUrl;
            ProjectUrl = moduleInfo.ProjectUrl;
            IconUrl = moduleInfo.IconUrl;
            Version = moduleInfo.Version.ToString();
            RequireLicenseAcceptance = moduleInfo.RequireLicenseAcceptance;
            ReleaseNotes = moduleInfo.ReleaseNotes;
            Copyright = moduleInfo.Copyright;
            IsRemovable = moduleInfo.IsRemovable;
            IsInstalled = moduleInfo.IsInstalled;
            PlatformVersion = moduleInfo.PlatformVersion.ToString();
            Groups = moduleInfo.Groups;
            if (moduleInfo.Dependencies != null)
            {
                Dependencies = moduleInfo.Dependencies.Select(x => new ModuleIdentity(x.Id, x.Version)).ToList();
            }
            ValidationErrors = moduleInfo.Errors;
        }

        public ModuleDescriptor()
        {
            ValidationErrors = new List<string>();
        }

        [JsonIgnore]
        public ModuleIdentity Identity
        {
            get
            {
                return new ModuleIdentity(Id, SemanticVersion.Parse(Version));
            }
        }
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
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<ModuleIdentity> Dependencies { get; set; }

        public ICollection<string> ValidationErrors { get; set; }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public ModuleIdentity InstalledVersion { get; set; }

    }
}
