using System.Windows.Controls;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Main.View
{
	/// <summary>
	/// Interaction logic for MainHomeView.xaml
	/// </summary>
	public partial class MainHomeView
	{
		public MainHomeView()
		{
			InitializeComponent();
		}

		private void ScrollViewer_HorizontalScroll_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			int delta = e.Delta * -1;

			if (sender == null)
				return;

			var scroll = sender as ScrollViewer;

			if (delta > 0)
			{
				if (scroll.HorizontalOffset + delta > 0)
				{
					scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + delta);
				}
				else
				{
					scroll.ScrollToRightEnd();
				}
			}
			else
			{
				if (scroll.ExtentWidth > scroll.HorizontalOffset + delta)
				{
					scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + delta);
				}
				else
				{
					scroll.ScrollToLeftEnd();
				}
			}
		}
	}
}
