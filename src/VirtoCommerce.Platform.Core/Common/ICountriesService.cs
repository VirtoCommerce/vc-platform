using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public interface ICountriesService
    {
        Task<IList<Country>> GetCountriesAsync();
        Task<IList<CountryRegion>> GetCountryRegionsAsync(string countryId);
    }
}
