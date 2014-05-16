using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
	public class RmaRequestViewModel : ViewModelBase, IRmaRequestViewModel
	{
		#region Dependencies

		private readonly OrderViewModel _parentViewModel;
		private readonly IAuthenticationContext _authContext;
		private readonly IViewModelsFactory<ICreateRefundViewModel> _wizardVmFactory;
		private readonly IViewModelsFactory<IOrderViewModel> _orderVmFactory;
		private readonly OrderClient _orderClient;

		#endregion

		public InteractionRequest<Confirmation> RmaRequestWizardDialogInteractionRequest { get; private set; }

		#region IRmaRequestViewModel members

		public RmaRequest CurrentRmaRequest { get; private set; }

		public DelegateCommand RmaRequestCancelCommand { get; private set; }
		public DelegateCommand RmaRequestCompleteCommand { get; private set; }
		public DelegateCommand ExchangeOrderCreateCommand { get; private set; }
		public DelegateCommand ExchangeOrderViewCommand { get; private set; }

		public bool IsExchangeOrderCreateShow
		{
			get
			{
				return _authContext.CheckPermission(PredefinedPermissions.OrdersCreateOrderExchange) &&
					CurrentRmaRequest.IsAllowCreateExchangeOrder();
			}
		}

		public string CurrentStatusText
		{
			get { return ((RmaRequestStatus)CurrentRmaRequest.GetCurrentStatus()).ToString(); }
		}

		#endregion

		public RmaRequestViewModel(
			IViewModelsFactory<ICreateRefundViewModel> wizardVmFactory,
			IViewModelsFactory<IOrderViewModel> orderVmFactory,
			IAuthenticationContext authContext,
			RmaRequest rmaRequestItem,
			OrderViewModel parentViewModel, OrderClient client)
		{
			_wizardVmFactory = wizardVmFactory;
			_orderVmFactory = orderVmFactory;
			CurrentRmaRequest = rmaRequestItem;
			_authContext = authContext;
			_parentViewModel = parentViewModel;
			_orderClient = client;

			RmaRequestWizardDialogInteractionRequest = new InteractionRequest<Confirmation>();

			RmaRequestCancelCommand = new DelegateCommand(RaiseRmaRequestCancelInteractionRequest, () => CurrentRmaRequest.IsCancellable(client));
			RmaRequestCompleteCommand = new DelegateCommand(RaiseRmaRequestCompleteInteractionRequest, () => CurrentRmaRequest.IsCompletable(client));
			ExchangeOrderCreateCommand = new DelegateCommand(RaiseExchangeOrderCreateInteractionRequest, () => CurrentRmaRequest.IsAllowCreateExchangeOrder());
			ExchangeOrderViewCommand = new DelegateCommand(RaiseExchangeOrderViewInteractionRequest, () => CurrentRmaRequest.ExchangeOrder != null);
		}

		private void RaiseRmaRequestCancelInteractionRequest()
		{
			CurrentRmaRequest.Cancel(_orderClient);
			RaiseCanExecuteChanged();
		}

		private void RaiseRmaRequestCompleteInteractionRequest()
		{
			if (!RmaRequestCompleteCommand.CanExecute())
				return;

			if (CurrentRmaRequest.ReturnTotal > 0)
			{
				var itemVM = _wizardVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("item", _parentViewModel.InnerItem),
					new KeyValuePair<string, object>("defaultAmount", CurrentRmaRequest.ReturnTotal)
					);
				itemVM.OnAfterSuccessfulSubmit = () =>
					{
						CurrentRmaRequest.RefundAmount = itemVM.InnerModel.Amount;
						CurrentRmaRequest.Complete(_orderClient);
						_parentViewModel.DoSaveChanges1();
						RaiseCanExecuteChanged();
						_parentViewModel.ReQueryPayments();
					};

				var confirmation = new ConditionalConfirmation { Title = "Create Refund for RMA Request".Localize(), Content = itemVM };
				_parentViewModel.CommonOrderWizardDialogInteractionRequest.Raise(confirmation, x =>
					{
						//if (x.Confirmed)
						//{
						//	// moved to: itemVM.OnAfterSuccessfulSubmit = () =>
						//}		
					});
			}
			else
			{
				CurrentRmaRequest.Complete(_orderClient);
				RaiseCanExecuteChanged();
			}
		}

		private void RaiseExchangeOrderCreateInteractionRequest()
		{
			_parentViewModel.RaiseCreateExchangeInteractionRequest(CurrentRmaRequest);
		}

		private void RaiseExchangeOrderViewInteractionRequest()
		{
			if (ExchangeOrderViewCommand.CanExecute())
			{
				var orderItem = CurrentRmaRequest.ExchangeOrder;
				var orderViewModel = _orderVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("item", orderItem));

				if (orderViewModel is IOpenTracking)
				{
					var openTracking = orderViewModel as IOpenTracking;
					openTracking.OpenItemCommand.Execute();
				}
			}
		}

		private void RaiseCanExecuteChanged()
		{
			RmaRequestCancelCommand.RaiseCanExecuteChanged();
			RmaRequestCompleteCommand.RaiseCanExecuteChanged();
			ExchangeOrderCreateCommand.RaiseCanExecuteChanged();
			ExchangeOrderViewCommand.RaiseCanExecuteChanged();

			OnPropertyChanged("CurrentStatusText");
			OnPropertyChanged("IsExchangeOrderCreateShow");
		}
	}
}
