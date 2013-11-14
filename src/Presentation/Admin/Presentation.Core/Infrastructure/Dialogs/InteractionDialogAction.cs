using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs
{
	public class InteractionDialogAction : TargetedTriggerAction<FrameworkElement>
	{
		public static readonly DependencyProperty DialogTypeProperty =
		DependencyProperty.Register("DialogType", typeof(Type), typeof(InteractionDialogAction), new PropertyMetadata(null));

		public static readonly DependencyProperty ContentTemplateProperty =
			DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(InteractionDialogAction), new PropertyMetadata(null));

		public Type DialogType
		{
			get { return (Type)GetValue(DialogTypeProperty); }
			set { SetValue(DialogTypeProperty, value); }
		}


	

		public DataTemplate ContentTemplate
		{
			get { return (DataTemplate)GetValue(ContentTemplateProperty); }
			set { SetValue(ContentTemplateProperty, value); }
		}

		protected override void Invoke(object parameter)
		{
			var args = parameter as InteractionRequestedEventArgs;
			if (args != null)
			{
				InteractionDialogBase dialog = this.GetDialog(args.Context);
				var callback = args.Callback;

				EventHandler handler = null;

				handler = (s, e) =>
				{
					dialog.Closed -= handler;
					callback();
				};

				dialog.Closed += handler;
			
				dialog.ShowDialog();
				
			}

		}

		InteractionDialogBase GetDialog(Notification notification)
		{
			var	retVal = Activator.CreateInstance(this.DialogType) as InteractionDialogBase;

			retVal.DataContext = notification;
			retVal.MessageTemplate = this.ContentTemplate;
			retVal.Owner = Application.Current.MainWindow;
			retVal.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			var owner = (FrameworkElement)this.AssociatedObject;
			retVal.Width = retVal.Owner.ActualWidth;
			retVal.Height = retVal.Owner.ActualHeight;
			//((Grid)this.AssociatedObject).Children.Add(dialog);
			return retVal;
		}
	}
}
