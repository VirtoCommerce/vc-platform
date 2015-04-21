using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IModuleManifestProvider
    {
        string RootPath { get; }
        IDictionary<string, ModuleManifest> GetModuleManifests();
    }
}
