using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Implementations
{
	public class SystemJobEditViewModel : ViewModelDetailAndWizardBase<SystemJob>, ISystemJobEditViewModel
	{
		#region Dependencies
		private readonly INavigationManager _navManager;
		private readonly IViewModelsFactory<IAddParameterViewModel> _vmFactory;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;
		#endregion

		#region ctor

		public SystemJobEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IAddParameterViewModel> vmFactory,
									  INavigationManager navManager, SystemJob item,
									  IHomeSettingsViewModel parent)
			: base(entityFactory, item, false)
		{
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_vmFactory = vmFactory;
            ViewTitle = new ViewTitleBase() { Title = "System job", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
			CommandInit();

			if (History == null)
			{
				OnUIThread(Refresh);
			}
		}

		private void Refresh()
		{
			using (var r = _repositoryFactory.GetRepositoryInstance())
			{
				var e1 = r.TaskSchedules.Where(t => t.SystemJobId == InnerItem.SystemJobId);
				var task = e1.SingleOrDefault();
				if (task == null)
				{
					Next = null;
					OnSpecifiedPropertyChanged("Next");
				}
				else
				{
					Next = task.NextScheduledStartTime;
					OnSpecifiedPropertyChanged("Next");
				}

				var e2 =
					r.SystemJobLogEntries.Where(l => l.SystemJobId == InnerItem.SystemJobId).OrderByDescending(t => t.StartTime).Take(500);
				History = e2.ToList();
				OnSpecifiedPropertyChanged("History");
			}
		}

		protected SystemJobEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IAddParameterViewModel> vmFactory, SystemJob item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_vmFactory = vmFactory;
			CommandInit();
		}

		public DateTime? Next
		{
			get;
			set;
		}

		public DelegateCommand HistoryRefreshCommand { get; private set; }
		public List<SystemJobLogEntry> History
		{
			get;
			set;
		}

		#endregion

		public DelegateCommand SearchItemsCommand { get; private set; }

		#region ViewModelBase Members
		public override string IconSource
		{
			get
			{
				return "Icon_SystemJob";
			}
		}
		public override string DisplayName
		{
			get
			{
				return InnerItem != null ? InnerItem.Name : string.Empty;
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
				  (SolidColorBrush)Application.Current.TryFindResource("SettingsDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(OriginalItem.SystemJobId,
															Configuration.NavigationNames.HomeName,
															Configuration.NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("System job ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to System job '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as IAppConfigRepository).SystemJobs.Where(x => x.SystemJobId == OriginalItem.SystemJobId)
					.Expand(c => c.JobParameters).SingleOrDefault();

			OnUIThread(() => { InnerItem = item; });
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem.JobParameters != null)
			{
				InnerItem.JobParameters.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.JobParameters.ToList().ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem.JobParameters != null)
			{
				InnerItem.JobParameters.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.JobParameters.ToList().ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
			}
		}

		protected override void OnViewModelPropertyChangedUI(object sender, PropertyChangedEventArgs e)
		{
			base.OnViewModelPropertyChangedUI(sender, e);
			if (ItemAddCommand != null)
			{
				ItemAddCommand.RaiseCanExecuteChanged();
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsLast
		{
			get
			{
				return this is ISystemJobParametersStepViewModel;
			}
		}

		#endregion

		#region ISystemJobEditViewModel

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<JobParameter> ItemEditCommand { get; private set; }
		public DelegateCommand<JobParameter> ItemDeleteCommand { get; private set; }

		public InteractionRequest<ConditionalConfirmation> RemoveConfirmRequest { get; private set; }

		#endregion

		#region Public methods

		public void RaiseCanExecuteChanged()
		{
			ItemEditCommand.RaiseCanExecuteChanged();
			ItemDeleteCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region Command Implementation

		private void RaiseItemAddInteractionRequest()
		{
			var item = (JobParameter)EntityFactory.CreateEntityForType("JobParameter");

			if (RaiseItemEditInteractionRequest(item, "Add job parameter".Localize()))
			{
				item.SystemJobId = InnerItem.SystemJobId;
				OnUIThread(() => InnerItem.JobParameters.Add(item));
			}
		}

		private void RaiseItemEditInteractionRequest(JobParameter originalItem)
		{
			var item = originalItem.DeepClone<JobParameter>(EntityFactory as AppConfigEntityFactory);
			if (RaiseItemEditInteractionRequest(item, "Edit parameter".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
			}
		}

		private bool RaiseItemEditInteractionRequest(JobParameter item, string title)
		{
			bool result = false;
			var confirmation = new ConditionalConfirmation
				{
					Title = title,
					Content = _vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item))
				};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseItemRemoveInteractionRequest(JobParameter originalItem)
		{
			InnerItem.JobParameters.Remove(originalItem);
		}

		#endregion

		#region Private methods

		private void CommandInit()
		{
			HistoryRefreshCommand = new DelegateCommand(Refresh);
			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest);
			ItemEditCommand = new DelegateCommand<JobParameter>(RaiseItemEditInteractionRequest, x => x != null);
			ItemDeleteCommand = new DelegateCommand<JobParameter>(RaiseItemRemoveInteractionRequest, x => x != null);
		}

		#endregion



	}
}
