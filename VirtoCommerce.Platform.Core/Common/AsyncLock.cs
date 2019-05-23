using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public class AsyncLock
    {
        private static ConcurrentDictionary<string, AsyncLock> _lockMap = new ConcurrentDictionary<string, AsyncLock>();
        private readonly Nito.AsyncEx.AsyncLock _asyncLock;

        private AsyncLock(Nito.AsyncEx.AsyncLock asyncLock)
        {
            _asyncLock = asyncLock;
        }
        public static AsyncLock GetLockByKey(string key)
        {
            return _lockMap.GetOrAdd(key, (x) => new AsyncLock(new Nito.AsyncEx.AsyncLock()));
        }

        public async Task<Releaser> LockAsync()
        {
            return new Releaser(await _asyncLock.LockAsync());
        }

        public struct Releaser : IDisposable
        {
            private readonly IDisposable _guardDisposable;

            internal Releaser(IDisposable disposable)
            {
                _guardDisposable = disposable;
            }

            public void Dispose()
            {
                if (_guardDisposable != null)
                {
                    _guardDisposable.Dispose();
                }
                GC.SuppressFinalize(this);
            }
        }
    }

}

