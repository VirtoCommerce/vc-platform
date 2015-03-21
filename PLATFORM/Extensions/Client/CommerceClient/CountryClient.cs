using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Client
{
    public class CountryClient
    {
        #region Cache Constants
        public const string CountriesCacheKey = "O:C:{0}";
        #endregion

        #region Private Variables

	    private readonly ICountryRepository _countryRepository = null;
        private readonly ICacheRepository _cacheRepository;
        private readonly bool _isEnabled;
        #endregion

        public CountryClient(ICountryRepository countryRepository, ICacheRepository cacheRepository)
        {
            _countryRepository = countryRepository;
            _cacheRepository = cacheRepository;
            _isEnabled = OrderConfiguration.Instance.Cache.IsEnabled;
        }

        public Country[] GetAllCountries()
        {
            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.CountriesCachePrefix,  string.Format(CountriesCacheKey, "all")),
                () => (from c in _countryRepository.Countries 
					   orderby c.Priority descending, c.DisplayName ascending select c).ExpandAll().ToArray(),
                OrderConfiguration.Instance.Cache.CountryTimeout,
                _isEnabled);
        }

        CacheHelper _cacheHelper;
        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }
}
