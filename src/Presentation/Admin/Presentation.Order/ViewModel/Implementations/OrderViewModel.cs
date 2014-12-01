using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
    public class OrderViewModel : ViewModelDetailBase<Foundation.Orders.Model.Order>, IOrderViewModel
    {
        #region Dependencies

        private readonly ICustomerRepository _customerRepository;
        private readonly IShippingRepository _shippingRepository;
        private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;
        private readonly INavigationManager _navManager;
        private readonly IViewModelsFactory<IRmaRequestViewModel> _rmaRequestVmFactory;
        private readonly IViewModelsFactory<IShipmentViewModel> _shipmentVmFactory;
        private readonly IViewModelsFactory<ICreatePaymentViewModel> _wizardPaymentVmFactory;
        private readonly IViewModelsFactory<ICreateRmaRequestViewModel> _wizardRmaVmFactory;
        private readonly IViewModelsFactory<ICreateExchangeViewModel> _wizardExchangeVmFactory;
        private readonly IViewModelsFactory<ICreateRefundViewModel> _wizardRefundVmFactory;
        private readonly IViewModelsFactory<IOrderAddressViewModel> _addressVmFactory;
        private readonly IViewModelsFactory<IOrderContactViewModel> _contactVmFactory;
        private readonly OrderClient _client;
        private readonly IOrderService _orderService;

        #endregion

        private const int ReturnsTabIndex = 3;
        internal OrderModel _innerModel;
        private bool _isRecalculating;

        #region Constructor

        public OrderViewModel(
            IViewModelsFactory<IOrderContactViewModel> contactVmFactory,
            IViewModelsFactory<IOrderAddressViewModel> addressVmFactory,
            IViewModelsFactory<ICreateRefundViewModel> wizardRefundVmFactory,
            IViewModelsFactory<ICreateExchangeViewModel> wizardExchangeVmFactory,
            IViewModelsFactory<ICreateRmaRequestViewModel> wizardRmaVmFactory,
            IViewModelsFactory<ICreatePaymentViewModel> wizardPaymentVmFactory,
            IViewModelsFactory<IShipmentViewModel> shipmentVmFactory,
            IViewModelsFactory<IRmaRequestViewModel> rmaRequestVmFactory,
            IRepositoryFactory<IOrderRepository> repositoryFactory,
            IShippingRepository shippingRepository, ICustomerRepository customerRepository,
            IOrderEntityFactory entityFactory, INavigationManager navManager, Foundation.Orders.Model.Order item,
            OrderClient client, IOrderService service)
            : base(entityFactory, item)
        {
            _contactVmFactory = contactVmFactory;
            _addressVmFactory = addressVmFactory;
            _wizardRefundVmFactory = wizardRefundVmFactory;
            _wizardExchangeVmFactory = wizardExchangeVmFactory;
            _wizardRmaVmFactory = wizardRmaVmFactory;
            _wizardPaymentVmFactory = wizardPaymentVmFactory;
            _shipmentVmFactory = shipmentVmFactory;
            _rmaRequestVmFactory = rmaRequestVmFactory;
            _repositoryFactory = repositoryFactory;
            _customerRepository = customerRepository;
            _shippingRepository = shippingRepository;
            _navManager = navManager;
            _client = client;
            _orderService = service;
            _innerModel = new OrderModel(InnerItem, client, service);

            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

            CommonOrderCommandConfirmRequest = new InteractionRequest<Confirmation>();
            DisableableCommandConfirmRequest = new InteractionRequest<Confirmation>();
            CommonOrderWizardDialogInteractionRequest = new InteractionRequest<Confirmation>();

            CancelOrderCommand = new DelegateCommand(RaiseCancelOrderInteractionRequest, () => _innerModel.IsOrderCancellable);
            HoldOrderCommand = new DelegateCommand(RaiseHoldOrderInteractionRequest, () => _innerModel.IsOrderHoldable);
            ReleaseHoldCommand = new DelegateCommand(RaiseReleaseHoldInteractionRequest, () => _innerModel.IsOrderHoldReleaseable);
            CreateRmaRequestCommand = new DelegateCommand(RaiseCreateRmaRequestInteractionRequest, () => _innerModel.CurrentStatus == (int)OrderStatus.Completed);
            CreateExchangeCommand = new DelegateCommand(RaiseCreateExchangeInteractionRequest, () => _innerModel.CurrentStatus == (int)OrderStatus.Completed);
            CreateRefundCommand = new DelegateCommand(RaiseCreateRefundInteractionRequest, () => _innerModel.CurrentStatus == (int)OrderStatus.Completed);

            CreatePaymentCommand = new DelegateCommand(RaiseCreatePaymentInteractionRequest);
            EditAddressCommand = new DelegateCommand<OrderAddress>(RaiseEditAddressInteractionRequest);

            ViewTitle = new ViewTitleBase()
            {
                Title = "Orders",
                SubTitle = (item != null && !string.IsNullOrEmpty(item.CustomerName)) ? item.CustomerName.ToUpper(CultureInfo.InvariantCulture) : null
            };

            OpenCustomerProfileCommand = new DelegateCommand(RaiseOpenCustomerProfileInteractionRequest, () => InnerItem.CustomerId != null);
        }

        #endregion

        #region ViewModelBase overrides

        public override string DisplayName
        {
            get
            {
                return InnerItem.TrackingNumber;
            }
        }

        public override string IconSource
        {
            get
            {
                return (string)Infrastructure.Converters.OrderStatusIconSourceConverter.Current.Convert(_innerModel.CurrentStatus, null, "OrderStatus", null);
                // return "order";
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                    (SolidColorBrush)Application.Current.TryFindResource("OrderDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.OrderGroupId),
                                                                NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }


        #endregion

        #region ViewModelDetailBase

        public override string ExceptionContextIdentity { get { return string.Format("Order ({0})", DisplayName); } }

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
                Content = string.Format("Save changes to order '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            var item = (Repository as IOrderRepository).Orders
                   .Where(x => x.OrderGroupId == OriginalItem.OrderGroupId)
                   .ExpandAll()
                   .Expand("RmaRequests/RmaReturnItems/RmaLineItems/LineItem")
                   .Expand("RmaRequests/Order")
                   .Expand("OrderForms/OrderGroup")
                   .Expand("OrderForms/Shipments/ShipmentItems/LineItem")
                   .SingleOrDefault();
            OnUIThread(() => InnerItem = item);
            _innerModel = new OrderModel(InnerItem, _client, _orderService);
        }

        protected override void InitializePropertiesForViewing()
        {
            OnUIThread(() =>
            {
                OrderShipmentViewModels = new ObservableCollection<IShipmentViewModel>();
                InitializeOrderShipmentViewModels();
                OnPropertyChanged("OrderShipmentViewModels");

                RmaRequestViewModels = new ObservableCollection<IRmaRequestViewModel>();
                foreach (var item in InnerItem.RmaRequests)
                {
                    var rmaRequestViewModel = _rmaRequestVmFactory.GetViewModelInstance(
                        new KeyValuePair<string, object>("orderRepository", Repository)
                        , new KeyValuePair<string, object>("rmaRequestItem", item)
                        , new KeyValuePair<string, object>("parentViewModel", this));
                    RmaRequestViewModels.Add(rmaRequestViewModel);
                }
                OnPropertyChanged("RmaRequestViewModels");

                Recalculate();

                // RaiseCanExecuteChanged();
                OnPropertyChanged("BillingAddress");
                OnPropertyChanged("FirstOrderForm");
                OnPropertyChanged("PaidTotal");
            });
        }

        protected override void AfterSaveChangesUI()
        {
            OriginalItem.InjectFrom(InnerItem);
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.OrderForms[0].Shipments.ToList().ForEach(x =>
                {
                    x.PropertyChanged += ViewModel_PropertyChanged;
                    x.ShipmentItems.CollectionChanged += ViewModel_PropertyChanged;
                    x.ShipmentItems.ToList().ForEach(y => y.PropertyChanged += ViewModel_PropertyChanged);
                });


            OrderShipmentViewModels.ToList().ForEach(x => x.PropertyChanged_EventsAdd());
            InnerItem.RmaRequests.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
        }

        protected override void CloseSubscriptionUI()
        {
            if (InnerItem.OrderForms.Count > 0)
            {
                InnerItem.OrderForms[0].Shipments.ToList().ForEach(x =>
                {
                    x.PropertyChanged -= ViewModel_PropertyChanged;
                    x.ShipmentItems.CollectionChanged -= ViewModel_PropertyChanged;
                    x.ShipmentItems.ToList().ForEach(y => y.PropertyChanged -= ViewModel_PropertyChanged);
                });

                if (OrderShipmentViewModels != null)
                    OrderShipmentViewModels.ToList().ForEach(x => x.PropertyChanged_EventsRemove());
                InnerItem.RmaRequests.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
            }
        }

        #endregion

        #region IOrderViewModel Members

        public ObservableCollection<IShipmentViewModel> OrderShipmentViewModels { get; private set; }
        public ObservableCollection<IRmaRequestViewModel> RmaRequestViewModels { get; private set; }

        public InteractionRequest<Confirmation> CommonOrderCommandConfirmRequest { get; private set; }
        public InteractionRequest<Confirmation> DisableableCommandConfirmRequest { get; private set; }
        public InteractionRequest<Confirmation> CommonOrderWizardDialogInteractionRequest { get; private set; }

        public DelegateCommand CancelOrderCommand { get; private set; }
        public DelegateCommand HoldOrderCommand { get; private set; }
        public DelegateCommand ReleaseHoldCommand { get; private set; }
        public DelegateCommand CreateRmaRequestCommand { get; private set; }
        public DelegateCommand CreateExchangeCommand { get; private set; }
        public DelegateCommand CreateRefundCommand { get; private set; }
        public DelegateCommand CreatePaymentCommand { get; private set; }

        public DelegateCommand<OrderAddress> EditAddressCommand { get; private set; }
        public DelegateCommand OpenCustomerProfileCommand { get; private set; }

        private object _lockObject = new object();
        private Address _customerAddress;
        public Address CustomerAddress
        {
            get
            {
                lock (_lockObject)
                {
                    if (_customerAddress == null && InnerItem != null && !string.IsNullOrEmpty(InnerItem.CustomerId))
                    {
                        var addressList = _customerRepository.Addresses
                                .Where(addr => addr.MemberId == InnerItem.CustomerId)
                                .OrderByDescending(x => x.Created)
                                .ToList();
                        if (addressList.Count > 0)
                        {
                            _customerAddress = addressList.FirstOrDefault(x => x.Type == AddressType.Primary.ToString()) ??
                                               addressList.FirstOrDefault();

                            OnPropertyChanged();
                        }
                    }
                }

                return _customerAddress;
            }
        }

        private ObservableCollection<ShippingMethod> _availableShippingMethods;
        public ObservableCollection<ShippingMethod> AvailableShippingMethods
        {
            get
            {
                if (_availableShippingMethods == null)
                {
                    var allShippingMethods = _shippingRepository.ShippingOptions.Expand(x => x.ShippingMethods)
                        .ToList()
                        .SelectMany(x => x.ShippingMethods)
                        .Distinct();

                    _availableShippingMethods = new ObservableCollection<ShippingMethod>(allShippingMethods);
                }
                return _availableShippingMethods;
            }
        }

        public void InitializeOrderShipmentViewModels()
        {
            foreach (var item in InnerItem.OrderForms[0].Shipments)
            {
                var orderShipmentViewModel = _shipmentVmFactory.GetViewModelInstance(
                    new KeyValuePair<string, object>("orderViewModel", this),
                    new KeyValuePair<string, object>("shipmentItem", item)
                    );
                OrderShipmentViewModels.Add(orderShipmentViewModel);
            }
        }

        #endregion

        #region Private methods

        internal void Recalculate()
        {
            if (!_isRecalculating)
            {
                _isRecalculating = true;
                CloseAllSubscription();

                try
                {
                    _innerModel.Recalculate();
                }
                finally
                {
                    SetAllSubscription();
                    _isRecalculating = false;
                }

                RaiseCanExecuteChanged();
            }
        }

        private void RaiseCancelOrderInteractionRequest()
        {
            CommonOrderCommandConfirmRequest.Raise(
                new ConditionalConfirmation { Content = "Are you sure you want to cancel order?".Localize(), Title = "Cancel Order".Localize() },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        _innerModel.CancelOrder();
                        Recalculate();
                    }
                });
        }

        private void RaiseHoldOrderInteractionRequest()
        {
            CommonOrderCommandConfirmRequest.Raise(
                new ConditionalConfirmation { Content = "Are you sure you want to put the order 'On Hold'?".Localize(), Title = "Order 'On Hold'".Localize() },
                (x) =>
                {
                    if (x.Confirmed)
                    {
                        _innerModel.HoldOrder();
                        Recalculate();
                    }
                });
        }

        private void RaiseReleaseHoldInteractionRequest()
        {
            _innerModel.ReleaseHold();
            Recalculate();
        }

        private void RaiseCreateRmaRequestInteractionRequest()
        {
            var rmaRequest = CreateEntity<RmaRequest>();
            rmaRequest.IsPhysicalReturnRequired = true;

            var itemVM = _wizardRmaVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("innerOrder", InnerItem),
                new KeyValuePair<string, object>("rmaRequest", rmaRequest));

            var confirmation = new Confirmation { Title = "Create RMA request".Localize(), Content = itemVM };
            CommonOrderWizardDialogInteractionRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    rmaRequest = itemVM.GetRmaRequest();
                    if (!rmaRequest.IsPhysicalReturnRequired)
                    {
                        rmaRequest.SetCurrentStatus((int)RmaRequestStatus.AwaitingCompletion, _client);
                    }

                    // workaround:begin1
                    // preparing for save: clear LineItem values otherwise .InjectFrom() will crash
                    foreach (var rmaItem in rmaRequest.RmaReturnItems)
                    {
                        rmaItem.RmaLineItems[0].LineItem = null;
                    }
                    Repository.Add(rmaRequest);
                    // workaround:end1

                    InnerItem.RmaRequests.Add(rmaRequest);
                    //await Save(false);
                    DoSaveChanges1();

                    // workaround:begin2
                    // set right values after save
                    foreach (var rmaItem in rmaRequest.RmaReturnItems)
                    {
                        var newLineItem = InnerItem.OrderForms[0].LineItems.First(x1 => x1.LineItemId == rmaItem.RmaLineItems[0].LineItemId);
                        rmaItem.RmaLineItems[0].LineItem = newLineItem;
                    }
                    // workaround:end2

                    var rmaRequestViewModel = _rmaRequestVmFactory.GetViewModelInstance(
                        new KeyValuePair<string, object>("rmaRequestItem", rmaRequest),
                        new KeyValuePair<string, object>("parentViewModel", this));
                    RmaRequestViewModels.Add(rmaRequestViewModel);

                    // reinitialize changes
                    CloseAllSubscription();
                    SetAllSubscription();

                    // show created RmaRequest item
                    SelectedTabIndex = ReturnsTabIndex;
                }
            });
        }

        private void RaiseCreateExchangeInteractionRequest()
        {
            RaiseCreateExchangeInteractionRequest(null);
        }

        internal void RaiseCreateExchangeInteractionRequest(RmaRequest item)
        {
            RmaRequest rmaRequest;
            if (item == null)
            {
                rmaRequest = CreateEntity<RmaRequest>();
            }
            else
            {
                rmaRequest = item.DeepClone(EntityFactory as IKnownSerializationTypes);
            }

            var itemVM = _wizardExchangeVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("innerOrder", InnerItem),
                new KeyValuePair<string, object>("rmaRequest", rmaRequest));

            var confirmation = new Confirmation { Title = "Create an exchange Order".Localize(), Content = itemVM };
            CommonOrderWizardDialogInteractionRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    rmaRequest = itemVM.GetRmaRequest();
                    var exchangeOrder = rmaRequest.ExchangeOrder;
                    if (InnerItem.RmaRequests.All(x1 => x1.RmaRequestId != rmaRequest.RmaRequestId))
                    {
                        // workaround:begin1
                        // preparing for save
                        Repository.Add(exchangeOrder);
                        rmaRequest.ExchangeOrder = null;
                        // clear LineItem values otherwise the next _repository.Add(rmaRequest); will crash
                        foreach (var rmaItem in rmaRequest.RmaReturnItems)
                        {
                            rmaItem.RmaLineItems[0].LineItem = null;
                        }
                        Repository.Add(rmaRequest);
                        // workaround:end1

                        InnerItem.RmaRequests.Add(rmaRequest);
                    }
                    else
                    {
                        item.ExchangeOrder = exchangeOrder;
                        OnUIThread(() => item.InjectFrom<CloneInjection>(rmaRequest));
                        Repository.Add(exchangeOrder);
                    }

                    DoSaveChanges1();

                    // workaround:begin2
                    // set right values after save
                    foreach (var rmaItem in rmaRequest.RmaReturnItems)
                    {
                        var newLineItem = InnerItem.OrderForms[0].LineItems.First(x1 => x1.LineItemId == rmaItem.RmaLineItems[0].LineItemId);
                        rmaItem.RmaLineItems[0].LineItem = newLineItem;
                    }
                    rmaRequest.ExchangeOrder = exchangeOrder;
                    // workaround:end2

                    var rmaRequestViewModel = _rmaRequestVmFactory.GetViewModelInstance(
                        new KeyValuePair<string, object>("rmaRequestItem", rmaRequest)
                        , new KeyValuePair<string, object>("parentViewModel", this));
                    RmaRequestViewModels.Add(rmaRequestViewModel);

                    // reinitialize changes
                    CloseAllSubscription();
                    SetAllSubscription();

                    // show created RmaRequest item
                    SelectedTabIndex = ReturnsTabIndex;

                    // open newly created ExchangeOrder
                    rmaRequestViewModel.ExchangeOrderViewCommand.Execute();
                }
            });
        }

        private void RaiseCreateRefundInteractionRequest()
        {
            if (!CreateRefundCommand.CanExecute())
                return;

            var itemVM = _wizardRefundVmFactory.GetViewModelInstance(
                  new KeyValuePair<string, object>("item", InnerItem)
                , new KeyValuePair<string, object>("defaultAmount", decimal.Zero)
                );

            var confirmation = new ConditionalConfirmation { Title = "Create Refund".Localize(), Content = itemVM };
            CommonOrderWizardDialogInteractionRequest.Raise(confirmation, x =>
            {
                if (x.Confirmed)
                {
                    ReQueryPayments();
                }
            });
        }

        private void RaiseCreatePaymentInteractionRequest()
        {
            var itemVM = _wizardPaymentVmFactory.GetViewModelInstance();

            var confirmation = new ConditionalConfirmation { Title = "Create payment".Localize(), Content = itemVM };
            CommonOrderWizardDialogInteractionRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    var item = CreateEntity<CreditCardPayment>();
                    item.Amount = itemVM.Amount;
                    item.PaymentMethodId = itemVM.PaymentMethod.PaymentMethodId;
                    item.PaymentMethodName = itemVM.PaymentMethod.Name;
                    item.Status = "Processing";
                    item.TransactionType = TransactionType.Authorization.ToString();
                    item.CreditCardExpirationMonth = 1;
                    item.CreditCardExpirationYear = 2011;

                    InnerItem.OrderForms[0].Payments.Add(item);
                    //IsModified = true;
                }
            });
        }

        private void RaiseCanExecuteChanged()
        {
            // order commands
            CancelOrderCommand.RaiseCanExecuteChanged();
            HoldOrderCommand.RaiseCanExecuteChanged();
            ReleaseHoldCommand.RaiseCanExecuteChanged();
            CreateRmaRequestCommand.RaiseCanExecuteChanged();
            CreateExchangeCommand.RaiseCanExecuteChanged();
            CreateRefundCommand.RaiseCanExecuteChanged();

            OnPropertyChanged("IconSource");

            // shipment and RmaRequest commands
            OrderShipmentViewModels.ToList().ForEach(x => x.RaiseCanExecuteChanged());
            // InnerItem.RmaRequests.ToList().ForEach(x => x.RaiseCanExecuteChanged());
        }

        private void RaiseEditAddressInteractionRequest(OrderAddress item)
        {
            if (item != null)
            {
                var itemClone = item.DeepClone(EntityFactory as IKnownSerializationTypes);
                var itemVM = _addressVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("addressItem", itemClone));

                var confirmation = new Confirmation { Title = "Edit order address".Localize(), Content = itemVM };

                // CommonOrderCommandConfirmRequest.Raise(confirmation, (x) =>
                DisableableCommandConfirmRequest.Raise(confirmation, (x) =>
                {
                    if (x.Confirmed)
                    {
                        OnUIThread(() => item.InjectFrom<CloneInjection>(itemClone));
                        // fake assign for GUI update trigger
                        item.OrderAddressId = item.OrderAddressId;
                        OnViewModelPropertyChangedUI(null, null);
                    }
                });
            }
        }

        private void RaiseOpenCustomerProfileInteractionRequest()
        {
            if (InnerItem != null && InnerItem.CustomerId != null)
            {
                var parameters = new[]
					{
						new KeyValuePair<string, object>("customerId", InnerItem.CustomerId), 
						new KeyValuePair<string, object>("fullName", InnerItem.CustomerName), 
					};

                var itemVM = _contactVmFactory.GetViewModelInstance(parameters);

                var confirmation = new ConditionalConfirmation { Title = "Edit customer profile".Localize(), Content = itemVM };

                CommonOrderCommandConfirmRequest.Raise(confirmation, (x) =>
                                    {
                                        if (x.Confirmed)
                                        {
                                            var content = (x.Content as IOrderContactViewModel);
                                            InnerItem.CustomerName = content.FullName;
                                        }
                                    });
            }
        }

        internal T CreateEntity<T>()
        {
            return (T)EntityFactory.CreateEntityForType(EntityFactory.GetEntityTypeStringName(typeof(T)));
        }

        #endregion

        #region VirtoCommerce.ManagementClient Properties

        public OrderAddress BillingAddress
        {
            get
            {
                OrderAddress result = null;

                if (InnerItem != null)
                {
                    result = InnerItem.OrderAddresses.FirstOrDefault(x => x.Name == "Billing");
                    if (result != null)
                        result.OrderGroup = InnerItem;
                }

                return result;
            }
        }

        public OrderForm FirstOrderForm
        {
            get
            {
                OrderForm result = null;

                if (InnerItem != null && InnerItem.OrderForms != null && InnerItem.OrderForms.Count > 0)
                {
                    result = InnerItem.OrderForms[0];
                }

                return result;
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        public decimal PaidTotal
        {
            get
            {
                return FirstOrderForm != null
                           ? FirstOrderForm.Payments.Where(x => x.Status == PaymentStatus.Completed.ToString()
                                                            && (x.TransactionType == TransactionType.Capture.ToString()
                                                             || x.TransactionType == TransactionType.Sale.ToString()))
                                                    .Sum(x => x.Amount)
                           : 0;
            }
        }

        #endregion

        internal void ReQueryPayments()
        {
            var currentPayments = ((IOrderRepository)Repository).Payments.Where(x1 => x1.OrderFormId == InnerItem.OrderForms[0].OrderFormId);
            InnerItem.OrderForms[0].Payments.SetItems(currentPayments);
        }

        internal void DoSaveChanges1()
        {
            DoSaveChanges();
            IsModified = false;
        }

        internal void SetAllSubscription1()
        {
            SetAllSubscription();
        }

        internal void CloseAllSubscription1()
        {
            CloseAllSubscription();
        }
    }
}
