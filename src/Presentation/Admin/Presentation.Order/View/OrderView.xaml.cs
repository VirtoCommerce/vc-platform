using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.View;

namespace VirtoCommerce.ManagementClient.Order.View
{
	public partial class OrderView : UserControl
	{
		public OrderView()
		{
			this.InitializeComponent();
		}

		#region IOrderView Members
		public void ActivateSectionByName(string tabName)
		{
			foreach (FrameworkElement item in tabControl.Items)
			{
				if (item.Name == tabName)
				{
					tabControl.SelectedItem = item;
				}
			}
		}

		#endregion

	}
}