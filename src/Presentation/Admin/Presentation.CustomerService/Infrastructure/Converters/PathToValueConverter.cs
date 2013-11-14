using System;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
    public class PathToValueConverter : IValueConverter, IMultiValueConverter
    {
        private static readonly ThreadLocal<PathToValueConverter> _instance = new ThreadLocal<PathToValueConverter>(() => new PathToValueConverter());
        public static PathToValueConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        #region IValueConverter members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = value;
            if (parameter != null)
            {
                var paramStrings = parameter.ToString().Split('.').ToList();
                paramStrings.ForEach(x =>
                {
                    if (result != null)
                    {
                        var prop = result.GetType().GetProperty(x);
                        if (prop != null)
                        {
                            result = prop.GetValue(result, null);
                        }
                        else
                        {
                            result = x;
                        }
                    }
                });
            }

            if (result is DateTime)
                result = ((DateTime)result).ToString("R");

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(values[1], targetType, values[0], culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
