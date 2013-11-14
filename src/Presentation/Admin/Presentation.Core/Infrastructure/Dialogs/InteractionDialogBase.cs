using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs
{
	public class InteractionDialogBase : Window
	{

		public InteractionDialogBase()
		{
		}

		public static readonly DependencyProperty MessageTemplateProperty =
			DependencyProperty.Register("MessageTemplate", typeof(DataTemplate), typeof(InteractionDialogBase), new PropertyMetadata(null));

		public DataTemplate MessageTemplate
		{
			get { return (DataTemplate)GetValue(MessageTemplateProperty); }
			set { SetValue(MessageTemplateProperty, value); }
		}
	}
}
