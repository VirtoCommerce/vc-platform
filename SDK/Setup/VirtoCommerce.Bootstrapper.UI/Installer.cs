using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Windows.Threading;

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

using VirtoCommerce.Bootstrapper.UI.Properties;
using System.Windows;

namespace VirtoCommerce.Bootstrapper.UI
{
    public sealed class Installer : BootstrapperApplication
    {
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private static void Debug()
        {
            Dispatcher.CurrentDispatcher.Thread.Join(TimeSpan.FromSeconds(20));
        }

        protected override void Run()
        {
            AttachExceptionLogging();
            //Debug();

            Engine.Log(LogLevel.Verbose, Resources.RunningSetup);

            var exitCode = new App(this).Run();

            Engine.Log(LogLevel.Verbose, Resources.StoppingSetup);

            Engine.Quit(exitCode);
        }

        [SecuritySafeCritical]
        private void AttachExceptionLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
            Dispatcher.CurrentDispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                Engine.Log(LogLevel.Error, string.Format("{0}: {1}", Resources.UnhandledException, exception != null ? exception.ToString() : e.ExceptionObject));
            }
            catch
            {
            }
        }

        [SecurityCritical]
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Engine.Log(LogLevel.Error, string.Format("{0}: {1}", Resources.UnhandledExceptionUI, e.Exception));
            e.Handled = true;
        }
    }
}