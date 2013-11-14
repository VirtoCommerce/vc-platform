using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class ContentTypeToDisplayNameConverter : IValueConverter
	{
		private static readonly ThreadLocal<ContentTypeToDisplayNameConverter> Instance = new ThreadLocal<ContentTypeToDisplayNameConverter>(() => new ContentTypeToDisplayNameConverter());
		public static ContentTypeToDisplayNameConverter Current
		{
			get
			{
				return Instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var resultString = value as string;
			if (resultString != null)
			{
				resultString = Regex.Replace(resultString, "([a-z])([A-Z])", "$1 $2");

			}
			return resultString;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
