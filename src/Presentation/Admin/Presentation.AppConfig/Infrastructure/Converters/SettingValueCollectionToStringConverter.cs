using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Converters
{
    public class SettingValueCollectionToStringConverter:IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			string retVal = string.Empty;

            if (value != null)
            {
                var settingsCollection = value as ObservableCollection<SettingValue>;
				retVal = string.Join(", ", settingsCollection.ToList().Select(val => val.ToString()));
            }

			return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
