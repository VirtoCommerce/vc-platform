using System;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
	/// <summary>
	/// generic int -> enum -> int converter
	/// </summary>
	/// <typeparam name="T">enum</typeparam>
	public class EnumToIntConverter<T> : SingleInstanceBase<EnumToIntConverter<T>>, IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return LocalizingConverter.Current.Convert((T)value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return System.Convert.ToInt32((T)value);
		}
	}
}
