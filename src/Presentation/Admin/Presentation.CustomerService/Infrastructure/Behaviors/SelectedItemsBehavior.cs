using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Behaviors
{
	public static class SelectedItemsBehavior
	{
		public static readonly DependencyProperty SelectedItemsChangedHandlerProperty =
			DependencyProperty.RegisterAttached("SelectedItemsChangedHandler",
				typeof(ICommand),
				typeof(SelectedItemsBehavior),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsChangedHandlerChanged)));


		public static ICommand GetSelectedItemsChangedHandler(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(SelectedItemsChangedHandlerProperty) as ICommand;
		}

		public static void SetSelectedItemsChangedHandler(DependencyObject element, ICommand value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(SelectedItemsChangedHandlerProperty, value);
		}

		public static void OnSelectedItemsChangedHandlerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

			ListView listView = (ListView)d;

			if (e.OldValue == null && e.NewValue != null)
			{
				listView.SelectionChanged += new SelectionChangedEventHandler(ItemsControl_SelectionChanged);
			}

			if (e.OldValue != null && e.NewValue == null)
			{
				listView.SelectionChanged -= new SelectionChangedEventHandler(ItemsControl_SelectionChanged);
			}
		}


		public static void ItemsControl_SelectionChanged(Object sender, SelectionChangedEventArgs e)
		{

			ListView listView = (ListView)sender;

			ICommand itemsChangedHandler = GetSelectedItemsChangedHandler(listView);

			itemsChangedHandler.Execute(listView.SelectedItems);
		}
	} 

}
