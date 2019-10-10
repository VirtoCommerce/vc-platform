using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Caching
{
    public class SecurityCacheRegion : CancellableCacheRegion<SecurityCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _usersRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeTokenForUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var cancellationTokenSource = _usersRegionTokenLookup.GetOrAdd(user.Id, new CancellationTokenSource());
            return new CompositeChangeToken(new[] { CreateChangeToken(), new CancellationChangeToken(cancellationTokenSource.Token) });
        }

        public static void ExpireUser(ApplicationUser user)
        {
            if (_usersRegionTokenLookup.TryRemove(user.Id, out var token))
            {
                token.Cancel();
            }
        }
    }
}
