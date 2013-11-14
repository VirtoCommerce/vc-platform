using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class PackagingIdToNameConverter: IMultiValueConverter
    {
        private static readonly ThreadLocal<PackagingIdToNameConverter> _instance =
            new ThreadLocal<PackagingIdToNameConverter>(()=>new PackagingIdToNameConverter());

        public static  PackagingIdToNameConverter Current
        {
            get { return _instance.Value; }
        }

        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string retVal = null;
            var packagingId = values[0] as string;
	        var _allPackagings = values[1] is List<Packaging> ? values[1] as List<Packaging> : null;
            if (packagingId != null && _allPackagings != null)
            {
                var packaging = _allPackagings.SingleOrDefault(p => p.PackageId == packagingId);
                if (packaging != null)
                    retVal = packaging.Name;
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
