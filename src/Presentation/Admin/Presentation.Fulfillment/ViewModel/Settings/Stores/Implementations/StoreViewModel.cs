using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations
{
	public class StoreViewModel : ViewModelDetailAndWizardBase<Store>, IStoreViewModel
	{

		#region Const
		const int SeoTabIndex = 2;
		#endregion

		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IStoreRepository> _repositoryFactory;

		private readonly IViewModelsFactory<IStoreOverviewStepViewModel> _overviewVmFactory;
		private readonly IViewModelsFactory<IStoreLocalizationStepViewModel> _localizationVmFactory;
		private readonly IViewModelsFactory<IStoreTaxesStepViewModel> _taxesVmFactory;
		private readonly IViewModelsFactory<IStorePaymentsStepViewModel> _paymentsVmFactory;
		private readonly IViewModelsFactory<IStoreNavigationStepViewModel> _navigationVmFactory;
		private readonly IViewModelsFactory<IStoreSettingStepViewModel> _settingVmFactory;
		private readonly IViewModelsFactory<IStoreLinkedStoresStepViewModel> _linkedStoresVmFactory;
		private readonly IViewModelsFactory<ISeoViewModel> _seoVmFactory;

		#endregion

		#region Constructor

		public StoreViewModel(IRepositoryFactory<IStoreRepository> repositoryFactory,
			IStoreEntityFactory entityFactory,
			IViewModelsFactory<IStoreOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<IStoreLocalizationStepViewModel> localizationVmFactory,
			IViewModelsFactory<IStoreTaxesStepViewModel> taxesVmFactory,
			IViewModelsFactory<IStorePaymentsStepViewModel> paymentsVmFactory,
			IViewModelsFactory<IStoreNavigationStepViewModel> navigationVmFactory,
			IViewModelsFactory<IStoreSettingStepViewModel> settingVmFactory,
			IViewModelsFactory<IStoreLinkedStoresStepViewModel> linkedStoresVmFactory,
			IViewModelsFactory<ISeoViewModel> seoVmFactory,
			IHomeSettingsViewModel parent,
			INavigationManager navManager, Store item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Store" };
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_overviewVmFactory = overviewVmFactory;
			_localizationVmFactory = localizationVmFactory;
			_taxesVmFactory = taxesVmFactory;
			_paymentsVmFactory = paymentsVmFactory;
			_navigationVmFactory = navigationVmFactory;
			_settingVmFactory = settingVmFactory;
			_linkedStoresVmFactory = linkedStoresVmFactory;
			_seoVmFactory = seoVmFactory;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		protected StoreViewModel(IRepositoryFactory<IStoreRepository> repositoryFactory, IStoreEntityFactory entityFactory, Store item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region Properties

		private int _selectedTabIndex;
		public int SelectedTabIndex
		{
			get { return _selectedTabIndex; }
			protected set { _selectedTabIndex = value; OnPropertyChanged(); }
		}

		#endregion

		#region ViewModelBase members

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
				return "Icon_Store";
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.StoreId),
															Configuration.NavigationNames.HomeName, Configuration.NavigationNames.MenuName,
															this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase

		public override string ExceptionContextIdentity { get { return string.Format("Store ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			if (SeoStepViewModel != null)
			{
				var isSeoValid = SeoStepViewModel.IsValid;
				if (!isSeoValid)
					SelectedTabIndex = SeoTabIndex;
				return InnerItem.Validate() && isSeoValid;
			}

			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Store '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}


		protected override void LoadInnerItem()
		{
			var item = (Repository as IStoreRepository).Stores.Where(x => x.StoreId == OriginalItem.StoreId)
				   .ExpandAll()
				   .Expand("LinkedStores/LinkedStore")
				   .SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{
			if (!IsWizardMode)
			{
				InitOverviewStep();
				InitLocalizationStep();
				InitTaxesStep();
				InitPaymentsStep();
				InitLinkedStep();
				InitSettingsStep();
				InitNavigationStep();
				InitSeoStep();
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				UpdateLinkedStoreList(Repository as IStoreRepository);
				UpdateLanguagesList(Repository as IStoreRepository);
				UpdateCurrenciesList(Repository as IStoreRepository);
				UpdateSettingsList(Repository as IStoreRepository);

				UpdateTaxCodeList(Repository as IStoreRepository);
				UpdateTaxJurisdictionsList(Repository as IStoreRepository);

				UpdateCardTypesList(Repository as IStoreRepository);
				UpdatePaymentgatewaysList(Repository as IStoreRepository);
			}
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}

			if (SeoStepViewModel != null)
				SeoStepViewModel.SaveSeoKeywordsChanges();
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem != null)
			{
				InnerItem.PropertyChanged += ViewModel_PropertyChanged;
				InnerItem.Languages.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.Currencies.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.TaxJurisdictions.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.TaxCodes.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.PaymentGateways.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.CardTypes.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.LinkedStores.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.Settings.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.Settings.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
			}

			if (TaxesStepViewModel != null)
			{
				if (TaxesStepViewModel.AvailableTaxCodes != null)
				{
					TaxesStepViewModel.AvailableTaxCodes.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
				}
				if (TaxesStepViewModel.AvailableTaxJurisdictions != null)
				{
					TaxesStepViewModel.AvailableTaxJurisdictions.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
				}
			}

			if (PaymentsStepViewModel != null)
			{
				if (PaymentsStepViewModel.AvailableStoreCardTypes != null)
				{
					PaymentsStepViewModel.AvailableStoreCardTypes.ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
				}
				if (PaymentsStepViewModel.AvailableStorePaymentGateways != null)
				{
					PaymentsStepViewModel.AvailableStorePaymentGateways.ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
				}
			}

			if (SeoStepViewModel != null)
			{
				if (SeoStepViewModel.SeoKeywords != null)
					SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged += ViewModel_PropertyChanged);
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem != null)
			{
				InnerItem.PropertyChanged -= ViewModel_PropertyChanged;
				InnerItem.Languages.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.Currencies.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.TaxJurisdictions.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.TaxCodes.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.PaymentGateways.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.CardTypes.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.LinkedStores.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.Settings.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.Settings.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
			}

			if (TaxesStepViewModel != null)
			{
				if (TaxesStepViewModel.AvailableTaxCodes != null)
				{
					TaxesStepViewModel.AvailableTaxCodes.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
				}
				if (TaxesStepViewModel.AvailableTaxJurisdictions != null)
				{
					TaxesStepViewModel.AvailableTaxJurisdictions.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
				}
			}

			if (PaymentsStepViewModel != null)
			{
				if (PaymentsStepViewModel.AvailableStoreCardTypes != null)
				{
					PaymentsStepViewModel.AvailableStoreCardTypes.ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
				}
				if (PaymentsStepViewModel.AvailableStorePaymentGateways != null)
				{
					PaymentsStepViewModel.AvailableStorePaymentGateways.ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
				}
			}

			if (SeoStepViewModel != null)
			{
				if (SeoStepViewModel.SeoKeywords != null)
					SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged -= ViewModel_PropertyChanged);
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

		#region ICreateStoreViewModel Members

		public IStoreOverviewStepViewModel OverviewStepViewModel { get; private set; }
		public IStoreLocalizationStepViewModel LocalizationStepViewModel { get; private set; }
		public IStoreTaxesStepViewModel TaxesStepViewModel { get; private set; }
		public IStorePaymentsStepViewModel PaymentsStepViewModel { get; private set; }
		public IStoreLinkedStoresStepViewModel LinkedStoresStepViewModel { get; private set; }
		public IStoreSettingStepViewModel SettingsStepViewModel { get; private set; }
		public IStoreNavigationStepViewModel NavigationStepViewModel { get; private set; }
		public ISeoViewModel SeoStepViewModel { get; private set; }

		private bool _IsInitializingOverview;
		public bool IsInitializingOverview
		{
			get { return _IsInitializingOverview; }
			set { _IsInitializingOverview = value; OnPropertyChanged(); }
		}

		private bool _IsInitializingPayments;
		public bool IsInitializingPayments
		{
			get { return _IsInitializingPayments; }
			set { _IsInitializingPayments = value; OnPropertyChanged(); }
		}

		private bool _IsInitializingLinkedStores;
		public bool IsInitializingLinkedStores
		{
			get { return _IsInitializingLinkedStores; }
			set { _IsInitializingLinkedStores = value; OnPropertyChanged(); }
		}

		#endregion

		#region Private methods

		private void UpdateLinkedStoreList(IStoreRepository repository)
		{
			var allLinkedStoresToCurrentItem =
				repository.StoreLinkedStores.Where(sls => sls.StoreId == InnerItem.StoreId).ToList();

			foreach (var storeLinkedStore in allLinkedStoresToCurrentItem)
			{
				var itemFromInnerItem =
					InnerItem.LinkedStores.SingleOrDefault(ls => ls.StoreLinkedStoreId == storeLinkedStore.StoreLinkedStoreId);
				if (itemFromInnerItem == null)
				{
					repository.Attach(storeLinkedStore);
					repository.Remove(storeLinkedStore);
				}
			}

			if (!IsWizardMode)
				repository.UnitOfWork.Commit();

		}

		private void UpdateLanguagesList(IStoreRepository repository)
		{
			var allLanguagesToCurrentItem = repository.StoreLanguages.Where(sl => sl.StoreId == InnerItem.StoreId).ToList();
			foreach (var storeLanguage in allLanguagesToCurrentItem)
			{
				var itemFromInnerItem =
					InnerItem.Languages.SingleOrDefault(sl => sl.StoreLanguageId == storeLanguage.StoreLanguageId);
				if (itemFromInnerItem == null)
				{
					repository.Attach(storeLanguage);
					repository.Remove(storeLanguage);
				}
			}

			if (!IsWizardMode)
				repository.UnitOfWork.Commit();
		}

		private void UpdateCurrenciesList(IStoreRepository repository)
		{

			var allCurrenciesForInnerItem = repository.StoreCurrencies.Where(sc => sc.StoreId == InnerItem.StoreId).ToList();

			foreach (var storeCurrency in allCurrenciesForInnerItem)
			{
				var itemFromInnerItem =
					InnerItem.Currencies.SingleOrDefault(sc => sc.CurrencyCode == storeCurrency.CurrencyCode);
				if (itemFromInnerItem == null)
				{
					repository.Attach(storeCurrency);
					repository.Remove(storeCurrency);
				}
			}

			if (!IsWizardMode)
				repository.UnitOfWork.Commit();
		}

		private void UpdateSettingsList(IStoreRepository repository)
		{
			var allSettignsForInnerItem = repository.StoreSettings.Where(ss => ss.StoreId == InnerItem.StoreId).ToList();

			foreach (var storeSetting in allSettignsForInnerItem)
			{
				var itemInInnerItem = InnerItem.Settings.SingleOrDefault(ss => ss.StoreSettingId == storeSetting.StoreSettingId);
				if (itemInInnerItem == null)
				{
					repository.Attach(storeSetting);
					repository.Remove(storeSetting);
				}
			}

			if (!IsWizardMode)
				repository.UnitOfWork.Commit();
		}

		private void UpdateCardTypesList(IStoreRepository repository)
		{
			var itemsToDelete = PaymentsStepViewModel.AvailableStoreCardTypes.Where(x => x.IsChecked == false).ToList();
			foreach (var itemToDelete in itemsToDelete)
			{
				var itemFromDb =
					repository.StoreCardTypes.Where(
						stc =>
							stc.CardType == itemToDelete.InnerItem.CardType && stc.StoreId == itemToDelete.InnerItem.StoreId)
						.SingleOrDefault();

				if (itemFromDb != null)
				{
					repository.Remove(itemFromDb);
					InnerItem.CardTypes.Remove(itemFromDb);
				}
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();


			var itemsToAdd = PaymentsStepViewModel.AvailableStoreCardTypes.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{
				var existingItem =
					InnerItem.CardTypes.SingleOrDefault(
						x => x.CardType == itemToAdd.InnerItem.CardType && x.StoreId == itemToAdd.InnerItem.StoreId);
				if (existingItem == null)
				{
					InnerItem.CardTypes.Add(itemToAdd.InnerItem);
				}
			}
		}

		private void UpdatePaymentgatewaysList(IStoreRepository repository)
		{
			var itemsToDelete = PaymentsStepViewModel.AvailableStorePaymentGateways.Where(x => x.IsChecked == false).ToList();
			foreach (var itemToDelete in itemsToDelete)
			{
				var itemFromDb =
					repository.StorePaymentGateways.Where(
						stc =>
							stc.PaymentGateway == itemToDelete.InnerItem.PaymentGateway && stc.StoreId == itemToDelete.InnerItem.StoreId)
						.SingleOrDefault();

				if (itemFromDb != null)
				{
					repository.Remove(itemFromDb);
					InnerItem.PaymentGateways.Remove(itemFromDb);
				}
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();


			var itemsToAdd = PaymentsStepViewModel.AvailableStorePaymentGateways.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{
				var existingItem =
					InnerItem.PaymentGateways.SingleOrDefault(
						x => x.PaymentGateway == itemToAdd.InnerItem.PaymentGateway && x.StoreId == itemToAdd.InnerItem.StoreId);
				if (existingItem == null)
				{
					InnerItem.PaymentGateways.Add(itemToAdd.InnerItem);
				}
			}
		}


		private void UpdateTaxJurisdictionsList(IStoreRepository repository)
		{
			var itemsToDelete = TaxesStepViewModel.AvailableTaxJurisdictions.Where(x => x.IsChecked == false).ToList();
			foreach (var itemToDelete in itemsToDelete)
			{
				var itemFromDb =
					repository.StoreTaxJurisdictions.Where(
						stc =>
							stc.TaxJurisdiction == itemToDelete.InnerItem.TaxJurisdiction && stc.StoreId == itemToDelete.InnerItem.StoreId)
						.SingleOrDefault();

				if (itemFromDb != null)
				{
					repository.Attach(itemFromDb);
					repository.Remove(itemFromDb);
					InnerItem.TaxJurisdictions.Remove(itemFromDb);
				}
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();


			var itemsToAdd = TaxesStepViewModel.AvailableTaxJurisdictions.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{
				var existingItem =
					InnerItem.TaxJurisdictions.SingleOrDefault(
						x => x.TaxJurisdiction == itemToAdd.InnerItem.TaxJurisdiction && x.StoreId == itemToAdd.InnerItem.StoreId);
				if (existingItem == null)
				{
					InnerItem.TaxJurisdictions.Add(itemToAdd.InnerItem);
				}
			}
		}

		private void UpdateTaxCodeList(IStoreRepository repository)
		{
			var itemsToDelete = TaxesStepViewModel.AvailableTaxCodes.Where(x => x.IsChecked == false).ToList();
			foreach (var itemToDelete in itemsToDelete)
			{
				var itemFromDb =
					repository.StoreTaxCodes.Where(
						stc =>
							stc.TaxCode == itemToDelete.InnerItem.TaxCode && stc.StoreId == itemToDelete.InnerItem.StoreId)
						.SingleOrDefault();

				if (itemFromDb != null)
				{
					repository.Attach(itemFromDb);
					repository.Remove(itemFromDb);
					InnerItem.TaxCodes.Remove(itemFromDb);
				}
			}

			if (!IsWizardMode)
				Repository.UnitOfWork.Commit();


			var itemsToAdd = TaxesStepViewModel.AvailableTaxCodes.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{
				var existingItem =
					InnerItem.TaxCodes.SingleOrDefault(
						x => x.TaxCode == itemToAdd.InnerItem.TaxCode && x.StoreId == itemToAdd.InnerItem.StoreId);
				if (existingItem == null)
				{
					InnerItem.TaxCodes.Add(itemToAdd.InnerItem);
				}
			}


		}

		#endregion

		#region Protected init methods

		protected void InitOverviewStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			OverviewStepViewModel =
				  _overviewVmFactory.GetViewModelInstance(itemParameter);
			(OverviewStepViewModel as StoreViewModel).IsWizardMode = false;
			OnPropertyChanged("OverviewStepViewModel");
			(OverviewStepViewModel as StoreOverviewStepViewModel).InitializePropertiesForViewing();
			IsInitializingOverview = false;
		}

		protected void InitLocalizationStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			LocalizationStepViewModel =
				 _localizationVmFactory.GetViewModelInstance(itemParameter);
			(LocalizationStepViewModel as StoreLocalizationStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("LocalizationStepViewModel");
		}

		protected void InitTaxesStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			TaxesStepViewModel = _taxesVmFactory.GetViewModelInstance(itemParameter);
			(TaxesStepViewModel as StoreTaxesStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("TaxesStepViewModel");
		}

		protected void InitPaymentsStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			PaymentsStepViewModel =
				   _paymentsVmFactory.GetViewModelInstance(itemParameter);
			(PaymentsStepViewModel as StorePaymentsStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("PaymentsStepViewModel");
			IsInitializingPayments = false;
		}

		protected void InitLinkedStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			LinkedStoresStepViewModel =
					_linkedStoresVmFactory.GetViewModelInstance(itemParameter);
			(LinkedStoresStepViewModel as StoreLinkedStoresStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("LinkedStoresStepViewModel");
			IsInitializingLinkedStores = false;
		}

		protected void InitSettingsStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			SettingsStepViewModel = _settingVmFactory.GetViewModelInstance(itemParameter);
			(SettingsStepViewModel as StoreSettingsStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("SettingsStepViewModel");
		}

		protected void InitNavigationStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			NavigationStepViewModel =
					_navigationVmFactory.GetViewModelInstance(itemParameter);
			(NavigationStepViewModel as StoreNavigationStepViewModel).InitializePropertiesForViewing();
			OnPropertyChanged("NavigationStepViewModel");
		}

		protected void InitSeoStep()
		{
			var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
			var languagesParameter = new KeyValuePair<string, object>("languages", InnerItem.Languages.Select(x => x.LanguageCode));
			SeoStepViewModel =
					_seoVmFactory.GetViewModelInstance(itemParameter, languagesParameter);
			OnPropertyChanged("SeoStepViewModel");
		}

		#endregion

	}
}
