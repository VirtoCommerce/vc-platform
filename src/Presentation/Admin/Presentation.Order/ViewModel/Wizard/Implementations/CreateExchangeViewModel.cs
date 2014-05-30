using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.Model.Builders;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
    public class CreateExchangeViewModel : CreateRmaRequestViewModel, ICreateExchangeViewModel
    {
        public ILineItemAddViewModel LineItemAddVM { get; private set; }

        private readonly IOrderEntityFactory _entityFactory;
        private readonly IRepositoryFactory<IShippingRepository> _shippingRepositoryFactory;
        private readonly IViewModelsFactory<IOrderAddressViewModel> _addressVmFactory;

        // main public constructor. calls CreateWizardSteps()
        public CreateExchangeViewModel(Foundation.Orders.Model.Order innerOrder,
            RmaRequest rmaRequest, IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
            IViewModelsFactory<IRmaRequestReturnItemsStepViewModel> returnItemsVmFactory,
            IViewModelsFactory<IRmaRequestRefundStepViewModel> refundVmFactory,
            IOrderEntityFactory entityFactory, ReturnBuilder returnBuilder,
            IViewModelsFactory<ILineItemAddViewModel> lineItemAddVmFactory,
            IViewModelsFactory<IExchangeOrderStepViewModel> exchangeVmFactory,
            IViewModelsFactory<IOrderAddressViewModel> addressVmFactory)
            : base(innerOrder, rmaRequest, entityFactory, returnBuilder, returnItemsVmFactory, refundVmFactory, lineItemAddVmFactory, exchangeVmFactory)
        {
            _entityFactory = entityFactory;
            _addressVmFactory = addressVmFactory;
            _shippingRepositoryFactory = shippingRepositoryFactory;
            _exchangeVmFactory = exchangeVmFactory;
        }

        // constructor (for single wizard step)
        protected CreateExchangeViewModel(
            IOrderEntityFactory entityFactory,
            IViewModelsFactory<IOrderAddressViewModel> addressVmFactory,
            ReturnBuilder returnBuilder,
            IViewModelsFactory<ILineItemAddViewModel> lineItemAddVmFactory,
            bool isCreatingExchangeOrderOnly,
            IViewModelsFactory<IReturnItemViewModel> returnItemVmFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory)
            : base(returnItemVmFactory, returnBuilder, isCreatingExchangeOrderOnly)
        {
            _shippingRepositoryFactory = shippingRepositoryFactory;
            _lineItemAddVmFactory = lineItemAddVmFactory;
            LineItemAddVM = lineItemAddVmFactory.GetViewModelInstance();

            if (this is IExchangeOrderStepViewModel)
            {
                _entityFactory = entityFactory;
                _addressVmFactory = addressVmFactory;

                // workaround for null pointer exception
                returnBuilder.WithExchangeShippingAddress(new OrderAddress() { OrderAddressId = "fake address" });

                LineItemAddVM.SelectedItemsToAdd.CollectionChanged += SelectedItemsToAdd_CollectionChanged;

                ShippingAddress = AvailableShippingAddresses.FirstOrDefault();
                ShippingMethod = AvailableShippingMethods.FirstOrDefault();
            }
        }

        protected OrderModel ExchangeOrder
        {
            get
            {
                return ReturnBuilder.ExchangeOrder;
            }
        }

        #region ICreateExchangeViewModel Members
        public object ShippingAddress
        {
            get
            {
                // return ExchangeOrder.OrderAddresses.FirstOrDefault(x => x.OrderAddressId == ExchangeOrder.OrderForms[0].Shipments[0].ShippingAddressId);
                return ExchangeOrder.OrderAddresses.FirstOrDefault(x => x.OrderAddressId == ExchangeOrder.OrderForms[0].Shipments[0].ShippingAddressId);
            }
            set
            {
                if (value is OrderAddress)
                {
                    // ExchangeOrder.OrderForms[0].Shipments[0].ShippingAddressId = value;
                    ReturnBuilder.WithExchangeShippingAddress((OrderAddress)value);
                    OnPropertyChanged();
                    OnIsValidChanged();
                }
                else
                {
                    RaiseShippingAddressCreateInteractionRequest();
                }
            }
        }

        public ShippingMethod ShippingMethod
        {
            get
            {
                return AvailableShippingMethods.FirstOrDefault((x) => x.ShippingMethodId == ExchangeOrder.OrderForms[0].Shipments[0].ShippingMethodId);
            }
            set
            {
                // ExchangeOrder.OrderForms[0].Shipments[0].ShippingMethodId = value;
                ReturnBuilder.WithExchangeShippingMethod(value);
                // OnPropertyChanged("ShippingMethod");
                OnIsValidChanged();
            }
        }

        #endregion

        public IEnumerable<OrderAddress> AvailableShippingAddresses
        {
            get
            {
                return ExchangeOrder.OrderAddresses;
            }
        }

        ObservableCollection<ShippingMethod> _availableShippingMethods;
        public ObservableCollection<ShippingMethod> AvailableShippingMethods
        {
            get
            {
                if (_availableShippingMethods == null)
                {
                    var _shippingRepository = _shippingRepositoryFactory.GetRepositoryInstance();
                    var allShippingMethods = _shippingRepository.ShippingOptions.Expand(x => x.ShippingMethods)
                        .ToList()
                        .SelectMany(x => x.ShippingMethods)
                        .Distinct();

                    _availableShippingMethods = new ObservableCollection<ShippingMethod>(allShippingMethods);
                }
                return _availableShippingMethods;
            }
        }

        #region CreateRmaRequestViewModel Members overrides

        #region IWizardStep Members
        public override bool IsValid
        {
            get
            {
                var retval = true;

                if (this is IExchangeOrderStepViewModel)
                {
                    retval = (ReturnBuilder.ExchangeLineItems.Count > 0)
                                && (ShippingAddress is OrderAddress)
                                && (ShippingMethod != null);
                }

                return retval;
            }
        }

        public override string Description
        {
            get { return "Select exchange Order Items".Localize(); }
        }
        #endregion

        protected override void CreateWizardSteps(IViewModelsFactory<IRmaRequestReturnItemsStepViewModel> returnItemsVmFactory,
            IViewModelsFactory<IRmaRequestRefundStepViewModel> refundVmFactory)
        {
            LineItemAddVM = _lineItemAddVmFactory.GetViewModelInstance();

            var builderParameter = new KeyValuePair<string, object>("returnBuilder", ReturnBuilder);

            if (!_isCreatingExchangeOrderOnly)
                RegisterStep(returnItemsVmFactory.GetViewModelInstance(builderParameter));

            RegisterStep(_exchangeVmFactory.GetViewModelInstance(builderParameter,
                new KeyValuePair<string, object>("lineItemAddVM", LineItemAddVM),
                new KeyValuePair<string, object>("isCreatingExchangeOrderOnly", _isCreatingExchangeOrderOnly)));
            RegisterStep(refundVmFactory.GetViewModelInstance(builderParameter));
        }

        #endregion

        private void SelectedItemsToAdd_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ILineItemViewModel item in e.NewItems)
                {
                    ReturnBuilder.AddExchangeItem(item.ItemToAdd, item.Quantity);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ILineItemViewModel item in e.OldItems)
                {
                    var exchangeLineItem = ReturnBuilder.ExchangeLineItems.Where(x => x.CatalogItemId == item.ItemToAdd.ItemId).FirstOrDefault();
                    ReturnBuilder.RemoveExchangeItem(exchangeLineItem);
                }
            }

            OnIsValidChanged();
        }

        private void RaiseShippingAddressCreateInteractionRequest()
        {
            var item = CreateEntity<OrderAddress>();
            var itemVM = _addressVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("addressItem", item));

            var confirmation = new ConditionalConfirmation(item.Validate)
                {
                    Title = "New order address".Localize(),
                    Content = itemVM
                };

            ReturnItemConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    ExchangeOrder.OrderAddresses.Add(item);
                    ShippingAddress = item;
                }
            });
        }

        private T CreateEntity<T>()
        {
            var entityName = _entityFactory.GetEntityTypeStringName(typeof(T));
            return (T)_entityFactory.CreateEntityForType(entityName);
        }

    }

    public class ExchangeOrderStepViewModel : CreateExchangeViewModel, IExchangeOrderStepViewModel
    {
        public ExchangeOrderStepViewModel(IOrderEntityFactory entityFactory, IViewModelsFactory<IOrderAddressViewModel> addressVmFactory, ReturnBuilder returnBuilder, IViewModelsFactory<ILineItemAddViewModel> lineItemAddVmFactory, bool isCreatingExchangeOrderOnly, IRepositoryFactory<IShippingRepository> shippingRepositoryFactory)
            : base(entityFactory, addressVmFactory, returnBuilder, lineItemAddVmFactory, isCreatingExchangeOrderOnly, null, shippingRepositoryFactory)
        {
        }

        public Shipment ExchangeShipment
        {
            get
            {
                return ExchangeOrder.OrderForms[0].Shipments[0];
            }
        }
    }
}
