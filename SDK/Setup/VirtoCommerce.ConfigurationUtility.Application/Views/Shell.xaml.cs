using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;

namespace VirtoCommerce.ConfigurationUtility.Application.Views
{
    public partial class Shell
	{
		#region Constructors

		public Shell()
		{
			InitializeComponent();
			RestoreSizeAndPosition();
			Closing += OnClosing;
			
		}

		#endregion


		#region Private MEthods

		private void RestoreSizeAndPosition()
		{
			try
			{
				Top = Properties.Settings.Default.Top;
				Left = Properties.Settings.Default.Left;
				Height = Properties.Settings.Default.Height;
				Width = Properties.Settings.Default.Width;
				if (Properties.Settings.Default.Maximized)
				{
					WindowState = WindowState.Maximized;
				}
			}
			catch
			{
			}
		}

		private void SaveSizeAndPosition()
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
					Properties.Settings.Default.Maximized = true;
				}
				else
				{
					Properties.Settings.Default.Top = Top;
					Properties.Settings.Default.Left = Left;
					Properties.Settings.Default.Height = Height;
					Properties.Settings.Default.Width = Width;
					Properties.Settings.Default.Maximized = false;
				}
				Properties.Settings.Default.Save();
			}
			catch
			{
			}
		}

		#endregion

		#region Handlers

		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveSizeAndPosition();
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
			foreach (var window in App.Current.Windows.OfType<InteractionDialogBase>())
			{
				window.Close();
			}
		}

		#endregion

	}
}
