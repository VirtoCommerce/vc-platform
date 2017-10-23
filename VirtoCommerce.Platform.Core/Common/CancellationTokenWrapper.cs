using System.Threading;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// Implements cancellation abstraction standard System.Threading.CancellationToken.
    /// </summary>
    /// <remarks>
    /// See remarks on ICancellationToken.
    /// </remarks>
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
