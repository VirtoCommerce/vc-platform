using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Core.Caching
{
    /// <summary>
    /// A <see cref="IChangeToken"/> implementation using <see cref="CancellationToken"/>.
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

        /// <inheritdoc />
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

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();

            public void Dispose()
            {
            }
        }
    }
}
