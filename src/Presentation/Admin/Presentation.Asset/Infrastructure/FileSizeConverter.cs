using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Asset.Infrastructure
{
	/// <summary>
	/// convert bytes to human readable text
	/// </summary>
	public class FileSizeConverter : IValueConverter
	{
		const int _Kilo = 1024;
		readonly string[] _SizeNames = new[] { "Bytes".Localize(), "KB".Localize(), "MB".Localize(), "GB".Localize(), "TB".Localize() };

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;

			long valueLong;
			if (long.TryParse(System.Convert.ToString(value), out valueLong) && valueLong > 0)
			{
				var count = 0;
				decimal valueTmp = valueLong;
				while (valueTmp >= _Kilo && count < _SizeNames.Length)
				{
					valueTmp = Math.Round(valueTmp / _Kilo, 1);
					count++;
				}
				result = string.Format("{0} {1}", valueTmp, _SizeNames[count]);
			}
			else
				result = value;

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
