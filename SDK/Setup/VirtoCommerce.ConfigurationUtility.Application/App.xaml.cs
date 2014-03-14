using System;
using System.Windows;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ConfigurationUtility.Application
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			Dispatcher.UnhandledException += OnDispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			base.OnStartup(e);
            SqlDbConfiguration.Register();
			var bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}

		


		private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			ShowErrDialog(e.Exception);
			e.Handled = true;
		}


		private static void ShowErrDialog(Exception e)
		{
			App.Current.Dispatcher.Invoke(new Action(() =>
			{
				var dlg = new ErrorDialog
				{
					StackTrace = e.StackTrace,
					Message = string.Format("An unhandled exception occurred: {0}", e.Message),
					ClipboardText = e.ToString(),
					Owner = App.Current.MainWindow
				};
				dlg.ShowDialog();
			}));

		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			ShowErrDialog((Exception)args.ExceptionObject);
		}

	}
}
