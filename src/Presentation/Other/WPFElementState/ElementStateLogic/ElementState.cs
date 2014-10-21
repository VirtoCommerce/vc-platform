using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;

namespace Virtoway.WPF.State
{
	public class ElementState : DependencyObject
	{
		#region Fields

		private readonly string				_uid;
		private readonly ElementStateMode	_mode;
		private readonly Type				_type;

		/// <stringValue>Holds the state of each element property</stringValue>
		private readonly Dictionary<DependencyProperty, object> _state =
			new Dictionary<DependencyProperty, object>();

		#endregion

		#region Initializers

		internal ElementState(DependencyObject element)
		{
			this._uid	= GetUId(element);
			this._mode	= FindMode(element);
			this._type	= element.GetType();
		}		

		#endregion

		#region Attached Properties

		#region Mode Property

		/// <stringValue>Identifies the Mode dependency property</stringValue>
		public static readonly DependencyProperty ModeProperty =
			DependencyProperty.RegisterAttached("Mode", typeof(ElementStateMode), typeof(ElementState),
			new PropertyMetadata(ElementStateMode.Default));

		public static void SetMode(DependencyObject element, ElementStateMode value)
		{
			element.SetValue(ModeProperty, value);
		}

		public static ElementStateMode GetMode(DependencyObject element)
		{
			return (ElementStateMode)element.GetValue(ModeProperty);
		}

		#endregion

		#region UId Property

		/// <stringValue>Identifies the UId dependency property</stringValue>
		public static readonly DependencyProperty UIdProperty =
			DependencyProperty.RegisterAttached("UId", typeof(string), typeof(ElementState),
			new PropertyMetadata(string.Empty));

		public static void SetUId(DependencyObject element, string value)
		{
			element.SetValue(UIdProperty, value);
		}

		public static string GetUId(DependencyObject element)
		{
			return (string)element.GetValue(UIdProperty);
		}

		#endregion

		#endregion

		#region Properties

		internal string UId
		{
			get { return _uid; }
		}

		internal ElementStateMode Mode
		{
			get { return _mode; }
		}

		internal Type Type
		{
			get { return _type; }
		} 

		#endregion

		#region Operations

		internal new object GetValue(DependencyProperty property)
		{
			if (!HasValue(property))
			{
				throw new ArgumentException(string.Format("There is no value for property name {0}", property.Name));
			}
			return _state[property];
		}

		internal bool HasValue(DependencyProperty property)
		{
			return _state.ContainsKey(property);
		}

		internal object AddValue(DependencyProperty property, object value)
		{
			if (_state.ContainsKey(property))
			{
				return _state[property];
			}
			else
			{
				// The property is not exist
				if (Mode == ElementStateMode.Persist &&
					ElementStateOperations.Persistency.Contains(UId, property.Name))
				{
					string stringValue = ElementStateOperations.Persistency.GetValue(UId, property.Name);
					value = PropertyValueConverter.ConvertFromString(Type, property, stringValue);
				}
				_state.Add(property, value);
			}
			return value;
		}

		internal void UpdateValue(DependencyProperty property, object value)
		{
			if (!_state.ContainsKey(property))
			{
				throw new ArgumentException(string.Format("Property name {0} is not in state", property.Name));
			}
			// Override the old value
			_state[property] = value;
			string stringValue = PropertyValueConverter.ConvertToString(Type, property, value);
			ElementStateOperations.Persistency.Update(UId, property.Name, stringValue);
		}

		private ElementStateMode FindMode(DependencyObject element)
		{
			ElementStateMode mode = GetMode(element);
			if (mode != ElementStateMode.Default)
			{
				return mode;
			}
			return FindParentMode(element as FrameworkElement);
		}

		private ElementStateMode FindParentMode(FrameworkElement element)
		{
			if (element == null)
			{
				throw new ArgumentException(string.Format("You must set the ElementState.Mode attached property", element));
			}
			ElementStateMode mode = GetMode(element.Parent);
			if (mode != ElementStateMode.Default)
			{
				return mode;				
			}
			return FindParentMode(element.Parent as FrameworkElement);
		}

		#endregion		
	}
}
