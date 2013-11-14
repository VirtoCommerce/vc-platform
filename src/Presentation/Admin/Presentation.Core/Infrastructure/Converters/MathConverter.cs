using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class MathConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                throw new ArgumentException("Math operation expected as a parameter");

            Func<decimal, decimal, decimal> operationFunction;
            var operation = parameter.ToString()[0];
            switch (operation)
            {
                case '+': operationFunction = (a, b) => (a + b); break;
                case '-': operationFunction = (a, b) => (a - b); break;
                case '*': operationFunction = (a, b) => (a * b); break;
                case '/': operationFunction = (a, b) => (a / b); break;
                default: throw new ArgumentException("Invalid operation " + operation);
            }

            decimal result;

			if (!values.Any() || !IsNumber(values[0]))
			{
				result = 0m;
			}
			else
	        {
				try
				{
					result = values.Cast<decimal>().Aggregate(operationFunction);
				}
				catch
				{
					result = 0m;
				}
	        }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

		public static bool IsNumber<T>(T obj)
		{
			var returnValue = false;
			if (!Equals(obj, null))
			{
				Type objType = obj.GetType();

				if (objType == typeof (decimal))
					returnValue = true;
				else if (objType.IsPrimitive)
				{
					returnValue = (objType != typeof (bool) &&
					               objType != typeof (char) &&
					               objType != typeof (IntPtr) &&
					               objType != typeof (UIntPtr)) ||
					              objType == typeof (decimal);
				}
			}
			return returnValue;
		}
    }
}
