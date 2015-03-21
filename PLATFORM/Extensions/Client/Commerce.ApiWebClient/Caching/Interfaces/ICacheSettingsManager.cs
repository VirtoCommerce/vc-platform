using System.Configuration;
using System.Web.Configuration;

namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface ICacheSettingsManager
    {
        /// <summary>
        /// Implementations should return the output cache provider settings.
        /// </summary>
        /// <returns>A <see cref="ProviderSettings"/> instance.</returns>
        ProviderSettings RetrieveOutputCacheProviderSettings();

        /// <summary>
        /// Implementation should return an output cache profile for the asked <see cref="cacheProfileName"/>.
        /// </summary>
        /// <param name="cacheProfileName">Name of the cache profile.</param>
        /// <returns>A <see cref="OutputCacheProfile"/> instance.</returns>
        OutputCacheProfile RetrieveOutputCacheProfile(string cacheProfileName);

        /// <summary>
        /// Implementation should return a value indicating whether caching is globally enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if caching is globally enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsCachingEnabledGlobally { get; }
    }
}
