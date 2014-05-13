using System;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class ItemPresentationValueConverter : IValueConverter
	{
		private static ThreadLocal<ItemPresentationValueConverter> _instance = new ThreadLocal<ItemPresentationValueConverter>(() => new ItemPresentationValueConverter());
		public static ItemPresentationValueConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var parameterString = String.Format("{0}_{1}", value.GetType().Name, parameter as string);
			var param = SettingsManager.Settings.Properties[parameterString];
			return param != null ? param.DefaultValue.ToString().Localize() : System.Windows.DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
