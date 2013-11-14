using System;
using System.Linq;
using System.Windows.Data;
using System.Threading;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class EnumToDescriptionConverter : IValueConverter
	{
		private static readonly ThreadLocal<EnumToDescriptionConverter> _instance = new ThreadLocal<EnumToDescriptionConverter>(() => CreateInstance());
		public static EnumToDescriptionConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		private static EnumToDescriptionConverter CreateInstance()
		{
			return new EnumToDescriptionConverter();
		}

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			var t = value.GetType();
			if (t.IsEnum == false)
				throw new ArgumentException("Value must be an Enum.");
			var descriptionAttribute = t
			  .GetField(value.ToString())
			  .GetCustomAttributes(typeof(DescriptionAttribute), false)
			  .FirstOrDefault() as DescriptionAttribute;


			return descriptionAttribute != null
			  ? descriptionAttribute.Description
			  : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
