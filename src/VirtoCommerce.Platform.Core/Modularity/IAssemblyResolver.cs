using System;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Interface for classes that are responsible for resolving and loading assembly files.
    /// </summary>
    [Obsolete("Use ModuleAssemblyLoader static class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Load an assembly when it's required by the application. 
        /// </summary>
        Assembly LoadAssemblyFrom(string assemblyPath);
    }
}
