using System;
using System.Diagnostics;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs
{
	/// <summary>
	/// Interaction logic for ErrorDialog.xaml
	/// </summary>
	public partial class ErrorDialog : Window
	{

		public string Caption { set; get; }
		public string StackTrace { set; get; }
		public string Message { set; get; }
		public string ClipboardText { set; get; }
		public bool IsFatalError { set; get; }

		public ErrorDialog()
		{
			InitializeComponent();
			this.DataContext = this;
			if (Application.Current != null && Application.Current.MainWindow != null)
			{
				Owner = Application.Current.MainWindow;
				Width = Application.Current.MainWindow.ActualWidth;
				MaxHeight = Application.Current.MainWindow.ActualHeight - 100;
			}
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			if (IsFatalError && Application.Current.MainWindow != null)
			{
				Application.Current.Shutdown();
			}
			else
			{
				Close();
			}
		}

		private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() =>
				{
					Clipboard.Clear();
					string result = ClipboardText ?? Message ?? "" + ' ' + StackTrace ?? "";
					if (result != null)
					{
						Clipboard.SetText(result);
					}
				}));
			}
			catch
			{
			}
		}

		public static void ShowErrorDialog(string message, string stackTrace, string clipboardText, bool isFatalError, string caption = "")
		{
			if (Application.Current != null && Application.Current.Dispatcher != null)
			{
				Application.Current.Dispatcher.Invoke(() =>
					{
						var errorDialog = new ErrorDialog()
							{
								Caption = caption,
								StackTrace = stackTrace,
								ClipboardText = clipboardText,
								Message = message,
								IsFatalError = isFatalError,
							};
						if (Application.Current.MainWindow != null)
						{
							errorDialog.Owner = Application.Current.MainWindow;
							errorDialog.Width = Application.Current.MainWindow.ActualWidth;
							errorDialog.MaxHeight = Application.Current.MainWindow.ActualHeight - 100;
						}
						errorDialog.ShowDialog();
					});
			}
			else
			{
				Trace.WriteLine("ShowErrorDialog" + message);
				Console.WriteLine(message);
				Debug.WriteLine(message);
			}
		}
	}
}
