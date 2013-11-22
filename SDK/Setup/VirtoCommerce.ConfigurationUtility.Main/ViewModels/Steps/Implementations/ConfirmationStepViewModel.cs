using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ConfigurationUtility.Main.Properties;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Implementations
{
	public class ConfirmationStepViewModel : WizardStepViewModelBase, IConfirmationStepViewModel
	{
		#region Constructors
#if DESIGN
        public ConfirmationStepViewModel()
        {
            if (IsInDesignMode)
            {
                ProjectLocation = @"C:\Users\Administrator\Documents\Visual Studio 2012\Projects\FontEnd";
                SqlServerAuthentication = @"Windows Authentication";
                DatabaseConnectionString = @"Data Source=.\MSSQLSERVER;Initial Catalog=VirtoCommerceDB;Persist Security Info=True;MultipleActiveResultSets=True";
                ElasticSearchIndexesLocation = @"http://localhost:9200/indexes";
            }
        }
#endif
		#endregion

		#region Overrides of WizardStepViewModelBase

		public override string Description
		{
			get { return Resources.Confirmation; }
		}

		public override bool IsValid
		{
			get { return true; }
		}

		public override bool IsLast
		{
			get { return true; }
		}

		#endregion

		#region Implementation of IConfirmationStepViewModel

		public string ProjectLocation
		{
			get { return _projectLocation; }
			set
			{
				_projectLocation = value;
				OnPropertyChanged();
			}
		}

		private string _projectLocation;

		public string SqlServerAuthentication
		{
			get { return _sqlServerAuthentication; }
			set
			{
				_sqlServerAuthentication = value;
				OnPropertyChanged();
			}
		}

		private string _sqlServerAuthentication;

		public string DatabaseConnectionString
		{
			get { return _databaseConnectionString; }
			set
			{
				_databaseConnectionString = value;
				OnPropertyChanged();
			}
		}

		private string _databaseConnectionString;

		public SearchConnection SearchConnection
		{
			get { return _searchConnection; }
			set
			{
				_searchConnection = value;
				OnPropertyChanged();
			}
		}

		private SearchConnection _searchConnection;


		#endregion

	}
}