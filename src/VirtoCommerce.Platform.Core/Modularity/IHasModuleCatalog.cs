using System;

namespace VirtoCommerce.Platform.Core.Modularity
{
    [Obsolete("Use ModuleRegistry.IsInstalled(moduleId) instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IHasModuleCatalog
    {
        IModuleCatalog ModuleCatalog { get; set; }
    }
}
