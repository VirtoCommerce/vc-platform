using System.Collections.Generic;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Diagnostics
{
    public class InstalledModuleInfo
    {
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
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<ModuleIdentity> Dependencies { get; set; }
        public ICollection<string> ValidationErrors { get; set; }

    }
}
