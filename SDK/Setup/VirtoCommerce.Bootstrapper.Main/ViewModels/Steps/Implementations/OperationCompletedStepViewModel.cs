using System;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Bootstrapper.Main.Infrastructure;
using VirtoCommerce.Bootstrapper.Main.Infrastructure.Native;
using VirtoCommerce.Bootstrapper.Main.Properties;
using WIXResult = Microsoft.Tools.WindowsInstallerXml.Bootstrapper.Result;
using System.Diagnostics;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public class OperationCompletedStepViewModel : WizardStepViewModelBase, IOperationCompletedStepViewModel
    {
        #region Dependencies

        private readonly BootstrapperApplication _installer;
        private readonly Engine _engine;
	    private readonly IViewModelsFactory<IMainViewModel> _mainVmFactory;

        #endregion

        #region Constructors

        public OperationCompletedStepViewModel()
        {
            LaunchConfigurationWizard = true;   
        }

        public OperationCompletedStepViewModel(
			BootstrapperApplication installer,
			Engine engine,
			IViewModelsFactory<IMainViewModel> mainVmFactory)
            : this()
        {
            _installer = installer;
            _engine = engine;
	        _mainVmFactory = mainVmFactory;

            SubscribeToInstallationEvents();
        }

        #endregion

		#region Links
		public string TroubleshootUri
		{
			get
			{
				return "http://docs.virtocommerce.com/x/yQBq";
			}
		}

		public string SDK_LogsUri
		{
			get
			{
				return _engine.StringVariables["WixBundleLog"];
			}
		}

		public string JRE_LogsUri
		{
			get
			{
				string retVal;
				try
				{
					retVal = _engine.StringVariables["WixBundleLog_JRE7"];
				}
				catch
				{
					retVal = string.Empty;
				}
				return retVal;
			}
		}

		public string VirtoCommerce_LogsUri
		{
			get
			{
				return _engine.StringVariables["WixBundleLog_VirtoCommerce"];
			}
		}

		public string Search_LogsUri
		{
			get
			{
				return _engine.StringVariables["WixBundleLog_VirtoCommerceSearch"];
			}
		}

		

		#endregion

		#region Windows Installer events

		private void SubscribeToInstallationEvents()
        {
            _installer.DetectComplete += OnDetectComplete;
            _installer.PlanComplete += OnPlanComplete;
            _installer.ApplyComplete += OnApplyComplete;
            _installer.Shutdown += OnShutdown;
        }

        private bool Reboot { get; set; }

        private void OnDetectComplete(object sender, DetectCompleteEventArgs e)
        {
            switch (Result)
            {
                case OperationResult.Downgrade:
                    _engine.Log(LogLevel.Error, Resources.Downgrade);
                    if (_installer.Command.Display != Display.Full)
                    {
                        _mainVmFactory.GetViewModelInstance().Shutdown((int)SystemErrorCodes.ERROR_INSTALL_PACKAGE_DOWNGRADE);
                    }
                    break;
                case OperationResult.Cancelled:
                    Cancel();
                    break;
                case OperationResult.Failed:
                    Fail();
                    break;
            }
        }

        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            switch (Result)
            {
                case OperationResult.Cancelled:
                    Cancel();
                    break;
                case OperationResult.Failed:
                    Fail();
                    break;
            }
        }

        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            ExitCode = e.Status;

            if (_installer.Command.Display != Display.Full)
            {
                _engine.Log(LogLevel.Verbose, Resources.AutomaticallyExitForNonInteractiveMode);
                _mainVmFactory.GetViewModelInstance().Shutdown(ExitCode);
            }
            
            Reboot = e.Restart != ApplyRestart.None;
            if (!HResult.Succeeded(e.Status))
            {
                if (_mainVmFactory.GetViewModelInstance().Cancelled)
                {
                    Cancel();
                }
                else
                {
                    Fail();
                }
            }
        }

        private void OnShutdown(object sender, ShutdownEventArgs e)
        {
			//if sdk installation selected, launch wizard checked and operation is SuccessfullInstall launch the wizard
			if (InstallSdk && LaunchConfigurationWizard && Result == OperationResult.SuccessfulInstall) // launch wizard
            {
                var basePath = _engine.FormatString("[InstallFolder]");
                Process.Start(new ProcessStartInfo(string.Format("{0}\\SDK\\Configuration\\{1}", basePath, "ConfigurationWizard.exe")));
            }
            e.Result = Reboot ? WIXResult.Restart : WIXResult.Close;
        }

        private void Cancel()
        {
            _engine.Log(LogLevel.Error, Resources.Cancelled);
            if (_installer.Command.Display != Display.Full)
            {
                _mainVmFactory.GetViewModelInstance().Shutdown((int)SystemErrorCodes.ERROR_CANCELLED);
            }
			Result = OperationResult.Cancelled;
			OnPropertyChanged("IsLaunchConfigurationWizardAvailable");
        }

        private void Fail()
        {
            _engine.Log(LogLevel.Error, Resources.Failed);
            if (_installer.Command.Display != Display.Full)
            {
                _mainVmFactory.GetViewModelInstance().Shutdown(ExitCode);
            }
        }

        #endregion

        public override string Description
        {
            get { return string.Empty; }
        }

        public override bool IsBackTrackingDisabled
        {
            get { return true; }
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override bool IsLast
        {
            get { return true; }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _message;

        public OperationResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
				OnPropertyChanged("IsLaunchConfigurationWizardAvailable");
				OnPropertyChanged("IsSearchInstalledSuccessfully");
            }
        }

		public bool InstallSdk
		{
			get
			{
#if DESIGN
                if (IsInDesignMode)
                {
                    return true;
                }
#endif
				var install = _engine.FormatString("[InstallSdk]");
				bool installBoolean;
				return !Boolean.TryParse(install, out installBoolean) || installBoolean;
			}
		}

		public bool IsLaunchConfigurationWizardAvailable
		{
			get { return _result == OperationResult.SuccessfulInstall && InstallSdk; }
		}

		public bool IsSearchInstalledSuccessfully
		{
			get { return _result == OperationResult.SuccessfulInstall && !InstallSdk; }
		}

        private OperationResult _result;

        public int ExitCode { get; set; }
        public bool LaunchConfigurationWizard { get; set; }
    }
}