using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Implementations
{
    public class ShippingMethodViewModel : ViewModelDetailAndWizardBase<ShippingMethod>, IShippingMethodViewModel
    {
        #region Fields

        const string SettingName_Currencies = "Currencies";

        //private IOrderEntityFactory _entityFactory;
        //private IShippingRepository _itemRepository;
        //private readonly IPaymentMethodRepository _paymentRepository;
        //private readonly IOrderRepository _orderRepository;

        #endregion

        #region Dependencies

        private readonly INavigationManager _navManager;
        private readonly IHomeSettingsViewModel _parent;
        private readonly IRepositoryFactory<IShippingRepository> _repositoryFactory;
        private readonly IRepositoryFactory<IPaymentMethodRepository> _paymentMethdRepositoryFactory;
        private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
        private readonly IViewModelsFactory<IGeneralLanguagesStepViewModel> _languagesVmFactory;
        private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;

        #endregion

        #region constructor

        public ShippingMethodViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory,
            IRepositoryFactory<IShippingRepository> repositoryFactory,
            IRepositoryFactory<IPaymentMethodRepository> paymentMethdRepositoryFactory,
            IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
            IOrderEntityFactory entityFactory, IHomeSettingsViewModel parent,
            INavigationManager navManager,
            ShippingMethod item)
            : base(entityFactory, item, false)
        {
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Shipping Method" };
            _repositoryFactory = repositoryFactory;
            _paymentMethdRepositoryFactory = paymentMethdRepositoryFactory;
            _orderRepositoryFactory = orderRepositoryFactory;
            _navManager = navManager;
            _parent = parent;
            _languagesVmFactory = languagesVmFactory;
            _appConfigRepositoryFactory = appConfigRepositoryFactory;

            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
        }


        protected ShippingMethodViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory,
            IRepositoryFactory<IShippingRepository> repositoryFactory,
            IRepositoryFactory<IPaymentMethodRepository> paymentMethdRepositoryFactory,
            IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
            IOrderEntityFactory entityFactory,
            ShippingMethod item)
            : base(entityFactory, item, true)
        {
            _repositoryFactory = repositoryFactory;
            _paymentMethdRepositoryFactory = paymentMethdRepositoryFactory;
            _orderRepositoryFactory = orderRepositoryFactory;
            _languagesVmFactory = languagesVmFactory;
            _appConfigRepositoryFactory = appConfigRepositoryFactory;
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
                return "Icon_ShippingMethod";
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
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.ShippingMethodId),
                                                            Configuration.NavigationNames.HomeName, NavigationNames.MenuName,
                                                            this));
            }
        }

        #endregion

        #region ViewModelDetailAndWizardBase members

        public override string ExceptionContextIdentity
        {
            get { return string.Format("Shipping method ({0})", DisplayName); }
        }

        protected override bool IsValidForSave()
        {
            return InnerItem.Validate();
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to Shipping method '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }


        protected override void LoadInnerItem()
        {

            var item = (Repository as IShippingRepository).ShippingMethods.Expand(sm => sm.ShippingMethodLanguages)
                .Expand(sm => sm.PaymentMethodShippingMethods)
                .Expand(sm => sm.ShippingMethodJurisdictionGroups)
                .Where(sm => sm.ShippingMethodId == OriginalItem.ShippingMethodId)
                .SingleOrDefault();

            OnUIThread(() => InnerItem = item);

        }

        protected override void InitializePropertiesForViewing()
        {
            OnUIThread(() =>
            {
                if (this is IShippingMethodOverviewStepViewModel || IsSingleDialogEditing)
                {
                    if (AvailableShippingOptions == null)
                    {
                        using (var repository = _repositoryFactory.GetRepositoryInstance())
                        {
                            AvailableShippingOptions = repository.ShippingOptions.ToArray();
                            OnPropertyChanged("AvailableShippingOptions");

                            AllAvailableCurrencies = GetAvailableCurrencies();
                            OnPropertyChanged("AllAvailableCurrencies");
                        }
                    }
                }

                using (var paymentRepository = _paymentMethdRepositoryFactory.GetRepositoryInstance())
                {
                    AllAvailablePaymentMethods.Clear();
                    AllAvailablePaymentMethods =
                        paymentRepository.PaymentMethods.Expand(pm => pm.PaymentMethodShippingMethods).ToList();

                    var view = CollectionViewSource.GetDefaultView(AllAvailablePaymentMethods);
                    view.Filter = FilterPaymentMethods;

                    SelectedPaymentMethods.Clear();
                    foreach (var paymentMethodShippingMethodItem in InnerItem.PaymentMethodShippingMethods)
                    {
                        var paymMethFromDb =
                            paymentRepository.PaymentMethods.Where(
                                pm => pm.PaymentMethodId == paymentMethodShippingMethodItem.PaymentMethodId)
                                              .SingleOrDefault();

                        if (paymMethFromDb != null)
                        {
                            SelectedPaymentMethods.Add(paymMethFromDb);
                        }
                    }
                    view.Refresh();
                }

                using (var orderRepository = _orderRepositoryFactory.GetRepositoryInstance())
                {
                    AllAvailableJurisdictionGroups.Clear();
                    AllAvailableJurisdictionGroups =
                        orderRepository.JurisdictionGroups
                        .Where(x => x.JurisdictionType == (int)JurisdictionTypes.All || x.JurisdictionType == (int)JurisdictionTypes.Shipping)
                        .Expand(jg => jg.ShippingMethodJurisdictionGroups).ToList();

                    var jurisGroupView = CollectionViewSource.GetDefaultView(AllAvailableJurisdictionGroups);
                    jurisGroupView.Filter = FilterJurisdictionGroups;

                    SelectedJurisdictionGroups.Clear();
                    foreach (var shippingMethodJurisdictionGroupItem in InnerItem.ShippingMethodJurisdictionGroups)
                    {
                        var shipMethJurGroupFromDb =
                            orderRepository.JurisdictionGroups.Where(
                                jg => jg.JurisdictionGroupId == shippingMethodJurisdictionGroupItem.JurisdictionGroupId)
                                            .SingleOrDefault();

                        if (shipMethJurGroupFromDb != null)
                        {
                            SelectedJurisdictionGroups.Add(shipMethJurGroupFromDb);
                        }
                    }

                    jurisGroupView.Refresh();
                }


            });

            if (!IsWizardMode)
            {
                LanguagesStepViewModel = _languagesVmFactory.GetViewModelInstance(
                    new KeyValuePair<string, object>("selectedLanguages", InnerItem.ShippingMethodLanguages));
                OnPropertyChanged("LanguagesStepViewModel");
                LanguagesStepViewModel.CollectionChanged = ViewModel_PropertyChanged;
            }
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.PaymentMethodShippingMethods.CollectionChanged +=
                ViewModel_PropertyChanged;
            SelectedPaymentMethods.CollectionChanged += ViewModel_PropertyChanged;
            SelectedJurisdictionGroups.CollectionChanged += ViewModel_PropertyChanged;
        }

        protected override void CloseSubscriptionUI()
        {
            InnerItem.PaymentMethodShippingMethods.CollectionChanged -=
                ViewModel_PropertyChanged;
            SelectedPaymentMethods.CollectionChanged -= ViewModel_PropertyChanged;
            SelectedJurisdictionGroups.CollectionChanged -= ViewModel_PropertyChanged;
        }

        protected override void BeforeSaveChanges()
        {
            if (!IsWizardMode)
            {
                UpdateOfLanguages(LanguagesStepViewModel);
                UpdateOfPaymentItems();
                UpdateOfJurisdictionGroup();
            }
        }

        protected override void AfterSaveChangesUI()
        {
            if (_parent != null)
            {
                _parent.RefreshItem(OriginalItem);
            }
        }

        #endregion

        #region IWizardStep Members

        public override string Description
        {
            get { return "Enter Shipping method general information.".Localize(); }
        }

        public override bool IsValid
        {
            get
            {
                var retval = true;
                if (this is IShippingMethodOverviewStepViewModel)
                {
                    InnerItem.Validate(false);
                    retval = InnerItem.Errors.Count == 0 && !string.IsNullOrEmpty(InnerItem.Name) &&
                             !string.IsNullOrEmpty(InnerItem.Description) && !string.IsNullOrEmpty(InnerItem.ShippingOptionId)
                             && !string.IsNullOrEmpty(InnerItem.Currency);
                    InnerItem.Errors.Clear();
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get { return this is IGeneralLanguagesStepViewModel; }
        }

        #endregion

        #region IShippingMethodViewModel Members

        public void UpdateOfLanguages(ICollectionChange<GeneralLanguage> languagesVm)
        {
            var repository = Repository as IShippingRepository;
            {
                foreach (var removedItem in languagesVm.RemovedItems)
                {
                    var item = InnerItem.ShippingMethodLanguages.FirstOrDefault(x => x.ShippingMethodLanguageId == removedItem.Id);
                    if (item == null)
                        continue;

                    InnerItem.ShippingMethodLanguages.Remove(item);
                    repository.Attach(item);
                    repository.Remove(item);
                }

                foreach (var updatedItem in languagesVm.UpdatedItems)
                {
                    var item = InnerItem.ShippingMethodLanguages.SingleOrDefault(x => x.ShippingMethodLanguageId == updatedItem.Id);
                    if (item == null)
                        continue;
                    item.InjectFrom(updatedItem);
                }

                foreach (var addedItem in languagesVm.AddedItems)
                {
                    var item = EntityFactory.CreateEntity<ShippingMethodLanguage>();
                    item.InjectFrom(addedItem);
                    InnerItem.ShippingMethodLanguages.Add(item);
                }

                if (!IsWizardMode)
                    repository.UnitOfWork.Commit();
            }


        }

        #endregion

        #region Properties

        public ShippingOption[] AvailableShippingOptions { get; private set; }
        public string[] AllAvailableCurrencies { get; private set; }



        private List<PaymentMethod> _allAvailablePaymentMethod = new List<PaymentMethod>();
        public List<PaymentMethod> AllAvailablePaymentMethods
        {
            get { return _allAvailablePaymentMethod; }
            set
            {
                _allAvailablePaymentMethod = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PaymentMethod> _selectedPaymentMethods = new ObservableCollection<PaymentMethod>();
        public ObservableCollection<PaymentMethod> SelectedPaymentMethods
        {
            get { return _selectedPaymentMethods; }
            set
            {
                _selectedPaymentMethods = value;
                OnPropertyChanged();
            }
        }


        private List<JurisdictionGroup> _allAvailableJurisdictionGroups = new List<JurisdictionGroup>();
        public List<JurisdictionGroup> AllAvailableJurisdictionGroups
        {
            get { return _allAvailableJurisdictionGroups; }
            set
            {
                _allAvailableJurisdictionGroups = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<JurisdictionGroup> _selectedJurisdictionGroups = new ObservableCollection<JurisdictionGroup>();
        public ObservableCollection<JurisdictionGroup> SelectedJurisdictionGroups
        {
            get { return _selectedJurisdictionGroups; }
            set
            {
                _selectedJurisdictionGroups = value;
                OnPropertyChanged();
            }
        }

        public IGeneralLanguagesStepViewModel LanguagesStepViewModel { get; set; }

        #endregion

        #region private members

        private bool FilterPaymentMethods(object item)
        {
            bool result = false;

            var paymentItem = item as PaymentMethod;
            if (!SelectedPaymentMethods.Contains(paymentItem))
            {
                result = true;
            }

            return result;
        }

        private bool FilterJurisdictionGroups(object item)
        {
            var result = false;

            var jurisdictionGroupItem = item as JurisdictionGroup;
            if (!SelectedJurisdictionGroups.Contains(jurisdictionGroupItem))
            {
                result = true;
            }

            return result;
        }


        private string[] GetAvailableCurrencies()
        {
            string[] result = null;
            using (var repository = _appConfigRepositoryFactory.GetRepositoryInstance())
            {
                var setting = repository.Settings.Where(x => x.Name == SettingName_Currencies).ExpandAll().SingleOrDefault();
                if (setting != null)
                {
                    result = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
                }
            }

            return result;
        }

        public void UpdateOfPaymentItems()
        {

            var selectedPaymentShippingItems = new List<PaymentMethodShippingMethod>();

            foreach (var selectedPaymentMethod in SelectedPaymentMethods)
            {
                var itemToAdd = new PaymentMethodShippingMethod
                    {
                        PaymentMethodId = selectedPaymentMethod.PaymentMethodId,
                        ShippingMethodId = InnerItem.ShippingMethodId
                    };

                selectedPaymentShippingItems.Add(itemToAdd);
            }

            var paymentShippingForDelete =
                InnerItem.PaymentMethodShippingMethods.Where(
                    x => !selectedPaymentShippingItems.Any(pm => pm.PaymentMethodId == x.PaymentMethodId)).ToList();

            foreach (var paymentMethodShippingMethodForDelete in paymentShippingForDelete)
            {
                InnerItem.PaymentMethodShippingMethods.Remove(paymentMethodShippingMethodForDelete);

                var itemToDelete =
                    OriginalItem.PaymentMethodShippingMethods.SingleOrDefault(
                        item => item.PaymentMethodShippingMethodId == paymentMethodShippingMethodForDelete.PaymentMethodShippingMethodId);
                if (itemToDelete != null)
                {
                    OriginalItem.PaymentMethodShippingMethods.Remove(itemToDelete);
                }

                Repository.Attach(paymentMethodShippingMethodForDelete);
                Repository.Remove(paymentMethodShippingMethodForDelete);
            }

            foreach (var paymentShippingToAdd in selectedPaymentShippingItems)
            {

                var sameItemFromInnerItem =
                    InnerItem.PaymentMethodShippingMethods.SingleOrDefault(
                        pmsm =>
                            pmsm.PaymentMethodId == paymentShippingToAdd.PaymentMethodId &&
                            pmsm.ShippingMethodId == paymentShippingToAdd.ShippingMethodId);

                if (sameItemFromInnerItem != null)
                    continue;

                InnerItem.PaymentMethodShippingMethods.Add(paymentShippingToAdd);
            }

            if (!IsWizardMode)
                Repository.UnitOfWork.Commit();

        }

        public void UpdateOfJurisdictionGroup()
        {

            var selectedShippingJurgroupItems = new List<ShippingMethodJurisdictionGroup>();
            foreach (var selectedJurisdictionGroup in SelectedJurisdictionGroups)
            {
                var itemToAdd = new ShippingMethodJurisdictionGroup
                    {
                        JurisdictionGroupId = selectedJurisdictionGroup.JurisdictionGroupId,
                        ShippingMethodId = InnerItem.ShippingMethodId
                    };

                selectedShippingJurgroupItems.Add(itemToAdd);
            }

            var shippingJurisgroupForDelete = InnerItem.ShippingMethodJurisdictionGroups.Where(
                x => !selectedShippingJurgroupItems.Any(sj => sj.JurisdictionGroupId == x.JurisdictionGroupId)).ToList();

            foreach (var shippingMethodJurisdictionGroupForDelete in shippingJurisgroupForDelete)
            {
                InnerItem.ShippingMethodJurisdictionGroups.Remove(shippingMethodJurisdictionGroupForDelete);
                Repository.Attach(shippingMethodJurisdictionGroupForDelete);
                Repository.Remove(shippingMethodJurisdictionGroupForDelete);
            }

            foreach (var shippingJurgroupItemToAdd in selectedShippingJurgroupItems)
            {
                var sameItemFromInnerItem =
                    InnerItem.ShippingMethodJurisdictionGroups.SingleOrDefault(
                        smjg => smjg.ShippingMethodId == shippingJurgroupItemToAdd.ShippingMethodId
                                && smjg.JurisdictionGroupId == shippingJurgroupItemToAdd.JurisdictionGroupId);
                if (sameItemFromInnerItem != null)
                    continue;

                InnerItem.ShippingMethodJurisdictionGroups.Add(shippingJurgroupItemToAdd);
            }

            if (!IsWizardMode)
                Repository.UnitOfWork.Commit();

        }

        #endregion
    }


}
