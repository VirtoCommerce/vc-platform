using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Caching
{
	public class CacheSettings : ValueObject<CacheSettings>
    {
        public string ProviderName { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public string Group { get; private set; }
        public bool IsEnabled { get; private set; }

        public CacheSettings(string group, TimeSpan timeout)
            : this(group, timeout, string.Empty, true)
        {
        }

        public CacheSettings(string group, TimeSpan timeout, string providerName, bool isEnabled)
        {
            Group = group;
            Timeout = timeout;
            ProviderName = providerName;
            IsEnabled = isEnabled;
        }
    }
}
