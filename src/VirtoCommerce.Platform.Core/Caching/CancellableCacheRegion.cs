using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Core.Caching
{
    public class CancellableCacheRegion
    {
        protected CancellableCacheRegion()
        {
        }
        //need to preserve the name for backward compatibility
        protected static readonly string _globalRegionName = "GlobalCacheRegion_";
        private static CancellationTokenSource _globalTokenSource = new CancellationTokenSource();
        protected static CancellationTokenSource GlobalTokenSource
        {
            get
            {
                return _globalTokenSource;
            }
        }

        //events are intentionally not used to restrict usage by multiple subscribers
        public static Action<TokenCancelledEventArgs> OnTokenCancelled { get; set; }

        protected static event EventHandler<TokenCancelledEventArgs> TokenCancelled;

        protected static void CancelAll()
        {
            var oldTokenSource = Interlocked.Exchange(ref _globalTokenSource, new CancellationTokenSource());

            if (oldTokenSource != null && !oldTokenSource.IsCancellationRequested && oldTokenSource.Token.CanBeCanceled)
            {
                oldTokenSource.Cancel();
                oldTokenSource.Dispose();
            }
        }

        public static void CancelForKey(string tokenKey, bool propagate = false)
        {
            var tokenCancelled = TokenCancelled;
            if (tokenCancelled != null)
            {
                tokenCancelled(null, new TokenCancelledEventArgs(tokenKey, propagate));
            }
        }
    }

    /// <summary>
    /// Represents strongly typed cache region contains cancellation token for a concrete cache region type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CancellableCacheRegion<T> : CancellableCacheRegion
    {
        private static readonly string _regionName = $"{typeof(T).Name}_{string.Join("_", typeof(T).GetGenericArguments().Select(x => x.Name))}";
        private static readonly string _regionNamePrefix = $"{typeof(T).Name}_{string.Join("_", typeof(T).GetGenericArguments().Select(x => x.Name))}:";
#pragma warning disable S2743 // Static fields should not be used in generic types
        // False-positive SLint warning disabled.
        // These fields really need for every class applied
        private static CancellationTokenSource _regionTokenSource = new CancellationTokenSource();
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _keyTokensDict = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static IDisposable _globalTokenDisposable;
        private static readonly object _lock = new object();
#pragma warning restore S2743 // Static fields should not be used in generic types
        static CancellableCacheRegion()
        {
            TokenCancelled += (e, args) =>
            {
                if (string.IsNullOrEmpty(args.TokenKey))
                {
                    return;
                }
                if (args.TokenKey == _regionName)
                {
                    ExpireRegion(args.Propagate);
                }
                else if (args.TokenKey.StartsWith(_regionNamePrefix))
                {
                    InnerExpireTokenForKey(args.TokenKey, args.Propagate);
                }
            };
        }
        protected CancellableCacheRegion()
        {
        }

        public static IChangeToken CreateChangeTokenForKey(string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var tokenKey = GenerateRegionTokenKey(key);
            var tokenSource = _keyTokensDict.GetOrAdd(tokenKey, _ => new CancellationTokenSource());

            EnsureGlobalTokenCallbackReistered();

            var changeToken = new LazyCancellationChangeToken(tokenSource.Token);
            var regionChangeToken = new LazyCancellationChangeToken(_regionTokenSource.Token);
            var globalChangeToken = new LazyCancellationChangeToken(GlobalTokenSource.Token);

            var compositionToken = new CompositeChangeToken(new[] { changeToken, regionChangeToken, globalChangeToken });
            return compositionToken;
        }

        public static IChangeToken CreateChangeToken()
        {
            EnsureGlobalTokenCallbackReistered();
            //workaround for backward compatibility
            if (_regionName == _globalRegionName)
            {
                return new LazyCancellationChangeToken(GlobalTokenSource.Token);
            }
            else
            {
                var regionChangeToken = new LazyCancellationChangeToken(_regionTokenSource.Token);
                var globalChangeToken = new LazyCancellationChangeToken(GlobalTokenSource.Token);

                var compositionToken = new CompositeChangeToken(new[] { regionChangeToken, globalChangeToken });
                return compositionToken;
            }
        }

        //This method left for backward compatibility
        public static void ExpireTokenForKey(string key)
        {
            ExpireTokenForKey(key, propagate: true);
        }

        public static void ExpireTokenForKey(string key, bool propagate)
        {
            if (!(key is null))
            {
                var tokenKey = GenerateRegionTokenKey(key);

                InnerExpireTokenForKey(tokenKey, propagate);
            }
        }

        //This method left for backward compatibility
        public static void ExpireRegion()
        {
            ExpireRegion(propagate: true);
        }

        public static void ExpireRegion(bool propagate)
        {
            var oldTokenSource = Interlocked.Exchange(ref _regionTokenSource, new CancellationTokenSource());
            if (oldTokenSource != null && !oldTokenSource.IsCancellationRequested && oldTokenSource.Token.CanBeCanceled)
            {
                oldTokenSource.Cancel();
                oldTokenSource.Dispose();
            }

            if (propagate)
            {
                OnTokenCancelled?.Invoke(new TokenCancelledEventArgs(_regionName));
            }

            foreach (var keyTokenSourceKey in _keyTokensDict.Keys.ToArray())
            {
                InnerExpireTokenForKey(keyTokenSourceKey, propagate: false);
            }
            //Backward compatibility with exists GlobalCacheRegion
            if (_regionName == _globalRegionName)
            {
                CancelAll();
            }
        }

        private static void EnsureGlobalTokenCallbackReistered()
        {
            if (_globalTokenDisposable == null)
            {
                lock (_lock)
                {
                    if (_globalTokenDisposable == null)
                    {
                        _globalTokenDisposable = GlobalTokenSource.Token.UnsafeRegister(state =>
                        {
                            ExpireRegion(propagate: false);
                            if (_globalTokenDisposable != null)
                            {
                                _globalTokenDisposable.Dispose();
                                _globalTokenDisposable = null;
                            }
                        }, null);
                    }
                }
            }
        }

        private static void InnerExpireTokenForKey(string tokenKey, bool propagate)
        {
            var result = _keyTokensDict.TryRemove(tokenKey, out var tokenSource);
            if (result && !tokenSource.IsCancellationRequested && tokenSource.Token.CanBeCanceled)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            if (propagate)
            {
                //Notify even if no token found for the key.
                //It is important for cached data consistency when scale-out configuration is used, because the other instances may contain cached entries with token has this key
                OnTokenCancelled?.Invoke(new TokenCancelledEventArgs(tokenKey));
            }
        }

        public static string GenerateRegionTokenKey(string key = null)
        {
            if (!(key is null))
            {
                return $"{_regionNamePrefix}{key}";
            }
            return $"{_regionName}";
        }
    }
}
