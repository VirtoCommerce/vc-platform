using System;
using System.IO;
using System.Windows;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;
using Virtoway.WPF.State;


namespace VirtoCommerce.ManagementClient
{
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			using (Stream stream = File.Open("VirtoCommerce.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				ElementStateOperations.Load(stream);
			}
			Dispatcher.UnhandledException += OnDispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;

			base.OnStartup(e);
			var bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			using (Stream stream = File.Open("VirtoCommerce.xml", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				ElementStateOperations.Save(stream);
			}
			base.OnExit(e);
		}
		private static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			ShowErrDialog(e.Exception);
			e.Handled = true;
		}

		private static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			ShowErrDialog((Exception)args.ExceptionObject);
		}

		private static void ShowErrDialog(Exception e)
		{
			ErrorDialog.ShowErrorDialog(string.Format("An unhandled exception occurred: {0}", e.Message), e.StackTrace,
			                            e.ToString(), false);
		}
	}
}
