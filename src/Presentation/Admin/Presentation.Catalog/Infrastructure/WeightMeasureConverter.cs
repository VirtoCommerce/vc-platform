using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
    public class WeightMeasureConverter : IValueConverter
    {
        const string labelKg = "kgs";
        const string labelLb = "lbs";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result;
            int intResult;
            if (int.TryParse(System.Convert.ToString(value), out intResult))
                result = intResult == 1 ? labelLb : labelKg;
            else
                result = labelKg;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
