using System;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Core.Caching
{
    /// <summary>
    /// A <see cref="IChangeToken"/> implementation using <see cref="CancellationChangeToken"/>.
    /// This class represents the copy of code <see cref="CancellationChangeToken"/> with only one intentionally overridden
    ///  ActiveChangeCallbacks = false.  This will forbid of automatically calling RegisterChangeCallback in the places such as <see cref="CompositeChangeToken"/>
    ///  and prevent of possible memory leaks when using shared CancellationToken  see: https://gist.github.com/tatarincev/4c942a7603a061d41deb393e0aa66545 
    /// </summary>
    public class LazyCancellationChangeToken : IChangeToken
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CancellationChangeToken"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        public LazyCancellationChangeToken(CancellationToken cancellationToken)
        {
            Token = cancellationToken;
        }

        /// False default value  will disable of calling RegisterChangeCallback in the other places such as <see cref="CompositeChangeToken"/>
        public bool ActiveChangeCallbacks { get; private set; } = false;

        /// <inheritdoc />
        public bool HasChanged => Token.IsCancellationRequested;

        private CancellationToken Token { get; }

        /// <inheritdoc />
        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {

            try
            {
                return Token.UnsafeRegister(callback, state);
            }
            catch (ObjectDisposedException)
            {
                return NullDisposable.Instance;
            }
        }

        private sealed class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            public void Dispose()
            {
                // Method intentionally left empty.
            }
        }
    }
}
