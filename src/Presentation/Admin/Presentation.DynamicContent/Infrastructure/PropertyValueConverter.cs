using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class PropertyValueConverter : IValueConverter
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
					retVal = value == null || value.ToString() == string.Empty ? "N/A" : value.ToString();
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

	}
}
