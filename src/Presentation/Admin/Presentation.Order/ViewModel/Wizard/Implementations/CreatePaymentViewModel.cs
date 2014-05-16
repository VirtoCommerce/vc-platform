using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreatePaymentViewModel : WizardViewModelBase, ICreatePaymentViewModel
	{
		private readonly IPaymentMethodRepository _paymentMethodRepository;

		public CreatePaymentViewModel(
			IViewModelsFactory<IPaymentMethodStepViewModel> paymentMethodStepVmFactory,
			IViewModelsFactory<IPaymentDetailsStepViewModel> detailsStepVmFactory,
			IPaymentMethodRepository paymentMethodRepository)
		{
			_paymentMethodRepository = paymentMethodRepository;

			var itemRepositoryParameter = new KeyValuePair<string, object>("itemRepository", _paymentMethodRepository);
			RegisterStep(paymentMethodStepVmFactory.GetViewModelInstance(itemRepositoryParameter));
			RegisterStep(detailsStepVmFactory.GetViewModelInstance(itemRepositoryParameter));
		}

		protected CreatePaymentViewModel(IPaymentMethodRepository paymentMethodRepository)
		{
			if (this is IPaymentMethodStepViewModel)
			{
				_paymentMethods = paymentMethodRepository.PaymentMethods.ToArray();
			}
		}

		#region ICreatePaymentViewModel Members

		private readonly PaymentMethod[] _paymentMethods;
		public PaymentMethod[] PaymentMethods
		{
			get
			{
				return _paymentMethods;
			}
		}

		private PaymentMethod _paymentMethod;
		public PaymentMethod PaymentMethod
		{
			get
			{
				return (this is IPaymentMethodStepViewModel) ? _paymentMethod : ((CreatePaymentViewModel)AllRegisteredSteps[0]).PaymentMethod;
			}
			set
			{
				_paymentMethod = value;
				OnIsValidChanged();
			}
		}

		private decimal _amount;
		public decimal Amount
		{
			get
			{
				return (this is IPaymentDetailsStepViewModel) ? _amount : ((CreatePaymentViewModel)AllRegisteredSteps[1]).Amount;
			}
			set
			{
				_amount = value;
				OnIsValidChanged();
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				if (this is IPaymentMethodStepViewModel)
				{
					retval = PaymentMethod != null;
				}
				else if (this is IPaymentDetailsStepViewModel)
				{
					retval = Amount > 0;
				}
				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return this is IPaymentDetailsStepViewModel;
			}
		}

		public override string Description
		{
			get { return "Enter payment details".Localize(); }
		}
		#endregion
	}

	public class PaymentMethodStepViewModel : CreatePaymentViewModel, IPaymentMethodStepViewModel
	{
		public PaymentMethodStepViewModel(IPaymentMethodRepository itemRepository)
			: base(itemRepository)
		{
		}
	}

	public class PaymentDetailsStepViewModel : CreatePaymentViewModel, IPaymentDetailsStepViewModel
	{
		public PaymentDetailsStepViewModel(IPaymentMethodRepository itemRepository)
			: base(itemRepository)
		{
		}
	}
}
