using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nager.Country;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Common
{
    public class CountriesService : ICountriesService
    {
        private readonly CountryProvider _provider = new();
        private readonly FileSystemCountriesService _fileSystemCountriesService;

        public CountriesService(FileSystemCountriesService fileSystemCountriesService)
        {
            _fileSystemCountriesService = fileSystemCountriesService;
        }

        public IList<Country> GetCountries()
        {
            return _provider.GetCountries()
                .Select(x => new Country { Id = x.Alpha3Code.ToString(), Name = x.CommonName })
                .ToList();
        }

        public Task<IList<Country>> GetCountriesAsync()
        {
            var result = GetCountries();
            
            return Task.FromResult(result);
        }

        public Country GetByCode(string code)
        {
            if (code.Length != 2 && code.Length != 3)
            {
                throw new ArgumentException("Code must be of 2 or 3 letters", nameof(code));
            }

            code = code.ToUpperInvariant();

            var countryInfo = _provider.GetCountry(code);

            if (countryInfo == null)
            {
                throw new ArgumentException($"Country with code {code} not found");
            }

            return new Country {Name = countryInfo.OfficialName, Id = countryInfo.Alpha3Code.ToString()};
        }

        public async Task<IList<CountryRegion>> GetCountryRegionsAsync(string countryId)
        {
            var country = GetByCode(countryId);

            return await _fileSystemCountriesService.GetCountryRegionsAsync(country.Id);
        }
    }
}
