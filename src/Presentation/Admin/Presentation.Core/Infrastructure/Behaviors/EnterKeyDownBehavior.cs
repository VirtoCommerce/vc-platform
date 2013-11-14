using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
    public static class EnterKeyDownBehavior
	{
		/// <summary>
		/// TheCommandToRun : The actual ICommand to run
		/// </summary>
        public static readonly DependencyProperty EnterKeyDownCommandProperty =
            DependencyProperty.RegisterAttached("EnterKeyDownCommand",
				typeof(ICommand),
                typeof(EnterKeyDownBehavior),
				new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnHandleEnterKeyDownCommandChanged)));

		public static readonly DependencyProperty CommandArgumentProperty =
			DependencyProperty.RegisterAttached("CommandArgument",
				typeof(object),
                typeof(EnterKeyDownBehavior),
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
        public static ICommand GetEnterKeyDownCommand(DependencyObject d)
		{
            return (ICommand)d.GetValue(EnterKeyDownCommandProperty);
		}

		/// <summary>
		/// Sets the TheCommandToRun property.
		/// </summary>
        public static void SetEnterKeyDownCommand(DependencyObject d, ICommand value)
		{
            d.SetValue(EnterKeyDownCommandProperty, value);
		}


		#region Handle the event

		/// <summary>
		/// Hooks up a weak event against the source Selectors press Enter key 
		/// if the Selector has asked for the HandleKeyDown to be handled
		/// If the source Selector has expressed an interest in not having its
        /// PreviewKeyDown handled the internal reference
		/// </summary>
        private static void OnHandleEnterKeyDownCommandChanged(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement ele = d as FrameworkElement;
			if (ele != null)
			{
                ele.PreviewKeyDown -= OnEnterKeyDown;
				if (e.NewValue != null)
				{
                    ele.PreviewKeyDown += OnEnterKeyDown;
				}
			}
		}


		/// <summary>
		/// Invoke the command we tagged.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private static void OnEnterKeyDown(object sender, KeyEventArgs e)
		{
			//check for Enter key down.
            if (e.Key == Key.Enter)
            {
                FrameworkElement ele = sender as FrameworkElement;

                DependencyObject originalSender = e.OriginalSource as DependencyObject;
                if (ele == null || originalSender == null)
                    return;

                ICommand command = (ICommand)(sender as DependencyObject).GetValue(EnterKeyDownCommandProperty);
                object argument = (sender as DependencyObject).GetValue(CommandArgumentProperty);
                if (command != null)
                {
                    if (command.CanExecute(argument))
                        command.Execute(argument);
                }
            }
		}
		#endregion
	}
}
