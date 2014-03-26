using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels
{
	public sealed class ConfigurationWizardViewModel : WizardViewModelBare, IConfigurationWizardViewModel, ISupportWizardSave
	{
		#region Dependencies
		private readonly IProjectLocationStepViewModel _projectLocationStepViewModel;
		private readonly IDatabaseSettingsStepViewModel _databaseSettingsStepViewModel;
		private readonly ISearchSettingsStepViewModel _searchSettingsStepViewModel;
		private readonly IConfigurationViewModel _configurationViewModel;
		private readonly IConfirmationStepViewModel _confirmationViewModel;
		private readonly NavigationManager _navigationManager;
		private readonly IRepositoryFactory<IProjectRepository> _projectRepositoryFactory;
		private readonly Project _item;
		#endregion

		#region Constructors

		public ConfigurationWizardViewModel(
			Project item,
			IRepositoryFactory<IProjectRepository> projectRepositoryFactory,
			IViewModelsFactory<IProjectLocationStepViewModel> projectLocationStepVmFactory,
			IViewModelsFactory<IDatabaseSettingsStepViewModel> databaseSettingsStepVmFactory,
			IViewModelsFactory<ISearchSettingsStepViewModel> searchSettingsStepVmFactory,
			IViewModelsFactory<IConfigurationViewModel> configurationVmFactory,
			IViewModelsFactory<IConfirmationStepViewModel> confirmationStepVmFactory,
			NavigationManager navigationManager
			)
		{
			_item = item;
			_projectRepositoryFactory = projectRepositoryFactory;
			_navigationManager = navigationManager;

			// initializing step instances
			_confirmationViewModel = confirmationStepVmFactory.GetViewModelInstance();
			var confirmParam = new KeyValuePair<string, object>("confirmationViewModel", _confirmationViewModel);

			_searchSettingsStepViewModel = searchSettingsStepVmFactory.GetViewModelInstance(confirmParam);
			var searchParam = new KeyValuePair<string, object>("searchViewModel", _searchSettingsStepViewModel);

			_databaseSettingsStepViewModel = databaseSettingsStepVmFactory.GetViewModelInstance(confirmParam);
			var dbParam = new KeyValuePair<string, object>("databaseViewModel", _databaseSettingsStepViewModel);

			_projectLocationStepViewModel = projectLocationStepVmFactory.GetViewModelInstance(confirmParam, searchParam, dbParam);
			var locationParam = new KeyValuePair<string, object>("projectLocationViewModel", _projectLocationStepViewModel);

			_configurationViewModel = configurationVmFactory.GetViewModelInstance(locationParam, dbParam, searchParam);

			Initialize();
			RegisterWizardSteps();
		}

		private void Initialize()
		{
			CancelCommand = new DelegateCommand<object>(x => Cancel(), x => true);
			CancelConfirmRequest = new InteractionRequest<Confirmation>();
		}

		#endregion

		#region Private members

		private void RegisterWizardSteps()
		{
#if DESIGN // TODO: Replace with Debug compilation condition and IsInDesignMode runtime condition
			if (IsInDesignMode)
			{
				RegisterStep(new ProjectLocationStepViewModel());
				RegisterStep(new DatabaseSettingsStepViewModel());
				RegisterStep(new SearchSettingsStepViewModel());
				RegisterStep(new ConfirmationStepViewModel());
				return;
			}
#endif
			RegisterStep(_projectLocationStepViewModel);
			RegisterStep(_databaseSettingsStepViewModel);
			RegisterStep(_searchSettingsStepViewModel);
			RegisterStep(_confirmationViewModel);
		}

		#endregion

		#region IMainViewModel implementation

		public DelegateCommand<object> CancelCommand { get; private set; }

		public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }

		private void Cancel()
		{
			var confirmation = new ConditionalConfirmation { Content = Resources.ConfirmCancellation, Title = Resources.Cancellation };
			CancelConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					// ERROR_CANCELLED
					// OnUIThread(() => Application.Current.Shutdown(0x4C7));
				}
			});
		}

		public bool PrepareAndSave()
		{
			var result = false;
			var allErrors = string.Empty;

			var tokenSource = new CancellationTokenSource();
			var token = tokenSource.Token;
			_configurationViewModel.CancellationSource = tokenSource;
			var configurationNavigationItem = new NavigationItem(NavigationNames.ConfigurationName, _configurationViewModel);
			// UnRegister previous, if any
			OnUIThread(() => _navigationManager.UnRegisterNavigationItem(configurationNavigationItem));
			_navigationManager.Navigate(configurationNavigationItem);

			// start configuring in tasks
			var taskLocation = Task.Factory.StartNew(() => _projectLocationStepViewModel.Configure(token), token);
			taskLocation.ContinueWith(x => tokenSource.Cancel(), TaskContinuationOptions.OnlyOnFaulted);

			var taskDbAndSearch = Task.Factory.StartNew(() =>
			{
				_databaseSettingsStepViewModel.Configure(token);
				_searchSettingsStepViewModel.Configure(token);
			}, token);
			taskDbAndSearch.ContinueWith(x => tokenSource.Cancel(), TaskContinuationOptions.OnlyOnFaulted);

			try
			{
				Task.WaitAll(new[] { taskLocation, taskDbAndSearch });
			}
			catch (AggregateException e)
			{
				allErrors += GetMessages(e);
			}

			// cancel all steps
			if (tokenSource.IsCancellationRequested)
			{
				var cancelTasks = new[]
					{
						Task.Factory.StartNew(() => _projectLocationStepViewModel.Cancel()),
						Task.Factory.StartNew(() => _databaseSettingsStepViewModel.Cancel()),
						Task.Factory.StartNew(() => _searchSettingsStepViewModel.Cancel())
					};

				try
				{
					Task.WaitAll(cancelTasks);
				}
				catch (AggregateException e)
				{
					allErrors += GetMessages(e);
				}
			}

			if (_projectLocationStepViewModel.Result == OperationResult.Failed ||
				_databaseSettingsStepViewModel.Result == OperationResult.Failed ||
				_searchSettingsStepViewModel.Result == OperationResult.Failed)
			{
				_configurationViewModel.Result = OperationResult.Failed;
			}
			else if (_projectLocationStepViewModel.Result == OperationResult.Cancelled ||
					 _databaseSettingsStepViewModel.Result == OperationResult.Cancelled ||
					 _searchSettingsStepViewModel.Result == OperationResult.Cancelled)
			{
				_configurationViewModel.Result = OperationResult.Cancelled;
			}

			if (_configurationViewModel.Result == OperationResult.Cancelled || _configurationViewModel.Result == OperationResult.Failed)
			{
				allErrors += _projectLocationStepViewModel.Message + Environment.NewLine +
							_databaseSettingsStepViewModel.Message + Environment.NewLine +
							_searchSettingsStepViewModel.Message;
			}

			if (!string.IsNullOrEmpty(allErrors))
			{
				_configurationViewModel.Message = allErrors;
			}
			else
			{
				// create item
				_item.BrowseUrl = "Click browse to resolve";
				_item.Name = _projectLocationStepViewModel.ProjectName;
				_item.Location.Type = LocationType.FileSystem;
				_item.Location.Url = _projectLocationStepViewModel.ProjectLocation;
				_item.Location.LocalPath = _projectLocationStepViewModel.ProjectLocation;

				var repo = _projectRepositoryFactory.GetRepositoryInstance();
				repo.Add(_item);
				repo.UnitOfWork.Commit();

				_configurationViewModel.Result = OperationResult.Successful;
				result = true;
			}

			return result;
		}

		private string GetMessages(AggregateException e)
		{
			// Display information about each exception.
#if (DEBUG)
			foreach (var v in e.InnerExceptions)
			{
				if (v is TaskCanceledException)
					Console.WriteLine("   TaskCanceledException: Task {0}", ((TaskCanceledException)v).Task.Id);
				else
					Console.WriteLine("   Exception: {0}", v.GetType().Name);
			}
#endif

			var result = string.Empty;
			foreach (var v in e.InnerExceptions)
			{
				if (!(v is TaskCanceledException))
				{
					result += v + Environment.NewLine;
				}
			}

			return result;
		}
	}

		#endregion
}