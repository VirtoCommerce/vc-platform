using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Bootstrapper.Main.Properties;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public class ModificationStepViewModel : WizardStepViewModelBase, IModificationStepViewModel
    {
        #region Windows Installer fields

        private readonly BootstrapperApplication _installer;
        private readonly Engine _engine;
	    private readonly IViewModelsFactory<IMainViewModel> _mainVmFactory; 

        #endregion

        #region Constructors

        public ModificationStepViewModel()
        {

        }

        public ModificationStepViewModel(
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

        #region WizardStepViewModelBase members

        public override string Description
        {
            get { return Resources.ModificationTitle; }
        }

        public override bool IsValid
        {
            get { return true; }
        }

        public override bool IsLast
        {
            get { return false; }
        }

        #endregion

        #region Windows Installer events

        private void SubscribeToInstallationEvents()
        {
            _installer.DetectComplete += OnDetectComplete;
        }

        private void OnDetectComplete(object sender, DetectCompleteEventArgs e)
        {
            if (_installer.Command.Action == LaunchAction.Uninstall)
            {
                _engine.Log(LogLevel.Verbose, Resources.InvokingAutomaticPlanForUninstall);
                Uninstall = true;
                _mainVmFactory.GetViewModelInstance().MoveNextCommand.Execute(null);
            }
        }

        #endregion

        #region IModificationStepViewModel implementation

        public bool Repair
        {
            get { return _repair; }
            set
            {
                _repair = value;
                OnPropertyChanged();
            }
        }

        private bool _repair;


        public bool Uninstall
        {
            get { return _uninstall; }
            set
            {
                _uninstall = value;
                OnPropertyChanged();
            }
        }

        private bool _uninstall = true;

        #endregion
    }
}