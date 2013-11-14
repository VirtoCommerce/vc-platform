using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;
using VirtoCommerce.ManagementClient.Core;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs
{
	/// <summary>
	/// Interaction logic for WizardDialog.xaml
	/// </summary>
	public partial class WizardDialog : InteractionDialogBase
	{

		

		#region Properties

		public bool IsCancelButtonPressed { get; set; }

		#endregion


		public WizardDialog()
		{
			InitializeComponent();
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (IsCancelButtonPressed)
			{
				var window = new Window();
				window.Style = FindResource("Virto_WizardWindowConfirmation_Style") as Style;

				window.Width = this.ActualWidth;
				window.Height = this.ActualHeight;
				window.Owner = this;
				window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

				if (window.ShowDialog() == true)
				{
					e.Cancel = false;
				}
				else
				{
					e.Cancel = true;
				}
				IsCancelButtonPressed = false;
			}
		}



	}
}
