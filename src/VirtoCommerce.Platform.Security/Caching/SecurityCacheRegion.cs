using System;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Caching
{
    public class SecurityCacheRegion : CancellableCacheRegion<SecurityCacheRegion>
    {
        public static IChangeToken CreateChangeTokenForUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return CreateChangeTokenForKey(user.Id);
        }

        public static void ExpireUser(ApplicationUser user)
        {
            if (user != null)
            {
                ExpireTokenForKey(user.Id);
            }
        }
    }
}
