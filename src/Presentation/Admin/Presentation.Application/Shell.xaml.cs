using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Commands;
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
            InitializeComponent();
            RestoreSizeAndPosition();
            Closing += OnClosing;
        }

        #endregion

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
            foreach (var window in Application.Current.Windows.OfType<InteractionDialogBase>())
            {
                window.Close();
            }
        }

        private void RestoreSizeAndPosition()
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
                Properties.Settings.Default.Save();
            }
            catch
            {
            }
        }

        private void MainMenu_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var delta = e.Delta*-1;

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
    }
}