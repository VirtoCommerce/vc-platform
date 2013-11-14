using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model.Countries;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class CountryIdToNameConverter : IMultiValueConverter
    {
        private static readonly ThreadLocal<CountryIdToNameConverter> _instance = new ThreadLocal<CountryIdToNameConverter>(() => new CountryIdToNameConverter());
        public static CountryIdToNameConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }
		
        #region IMultiValueConverter Members
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
			string retVal = null;
			var countryCode = values[0] as string;
			var _allCountries = values[1] is List<Country> ? values[1] as List<Country> : null;
            if (countryCode != null && _allCountries != null)
            {
                var country = _allCountries.FirstOrDefault(x => x.CountryId == countryCode);
                if (country != null)
                    retVal = country.DisplayName;
            }
            
            return retVal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
