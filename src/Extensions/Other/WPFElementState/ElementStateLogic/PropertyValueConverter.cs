using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace Virtoway.WPF.State
{
	class PropertyValueConverter : IValueConverter
	{
		public ElementState			State		{ get; set; }
		public DependencyObject		Target		{ get; set; }
		public DependencyProperty	Property	{ get; set; }

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (ElementStateOperations.HasPropertyValue(Target, Property))
			{
				object propertyValue = ElementStateOperations.GetPropertyValue(Target, Property);
				return propertyValue;
			}
			throw new ArgumentException(string.Format("The property {0}.{1} is not in state", Target, Property.Name));
 		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			State.UpdateValue(Property, value);
			return Binding.DoNothing;
		}

		#endregion

		#region Utilities

		public static object ConvertFromString(Type targetType, DependencyProperty property, string stringValue)
		{
			DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(property, targetType);
			return descriptor.Converter.ConvertFromString(stringValue);
		}

		public static string ConvertToString(Type targetType, DependencyProperty property, object value)
		{
			DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(property, targetType);
			return descriptor.Converter.ConvertToString(value);
		}
		
		#endregion
	} 
}
