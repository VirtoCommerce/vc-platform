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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;

namespace VirtoCommerce.Bootstrapper.Main.Infrastructure.Dialogs
{
	/// <summary>
	/// Interaction logic for ConfirmationLocalModalInteractionDialog.xaml
	/// </summary>
	public partial class ConfirmationLocalModalInteractionDialog : InteractionDialogBase
	{
		public ConfirmationLocalModalInteractionDialog()
		{
			InitializeComponent();
			Loaded += ConfirmationLocalModalInteractionDialog_Loaded;
		}

		void ConfirmationLocalModalInteractionDialog_Loaded(object sender, RoutedEventArgs e)
		{
			DoubleAnimation doubleAnim = new DoubleAnimation(-460, 0,
				new Duration(new TimeSpan(0, 0, 0, 0, 200)));

			var trans = new TranslateTransform();
			this.RenderTransform = trans;

			trans.BeginAnimation(TranslateTransform.XProperty, doubleAnim);
		}

	}
}
