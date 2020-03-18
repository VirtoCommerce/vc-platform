using System.Collections.Generic;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    public class ManagedAssemblyLoadContext
    {
        public string BasePath { get; set; }
        public IEnumerable<string> AdditionalProdingPath { get; set; }
    }
}
