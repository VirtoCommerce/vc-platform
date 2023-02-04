using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AsyncKeyedLock;

namespace VirtoCommerce.Platform.Core.Common
{
    public sealed class AsyncLock
    {
        private readonly string _key;

        public AsyncLock(string key)
        {
            _key = key;
        }

        private static readonly AsyncKeyedLocker<string> _asyncKeyedLocker = new(o =>
        {
            o.PoolSize = 20;
            o.PoolInitialFill = 1;
        });

        public static AsyncLock GetLockByKey(string key)
        {
            return new AsyncLock(key);
        }

        // TODO: Rename to LockAsync after resolving problem with backward compatibility
        // in the modules (look on this ticket https://virtocommerce.atlassian.net/browse/PT-3548)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask<IDisposable> GetReleaserAsync()
        {
            return _asyncKeyedLocker.LockAsync(_key);
        }

        [Obsolete("Left for backward compatibility. Use GetReleaserAsync")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask<IDisposable> LockAsync()
        {
            return _asyncKeyedLocker.LockAsync(_key);
        }
    }
}
