using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Common
{
    public class FileSystemCountriesService : ICountriesService
    {
        private readonly PlatformOptions _platformOptions;
        private readonly IPlatformMemoryCache _memoryCache;

        public FileSystemCountriesService(IOptions<PlatformOptions> platformOptions, IPlatformMemoryCache memoryCache)
        {
            _platformOptions = platformOptions.Value;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get Full list of countries defined by ISO 3166-1 alpha-3
        /// based on https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
        /// </summary>
        public Task<IList<Country>> GetCountriesAsync()
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetCountriesAsync));
            return _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var filePath = Path.GetFullPath(_platformOptions.CountriesFilePath);
                return JsonConvert.DeserializeObject<IList<Country>>(await File.ReadAllTextAsync(filePath));
            });
        }
    }
}
