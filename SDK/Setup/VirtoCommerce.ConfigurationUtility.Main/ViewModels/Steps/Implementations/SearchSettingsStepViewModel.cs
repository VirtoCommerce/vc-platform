using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.PowerShell.SearchSetup.Cmdlet;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Implementations
{
    public class SearchSettingsStepViewModel : WizardStepViewModelBase, ISearchSettingsStepViewModel
    {
        #region const
        public string ScopeDescription
        {
            get
            {
                return
                    "Search scope allows to narrow searches based on the content sources of items on the portal.\r\nThe scope is set by name. By default each project has it's unique scope defined by the project name but can be modified in this step.";
            }
        }
        private const string SampleScope = "SampleProject";
        private const string SearchLocation = "http://localhost:9200";
        #endregion

        #region Dependencies
        private readonly IConfirmationStepViewModel _confirmationViewModel;
        #endregion

        #region Constructors

#if DESIGN
        public SearchSettingsStepViewModel()
        {
            if (IsInDesignMode)
            {
                IndexesLocation = "http://localhost:9200/"
            }
        }
#endif

        public SearchSettingsStepViewModel(IConfirmationStepViewModel confirmationViewModel)
        {
            _confirmationViewModel = confirmationViewModel;

            Initialize();
        }

        private void Initialize()
        {
            IsInitializing = true;
            IndexScope = SampleScope;
            IndexesLocation = SearchLocation;
            IsSearchProviderElastic = true;
            LuceneFolderLocation = "~/app_data/Virto/search";
            IsInitializing = false;
        }

        #endregion

        #region Overrides of WizardStepViewModelBase

        public override string Description
        {
            get { return Resources.SearchSettings; }
        }

        public override bool IsValid
        {
            get { return ValidateIndexesLocation() && string.IsNullOrEmpty(ValidateSearchScope()); }
        }

        public override bool IsLast
        {
            get { return false; }
        }

        #endregion

        #region Implementation of IConfigureStep

        public string Action
        {
            get { return Resources.SearchSettingsAction; }
        }

        public string Message { get; private set; }

        public void Configure(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            Result = null; // sets result to InProgress
            try
            {
                var searchConnection = _confirmationViewModel.SearchConnection;
                // Update Search index

                // handle special case for lucene, resolve relative path to the actual folder
                if (searchConnection.Provider.Equals(
                    "lucene", StringComparison.OrdinalIgnoreCase) && searchConnection.DataSource.StartsWith("~/"))
                {
                    var dataSource = searchConnection.DataSource.Replace(
                        "~/", _confirmationViewModel.ProjectLocation + "\\");

                    searchConnection = new SearchConnection(dataSource, searchConnection.Scope, searchConnection.Provider);
                }

                new UpdateSearchIndex().Index(searchConnection, _confirmationViewModel.DatabaseConnectionString, null, true);
                ct.ThrowIfCancellationRequested();

                Result = OperationResult.Successful;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception e)
            {
#if (DEBUG)
                MessageBox.Show(e.StackTrace);
#endif

                Message = string.Format("{0} {1}: {2}", Resources.SearchSettingsAction, Resources.Failed, e.ExpandExceptionMessage());
                Result = OperationResult.Failed;
                throw;
            }
        }

        public void Cancel()
        {
            Result = OperationResult.Cancelling;

            // TODO: implement cancellation
            Result = OperationResult.Cancelled;
        }

        public OperationResult? Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                OnUIThread(() => _result.UpdateState(Configuration, Resources.SearchSettingsAction));
            }
        }

        private OperationResult? _result;

        #endregion

        #region Implementation of ISearchSettingsStepViewModel

        public bool IsSearchProviderElastic
        {
            get { return _isSearchProviderElastic; }
            set
            {
                _isSearchProviderElastic = value;
                OnPropertyChanged();
                OnConnectionStringChanges();
            }
        }

        public bool IsSearchProviderLucene
        {
            get { return !_isSearchProviderElastic; }
            set
            {
                _isSearchProviderElastic = !value;
                OnPropertyChanged();
                OnConnectionStringChanges();
            }
        }

        public string IndexesLocation
        {
            get { return _indexesLocation; }

            set
            {
                _indexesLocation = value;
                OnPropertyChanged();
                OnIsValidChanged();
                OnConnectionStringChanges();
            }
        }

        private string _indexesLocation;

        public string LuceneFolderLocation
        {
            get { return _luceneFolderLocation; }
            set
            {
                _luceneFolderLocation = value;
                OnPropertyChanged();
                OnConnectionStringChanges();
            }
        }

        public string IndexScope
        {
            get
            {
                return _indexScope;
            }

            set
            {
                _indexScope = value;
                if (!string.IsNullOrEmpty(_indexScope))
                    _indexScope = _indexScope.ToLower();
                OnPropertyChanged();
                OnIsValidChanged();
                OnConnectionStringChanges();
            }
        }

        private void OnConnectionStringChanges()
        {
            SearchConnection searchConnection;
            if (IsSearchProviderLucene)
            {
                searchConnection = new SearchConnection(LuceneFolderLocation, IndexScope, "lucene");
            }
            else
            {
                searchConnection = new SearchConnection(IndexesLocation, IndexScope);
            }

            _confirmationViewModel.SearchConnection = searchConnection;
        }

        private string _indexScope;
        private bool _isSearchProviderElastic;
        private string _luceneFolderLocation;

        public IConfigurationViewModel Configuration { get; set; }

        #endregion

        #region Implementation of IDataErrorInfo

        public string this[string name]
        {
            get
            {
                string result = null;

                if (name == "IndexesLocation")
                {
                    if (!ValidateIndexesLocation())
                    {
                        result = Resources.InvalidElasticSearchIndexesLocationUrl;
                    }
                }
                else if (name == "IndexScope")
                {
                    result = ValidateSearchScope();
                }

                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }

        #endregion

        readonly static char[] invalidNameCharactersVisible = @" ~`!@#$%^&*()+=/?[]{}.,\|".ToArray();
        readonly static char[] invalidCharactersAll = Path.GetInvalidPathChars().Union(invalidNameCharactersVisible).ToArray();

        private bool ValidateIndexesLocation()
        {
            return Uri.IsWellFormedUriString(IndexesLocation, UriKind.Absolute);
        }

        private string ValidateSearchScope()
        {
            string result = null;
            if (IndexScope.IndexOfAny(invalidCharactersAll) > -1)
            {
                result = string.Format(Resources.InvalidSearchScope, string.Join(" ", invalidNameCharactersVisible));
            }

            return result;
        }

    }
}