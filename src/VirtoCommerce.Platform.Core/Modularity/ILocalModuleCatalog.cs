using System;

namespace VirtoCommerce.Platform.Core.Modularity
{
    [Obsolete("Use ModuleBootstrapper for read-only module queries.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface ILocalModuleCatalog : IModuleCatalog
    {
    }
}
