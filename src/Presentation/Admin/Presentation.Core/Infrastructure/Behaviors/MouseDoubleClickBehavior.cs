using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
	public static class MouseDoubleClickBehavior
	{
		/// <summary>
		/// TheCommandToRun : The actual ICommand to run
		/// </summary>
		public static readonly DependencyProperty DoubleClickCommandProperty =
			DependencyProperty.RegisterAttached("DoubleClickCommand",
				typeof(ICommand),
				typeof(MouseDoubleClickBehavior),
				new FrameworkPropertyMetadata((ICommand)null,
					new PropertyChangedCallback(OnHandleDoubleClickCommandChanged)));

		public static readonly DependencyProperty CommandArgumentProperty =
			DependencyProperty.RegisterAttached("CommandArgument",
				typeof(object),
				typeof(MouseDoubleClickBehavior),
				new FrameworkPropertyMetadata(null, null));


		public static object GetCommandArgument(DependencyObject d)
		{
			return d.GetValue(CommandArgumentProperty);
		}

		public static void SetCommandArgument(DependencyObject d, object value)
		{
			d.SetValue(CommandArgumentProperty, value);
		}


		/// <summary>
		/// Gets the TheCommandToRun property.
		/// </summary>
		public static ICommand GetDoubleClickCommand(DependencyObject d)
		{
			return (ICommand)d.GetValue(DoubleClickCommandProperty);
		}

		/// <summary>
		/// Sets the TheCommandToRun property.
		/// </summary>
		public static void SetDoubleClickCommand(DependencyObject d, ICommand value)
		{
			d.SetValue(DoubleClickCommandProperty, value);
		}


		#region Handle the event

		/// <summary>
		/// Hooks up a weak event against the source Selectors MouseDoubleClick
		/// if the Selector has asked for the HandleDoubleClick to be handled
		/// If the source Selector has expressed an interest in not having its
		/// MouseDoubleClick handled the internal reference
		/// </summary>
		private static void OnHandleDoubleClickCommandChanged(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement ele = d as FrameworkElement;
			if (ele != null)
			{
				ele.PreviewMouseDown -= OnMouseDoubleClick;
				if (e.NewValue != null)
				{
					ele.PreviewMouseDown += OnMouseDoubleClick;
				}
			}
		}


		/// <summary>
		/// Invoke the command we tagged.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			//check for double clicks.
			if (e.ClickCount != 2)
				return;
			FrameworkElement ele = sender as FrameworkElement;

			DependencyObject originalSender = e.OriginalSource as DependencyObject;
			if (ele == null || originalSender == null)
				return;

			ICommand command = (ICommand)(sender as DependencyObject).GetValue(DoubleClickCommandProperty);
			object argument = (sender as DependencyObject).GetValue(CommandArgumentProperty);
			if (command != null)
			{
				if (command.CanExecute(argument))
					command.Execute(argument);
			}
		}
		#endregion
	}
}
