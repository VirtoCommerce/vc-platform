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

namespace VirtoCommerce.ManagementClient.Customers.View
{
    /// <summary>
    /// Interaction logic for CustomerServiceDetailView.xaml
    /// </summary>
    public partial class CustomersDetailView : UserControl
    {
        public CustomersDetailView()
        {
            InitializeComponent();
        }

        private void DisableRightClickContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }


        
    }
}
