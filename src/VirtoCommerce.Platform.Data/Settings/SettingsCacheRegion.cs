using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsCacheRegion : CancellableCacheRegion<SettingsCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _settingsRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(ObjectSettingEntry settingEntry)
        {
            if (settingEntry == null)
            {
                throw new ArgumentNullException(nameof(settingEntry));
            }
            var cancellationTokenSource = _settingsRegionTokenLookup.GetOrAdd(settingEntry.GetCacheKey(), new CancellationTokenSource());
            return new CompositeChangeToken(new[] { CreateChangeToken(), new CancellationChangeToken(cancellationTokenSource.Token) });
        }

        public static void ExpireSetting(ObjectSettingEntry settingEntry)
        {
            if (settingEntry != null)
            {
                if (_settingsRegionTokenLookup.TryRemove(settingEntry.GetCacheKey(), out var token))
                {
                    token.Cancel();
                }
            }
        }
    }
}
