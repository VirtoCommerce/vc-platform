using System.Windows;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    public partial class HorizontalTabDefaultView : ViewBase
    {
		public HorizontalTabDefaultView()
        {
            InitializeComponent();
        }

	    private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
	    {
		    var ctx = DataContext as SubTabsDefaultViewModel;
			if (ctx != null)
			{
				tabControl.Focus();
				var uie = tabControl.SelectedContent as UIElement;
				//tabControl.Items
				//uie.Focus();
			}
	    }
    }
}
