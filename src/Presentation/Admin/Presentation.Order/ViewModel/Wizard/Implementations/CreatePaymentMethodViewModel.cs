using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreatePaymentMethodViewModel : WizardContainerStepsViewModel, ICreatePaymentMethodViewModel
	{
		private readonly IGeneralLanguagesStepViewModel languagesStep2;
		private readonly IPaymentMethodPropertiesStepViewModel paymentMethodPropertiesStep3;

		public CreatePaymentMethodViewModel(IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory, IViewModelsFactory<IPaymentMethodOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IPaymentMethodPropertiesStepViewModel> paymentPropertiesVmFactory, PaymentMethod item)
		{
			var _itemModel = new PaymentMethodStepModel
			{
				InnerItem = item,
				ParentWizard = this
			};
			var itemParameter = new KeyValuePair<string, object>("itemModel", _itemModel);

			// must be created first
			paymentMethodPropertiesStep3 = paymentPropertiesVmFactory.GetViewModelInstance(itemParameter);
			// this step is added or removed at RUNTIME
			// RegisterStep(paymentMethodPropertiesStep3);

			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
			languagesStep2 = langVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("selectedLanguages", item.PaymentMethodLanguages));
			RegisterStep(languagesStep2);
		}

		public void RegisterPropertiesStep()
		{
			if (!AllRegisteredSteps.Contains(paymentMethodPropertiesStep3))
			{
				RegisterStep(paymentMethodPropertiesStep3);
				languagesStep2.IsLast_2 = false;
			}
		}

		public void UnRegisterPropertiesStep()
		{
			if (AllRegisteredSteps.Contains(paymentMethodPropertiesStep3))
			{
				UnregisterStep(paymentMethodPropertiesStep3);
				languagesStep2.IsLast_2 = true;
			}
		}

		protected override void BeforePrepareSteps()
		{
			var languagesVM = AllRegisteredSteps.First(x => x is IGeneralLanguagesStepViewModel) as IGeneralLanguagesStepViewModel;
			var itemVM = AllRegisteredSteps.First(x => x is IPaymentMethodViewModel) as IPaymentMethodViewModel;
			itemVM.LanguagesStepViewModel = languagesVM;
		}
	}

	public class PaymentMethodStepModel
	{
		public PaymentMethod InnerItem;
		public ObservableCollection<GeneralPropertyValueEditViewModel> PropertyValues;
		public CreatePaymentMethodViewModel ParentWizard;
	}

	public abstract class CreatePaymentMethodStepViewModel : PaymentMethodViewModel
	{
		protected PaymentMethodStepModel stepModel;

		protected CreatePaymentMethodStepViewModel(
			IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IOrderEntityFactory entityFactory,
			PaymentMethodStepModel itemModel)
			: base(repositoryFactory, shippingRepositoryFactory, langVmFactory, entityFactory, itemModel.InnerItem)
		{
			stepModel = itemModel;
		}
	}

	public class PaymentMethodOverviewStepViewModel : CreatePaymentMethodStepViewModel, IPaymentMethodOverviewStepViewModel, ISupportWizardPrepare
	{
		public PaymentMethodOverviewStepViewModel(
			IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IOrderEntityFactory entityFactory,
			PaymentMethodStepModel itemModel)
			: base(repositoryFactory, shippingRepositoryFactory, langVmFactory, entityFactory, itemModel)
		{
			PropertyValues = stepModel.PropertyValues;
		}

		public override PaymentGateway SelectedGateway
		{
			get { return base.SelectedGateway; }
			set
			{
				base.SelectedGateway = value;
				if (PropertyValues.Count > 0)
				{
					stepModel.ParentWizard.RegisterPropertiesStep();
				}
				else
				{
					stepModel.ParentWizard.UnRegisterPropertiesStep();
				}
			}
		}

		#region ViewModelDetailAndWizardBase members

		protected override void InitializePropertiesForViewing()
		{
			OnUIThread(InitShipingMethods);
		}

		public void Prepare()
		{
			InitializeLanguagesForSave();
			GetShippingItemToInnerItem();
		}

		#endregion

		#region IWizardStep members

		public override string Description
		{
			get { return "Enter Payment method details".Localize(); }
		}

		public override bool IsLast
		{
			get { return false; }
		}

		public override bool IsValid
		{
			get
			{
				bool result = InnerItem.Validate(false) && !String.IsNullOrEmpty(InnerItem.Name)
						&& !string.IsNullOrEmpty(InnerItem.Description);

				return result;
			}
		}

		#endregion
	}

	public class PaymentMethodPropertiesStepViewModel : CreatePaymentMethodStepViewModel, IPaymentMethodPropertiesStepViewModel, ISupportWizardPrepare
	{
		public PaymentMethodPropertiesStepViewModel(
			IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IOrderEntityFactory entityFactory,
			PaymentMethodStepModel itemModel)
			: base(repositoryFactory, shippingRepositoryFactory, langVmFactory, entityFactory, itemModel)
		{
			PropertyValues = new ObservableCollection<GeneralPropertyValueEditViewModel>();
			stepModel.PropertyValues = PropertyValues;
		}

		#region ViewModelDetailAndWizardBase members

		protected override void InitializePropertiesForViewing() { }

		protected override void SetSubscriptionUI() { }

		#endregion

		#region IWizardStep members

		public override string Description
		{
			get { return "Enter Payment method property values".Localize(); }
		}

		public override bool IsLast
		{
			get { return true; }
		}

		public override bool IsValid
		{
			get { return true; }
		}

		#endregion

		public void Prepare()
		{
			InitializeGatewayValuesForSave();
		}
	}
}
