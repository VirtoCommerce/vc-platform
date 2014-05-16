using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.Model.Builders;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
	public class ShipmentViewModel : ViewModelBase, IShipmentViewModel
	{
		private readonly OrderModel _currentOrder;
		private readonly IOrderEntityFactory _entityFactory;
		private readonly IRepositoryFactory<IPricelistRepository> _repositoryFactory;
		private readonly PriceListClient _priceListClient;
		private readonly IViewModelsFactory<ILineItemViewModel> _lineItemVmFactory;
		private readonly IViewModelsFactory<ISplitShipmentViewModel> _splitVmFactory;
		private readonly IViewModelsFactory<ILineItemAddViewModel> _wizardLineItemVmFactory;
		private readonly OrderClient _orderClient;

		public OrderViewModel ParentViewModel { get; private set; }

		public ShipmentViewModel(OrderClient client, IViewModelsFactory<ISplitShipmentViewModel> splitVmFactory, IViewModelsFactory<ILineItemAddViewModel> wizardLineItemVmFactory, IViewModelsFactory<ILineItemViewModel> lineItemVmFactory, OrderViewModel orderViewModel, Shipment shipmentItem, IOrderEntityFactory entityFactory, IRepositoryFactory<IPricelistRepository> repositoryFactory, PriceListClient priceListClient)
		{
			_orderClient = client;
			ParentViewModel = orderViewModel;
			_currentOrder = orderViewModel._innerModel;
			CurrentShipment = shipmentItem;
			_entityFactory = entityFactory;
			_repositoryFactory = repositoryFactory;
			_priceListClient = priceListClient;
			_lineItemVmFactory = lineItemVmFactory;
			_wizardLineItemVmFactory = wizardLineItemVmFactory;
			_splitVmFactory = splitVmFactory;

			CommonShipmentConfirmRequest = orderViewModel.CommonOrderCommandConfirmRequest;

			ReleaseShipmentCommand = new DelegateCommand(RaiseReleaseShipmentInteractionRequest, () => CurrentShipment.IsReleaseable(_currentOrder.InnerItem, client));
			CompleteShipmentCommand = new DelegateCommand(RaiseCompleteShipmentInteractionRequest, () => CurrentShipment.IsCompletable(_currentOrder.InnerItem, client));
			CancelShipmentCommand = new DelegateCommand(RaiseCancelShipmentInteractionRequest, () => CurrentShipment.IsCancellable(_currentOrder.InnerItem, client));
			AddLineItemCommand = new DelegateCommand(RaiseAddLineItemInteractionRequest, () => CurrentShipment.IsModifyable(_currentOrder.InnerItem));
			MoveLineItemCommand = new DelegateCommand<ShipmentItem>(RaiseMoveLineItemInteractionRequest, x => x != null && CurrentShipment.IsModifyable(_currentOrder.InnerItem));
			RemoveLineItemCommand = new DelegateCommand<ShipmentItem>(RaiseRemoveLineItemInteractionRequest, x => x != null && CurrentShipment.IsModifyable(_currentOrder.InnerItem));
			ViewLineItemDetailsCommand = new DelegateCommand<object>(RaiseViewLineItemDetailsInteractionRequest, x => x != null);
		}

		public override string IconSource
		{
			get
			{
				return (string)Infrastructure.Converters.OrderStatusIconSourceConverter.Current.Convert(CurrentShipment.GetCurrentStatus(_currentOrder.InnerItem), null, "ShipmentStatus", null);
			}
		}

		#region IShipmentViewModel Members

		public Shipment CurrentShipment
		{
			get;
			set;
		}

		public DelegateCommand CancelShipmentCommand
		{
			get;
			private set;
		}

		public DelegateCommand ReleaseShipmentCommand
		{
			get;
			private set;
		}

		public DelegateCommand CompleteShipmentCommand
		{
			get;
			private set;
		}

		public DelegateCommand AddLineItemCommand
		{
			get;
			private set;
		}

		public DelegateCommand<ShipmentItem> MoveLineItemCommand
		{
			get;
			private set;
		}

		public DelegateCommand<ShipmentItem> RemoveLineItemCommand
		{
			get;
			private set;
		}

		public DelegateCommand<object> ViewLineItemDetailsCommand
		{
			get;
			private set;
		}

		public DelegateCommand<object> ChangeShipmentAddressCommand
		{
			get;
			private set;
		}

		public InteractionRequest<Confirmation> CommonShipmentConfirmRequest
		{
			get;
			private set;
		}

		public void PropertyChanged_EventsAdd()
		{
			CurrentShipment.PropertyChanged += Shipment_PropertyChanged;
		}

		public void PropertyChanged_EventsRemove()
		{
			CurrentShipment.PropertyChanged -= Shipment_PropertyChanged;
		}

		#endregion

		public string CurrentStatusText
		{
			get { return CurrentShipment.GetCurrentStatus(_currentOrder.InnerItem); }
		}

		private void RaiseReleaseShipmentInteractionRequest()
		{
			CommonShipmentConfirmRequest.Raise(
			  new ConditionalConfirmation { Content = "Are you sure you want to release this shipment?".Localize(), Title = "Release shipment".Localize() },
			  (x) =>
			  {
				  if (x.Confirmed)
				  {
					  CurrentShipment.Release(_currentOrder.InnerItem, _orderClient);
				  }
			  });
		}

		private void RaiseCompleteShipmentInteractionRequest()
		{
			CommonShipmentConfirmRequest.Raise(
			  new ConditionalConfirmation { Content = "Are you sure you want to complete this shipment?".Localize(), Title = "Complete shipment".Localize() },
			  (x) =>
			  {
				  if (x.Confirmed)
				  {
					  CurrentShipment.Complete(_currentOrder.InnerItem, _orderClient);
				  }
			  });
		}

		private void RaiseCancelShipmentInteractionRequest()
		{
			CommonShipmentConfirmRequest.Raise(
				new ConditionalConfirmation { Content = "Are you sure you want to cancel this shipment?".Localize(), Title = "Cancel shipment".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						CurrentShipment.Cancel(_currentOrder.InnerItem, _orderClient);
					}
				});
		}

		private void RaiseAddLineItemInteractionRequest()
		{
			var itemVM = _wizardLineItemVmFactory.GetViewModelInstance();

			var confirmation = new ConditionalConfirmation { Title = "Add new items".Localize(), Content = itemVM };

			CommonShipmentConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					ParentViewModel.CloseAllSubscription1();
					var builder = new ShipmentBuilder(CurrentShipment, _currentOrder.OrderForms[0], _entityFactory, _repositoryFactory, _priceListClient);
					itemVM.SelectedItemsToAdd.ToList().ForEach(i => builder.AddShipmentItem(i.ItemToAdd, i.Quantity));

					ParentViewModel.Recalculate();
					// fake assignment for triggers to fire
					ParentViewModel.InnerItem.OrderGroupId = ParentViewModel.InnerItem.OrderGroupId;
				}
			});
		}

		private void RaiseMoveLineItemInteractionRequest(ShipmentItem movingItem)
		{
			if (movingItem != null)
			{
				var itemVM = _splitVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("currentOrder", _currentOrder.InnerItem)
					, new KeyValuePair<string, object>("sourceShipment", CurrentShipment)
					, new KeyValuePair<string, object>("movingItem", movingItem));

				var confirmation = new ConditionalConfirmation { Title = "Split shipment".Localize(), Content = itemVM };

				CommonShipmentConfirmRequest.Raise(confirmation, (x) =>
				{
					if (x.Confirmed)
					{
						var quantity = itemVM.MoveQuantity;
						var targetShipment = itemVM.TargetShipment;

						var anyOperationSucceded = false;
						try
						{
							ParentViewModel.CloseAllSubscription1();

							_currentOrder.MoveShippingItem(movingItem, quantity, targetShipment, CurrentShipment);
							if (itemVM.IsNewShipmentSelected)
							{
								targetShipment.ShipmentId = targetShipment.GenerateNewKey();
								targetShipment.OrderFormId = _currentOrder.OrderForms[0].OrderFormId;
								_currentOrder.OrderForms[0].Shipments.Add(targetShipment);
							}

							ParentViewModel.OrderShipmentViewModels.Clear();
							ParentViewModel.InitializeOrderShipmentViewModels();

							ParentViewModel.Recalculate();
							anyOperationSucceded = true;
						}
						finally
						{
							ParentViewModel.SetAllSubscription1();
							// fake assignment for triggers to fire
							if (anyOperationSucceded)
								ParentViewModel.InnerItem.OrderGroupId = ParentViewModel.InnerItem.OrderGroupId;
						}
						RaiseCanExecuteChanged();
					}
				});
			}
		}

		private void RaiseRemoveLineItemInteractionRequest(ShipmentItem removingItem)
		{
			var itemVM = _lineItemVmFactory.GetViewModelInstance();
			itemVM.Initialize(removingItem);

			CommonShipmentConfirmRequest.Raise(
				new ConditionalConfirmation { Title = "Item removal details".Localize(), Content = itemVM },
				(x) =>
				{
					if (x.Confirmed)
					{
						// removing is moving item to fake shipment
						var fakeShipment = new Shipment { OrderForm = CurrentShipment.OrderForm };
						_currentOrder.MoveShippingItem(removingItem, itemVM.Quantity, fakeShipment, CurrentShipment);

						// update LineItem Quantity or remove it completely
						var lineItem = removingItem.LineItem;
						if (lineItem.Quantity > itemVM.Quantity)
						{
							lineItem.Quantity -= itemVM.Quantity;
						}
						else
						{
							_currentOrder.OrderForms[0].LineItems.Remove(lineItem);
						}

						ParentViewModel.Recalculate();
					}
				});
		}

		private void RaiseViewLineItemDetailsInteractionRequest(object selectedItemObject)
		{
			throw new NotImplementedException();
			//var selectedItem = selectedItemObject as ShipmentItem;
			//if (selectedItem != null)
			//{
			//    var confirmation = new ConditionalConfirmation();
			//    confirmation.Title = "Line Item details";
			//    var itemVM = Container.Resolve<ILineItemViewModel>();

			//  //   itemVM.ItemToAdd = selectedItem.LineItem.;
			//    confirmation.Content = itemVM;

			//    CommonShipmentConfirmRequest.Raise(confirmation, (x) =>
			//    {
			//        if (x.Confirmed)
			//        {

			//        }
			//    });
			//}
		}

		private void Shipment_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Status property is changed in commands. Some Status changes may affect whole Order Status. Recalculate every time.
			if (e.PropertyName == "Status" || e.PropertyName == "ShippingAddressId" || e.PropertyName == "ShippingMethodId")
			{
				ParentViewModel.Recalculate();
			}
		}

		public void RaiseCanExecuteChanged()
		{
			ReleaseShipmentCommand.RaiseCanExecuteChanged();
			CompleteShipmentCommand.RaiseCanExecuteChanged();
			CancelShipmentCommand.RaiseCanExecuteChanged();
			AddLineItemCommand.RaiseCanExecuteChanged();
			MoveLineItemCommand.RaiseCanExecuteChanged();
			RemoveLineItemCommand.RaiseCanExecuteChanged();
			ViewLineItemDetailsCommand.RaiseCanExecuteChanged();

			OnPropertyChanged("IconSource");
			OnPropertyChanged("CurrentStatusText");
		}
	}
}
