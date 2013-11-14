using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Converters;

namespace VirtoCommerce.ManagementClient.Import.Infrastructure
{
	public class ImportEntityTypeConverter : IValueConverter
	{
		private static ThreadLocal<ImportEntityTypeConverter> _instance = new ThreadLocal<ImportEntityTypeConverter>(() => new ImportEntityTypeConverter());
		public static ImportEntityTypeConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value != null ? Enum.Parse(typeof(ImportEntityType), value.ToString()) : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value != null ? value.ToString() : value;
		}
	}
}
