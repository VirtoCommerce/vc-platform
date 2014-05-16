using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class ModifiedDateToPastTimeConverter : IValueConverter
	{

		private static readonly ThreadLocal<ModifiedDateToPastTimeConverter> _instance = new ThreadLocal<ModifiedDateToPastTimeConverter>(() => CreateInstance());
		public static ModifiedDateToPastTimeConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		private static ModifiedDateToPastTimeConverter CreateInstance()
		{
			return new ModifiedDateToPastTimeConverter();
		}


		#region IValueConverter Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			var modifDate = (DateTime.SpecifyKind(DateTime.Parse(value.ToString()), DateTimeKind.Utc)).ToLocalTime();

			var datePart = new StringBuilder();
			var stringPart = new StringBuilder();




			var resultDays = (DateTime.Now - modifDate).Days;

			if (resultDays >= 365)
			{
				stringPart.Append(string.Format("{0:yyyy MMMM dd}", modifDate));
				datePart.Append(string.Format("{0:T}", modifDate));
			}
			else
			{
				if (resultDays > 1)
				{
					stringPart.Append(string.Format("{0:M}", modifDate));
					datePart.Append(string.Format("{0:T}", modifDate));
				}
				else if (resultDays == 1)
				{
					stringPart.Append("Yesterday".Localize());
					datePart.Append(string.Format("{0:T}", modifDate));
				}
				else if (resultDays < 1)
				{
					var resultMinutes = (DateTime.Now - modifDate).Minutes;
					var resultHours = (DateTime.Now - modifDate).Hours;
					if (resultMinutes < 1 && resultHours == 0)
					{
						stringPart.Append("LESS THAN A MINUTE AGO".Localize());
					}
					else
					{
						stringPart.Append("Today".Localize());
						datePart.Append(string.Format("{0:T}", modifDate));
					}

				}
			}

			if (!string.IsNullOrEmpty(stringPart.ToString()) && !string.IsNullOrEmpty(datePart.ToString()))
			{
				stringPart.Append(", ");
			}

			return stringPart + datePart.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
