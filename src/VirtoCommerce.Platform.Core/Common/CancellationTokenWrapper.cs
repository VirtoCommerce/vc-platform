using System;
using System.Threading;

namespace VirtoCommerce.Platform.Core.Common
{

    [Obsolete("Hangfire compatibility shim for legacy queue items. Use CancellationToken.",
        DiagnosticId = "VC0014",
        UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class CancellationTokenWrapper : ICancellationToken
    {
        public CancellationToken CancellationToken { get; }

        public CancellationTokenWrapper(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        public virtual void ThrowIfCancellationRequested()
        {
            CancellationToken.ThrowIfCancellationRequested();
        }
    }
}
