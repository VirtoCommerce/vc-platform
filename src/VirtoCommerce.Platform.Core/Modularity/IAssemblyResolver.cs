using System.Reflection;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Interface for classes that are responsible for resolving and loading assembly files. 
    /// </summary>
    public interface IAssemblyResolver
    {
        /// <summary>
        /// Load an assembly when it's required by the application. 
        /// </summary>
        Assembly LoadAssemblyFrom(string assemblyPath);
    }
}
