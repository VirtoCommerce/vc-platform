using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nager.Country;
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
        private readonly CountryProvider _provider = new();

        public FileSystemCountriesService(IOptions<PlatformOptions> platformOptions, IPlatformMemoryCache memoryCache)
        {
            _platformOptions = platformOptions.Value;
            _memoryCache = memoryCache;
        }

        public IList<Country> GetCountries()
        {
#pragma warning disable S4462
            return GetCountriesAsync().GetAwaiter().GetResult();
#pragma warning restore S4462
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
                var countriesList = JsonConvert.DeserializeObject<IList<Country>>(await File.ReadAllTextAsync(filePath));
                countriesList = countriesList.OrderBy(x => x.Name).ToList();

                return countriesList;
            });
        }

        public async Task<IList<CountryRegion>> GetCountryRegionsAsync(string countryId)
        {
            countryId = GetIso3Code(countryId);

            var cacheKey = CacheKey.With(GetType(), nameof(GetCountryRegionsAsync));
            var countries = await _memoryCache.GetOrCreateExclusive(cacheKey, async (cacheEntry) =>
            {
                var filePath = Path.GetFullPath(_platformOptions.CountryRegionsFilePath);
                return JsonConvert.DeserializeObject<IList<Country>>(await File.ReadAllTextAsync(filePath));
            });

            var country = countries.FirstOrDefault(x => x.Id.Equals(countryId, StringComparison.InvariantCultureIgnoreCase));
            if (country != null)
            {
                return country.Regions.OrderBy(x => x.Name).ToList();
            }

            return Array.Empty<CountryRegion>();
        }

        public Country GetByCode(string code)
        {
            code = GetIso3Code(code);

            var country = GetCountries().FirstOrDefault(x => x.Id == code);

            if (country == null)
            {
                throw new ArgumentException($"Country with code {code} not found");
            }

            return country;
        }

        private string GetIso3Code(string code)
        {
            if (code.Length == 2)
            {
                var country = _provider.GetCountry(code);

                if (country == null)
                {
                    return null;
                }

                code = country.Alpha3Code.ToString();
            }

            return code;
        }
    }
}
