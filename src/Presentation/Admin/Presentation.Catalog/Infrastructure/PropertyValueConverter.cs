using System;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
    public class PropertyValueConverter : IValueConverter, IMultiValueConverter
    {
        private static ThreadLocal<PropertyValueConverter> _instance = new ThreadLocal<PropertyValueConverter>(() => new PropertyValueConverter());
        public static PropertyValueConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var parameterString = parameter as string;
            object retVal = null;

            if (parameterString == "PropertyValueType")
            {
                retVal = ((PropertyValueType)value).ToString();
            }
            else if (parameterString == "Int2PropertyValueType")
            {
                retVal = (PropertyValueType)value;
            }
            else
            {
                PropertyAndPropertyValueBase input = null;
                var item = value as PropertyValueBase;
                if (item == null)
                {
                    input = value as PropertyAndPropertyValueBase;
                    if (input != null)
                        item = input.Value;
                }

                // decoding dictionary value
                if (input != null && input.Property != null)
                {
                    var parameterProperty = input.Property;
                    if (!parameterProperty.IsEnum)
                    {
                        item = input.Value;
                    }
                    else if (parameterProperty.IsMultiValue && input.Values != null && input.Values.Any())
                    {
                        retVal = string.Join(", ", input.Values.ToList().Select(val => parameterProperty.PropertyValues.FirstOrDefault(x => x.PropertyValueId == val.KeyValue).ToString()));
                    }
                    else if (input.Value != null)
                        item = parameterProperty.PropertyValues.FirstOrDefault(x => x.PropertyValueId == input.Value.KeyValue);
                }

                if (item != null &&
                    (input == null || input.Property == null || !input.Property.IsMultiValue))
                {
                    retVal = item.ToString();
                }

                if (retVal == null)
                    retVal = "N/A".Localize(null, LocalizationScope.DefaultCategory);
            }

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            object retVal = null;

            if (parameterString == "Int2PropertyValueType")
                if (value is PropertyValueType)
                    retVal = ((PropertyValueType)value).GetHashCode();
                else
                    retVal = -1;

            return retVal;
        }

        #region IMultiValueConverter
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
