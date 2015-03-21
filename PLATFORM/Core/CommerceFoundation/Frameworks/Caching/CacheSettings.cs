using System;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
	public class CacheSettings
	{
		public string ProviderName { get; private set; }
		public TimeSpan Timeout { get; private set; }
		public string Group { get; private set; }
		public bool IsEnabled { get; private set; }

		public CacheSettings(string group, TimeSpan timeout, string providerName, bool isEnabled)
		{
			Timeout = timeout;
			IsEnabled = isEnabled;
			Group = group;
			ProviderName = providerName;
		}
	}

}
