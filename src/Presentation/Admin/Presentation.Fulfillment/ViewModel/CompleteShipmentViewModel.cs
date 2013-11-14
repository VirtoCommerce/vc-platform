using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
    public class CompleteShipmentViewModel : ViewModelBase, ICompleteShipmentViewModel
    {
        #region Requests
        public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
        #endregion

        #region Commands
        public DelegateCommand SearchShipmentCommand { get; private set; }
        #endregion

        #region Dependencies
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
        #endregion

		public CompleteShipmentViewModel(IRepositoryFactory<IOrderRepository> orderRepositoryFactory, string shipmentId)
        {
			_orderRepositoryFactory = orderRepositoryFactory;
			ShipmentId = shipmentId;

            Initialize();

            CommonConfirmRequest = new InteractionRequest<Confirmation>();
            SearchShipmentCommand = new DelegateCommand(DoSearchShipment);
        }

        private void DoSearchShipment()
        {
            //var _searchItemVM = _vmFactory.Create<ISearchShipmentViewModel>(
            //	new KeyValuePair<string, object>("shipmentId", string.Empty));

            //CommonConfirmRequest.Raise(new ConditionalConfirmation()
            //{
            //	Content = _searchItemVM,
            //	Title = "Search for shipment"
            //},
            //(x) =>
            //{
            //	if (x.Confirmed)
            //	{
            //		ShipmentId = _searchItemVM.SelectedShipment.ShipmentId;
            //		OnPropertyChanged("ShipmentId");
            //	}
            //});
        }

        #region ICompleteShipmentViewModel

        public string ShipmentId
        {
            get;
            set;
        }

        public string TrackingNumber
        {
            get;
            set;
        }

        #endregion

        #region ViewModel properties
        //private ObservableCollection<Shipment> _shipments;
        //public ObservableCollection<Shipment> Shipments
        //{
        //	get
        //	{
        //		if (_shipments == null)
        //			_shipments = new ObservableCollection<Shipment>();
        //		return _shipments;
        //	}
        //	set
        //	{
        //		_shipments = value;
        //		OnPropertyChanged();
        //	}
        //}

        public ObservableCollection<string> Shipments { get; set; }
        #endregion

        #region private members
        private async void Initialize()
        {
            Shipments = new ObservableCollection<string>();

            var res = await Task.Run(
				() =>
					{
						using (var orderRepository = _orderRepositoryFactory.GetRepositoryInstance())
						{
							return orderRepository.Shipments.Expand("OrderForm/OrderGroup")
							                .Where(ship =>
							                       ship.PicklistId != null
							                       && (ship.Status == null
							                           || ship.Status == Enum.GetName(typeof (ShipmentStatus), ShipmentStatus.Packing)
							                           &&
							                           (ship.OrderForm.OrderGroup.Status !=
							                            Enum.GetName(typeof (OrderStatus), OrderStatus.OnHold) ||
							                            ship.OrderForm.OrderGroup.Status !=
							                            Enum.GetName(typeof (OrderStatus), OrderStatus.Cancelled))
							                          ))
							                .OrderByDescending(item => item.LastModified)
							                .ToList()
							                .Select(x => x.ShipmentId);
							}
						}
                    
                        );
            Shipments.SetItems(res);
        }

        #endregion


    }
}
