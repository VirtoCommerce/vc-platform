using System;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.Infrastructure
{
	public class StoreSettingValueConverter : IValueConverter, IMultiValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object retVal = null;

			var item = value as StoreSetting;
			if (item != null)
			{
				var valueType = item.ValueType;
				if (valueType == StoreSettingViewModel.textShortText)
					retVal = item.ShortTextValue;
				else if (valueType == StoreSettingViewModel.textLongText || valueType == StoreSettingViewModel.textXML)
					retVal = item.LongTextValue;
				else if (valueType == StoreSettingViewModel.textDecimal)
					retVal = item.DecimalValue.ToString();
				else if (valueType == StoreSettingViewModel.textInteger)
					retVal = item.IntegerValue.ToString();
				else if (valueType == StoreSettingViewModel.textBoolean)
					retVal = item.BooleanValue.ToString();
				else if (valueType == StoreSettingViewModel.textDateTime)
					retVal = item.DateTimeValue.HasValue ? item.DateTimeValue.Value.ToString("d") : "N/A".Localize(null, LocalizationScope.DefaultCategory);
			}
			else
				retVal = value == null ? "N/A".Localize(null, LocalizationScope.DefaultCategory) : value.ToString();

			return retVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#region IMultiValueConverter Members

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
