using System;

namespace VirtoCommerce.Platform.Core.Modularity
{
    [Obsolete("Use ModuleRegistry for read-only module queries.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface ILocalModuleCatalog : IModuleCatalog
    {
    }
}
