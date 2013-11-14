using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class IsIClosableConverter:IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;

            if (value != null)
            {
                if (value is IClosable)
                {
                    result = true;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
