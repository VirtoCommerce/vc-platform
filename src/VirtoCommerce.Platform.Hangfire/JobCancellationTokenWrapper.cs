using System;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Hangfire
{
    [Obsolete("Hangfire compatibility shim for legacy queue items. Use CancellationToken.",
    DiagnosticId = "VC0014",
    UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class JobCancellationTokenWrapper : ICancellationToken
    {
        public IJobCancellationToken JobCancellationToken { get; }

        public JobCancellationTokenWrapper(IJobCancellationToken jobCancellationToken)
        {
            JobCancellationToken = jobCancellationToken;
        }

        #region Implementation of ICancellationToken

        public void ThrowIfCancellationRequested()
        {
            JobCancellationToken.ThrowIfCancellationRequested();
        }

        #endregion
    }
}
