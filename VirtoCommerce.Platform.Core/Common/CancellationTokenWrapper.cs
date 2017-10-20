using System.Threading;

namespace VirtoCommerce.Platform.Core.Common
{
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
