using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;

namespace VirtoCommerce.ManagementClient
{
	public partial class Shell
	{
		#region Static Fields

		private static TaskScheduler _dispatcherSyncContext;

		#endregion

		#region Constructors

		static Shell()
		{
			if (Application.Current == null)
			{
				_dispatcherSyncContext = TaskScheduler.Default;
			}
			else if (System.Threading.Thread.CurrentThread.ManagedThreadId ==
					 Application.Current.Dispatcher.Thread.ManagedThreadId)
			{
				_dispatcherSyncContext = TaskScheduler.FromCurrentSynchronizationContext();
			}
			else
			{
				_dispatcherSyncContext =
					 (TaskScheduler)
					  App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send,
											 new Func<TaskScheduler>(
											 TaskScheduler.FromCurrentSynchronizationContext));
			}
		}

		public Shell()
		{
			RestoreSizeAndCulture();
			InitializeComponent();
			Closing += OnClosing;
		}

		#endregion

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveSizeAndCulture();
			var commandProxy = ServiceLocator.Current.GetInstance<GlobalCommandsProxy>();
			if (commandProxy != null)
			{
				// commandProxy.SaveChangesAllCommand.Execute(null);

				Queue<ICommand> commands;
				lock (commandProxy.CancelAllCommand.RegisteredCommands)
				{
					commands =
						new Queue<ICommand>(
							commandProxy.CancelAllCommand.RegisteredCommands.Where(x => x.CanExecute(null)).ToList());
				}

				while (commands.Count > 0)
				{
					ICommand command = commands.Dequeue();
					command.Execute(e);
					if (e.Cancel)
					{
						return;
					}
				}
			}

			// Close all hidden dialog window
			foreach (var window in Application.Current.Windows.OfType<InteractionDialogBase>())
			{
				window.Close();
			}
		}

		private void RestoreSizeAndCulture()
		{
			try
			{
				Top = Properties.Settings.Default.Top;
				Left = Properties.Settings.Default.Left;
				Height = Properties.Settings.Default.Height;
				Width = Properties.Settings.Default.Width;
				if (Properties.Settings.Default.Maximised)
				{
					WindowState = WindowState.Maximized;
				}
				CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(Properties.Settings.Default.CurrentUICulture);
			}
			catch
			{
			}
		}

		private void SaveSizeAndCulture()
		{
			try
			{
				if (WindowState == WindowState.Maximized)
				{
					// Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
					Properties.Settings.Default.Top = RestoreBounds.Top;
					Properties.Settings.Default.Left = RestoreBounds.Left;
					Properties.Settings.Default.Height = RestoreBounds.Height;
					Properties.Settings.Default.Width = RestoreBounds.Width;
					Properties.Settings.Default.Maximised = true;
				}
				else
				{
					Properties.Settings.Default.Top = Top;
					Properties.Settings.Default.Left = Left;
					Properties.Settings.Default.Height = Height;
					Properties.Settings.Default.Width = Width;
					Properties.Settings.Default.Maximised = false;
				}
				Properties.Settings.Default.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
				Properties.Settings.Default.Save();
			}
			catch
			{
			}
		}

		private void MainMenu_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var delta = e.Delta * -1;

			if (sender == null)
				return;

			var listView = sender as ListView;

			var scroll = VirtoCommerce.ManagementClient.Core.Infrastructure.UIHelper.FindVisualChild<ScrollViewer>(listView);

			if (delta > 0)
			{
				if (scroll.HorizontalOffset + delta > 0)
				{
					scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + delta);
				}
				else
				{
					scroll.ScrollToRightEnd();
				}
			}
			else
			{
				if (scroll.ExtentWidth > scroll.HorizontalOffset + delta)
				{
					scroll.ScrollToHorizontalOffset(scroll.HorizontalOffset + delta);
				}
				else
				{
					scroll.ScrollToLeftEnd();
				}
			}
		}

		private void UIElement_OnPreviewKeyUp(object sender, KeyEventArgs e)
		{
			var aaa = Keyboard.FocusedElement;
			var mods = Keyboard.Modifiers;
			//	var isCtrl = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
			//if (aaa.IsEnabled && (aaa is TextBoxBase || aaa is PasswordBox || (aaa is ComboBox) && ((ComboBox)aaa).IsEditable))
			//{
			//	// backspace can be ignored
			//	Console.WriteLine("backspace can be ignored key :" + e.Key);
			//}
			//else
			//{
			//	Console.WriteLine("Got key :" + e.Key + ". Mods: " + mods);
			//}

			var ctrl = aaa as CacheContentControl;
			if (ctrl == null)
			{
				ctrl = UIHelper.FindVisualChild<CacheContentControl>(aaa as UIElement);
			}
			if (ctrl == null)
			{
				ctrl = UIHelper.FindAncestor<CacheContentControl>(aaa as UIElement);
			}

			if (ctrl != null)
			{
				ctrl.Focus();
				Keyboard.Focus(ctrl);
			}

			if (ctrl != null && ctrl.DataContext is ViewModelBase)
			{
				var currentVM = (ViewModelBase)ctrl.DataContext;
				currentVM.InitializeGestures();
			}
		}

		ActionBinding[] entries;
		KeyGestureConverter gestureConverter;

		private void InitKeyboardShortcuts()
		{
			gestureConverter = new KeyGestureConverter();

			try
			{
				var xdoc = XDocument.Load("KeyboardShortcuts.xml");
				entries = (from item in xdoc.Descendants("binding")
						   select new ActionBinding
							   {
								   Name = (GestureActionName)Enum.Parse(typeof(GestureActionName), (string)item.Attribute("action")),
								   Gestures = item.Elements("gesture").Select(x => GetGesture(x.Value)).ToArray()
							   }
						  ).ToArray();
			}
			catch
			{
			}
		}

		private KeyGesture GetGesture(string value)
		{
			return (KeyGesture)gestureConverter.ConvertFrom(value);
		}

		private string NormalizeGesture(string value)
		{
			var gesture = GetGesture(value);
			return NormalizeGesture(gesture.Modifiers, gesture.Key);
		}

		private string NormalizeGesture(ModifierKeys mods, Key key)
		{
			string result;
			if (mods == ModifierKeys.None)
				result = key.ToString();
			else
				result = mods + "+" + key;
			return result;
		}

		private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
		{
			InitKeyboardShortcuts();
			Core.Infrastructure.Common.InputBindings.Initialize(InputBindings, entries);
		}
	}
}