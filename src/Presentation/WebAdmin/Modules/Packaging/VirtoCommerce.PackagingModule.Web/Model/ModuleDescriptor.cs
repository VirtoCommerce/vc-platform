using System;
using System.Collections.Generic;

namespace VirtoCommerce.PackagingModule.Web.Model
{
    public class ModuleDescriptor
    {
        public ModuleDescriptor()
        {
            ValidationErrors = new List<string>();
        }
        public string Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public Uri LicenseUrl { get; set; }
        public Uri ProjectUrl { get; set; }
        public Uri IconUrl { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string ReleaseNotes { get; set; }
        public string Copyright { get; set; }
        public string Tags { get; set; }
        public IEnumerable<string> Dependencies { get; set; }

        public ICollection<string> ValidationErrors { get; set; }
        public bool IsRemovable { get; set; }

        public string FileName { get; set; }
    }
}
