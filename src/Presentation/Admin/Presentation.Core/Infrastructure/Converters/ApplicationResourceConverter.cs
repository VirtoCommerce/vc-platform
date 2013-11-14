using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class ApplicationResourceConverter : IValueConverter
    {
        private static readonly ThreadLocal<ApplicationResourceConverter> _instance = new ThreadLocal<ApplicationResourceConverter>(() => CreateInstance());
        public static ApplicationResourceConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        private static ApplicationResourceConverter CreateInstance()
        {
            return new ApplicationResourceConverter();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            return Application.Current.TryFindResource(value); // Use the application as root.
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
        { 
            throw new System.NotImplementedException(); 
        }
    }
}
