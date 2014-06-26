using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Inventory.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.PickLists.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Rmas.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Implementations
{
	public class MainFulfillmentViewModel : SubTabsDefaultViewModel, IMainFulfillmentViewModel
	{
		#region const
		private const string RecalculateWorkflowName = "OrderRecalculateWorkflow";
		#endregion

		#region Dependencies
		private readonly IInventoryHomeViewModel _inventoryHomeVm;
		private readonly IPicklistHomeViewModel _picklistHomeVm;
		private readonly IRmaHomeViewModel _rmaHomeVm;
		private readonly IViewModelsFactory<ICompleteShipmentViewModel> _completeShipmentVmFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IOrderService _orderService;
		#endregion

		#region Constructor
		public MainFulfillmentViewModel(
			IInventoryHomeViewModel inventoryVm,
			IPicklistHomeViewModel picklistVm,
			IRmaHomeViewModel rmaVm,
			IViewModelsFactory<ICompleteShipmentViewModel> completeShipmentVmFactory,
			IRepositoryFactory<IOrderRepository> orderRepositoryFactory,
			IOrderService orderService,
			IAuthenticationContext authContext)
		{
            ViewTitle = new ViewTitleBase { Title = "Fulfillment", SubTitle = "FULFILLMENT SERVICE".Localize() };
			_inventoryHomeVm = inventoryVm;
			_inventoryHomeVm.ParentViewModel = this;

			_picklistHomeVm = picklistVm;
			_picklistHomeVm.ParentViewModel = this;

			_rmaHomeVm = rmaVm;
			_rmaHomeVm.ParentViewModel = this;

			_completeShipmentVmFactory = completeShipmentVmFactory;
			_orderRepositoryFactory = orderRepositoryFactory;
			_authContext = authContext;

			_orderService = orderService;

			PopulateTabItems();
			CompleteShipmentCommand = new DelegateCommand(RaiseCompleteShipment);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CommonNotifyRequest = new InteractionRequest<Notification>();
		}
		#endregion


		#region Commands
		public DelegateCommand CompleteShipmentCommand { get; private set; }
		#endregion

		#region Requests
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }
		#endregion


		private void RaiseCompleteShipment()
		{
			var shipmentId = "";
			if (CurrentTab.IdTab == NavigationNames.PicklistHomeName)
			{
				var x = (PicklistHomeViewModel)CurrentTab.ViewModel;
				if (x.SelectedItem != null)
				{
					shipmentId = x.SelectedItem.Data.Picklist.Shipments[0].ShipmentId;
				}
			}
			var vm = _completeShipmentVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("shipmentId", shipmentId));

			var confirmation = new ConditionalConfirmation { Title = "Complete shipment".Localize(), Content = vm };

			CommonConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					using (var orderRepository = _orderRepositoryFactory.GetRepositoryInstance())
					{
						var query = orderRepository.Shipments.Expand("OrderForm").Where(ship => ship.ShipmentId == vm.ShipmentId);
						var current = query.FirstOrDefault();
						if (current != null)
						{
							var res = true;
							try
							{
								current.ShipmentTrackingNumber = vm.TrackingNumber;
								current.Status = ShipmentStatus.Shipped.ToString();
								// Load complete item here

								var q = orderRepository.Orders
														.Expand("OrderForms/Shipments/ShipmentItems/LineItem")
                                                        .Expand("OrderForms/LineItems")
														.Where(order => order.OrderGroupId == current.OrderForm.OrderGroupId);
								var originalItem = q.SingleOrDefault();

								if (originalItem != null)
								{
									var result = _orderService.ExecuteWorkflow(RecalculateWorkflowName, originalItem);
									OnUIThread(() => originalItem.InjectFrom<CloneInjection>(result.OrderGroup));
								}

								orderRepository.UnitOfWork.Commit();
							}
							catch (Exception)
							{
								res = false;
							}

							if (res)
							{
								var notification = new Notification
									{
										Title = "Shipment completed".Localize(),
										Content = string.Format("Shipment: {0}{1}{2}{3}Completed successfully".Localize(),
																vm.ShipmentId,
																Environment.NewLine,
																string.IsNullOrEmpty(vm.TrackingNumber)
																	? string.Empty
																	: string.Format("TrackingNumber: {0}".Localize(), vm.TrackingNumber),
																Environment.NewLine)
									};
								CommonNotifyRequest.Raise(notification);
							}
							else
							{
								current.ShipmentTrackingNumber = null;
								current.Status = ShipmentStatus.Packing.ToString();
								orderRepository.UnitOfWork.Commit();
								var notification = new Notification
									{
										Title = "Shipment not completed".Localize(),
										Content = string.Format("Shipment: {0}\nNot completed".Localize(),
																vm.ShipmentId)
									};
								CommonNotifyRequest.Raise(notification);
							}
						}
					}
				}
			});

		}

		#region private members

		//private Order _originalItem;

		//public void Recalculate(Order originalItem)
		//{

		//}

		//private bool RaiseCanExecuteChanged()
		//{

		//	var retVal =
		//			_orderRepository.Shipments.Expand("OrderForm/OrderGroup")
		//				.Where(ship =>
		//					ship.PicklistId != null
		//					&& (ship.Status == null
		//						|| ship.Status == Enum.GetName(typeof(ShipmentStatus), ShipmentStatus.Packing)
		//							 && (ship.OrderForm.OrderGroup.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.OnHold) ||
		//								ship.OrderForm.OrderGroup.Status != Enum.GetName(typeof(OrderStatus), OrderStatus.Cancelled))));


		//	return retVal.Any();
		//}

		#endregion

		private void PopulateTabItems()
		{
			SubItems = new List<ItemTypeHomeTab>();

			if (_authContext.CheckPermission(PredefinedPermissions.FulfillmentPicklistsManage) ||
				_authContext.CheckPermission(PredefinedPermissions.FulfillmentInventoryReceive))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.HomeName, Caption = "Inventory", Category = NavigationNames.ModuleName, Order = 10, ViewModel = _inventoryHomeVm });
			}
			if (_authContext.CheckPermission(PredefinedPermissions.FulfillmentPicklistsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.PicklistHomeName, Caption = "Picklists", Category = NavigationNames.ModuleName, Order = 20, ViewModel = _picklistHomeVm });
			}
			if (_authContext.CheckPermission(PredefinedPermissions.FulfillmentReturnsManage))
			{
                SubItems.Add(new ItemTypeHomeTab { IdTab = NavigationNames.RmaHomeName, Caption = "Returns & Exchanges", Category = NavigationNames.ModuleName, Order = 30, ViewModel = _rmaHomeVm });
			}
			if (SubItems.Any())
			{
				CurrentTab = SubItems[0];
			}
		}
	}
}
