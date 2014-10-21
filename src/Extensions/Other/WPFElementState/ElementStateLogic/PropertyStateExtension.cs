using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;

namespace Virtoway.WPF.State
{
	public class PropertyStateExtension : MarkupExtension
	{
		#region Fields

		private string _default; 
		
		#endregion

		#region Overrides

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IProvideValueTarget provideValue = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValue == null)
			{
				return this;
			}

			DependencyObject	target		= provideValue.TargetObject as DependencyObject;
			DependencyProperty	property	= provideValue.TargetProperty as DependencyProperty;
			
			// If not dependency object or dependency property, return this instance
			if (target == null || property == null)
			{
				return this;
			}

			if (!ElementStateOperations.HasPropertyValue(target, property))
			{
				object defaultValue = PropertyValueConverter.ConvertFromString(target.GetType(), property, Default);
				ElementStateOperations.AddPropertyValue(target, property, defaultValue);
			}

			BindingBase binding = ElementStateOperations.CreateBinding(target, property);
			
			object startValue = binding.ProvideValue(serviceProvider);
			return startValue;
		}

		#endregion

		#region Properties

		public string Default
		{
			get { return _default; }
			set { _default = value; }
		}
 
		#endregion		
	}
}
