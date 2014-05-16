using System.Windows;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	public class WatermarkedTextBox : TextBox
	{
		private const string _defaultWatermark = "...";

		public static readonly DependencyProperty WatermarkTextProperty =
				DependencyProperty.Register("WatermarkText",
				typeof(string),
				typeof(WatermarkedTextBox), new PropertyMetadata(_defaultWatermark));

		private static DependencyPropertyKey HasTextPropertyKey =
			DependencyProperty.RegisterReadOnly(
				"HasText",
				typeof(bool),
				typeof(WatermarkedTextBox),
				new PropertyMetadata());
		public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

		static WatermarkedTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(WatermarkedTextBox),
				new FrameworkPropertyMetadata(typeof(WatermarkedTextBox)));
		}


		public string WatermarkText
		{
			get { return (string)GetValue(WatermarkTextProperty); }
			set { SetValue(WatermarkTextProperty, value); }
		}

		public bool HasText
		{
			get { return (bool)GetValue(HasTextProperty); }
			private set { SetValue(HasTextPropertyKey, value); }
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			base.OnTextChanged(e);

			HasText = Text.Length != 0;
		}
	}
}
