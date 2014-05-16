using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.Model;
using VirtoCommerce.ManagementClient.Order.Model.Builders;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreateRmaRequestViewModel : WizardViewModelBase, ICreateRmaRequestViewModel, IMultiSelectControlCommands
	{
		#region Dependencies

		private readonly IViewModelsFactory<IReturnItemViewModel> _returnItemVmFactory;
		protected IViewModelsFactory<ILineItemAddViewModel> _lineItemAddVmFactory;
		protected IViewModelsFactory<IExchangeOrderStepViewModel> _exchangeVmFactory;

		#endregion
		protected bool _isCreatingExchangeOrderOnly;

		public CreateRmaRequestViewModel(Foundation.Orders.Model.Order innerOrder,
			RmaRequest rmaRequest, IOrderEntityFactory orderEntityFactory, ReturnBuilder returnBuilder,
			IViewModelsFactory<IRmaRequestReturnItemsStepViewModel> returnItemsVmFactory,
			IViewModelsFactory<IRmaRequestRefundStepViewModel> refundVmFactory,
			IViewModelsFactory<ILineItemAddViewModel> lineItemAddVmFactory, IViewModelsFactory<IExchangeOrderStepViewModel> exchangeVmFactory)
		{
			_exchangeVmFactory = exchangeVmFactory;
			_lineItemAddVmFactory = lineItemAddVmFactory;
			_isCreatingExchangeOrderOnly = innerOrder.RmaRequests.Any(x => x.RmaRequestId == rmaRequest.RmaRequestId);

			var item = innerOrder.DeepClone(orderEntityFactory as IKnownSerializationTypes);

			ReturnBuilder = returnBuilder;
			ReturnBuilder.WithOrderOrRmaRequest(item, rmaRequest);

			CreateWizardSteps(returnItemsVmFactory, refundVmFactory);
		}

		protected CreateRmaRequestViewModel(IViewModelsFactory<IReturnItemViewModel> returnItemVmFactory, ReturnBuilder returnBuilder, bool isCreatingExchangeOrderOnly = false)
		{
			_returnItemVmFactory = returnItemVmFactory;
			ReturnBuilder = returnBuilder;
			_isCreatingExchangeOrderOnly = isCreatingExchangeOrderOnly;

			ReturnItemConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public ReturnBuilder ReturnBuilder
		{
			get;
			private set;
		}

		public ObservableCollection<ReturnBuilder.ReturnLineItem> AvailableItems
		{
			get
			{
				return ReturnBuilder.AvailableForReturnLineItems;
			}
		}
		public ObservableCollection<RmaReturnItem> SelectedItems
		{
			get
			{
				return ReturnBuilder.RmaReturnItems;
			}
		}

		#region ICreateRmaRequestViewModel Members
		public InteractionRequest<Confirmation> ReturnItemConfirmRequest { get; private set; }

		public bool IsPhysicalReturnRequired { get; set; }

		private string _refundOption;
		public string RefundOption
		{
			get
			{
				return _refundOption;
			}
			set
			{
				_refundOption = value;
				OnIsValidChanged();
			}
		}

		public RmaRequest GetRmaRequest()
		{
			var result = ReturnBuilder.CompleteReturnBuild();
			//if (!_isCreatingExchangeOrderOnly)
			//    result.GenerateRmaRequestId();
			return result;
		}

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				if (this is IRmaRequestReturnItemsStepViewModel)
				{
					retval = ReturnBuilder.RmaReturnItems.Count > 0;
				}
				//else if (this is IRmaRequestRefundStepViewModel)
				//{
				//	// retval = !String.IsNullOrEmpty(this.RefundOption);
				//}
				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return this is IRmaRequestRefundStepViewModel;
			}
		}

		public override string Description
		{
			get { return "Enter Order Return details".Localize(); }
		}
		#endregion

		#region IMultiSelectControlCommands Members
		public void SelectItem(object selectedObj)
		{
			var itemVM = _returnItemVmFactory.GetViewModelInstance();
			itemVM.ReturnLineItem = selectedObj as ReturnBuilder.ReturnLineItem;

			var confirmation = new ConditionalConfirmation { Title = "Specify return data".Localize(), Content = itemVM };
			ReturnItemConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					ReturnBuilder.AddReturnItem(itemVM.ReturnLineItem, itemVM.QuantityToMove, itemVM.SelectedReason);
					// recalculate

					// OnPropertyChanged("ReturnTotal");
					OnIsValidChanged();
				}
			});
		}

		public void SelectAllItems(ICollectionView availableItemsCollectionView)
		{
			var itemVM = _returnItemVmFactory.GetViewModelInstance();
			itemVM.IsBulkReturn = true;

			var confirmation = new ConditionalConfirmation { Title = "Specify return reason".Localize(), Content = itemVM };

			ReturnItemConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					var itemsList = new List<ReturnBuilder.ReturnLineItem>(availableItemsCollectionView.Cast<ReturnBuilder.ReturnLineItem>());

					foreach (var obj in itemsList)
					{
						ReturnBuilder.AddReturnItem(obj, obj.LineItem.Quantity, itemVM.SelectedReason);
					}
					OnPropertyChanged("ReturnTotal");
					OnIsValidChanged();
				}
			});

		}

		public void UnSelectItem(object selectedObj)
		{
			var returnItem = (RmaReturnItem)selectedObj;
			ReturnBuilder.RemoveReturnItem(returnItem, returnItem.RmaLineItems[0].ReturnQuantity);
			OnPropertyChanged("ReturnTotal");
			OnIsValidChanged();
		}

		public void UnSelectAllItems(IList currentListItems)
		{
			foreach (var returnItem in ReturnBuilder.RmaReturnItems.ToArray())
			{
				ReturnBuilder.RemoveReturnItem(returnItem, returnItem.RmaLineItems[0].ReturnQuantity);
			}
			OnPropertyChanged("ReturnTotal");
			OnIsValidChanged();
		}
		#endregion

		protected virtual void CreateWizardSteps(
			IViewModelsFactory<IRmaRequestReturnItemsStepViewModel> returnItemsVmFactory,
			IViewModelsFactory<IRmaRequestRefundStepViewModel> refundVmFactory)
		{
			var builderParameter = new KeyValuePair<string, object>("returnBuilder", ReturnBuilder);
			RegisterStep(returnItemsVmFactory.GetViewModelInstance(builderParameter));
			RegisterStep(refundVmFactory.GetViewModelInstance(builderParameter));
		}
	}

	public class RmaRequestReturnItemsStepViewModel : CreateRmaRequestViewModel, IRmaRequestReturnItemsStepViewModel
	{
		public RmaRequestReturnItemsStepViewModel(ReturnBuilder returnBuilder, IViewModelsFactory<IReturnItemViewModel> returnItemVmFactory)
			: base(returnItemVmFactory, returnBuilder)
		{
		}
	}

	public class RmaRequestRefundStepViewModel : CreateRmaRequestViewModel, IRmaRequestRefundStepViewModel
	{
		public RmaRequestRefundStepViewModel(ReturnBuilder returnBuilder, IPaymentMethodRepository repository, StoreClient client)
			: base(null, returnBuilder)
		{
			var paymentMethodNames = ReturnBuilder.CurrentOrder.OrderForms[0].Payments.Select(x => x.PaymentMethodName).Distinct();
			PaymentMethods = paymentMethodNames.ToArray();

			NewPaymentSource = CreateRefundViewModel.GetNewPaymentModel(ReturnBuilder.CurrentOrder.InnerItem, repository, client);
		}

		public override string Description
		{
			get { return "Adjust Return processing details".Localize(); }
		}

		public string[] PaymentMethods { get; private set; }

		public PaymentModel NewPaymentSource { get; set; }
	}
}
