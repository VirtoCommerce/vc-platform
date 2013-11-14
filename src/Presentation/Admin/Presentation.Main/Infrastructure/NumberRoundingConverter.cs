using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace VirtoCommerce.ManagementClient.Main.Infrastructure
{
	/// <summary>
	/// convert numbers to human readable text
	/// </summary>
	public class NumberRoundingConverter : MarkupExtension, IValueConverter
	{
		const int _Kilo = 1000;

		private static NumberRoundingConverter _converter = null;
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _converter ?? (_converter = new NumberRoundingConverter());
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;

			long valueLong;
			if (long.TryParse(System.Convert.ToString(value), out valueLong))
			{
				decimal valueTmp = valueLong;
				if (valueLong >= 100000)
				{
					if (valueLong < 1000000) // 1 M
					{
						valueTmp = Math.Round(valueTmp / _Kilo);
						result = string.Format("{0} k", valueTmp);
					}
					else if (valueLong < 10000000) // 10 M
					{
						valueTmp = Math.Round(valueTmp / _Kilo / _Kilo, 1);
						result = string.Format("{0} M", valueTmp);
					}
					else
					{
						valueTmp = Math.Round(valueTmp / _Kilo / _Kilo);
						result = string.Format("{0} M+", valueTmp);
					}
				}
				else
				{
					result = value;
				}
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
