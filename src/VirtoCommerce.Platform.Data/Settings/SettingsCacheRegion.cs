using System;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsCacheRegion : CancellableCacheRegion<SettingsCacheRegion>
    { 
        public static IChangeToken CreateChangeToken(ObjectSettingEntry settingEntry)
        {
            if (settingEntry == null)
            {
                throw new ArgumentNullException(nameof(settingEntry));
            }
            return CreateChangeTokenForKey(settingEntry.GetCacheKey());
        }

        public static void ExpireSetting(ObjectSettingEntry settingEntry)
        {
            ExpireTokenForKey(settingEntry.GetCacheKey());
        }
    }
}
