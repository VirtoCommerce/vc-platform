using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Bootstrapper.Main.Properties;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public class InstallationStepViewModel : WizardStepViewModelBase, IInstallationStepViewModel
    {
        #region Windows Installer fields

        private readonly Engine _engine;

        #endregion

        #region Constructors

        public InstallationStepViewModel()
        {
            BrowseCommand = new DelegateCommand<object>(x => Browse(), x => true);
        }

        public InstallationStepViewModel(Engine engine) : this()
        {
	        _engine = engine;
        }

        #endregion

        #region WizardStepViewModelBase members

        public override string Description
        {
            get { return Resources.InstallationTitle; }
        }

        public override bool IsValid
        {
			get { return IsLicenseAccepted && (InstallSdk || InstallElasticSearch); }
        }

        public override bool IsLast
        {
            get { return false; }
        }

        #endregion

        #region IInstallationStepViewModel implementation

        public string LicenseUri
        {
            get
            {
                return "http://virtocommerce.com/licensing";
            }
        }

        public bool IsLicenseAccepted
        {
            get { return _isLicenseAccepted; }
            set
            {
                _isLicenseAccepted = value;
                OnIsValidChanged();
            }
        }

        private bool _isLicenseAccepted;

        public string InstallFolder
        {
            get
            {
#if DESIGN
                if (IsInDesignMode)
                {
                    return @"C:\Program Files\VirtoCommerce SDK";
                }
#endif
                return _engine.FormatString("[InstallFolder]");
            }

            set
            {
                _engine.StringVariables["InstallFolder"] = value;
                OnPropertyChanged();
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
            set
            {
                _engine.StringVariables["InstallSdk"] = value.ToString();
				OnPropertyChanged();
				OnIsValidChanged();
            }
        }

        public bool InstallElasticSearch
        {
            get
            {
#if DESIGN
                if (IsInDesignMode)
                {
                    return true;
                }
#endif
                var install = _engine.FormatString("[InstallElasticSearch]");
                var installBoolean = true;
                return !Boolean.TryParse(install, out installBoolean) || installBoolean;
            }
            set
            {
                _engine.StringVariables["InstallElasticSearch"] = value.ToString();
                OnPropertyChanged();
				OnIsValidChanged();
            }
        }

        public DelegateCommand<object> BrowseCommand { get; private set; }

        private void Browse()
        {
            var browserDialog = new System.Windows.Forms.FolderBrowserDialog
                                    {
                                        RootFolder = Environment.SpecialFolder.MyComputer,
                                        SelectedPath = InstallFolder
                                    };

            var result = browserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                InstallFolder = browserDialog.SelectedPath;
            }
        }

        #endregion
    }
}