using System;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;

using VirtoCommerce.Foundation.Customers.Model;


namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class CaseFilterTypeToDescriptionConverter : IValueConverter
	{

		private static readonly ThreadLocal<CaseFilterTypeToDescriptionConverter> _instance = new ThreadLocal<CaseFilterTypeToDescriptionConverter>(() => CreateInstance());
		public static CaseFilterTypeToDescriptionConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		private static CaseFilterTypeToDescriptionConverter CreateInstance()
		{
			return new CaseFilterTypeToDescriptionConverter();
		}


		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var caseFilterType = (CaseFilterType)value;

			var descriptionAttribute = typeof(CaseFilterType)
				.GetField(caseFilterType.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.FirstOrDefault() as DescriptionAttribute;

			return descriptionAttribute != null
				? descriptionAttribute.Description
				: caseFilterType.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
