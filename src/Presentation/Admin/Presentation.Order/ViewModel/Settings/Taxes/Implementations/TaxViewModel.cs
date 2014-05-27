using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations
{
	public class TaxViewModel : ViewModelDetailAndWizardBase<Tax>, ITaxViewModel
	{
		#region Fields

		private TaxCategory[] allAvailableTaxCategories;
		private JurisdictionGroup[] allAvailableJurisdictionGroups;

		#endregion

		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;
		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		private readonly IViewModelsFactory<IGeneralLanguagesStepViewModel> _langVmFactory;
		private readonly IViewModelsFactory<ITaxValueViewModel> _valueVmFactory;

		#endregion

		#region constructor

		public TaxViewModel(
			IRepositoryFactory<IOrderRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			IOrderEntityFactory entityFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IViewModelsFactory<ITaxValueViewModel> valueVmFactory,
			IHomeSettingsViewModel parent,
			INavigationManager navManager,
			Tax item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Tax" };
			_repositoryFactory = repositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_valueVmFactory = valueVmFactory;
			_langVmFactory = langVmFactory;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			CommandInit();

		}


		protected TaxViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory, IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IViewModelsFactory<ITaxValueViewModel> valueVmFactory, Tax item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_valueVmFactory = valueVmFactory;
			_langVmFactory = langVmFactory;
			CommandInit();
		}

		private void CommandInit()
		{
			DisableableConfirmRequest = new InteractionRequest<ConditionalConfirmation>();

			ValueAddCommand = new DelegateCommand(RaiseValueAddInteractionRequest);
			ValueEditCommand = new DelegateCommand<TaxValue>(RaiseValueEditInteractionRequest, x => x != null);
			ValueRemoveCommand = new DelegateCommand<TaxValue>(RaiseValueRemoveInteractionRequest, x => x != null);
		}

		#endregion

		#region ViewModelBase Members

		public override string DisplayName
		{
			get
			{
				return (InnerItem == null) ? string.Empty : InnerItem.Name;
			}
		}

		public override string IconSource
		{
			get
			{
				return "Icon_Taxes";
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.TaxId),
															Configuration.NavigationNames.HomeName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Tax ({0})", DisplayName); } }

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
				Content = string.Format("Save changes to Tax '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}


		protected override void LoadInnerItem()
		{
			var item = (Repository as ITaxRepository).Taxes
				.Expand(x => x.TaxLanguages)
				.Expand("TaxValues/JurisdictionGroup")
				.Where(x => x.TaxId == OriginalItem.TaxId).SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{

			LanguagesStepViewModel = _langVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("selectedLanguages", InnerItem.TaxLanguages));

			OnPropertyChanged("LanguagesStepViewModel");
			LanguagesStepViewModel.CollectionChanged = ViewModel_PropertyChanged;

			if (!IsWizardMode)
			{
				if (allAvailableTaxCategories == null)
				{
					allAvailableTaxCategories = CatalogRepository.TaxCategories.OrderBy(x => x.Name).ToArray();

					allAvailableJurisdictionGroups =
						OrderRepository.JurisdictionGroups.OrderBy(x => x.DisplayName).ToArray();
				}
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				UpdateOfLanguages(LanguagesStepViewModel);
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


		protected override void SetSubscriptionUI()
		{
			if (InnerItem.TaxValues != null)
			{
				InnerItem.TaxValues.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.TaxValues.ToList().ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
			}
			if (InnerItem.TaxLanguages != null)
			{
				InnerItem.TaxLanguages.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.TaxLanguages.ToList().ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
			}
		}



		protected override void CloseSubscriptionUI()
		{
			if (InnerItem.TaxValues != null)
			{
				InnerItem.TaxValues.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.TaxValues.ToList().ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
			}
			if (InnerItem.TaxLanguages != null)
			{
				InnerItem.TaxLanguages.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.TaxLanguages.ToList().ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
			}
		}

		#endregion

		#region IWizardStep members

		public override bool IsValid
		{
			get
			{
				bool result = false;

				result = InnerItem.Validate(false) && !string.IsNullOrEmpty(InnerItem.Name);

				return result;
			}
		}

		public override bool IsLast
		{
			get { return false; }
		}

		public override string Description
		{
			get { return null; }
		}

		#endregion

		#region ITaxViewModel members

		public InteractionRequest<ConditionalConfirmation> DisableableConfirmRequest { get; private set; }

		public DelegateCommand ValueAddCommand { get; private set; }
		public DelegateCommand<TaxValue> ValueEditCommand { get; private set; }
		public DelegateCommand<TaxValue> ValueRemoveCommand { get; private set; }

		public IGeneralLanguagesStepViewModel LanguagesStepViewModel { get; set; }

		private ICatalogRepository _catalogRepository;
		private ICatalogRepository CatalogRepository  //todo is needed? Could we resolve repository from Factory just for a operation?
		{
			get
			{
				return _catalogRepository ??
					   (_catalogRepository = _catalogRepositoryFactory.GetRepositoryInstance());
			}
		}

		private IOrderRepository _orderRepository;
		private IOrderRepository OrderRepository   //todo is needed? Could we resolve repository from Factory just for a operation?
		{
			get
			{
				return _orderRepository ??
					   (_orderRepository = _repositoryFactory.GetRepositoryInstance());
			}
		}


		public void UpdateOfLanguages(ICollectionChange<GeneralLanguage> languagesVm)
		{
			var repository = Repository as ITaxRepository;
			foreach (var removedItem in languagesVm.RemovedItems)
			{
				var item = InnerItem.TaxLanguages.FirstOrDefault(x => x.TaxLanguageId == removedItem.Id);
				if (item == null)
					continue;

				InnerItem.TaxLanguages.Remove(item);
				repository.Attach(item);
				repository.Remove(item);
			}

			foreach (var updatedItem in languagesVm.UpdatedItems)
			{
				var item = InnerItem.TaxLanguages.SingleOrDefault(x => x.TaxLanguageId == updatedItem.Id);
				if (item == null)
					continue;
				item.InjectFrom(updatedItem);

			}

			foreach (var addedItem in languagesVm.AddedItems)
			{
				var item = new TaxLanguage();
				item.InjectFrom(addedItem);
				InnerItem.TaxLanguages.Add(item);
			}
			if (!IsWizardMode)
				repository.UnitOfWork.Commit();
		}

		#endregion

		#region PublicMethods members

		public void RaiseCanExecuteChanged()
		{
			ValueAddCommand.RaiseCanExecuteChanged();
			ValueEditCommand.RaiseCanExecuteChanged();
			ValueRemoveCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region Command Implementation

		private void RaiseValueAddInteractionRequest()
		{
			var item = new TaxValue();

			if (RaiseValueEditInteractionRequest(item, "Create Tax Value".Localize()))
			{
				item.TaxId = InnerItem.TaxId;
				InnerItem.TaxValues.Add(item);
			}
		}

		private void RaiseValueEditInteractionRequest(TaxValue originalItem)
		{
			TaxValue item = new TaxValue();
			item.InjectFrom<CloneInjection>(originalItem);
			if (RaiseValueEditInteractionRequest(item, "Edit Tax Value".Localize()))
			{
				// copy all values to original:
				originalItem.InjectFrom<CloneInjection>(item);
				// fake assign for UI triggers to display correct values.
				originalItem.JurisdictionGroupId = originalItem.JurisdictionGroupId;
			}
		}

		private bool RaiseValueEditInteractionRequest(TaxValue item, string title)
		{
			var result = false;
			var parameters = new List<KeyValuePair<string, object>>
				{
					new KeyValuePair<string, object>("item", item),
					new KeyValuePair<string, object>("allAvailableTaxCategories", allAvailableTaxCategories),
					new KeyValuePair<string, object>("allAvailableJurisdictionGroups", allAvailableJurisdictionGroups)
				};


			var itemVM = _valueVmFactory.GetViewModelInstance(parameters.ToArray());
			var confirmation = new ConditionalConfirmation { Title = title, Content = itemVM };
			DisableableConfirmRequest.Raise(confirmation, x =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseValueRemoveInteractionRequest(TaxValue selectedItem)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to remove this Tax Value?".Localize()),
				Title = "Remove confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
			CommonConfirmRequest.Raise(confirmation,
				(x) =>
				{
					if (x.Confirmed)
					{
						var item = InnerItem.TaxValues.SingleOrDefault(z => z.TaxValueId == selectedItem.TaxValueId);
						if (item != null)
						{
							InnerItem.TaxValues.Remove(item);
						}
					}
				});
		}

		#endregion

		#region Public Properties

		public List<TaxCategory> TaxCategories
		{
			get
			{
				return CatalogRepository.TaxCategories.ToList();
			}
		}

		#endregion
	}
}
