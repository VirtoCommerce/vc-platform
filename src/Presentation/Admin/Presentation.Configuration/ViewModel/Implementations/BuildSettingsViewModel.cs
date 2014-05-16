using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Search.Model;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Configuration.ViewModel.Implementations
{
	public class BuildSettingsViewModel : ViewModelBase, IBuildSettingsViewModel, ISupportDelayInitialization
	{
		#region Dependencies
		private readonly IBuildSettingsRepository _repository;
		#endregion

		#region Fields
		public Timer _syncTimer;
		#endregion

		#region ctor
		public BuildSettingsViewModel(IBuildSettingsRepository repository)
		{
			_repository = repository;

			ItemRebuildCommand = new DelegateCommand<BuildSettings>(RaiseItemRebuildInteractionRequest, x => x != null);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}
		#endregion

		#region IBuildSettingsViewModel

		public DelegateCommand<BuildSettings> ItemRebuildCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		private ObservableCollection<BuildSettings> _items;
		public ObservableCollection<BuildSettings> Items
		{
			get
			{
				return _items;
			}
			private set
			{
				_items = value;
				OnPropertyChanged();
			}
		}

		private bool _isActive;
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (value)
				{
					Start(10);
					_isActive = true;
				}
				else
				{
					Stop();
					_isActive = false;
				}
			}
		}
		#endregion

		#region Commands implementation

		private void RaiseItemRebuildInteractionRequest(BuildSettings item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to rebuild index \'{0}\'?".Localize(), item.DocumentType),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			if (IsActive)
				Stop();
			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					item.Status = System.Convert.ToInt32(BuildStatus.NeverStarted);
					_repository.UnitOfWork.Commit();
				}
			});
			if (IsActive)
				Start(10);

		}

		public void RaiseCanExecuteChanged()
		{
			ItemRebuildCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region ISupportDelayInitialization

		public void InitializeForOpen()
		{
			OnUIThread(async () =>
				{
					ShowLoadingAnimation = true;
					var items = await Task.Run(() => _repository.BuildSettings);

					Items = new ObservableCollection<BuildSettings>(items);
					ShowLoadingAnimation = false;
				});
		}

		#endregion

		#region Private members
		private void SyncTimerTick(object sender)
		{
			InitializeForOpen();
		}

		/// <summary>
		/// starts new timer
		/// </summary>
		/// <param name="PeriodInSeconds">period in seconds to execute handler</param>
		private void Start(int PeriodInSeconds)
		{
			_syncTimer = new Timer(SyncTimerTick, null, 0, PeriodInSeconds * 1000);
		}
		private void Stop()
		{
			_syncTimer.Dispose();
		}

		#endregion
	}
}
