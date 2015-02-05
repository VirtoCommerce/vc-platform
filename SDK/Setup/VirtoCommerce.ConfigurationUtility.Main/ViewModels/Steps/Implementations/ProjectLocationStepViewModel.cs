using Microsoft.Practices.Prism.Commands;
using Microsoft.Web.Administration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.PowerShell.FrontEndSetup;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Implementations
{
    public class ProjectLocationStepViewModel : WizardStepViewModelBase, IProjectLocationStepViewModel
    {
        #region Dependencies
        private readonly IConfirmationStepViewModel _confirmationViewModel;
        private readonly ISearchSettingsStepViewModel _searchViewModel;
        private readonly IDatabaseSettingsStepViewModel _databaseViewModel;

        #endregion

        private const string CommerceProjectName = "SampleProject";
        private const string CommerceProjectPath = "Virto Commerce 1.13\\Projects";
        #region Constructors

#if DESIGN // TODO: Replace with Debug compilation condition and IsInDesignMode runtime condition
		public ProjectLocationStepViewModel()
		{
			if (IsInDesignMode)
			{
				Initialize();
				ProjectLocation = @"C:\Users\Administrator\Documents\Visual Studio 2012\Projects\FontEnd";
			}
		}
#endif


        public ProjectLocationStepViewModel(IConfirmationStepViewModel confirmationViewModel, ISearchSettingsStepViewModel searchViewModel, IDatabaseSettingsStepViewModel databaseViewModel)
        {
            _confirmationViewModel = confirmationViewModel;
            _searchViewModel = searchViewModel;
            _databaseViewModel = databaseViewModel;

            Initialize();
        }

        private void Initialize()
        {
            ProjectPath = string.Format("{0}{1}{2}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.DirectorySeparatorChar, CommerceDirectoryName);
            ProjectName = CommerceProjectName;

            BrowseCommand = new DelegateCommand<object>(x => Browse(), x => true);
        }

        #endregion

        public string CommerceDirectoryName
        {
            get { return string.Format(CommerceProjectPath); }
        }

        #region Overrides of WizardStepViewModelBase

        public override string Description
        {
            get { return Resources.ProjectLocation; }
        }

        public override bool IsValid
        {
            get
            {
                return ValidateProjectPath() && string.IsNullOrEmpty(ValidateProjectName());
            }
        }

        public override bool IsLast
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IConfigureStep

        public string Action
        {
            get { return Resources.ProjectLocationAction; }
        }

        public string Message { get; private set; }

        public void Configure(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Result = null; // sets result to InProgress
            try
            {
                var contentFolder =
                    Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ConfigurationWizardViewModel).Assembly.Location),
                                                  @"..\Resources\FrontEnd"));

                // Copy template to project location
                DirectoryExtensions.Copy(new DirectoryInfo(contentFolder), new DirectoryInfo(ProjectLocation));
                ct.ThrowIfCancellationRequested();

                // create initial folders: import, reports
                Directory.CreateDirectory(Path.Combine(ProjectLocation, "App_Data\\Virto\\Storage\\import"));
                Directory.CreateDirectory(Path.Combine(ProjectLocation, "App_Data\\Virto\\Storage\\reports"));

                if (_databaseViewModel.InstallSamples)
                {
                    var catalogImagesFolder =
                        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ConfigurationWizardViewModel).Assembly.Location),
                                                      @"..\Resources\Catalog"));

                    // Copy test data images
                    DirectoryExtensions.Copy(new DirectoryInfo(catalogImagesFolder),
                                             new DirectoryInfo(string.Format("{0}\\App_Data\\Virto\\Storage\\Catalog", ProjectLocation)));
                    ct.ThrowIfCancellationRequested();
                }

                // Fix connection strings
                new InitializeFrontEndConfigs().Initialize(ProjectLocation, _confirmationViewModel.DatabaseConnectionString,
                                                           _confirmationViewModel.SearchConnection.ToString());

                // Create desktop shortcut

                Result = OperationResult.Successful;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
                Message = string.Format("{0} {1}: {2}", Resources.ProjectLocationAction, Resources.Failed, e.ExpandExceptionMessage());

                Result = OperationResult.Failed;
                throw;
            }
        }

        public void Cancel()
        {
            Result = OperationResult.Cancelling;

            try
            {
                if (Directory.Exists(ProjectLocation))
                {
                    Directory.Delete(ProjectLocation, true);
                }
                Result = OperationResult.Cancelled;
            }
            catch (Exception e)
            {
                Message = string.Format("{0} {1}: {2}", Resources.ProjectLocationAction, Resources.Failed, e.ExpandExceptionMessage());
                Result = OperationResult.Failed;
            }
        }

        public OperationResult? Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                OnUIThread(() => _result.UpdateState(Configuration, Resources.ProjectLocationAction));
            }
        }

        private OperationResult? _result;

        #endregion

        #region Implementation of IProjectLocationStepViewModel

        public IConfigurationViewModel Configuration { get; set; }

        public string ProjectLocation
        {
            get
            {
                return string.Format("{0}{1}{2}", ProjectPath, Path.DirectorySeparatorChar, ProjectName);
            }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                _searchViewModel.IndexScope = value;
                _databaseViewModel.DatabaseName = value;
                _confirmationViewModel.ProjectLocation = ProjectLocation;
                OnPropertyChanged();
                OnPropertyChanged("ProjectLocation");
                OnIsValidChanged();
            }
        }

        private string _projectName;

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                _projectPath = value;
                _confirmationViewModel.ProjectLocation = ProjectLocation;
                OnPropertyChanged();
                OnPropertyChanged("ProjectLocation");
                OnPropertyChanged("ProjectName");
                OnIsValidChanged();
            }
        }

        private string _projectPath;

        public DelegateCommand<object> BrowseCommand { get; private set; }

        private void Browse()
        {
            var browserDialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer,
                    SelectedPath = ProjectLocation,
                    ShowNewFolderButton = true
                };
            var result = browserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProjectPath = browserDialog.SelectedPath;
            }
        }

        #endregion

        #region Implementation of IDataErrorInfo

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "ProjectName")
                {
                    result = ValidateProjectName();
                }
                else if (name == "ProjectPath" && !ValidateProjectPath())
                {
                    result = Resources.ProjectLocationError;
                }

                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion

        readonly static char[] invalidNameCharactersVisible =
            SiteCollection.InvalidSiteNameCharacters().Union(@"~`!@#$%^&*()+=/?[]{}.,\|".ToArray()).ToArray();
        readonly static char[] invalidCharactersAll = Path.GetInvalidPathChars().Union(invalidNameCharactersVisible).ToArray();

        private string ValidateProjectName()
        {
            string result = null;
            if (ProjectName.IndexOfAny(invalidCharactersAll) > -1)
            {
                result = string.Format(Resources.InvalidProjectName, string.Join(" ", invalidNameCharactersVisible));
            }
            else if (Directory.Exists(ProjectLocation))
            {
                result = string.Format(Resources.ProjectAlreadyExists, ProjectName);
            }

            return result;
        }

        private bool ValidateProjectPath()
        {
            try
            {
                //checks if the path is valid
                new FileInfo(ProjectLocation);
                return true;
            }
            catch //Specified error could be passed according to Exception type
            {
            }
            return false;
        }
    }
}