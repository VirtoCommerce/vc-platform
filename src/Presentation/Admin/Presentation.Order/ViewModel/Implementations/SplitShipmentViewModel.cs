using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.Services;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{

	public class SplitShipmentViewModel : ViewModelBase, ISplitShipmentViewModel
	{
		private const string tmpShipmentId = "New Shipment";

		#region Dependencies

		private readonly IOrderEntityFactory _entityFactory;
		private readonly IShippingRepository _shippingRepository;
		private readonly IViewModelsFactory<IOrderAddressViewModel> _addressVmFactory;

		#endregion

		#region constructor
#if DESIGN
		public SplitShipmentViewModel()
		{
			_shippingRepository = new MockOrderService();
		}
#endif

		public SplitShipmentViewModel(IViewModelsFactory<IOrderAddressViewModel> addressVmFactory, IOrderEntityFactory entityFactory, IShippingRepository shippingRepository, Foundation.Orders.Model.Order currentOrder, Shipment sourceShipment, ShipmentItem movingItem)
		{
			_addressVmFactory = addressVmFactory;
			_shippingRepository = shippingRepository;
			_entityFactory = entityFactory;
			CurrentOrder = currentOrder;
			SourceShipment = sourceShipment;
			MovingShippingItem = movingItem;

			CreateNewAddressRequest = new InteractionRequest<Confirmation>();
		}

		#endregion

		/// <summary>
		/// variable for quick fixing of #680 issue: new address dialog popups TWO times.
		/// </summary>
		private bool isOrderAddressesAdding;
		/// <summary>
		/// selected address for the new shipment.
		/// </summary>
		public object SelectedOrderAddress
		{
			get
			{
				var retVal = CurrentOrder.OrderAddresses.FirstOrDefault(x => x.OrderAddressId == TargetShipment.ShippingAddressId);
				return retVal;
			}
			set
			{
				if (value == null || isOrderAddressesAdding)
					return;

				if (IsDummyAddress(value))
				{
					var newOrderAddress = CreateEntity<OrderAddress>();
					var orderAddressViewModel = _addressVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("addressItem", newOrderAddress));

					var confirmation = new ConditionalConfirmation(newOrderAddress.Validate)
						{
							Title = "New Order Address".Localize(),
							Content = orderAddressViewModel
						};
					CreateNewAddressRequest.Raise(confirmation, (x) =>
					{
						if (x.Confirmed)
						{
							isOrderAddressesAdding = true;
							CurrentOrder.OrderAddresses.Add(newOrderAddress);
							isOrderAddressesAdding = false;

							TargetShipment.ShippingAddressId = newOrderAddress.OrderAddressId;
							OnPropertyChanged();
						}
						// else: selected address reverted in XAML. No actions in ViewModel.
					});
				}
				else
				{
					TargetShipment.ShippingAddressId = ((OrderAddress)value).OrderAddressId;
					OnPropertyChanged();
				}
			}
		}

		public bool IsNewShipmentSelected
		{
			get
			{
				return TargetShipment.ShipmentId == tmpShipmentId;
			}
		}

		#region ISplitShipmentViewModel Members

		private int _moveQuantity = 1;
		public int MoveQuantity
		{
			get
			{
				return _moveQuantity;
			}
			set
			{
				_moveQuantity = value;
				OnPropertyChanged();
			}
		}


		private Shipment _targetShipment;
		public Shipment TargetShipment
		{
			get { return _targetShipment ?? (_targetShipment = AvailableTargetShipments.FirstOrDefault()); }
			set
			{
				_targetShipment = value;
				OnPropertyChanged();
				OnPropertyChanged("IsNewShipmentSelected");
				OnPropertyChanged("SelectedOrderAddress");
			}
		}

		public Foundation.Orders.Model.Order CurrentOrder { get; private set; }
		public Shipment SourceShipment { get; private set; }
		public ShipmentItem MovingShippingItem { get; private set; }

		public InteractionRequest<Confirmation> CreateNewAddressRequest
		{
			get;
			private set;
		}

		ObservableCollection<ShippingMethod> _availableShippingMethods;
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

		ObservableCollection<Shipment> _availableTargetShipments;
		public ObservableCollection<Shipment> AvailableTargetShipments
		{
			get
			{
				if (_availableTargetShipments == null)
				{
					_availableTargetShipments = new ObservableCollection<Shipment>();
					foreach (var shipment in CurrentOrder.OrderForms[0].Shipments.Where(x => x != SourceShipment && x.IsModifyable(CurrentOrder)))
					{
						_availableTargetShipments.Add(shipment);
					}

					//Add new shipment
					var newShipment = CreateEntity<Shipment>();
					newShipment.ShipmentId = tmpShipmentId;
					newShipment.ShippingAddressId = SourceShipment.ShippingAddressId;
					newShipment.ShippingMethodId = SourceShipment.ShippingMethodId;
					newShipment.ShippingMethodName = SourceShipment.ShippingMethodName;
					newShipment.Status = ShipmentStatus.InventoryAssigned.ToString();
					_availableTargetShipments.Add(newShipment);
				}

				return _availableTargetShipments;
			}
		}
		#endregion



		#region private members
		private bool IsDummyAddress(object address)
		{
			return !(address is OrderAddress);
		}

		private T CreateEntity<T>()
		{
			var entityName = _entityFactory.GetEntityTypeStringName(typeof(T));
			return (T)_entityFactory.CreateEntityForType(entityName);
		}
		#endregion

	}
}
