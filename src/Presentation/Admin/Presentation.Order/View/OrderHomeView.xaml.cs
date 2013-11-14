using System;
using System.Collections.Generic;
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
using VirtoCommerce.ManagementClient.Order.View;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.View
{
	/// <summary>
	/// Interaction logic for OrderHomeView.xaml
	/// </summary>
	public partial class OrderHomeView : UserControl
	{
		public OrderHomeView()
		{
			this.InitializeComponent();
			InitializeListTemplates();
		}

		private void InitializeListTemplates()
		{
			//_defaultItemsPanelTemplate = lb_orderList.ItemsPanel;
		}

		private void SearchTextBox_Search(object sender, RoutedEventArgs e)
		{
			
		}

		//private void radioBtnSimpleOrderList_Checked(object sender, RoutedEventArgs e)
		//{
		//     lb_orderList.ItemTemplate = this.Resources["OrderListItemSmallTemplate"] as DataTemplate;
		//     lb_orderList.ItemsPanel = this.Resources["ListBoxItemsPanelTemplate"] as ItemsPanelTemplate;
		//}

		//private void radioBtnDetailOrderList_Checked(object sender, RoutedEventArgs e)
		//{
		//    if (lb_orderList != null)
		//    {
		//        lb_orderList.ItemTemplate = this.Resources["OrderListItemDetailedTemplate"] as DataTemplate;
		//        lb_orderList.ItemsPanel = _defaultItemsPanelTemplate;
		//    }

		//}
	}
}