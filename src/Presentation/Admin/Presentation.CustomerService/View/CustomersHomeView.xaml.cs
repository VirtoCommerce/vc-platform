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
using Microsoft.Practices.Prism.Regions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.ViewModel;

namespace VirtoCommerce.ManagementClient.Customers.View
{
	/// <summary>
	/// Interaction logic for CaseHomeView.xaml
	/// </summary>
	public partial class CustomersHomeView : UserControl
	{
		public CustomersHomeView()
		{
			InitializeComponent();
		}

		private void DisableRightClickContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			e.Handled = true;
		}

	}
}
