using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Bootstrapper.Main.Properties;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public class LayoutStepViewModel : WizardStepViewModelBase, ILayoutStepViewModel
    {
        #region Windows Installer fields

        private readonly Engine _engine;

        #endregion

        #region Constructors

        public LayoutStepViewModel()
        {
            BrowseCommand = new DelegateCommand<object>(x => Browse(), x => true);
        }

		public LayoutStepViewModel(
			Engine engine)
			: this()
        {
			_engine = engine;
        }

        #endregion

        #region WizardStepViewModelBase members

        public override string Description
        {
            get { return Resources.LayoutTitle; }
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

        #region ILayoutStepViewModel implementation

        public string LayoutDirectory
        {
            get
            {
#if DESIGN
                if (IsInDesignMode)
                {
                    return @"C:\Program Files\VirtoCommerce SDK\Setup";
                }
#endif
                if (!_engine.StringVariables.Contains("WixBundleLayoutDirectory") || string.IsNullOrWhiteSpace(_engine.StringVariables["WixBundleLayoutDirectory"]))
                {
                    _engine.StringVariables["WixBundleLayoutDirectory"] = _engine.FormatString("[InstallFolder]") + @"\Setup";
                }
                return _engine.StringVariables["WixBundleLayoutDirectory"];
            }

            set
            {
                _engine.StringVariables["WixBundleLayoutDirectory"] = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<object> BrowseCommand { get; private set; }

        private void Browse()
        {
            var browserDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                SelectedPath = LayoutDirectory
            };

            var result = browserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                LayoutDirectory = browserDialog.SelectedPath;
            }
        }

        #endregion
    }
}