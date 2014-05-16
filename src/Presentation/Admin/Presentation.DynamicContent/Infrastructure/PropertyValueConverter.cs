using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class PropertyValueConverter : IValueConverter, IMultiValueConverter
	{
		private static readonly ThreadLocal<PropertyValueConverter> Instance = new ThreadLocal<PropertyValueConverter>(() => new PropertyValueConverter());
		public static PropertyValueConverter Current
		{
			get
			{
				return Instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var parameterString = parameter as string;
			object retVal;
			switch (parameterString)
			{
				case "ContentType":
					retVal = ((DynamicContentType)value).ToString();
					break;
				case "String2ContentType":
					retVal = (DynamicContentType)Enum.Parse(typeof(DynamicContentType), value.ToString());
					break;
				default:
					retVal = value == null || value.ToString() == string.Empty ? "N/A".Localize(null, LocalizationScope.DefaultCategory) : value.ToString();
					break;
			}
			return retVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var parameterString = parameter as string;
			object retVal = null;

			if (parameterString == "String2ContentType")
				if (value is DynamicContentType)
					retVal = ((DynamicContentType)value).ToString();
				else
					retVal = string.Empty;

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
