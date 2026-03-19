using System.Collections.Generic;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    public class ManagedAssemblyLoadContext
    {
        public string PlatformPath { get; set; }
        public string BasePath { get; set; }
        public ICollection<string> AdditionalProbingPaths { get; set; }
    }
}
