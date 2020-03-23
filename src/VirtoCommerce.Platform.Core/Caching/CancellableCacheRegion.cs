using System.Threading;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Core.Caching
{
    /// <summary>
    /// Represents strongly typed cache region contains cancellation token for a concrete cache region type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CancellableCacheRegion<T>
    {
        private static CancellationTokenSource _regionTokenSource;
        private static CancellationChangeToken _regionChangeToken;
        private static object _lock = new object();

        protected CancellableCacheRegion()
        {
        }

        public static IChangeToken CreateChangeToken()
        {
            if (_regionChangeToken == null)
            {
                lock (_lock)
                {
                    if (_regionChangeToken == null)
                    {
                        _regionTokenSource = new CancellationTokenSource();
                        _regionChangeToken = new CancellationChangeToken(_regionTokenSource.Token);
                    }
                }
            }
            return _regionChangeToken;
        }

        public static void ExpireRegion()
        {
            lock (_lock)
            {
                if (_regionTokenSource != null)
                {
                    _regionTokenSource.Cancel();
                    _regionTokenSource.Dispose();
                    //Need to reset cached tokens because they are already changed
                    _regionTokenSource = null;
                    _regionChangeToken = null;
                }
            }
        }
    }
}
