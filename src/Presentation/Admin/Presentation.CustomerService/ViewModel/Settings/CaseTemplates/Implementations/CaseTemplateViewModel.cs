using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Implementations
{
	public class CaseTemplateViewModel : ViewModelDetailAndWizardBase<CaseTemplate>, ICaseTemplateViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ICaseTemplatePropertyViewModel> _templatePropertyVmFactory;

		#endregion

		#region Constructor

		public CaseTemplateViewModel(
			IViewModelsFactory<ICaseTemplatePropertyViewModel> templatePropertyVmFactory,
			IRepositoryFactory<ICustomerRepository> repositoryFactory,
			ICustomerEntityFactory entityFactory,
			IHomeSettingsViewModel parent,
			INavigationManager navManager, CaseTemplate item)
			: base(entityFactory, item, false)
		{
			ViewTitle = new ViewTitleBase()
				{
					SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name : "",
                    Title = "CASE TYPES"
				};
			_templatePropertyVmFactory = templatePropertyVmFactory;
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			InitCommands();
		}


		protected CaseTemplateViewModel(
			IViewModelsFactory<ICaseTemplatePropertyViewModel> templatePropertyVmFactory,
			IRepositoryFactory<ICustomerRepository> repositoryFactory,
			ICustomerEntityFactory entityFactory,
			CaseTemplate item)
			: base(entityFactory, item, true)
		{
			_templatePropertyVmFactory = templatePropertyVmFactory;
			_repositoryFactory = repositoryFactory;
			InitCommands();
		}

		#endregion

		#region ViewModelBase overrides

		public override string DisplayName
		{
			get { return (InnerItem == null) ? string.Empty : InnerItem.Name; }
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CaseTemplateId),
															Configuration.NavigationNames.HomeName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Case channel ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Case channel '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as ICustomerRepository).CaseTemplates.Where(
					x => x.CaseTemplateId == OriginalItem.CaseTemplateId)
					.ExpandAll()
					.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem != null && InnerItem.CaseTemplateProperties != null)
			{
				InnerItem.CaseTemplateProperties.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.CaseTemplateProperties.ToList()
					.ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem != null && InnerItem.CaseTemplateProperties != null)
			{
				InnerItem.CaseTemplateProperties.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.CaseTemplateProperties.ToList()
					.ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
			}
		}

		protected override void AfterSaveChangesUI()
		{

			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsLast
		{
			get { return true; }
		}

		public override string Description
		{
			get { return "Enter Case channels details".Localize(); }
		}



		#endregion

		#region ICaseTemplateViewModel members

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<CaseTemplateProperty> ItemEditCommand { get; private set; }
		public DelegateCommand<CaseTemplateProperty> ItemDeleteCommand { get; private set; }

		#endregion

		#region CommandImplementation

		private void RaiseItemAddInteractionRequest()
		{
			var item = new CaseTemplateProperty();
			if (RaiseItemEditInteractionRequest(item, "Create Case Channels Property".Localize()))
			{
				item.CaseTemplateId = InnerItem.CaseTemplateId;
				InnerItem.CaseTemplateProperties.Add(item);
			}
		}

		private void RaiseItemEditInteractionRequest(CaseTemplateProperty originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			if (RaiseItemEditInteractionRequest(item, "Edit Case Channels Property".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
			}
		}

		private bool RaiseItemEditInteractionRequest(CaseTemplateProperty item, string title)
		{
			var result = false;

			var itemVM = _templatePropertyVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
			var confirmation = new ConditionalConfirmation(item.Validate) { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseItemDeleteInteractionRequest(CaseTemplateProperty item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Case Channels Property '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					using (var repository = _repositoryFactory.GetRepositoryInstance())
					{
						if (!IsWizardMode)
						{
							repository.Attach(item);
							repository.Remove(item);
						}
						InnerItem.CaseTemplateProperties.Remove(item);
					}
				}
			});
		}

		#endregion

		#region private members

		private void InitCommands()
		{
			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest);
			ItemEditCommand = new DelegateCommand<CaseTemplateProperty>(RaiseItemEditInteractionRequest, x => x != null);
			ItemDeleteCommand = new DelegateCommand<CaseTemplateProperty>(RaiseItemDeleteInteractionRequest, x => x != null);
		}

		#endregion
	}
}
