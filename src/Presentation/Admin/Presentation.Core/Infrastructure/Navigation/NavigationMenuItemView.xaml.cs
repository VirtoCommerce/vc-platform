using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation
{
    /// <summary>
    /// Interaction logic for NavigationItem.xaml
    /// </summary>
    public partial class NavigationMenuItemView : UserControl
    {
        public NavigationMenuItemView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets parent ListBoxItem selection when DataTemplate element  got focus
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UIElement_GotFocus(object sender, RoutedEventArgs e)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                var listBoxItem = UIHelper.FindAncestor<ListBoxItem>(element);
                if (listBoxItem != null)
                {
                    listBoxItem.IsSelected = true;
                }
            }
        }
    }
}
