using System.Windows;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
	public static class TabControlKeyNavigationsIgnoreBehavior
	{
		//Setter for use in XAML: this "enables" this behavior
		//<TabControl local:TabControlKeyNavigationsIgnoreBehavior.Enabled="True">
		public static void SetEnabled(DependencyObject depObj, bool value)
		{
			depObj.SetValue(EnabledProperty, value);
		}

		public static readonly DependencyProperty EnabledProperty =
			DependencyProperty.RegisterAttached("Enabled", typeof(bool),
			typeof(TabControlKeyNavigationsIgnoreBehavior),
			new FrameworkPropertyMetadata(false, OnEnabledSet));

		static void OnEnabledSet(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			var uiElement = depObj as UIElement;
			uiElement.PreviewKeyDown +=
			  (o, e) =>
			  {
				  e.Handled = (e.Key == Key.Tab || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right) &&
					  Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
			  };
		}
	}
}