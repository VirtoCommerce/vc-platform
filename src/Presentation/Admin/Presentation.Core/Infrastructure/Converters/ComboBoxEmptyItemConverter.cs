using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Windows.Data;
using System.Globalization;
using System.Collections;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
	public class ComboBoxEmptyItemConverter : IValueConverter
	{
		/// <summary>
		/// this object is the empty item in the combobox. A dynamic object that
		/// returns null for all property request.
		/// </summary>
		private class EmptyItem : DynamicObject
		{
			public override bool TryGetMember(GetMemberBinder binder, out object result)
			{
				// just set the result to null and return true
				result = null;
				return true;
			}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// assume that the value at least inherits from IEnumerable
			// otherwise we cannot use it.
			var container = value as IEnumerable;

			if (container != null)
			{
				// create an array with a single EmptyItem object that serves to show en empty line
				IEnumerable<object> emptyItem = new object[] { new EmptyItem() };
				// everything inherits from object, so we can safely create a generic IEnumerable
				var genericContainer = container.OfType<object>();
				// use Linq to concatenate the two enumerable
				return genericContainer.Concat(emptyItem);
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
