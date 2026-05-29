using System;

namespace VirtoCommerce.Platform.Core.Common
{

    [Obsolete("Hangfire compatibility shim for legacy queue items. Use CancellationToken.",
        DiagnosticId = "VC0014",
        UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface ICancellationToken
    {
        void ThrowIfCancellationRequested();
    }
}
