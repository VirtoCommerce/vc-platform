using System;

namespace VirtoCommerce.Platform.Core.Domain
{
    [Obsolete("Use VirtoCommerce.Platform.Core.Common.IHasLanguageCode instead.", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IHasLanguage
    {
        string LanguageCode { get; }
    }
}
