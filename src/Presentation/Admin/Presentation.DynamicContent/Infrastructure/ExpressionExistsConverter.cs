using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class ExpressionExistsConverter : IValueConverter
	{
		private const string NoValue = "No";
		private const string YesValue = "Yes";

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.IsNullOrEmpty((string)value) ? NoValue : YesValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
