using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Modularity
{
    public class ModuleDescriptor
    {
        public ModuleDescriptor()
        {
            ValidationErrors = new List<string>();
        }
        [JsonIgnore]
        public Core.Modularity.ModuleIdentity Identity
        {
            get
            {
                return new Core.Modularity.ModuleIdentity(Id, Version);
            }
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
        public IEnumerable<string> Groups { get; set; }
        public IEnumerable<ModuleIdentity> Dependencies { get; set; }

        public ICollection<string> ValidationErrors { get; set; }
        public bool IsRemovable { get; set; }
        public bool IsInstalled { get; set; }
        public ModuleIdentity InstalledVersion { get; set; }

    }
}