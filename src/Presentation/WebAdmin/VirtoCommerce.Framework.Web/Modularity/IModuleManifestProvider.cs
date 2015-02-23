using System.Collections.Generic;

namespace VirtoCommerce.Framework.Web.Modularity
{
    public interface IModuleManifestProvider
    {
        string RootPath { get; }
        IDictionary<string, ModuleManifest> GetModuleManifests();
    }
}
