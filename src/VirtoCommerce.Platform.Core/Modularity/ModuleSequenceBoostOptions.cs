using System;

namespace VirtoCommerce.Platform.Core.Modularity;

[Obsolete("Use LocalStorageModuleCatalogOptions class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class ModuleSequenceBoostOptions
{
    public string[] ModuleSequenceBoost { get; set; } = [];
}
