using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Core.Caching
{
    //Store all cancellation tokens  that are associated for cached entries
    public static class CacheCancellableTokensRegistry
    {
        private static ConcurrentDictionary<string, CancellationTokenSource> _tokensDict = new ConcurrentDictionary<string, CancellationTokenSource>();
        //events are intentionally not used to restrict usage by multiple subscribers
        public static Action<TokenCancelledEventArgs> OnTokenCancelled { get; set; }

        public static IChangeToken CreateChangeToken(string tokenKey)
        {
            var tokenSource = _tokensDict.GetOrAdd(tokenKey, _ => new CancellationTokenSource());
            return new CancellationChangeToken(tokenSource.Token);
        }

        public static bool TryCancelToken(string tokenKey, bool raiseEvent = true)
        {
            var result = _tokensDict.TryRemove(tokenKey, out var token);
            if (result)
            {
                token.Cancel();
                token.Dispose();
            }
            if (raiseEvent)
            {
                //Notify even if no token found for the key.
                //It is important for cached data consistency when scale-out configuration is used, because the other instances may contain cached entries with token has this key
                TriggerOnCancel(tokenKey);
            }
            return result;
        }

        private static void TriggerOnCancel(string tokenKey)
        {
            OnTokenCancelled?.Invoke(new TokenCancelledEventArgs(tokenKey));
        }
    }

    /// <summary>
    /// Represents strongly typed cache region contains cancellation token for a concrete cache region type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CancellableCacheRegion<T>
    {   
        private static readonly string _regionName = typeof(T).Name;

        protected CancellableCacheRegion()
        {
        }

        public static IChangeToken CreateChangeTokenForKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return new CompositeChangeToken(new[] { CreateChangeToken(), CacheCancellableTokensRegistry.CreateChangeToken(GenerateRegionTokenKey(key)) });
        }

        public static IChangeToken CreateChangeToken()
        {
            return CacheCancellableTokensRegistry.CreateChangeToken(GenerateRegionTokenKey());
        }

        public static void ExpireTokenForKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            CacheCancellableTokensRegistry.TryCancelToken(GenerateRegionTokenKey(key));
        }

        public static void ExpireRegion()
        {
            CacheCancellableTokensRegistry.TryCancelToken(GenerateRegionTokenKey());
        }

        private static string GenerateRegionTokenKey(string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return $"{_regionName}:{key}";
            }
            return $"{_regionName}";
        }

    }
}
