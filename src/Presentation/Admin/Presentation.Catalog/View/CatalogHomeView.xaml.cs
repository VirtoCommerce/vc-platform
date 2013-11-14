using System.Windows.Controls;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Catalog.View
{
	/// <summary>
	/// Interaction logic for CatalogHomeView.xaml
	/// </summary>
	public partial class CatalogHomeView : UserControl
	{
		public CatalogHomeView()
		{
			InitializeComponent();
		}

		private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var scv = (ScrollViewer)sender;
			scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
			e.Handled = true;
		}
	}
}
