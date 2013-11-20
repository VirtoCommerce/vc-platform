using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels
{
	public sealed class ConfigurationViewModel : ViewModelBase, IConfigurationViewModel
	{
		#region Dependencies
		private readonly IProjectLocationStepViewModel _projectLocationStepViewModel;
		private readonly IDatabaseSettingsStepViewModel _databaseSettingsStepViewModel;
		private readonly ISearchSettingsStepViewModel _searchSettingsStepViewModel;
		private readonly NavigationManager _navigationManager;
		#endregion

#if DESIGN // TODO: Replace with Debug compilation condition and IsInDesignMode runtime condition
		public ConfigurationViewModel()
		{
			Initialize();
			if (IsInDesignMode)
			{
				Steps.Add(new KeyValuePair<string, string>(Resources.ProjectLocationAction, Resources.Completed));
				Steps.Add(new KeyValuePair<string, string>(Resources.DatabaseSettingsAction, Resources.InProgress));
				Steps.Add(new KeyValuePair<string, string>(Resources.SearchSettingsAction, Resources.Pending));
			}
		}
#endif
		#region Constructor
		public ConfigurationViewModel(
			IProjectLocationStepViewModel projectLocationViewModel,
			IDatabaseSettingsStepViewModel databaseViewModel,
			ISearchSettingsStepViewModel searchViewModel,
			NavigationManager navigationManager)
		{
			_projectLocationStepViewModel = projectLocationViewModel;
			_projectLocationStepViewModel.Configuration = this;
			_databaseSettingsStepViewModel = databaseViewModel;
			_databaseSettingsStepViewModel.Configuration = this;
			_searchSettingsStepViewModel = searchViewModel;
			_searchSettingsStepViewModel.Configuration = this;

			_navigationManager = navigationManager;

			Initialize();
		}

		private void Initialize()
		{
			CancelCommand = new DelegateCommand<object>(x => Cancel(), x => !_cancelled || IsValid);
			CancelConfirmRequest = new InteractionRequest<Confirmation>();

			InitStepsAndViewTitle();
		}

		private void InitStepsAndViewTitle()
		{
			ViewTitle = new ViewTitleBase { Title = Resources.InProgress };

			Steps = new ObservableCollection<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>(Resources.ProjectLocationAction, Resources.Pending),
					new KeyValuePair<string, string>(Resources.DatabaseSettingsAction, Resources.Pending),
					new KeyValuePair<string, string>(Resources.SearchSettingsAction, Resources.Pending)
				};
		}

		#endregion

		#region IConfigurationViewModel implementation

		public ObservableCollection<KeyValuePair<string, string>> Steps { get; private set; }

		public bool IsValid
		{
			get
			{
				return _isValid;
			}
			set
			{
				_isValid = value;
				OnPropertyChanged();
				CancelCommand.RaiseCanExecuteChanged();
			}
		}

		private bool _isValid;

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
				IsValid = true;
				switch (_result)
				{
					case OperationResult.Successful:
						ViewTitle.Title = Resources.Successful;
						break;
					case OperationResult.Cancelling:
						ViewTitle.Title = Resources.Cancelling;
						break;
					case OperationResult.Cancelled:
						ViewTitle.Title = Resources.Cancelled;
						break;
					case OperationResult.Failed:
						ViewTitle.Title = Resources.Failed;
						break;
				}
			}
		}

		private OperationResult _result;

		public DelegateCommand<object> CancelCommand { get; private set; }
		public CancellationTokenSource CancellationSource { private get; set; }

		public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }

		private void Cancel()
		{
			if (IsValid)
				return;

			var confirmation = new ConditionalConfirmation { Content = Resources.ConfirmCancellation, Title = Resources.Cancellation };
			CancelConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					CancellationSource.Cancel();

					Result = OperationResult.Cancelling;
					_cancelled = true;
					IsValid = true;
					CancelCommand.RaiseCanExecuteChanged();
				}
			});
		}

		private bool _cancelled;

		public void Finish()
		{
			_navigationManager.NavigateByName(NavigationNames.ProjectsName);
		}

		#endregion

	}
}