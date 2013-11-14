using System;
using System.Linq;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
    /// <summary>
    /// multivalue converter for forcing UI update.
    /// </summary>
    public class ValueBindingUpdateValueConverter : IValueConverter, IMultiValueConverter
    {
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
                        result = prop.GetValue(result, null);
                    }
                });
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #region IMultiValueConverter Members

        /// <summary>
        /// use this method in order to update GUI
        /// </summary>
        /// <param name="values">real value is in index[0]. Other are fake values that respond to INotifyPropertyChanged</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">optional. Can show path to return in real value</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(values[0], targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
