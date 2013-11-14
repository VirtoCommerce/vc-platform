using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Implementations;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class CreateShippingMethodViewModel : WizardContainerStepsViewModel, ICreateShippingMethodViewModel
    {
        public CreateShippingMethodViewModel(
			IViewModelsFactory<IShippingMethodOverviewStepViewModel> overviewVmFactory, 
			IViewModelsFactory<IShippingMethodSettingsStepViewModel> settingsVmFactory, 
			IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory, 
			ShippingMethod item)
        {
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter));
			RegisterStep(settingsVmFactory.GetViewModelInstance(itemParameter));
			RegisterStep(languagesVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("selectedLanguages", item.ShippingMethodLanguages)));
        }

		#region WizardContainerStepsViewModel

		protected override void BeforePrepareSteps()
		{
			var languagesVM = AllRegisteredSteps.First(x => x is IGeneralLanguagesStepViewModel) as IGeneralLanguagesStepViewModel;
			var settingsStep = AllRegisteredSteps.First(x => x is IShippingMethodSettingsStepViewModel) as ShippingMethodViewModel;
			settingsStep.UpdateOfLanguages(languagesVM);
		}
		
		#endregion
    }

	public class ShippingMethodOverviewStepViewModel : ShippingMethodViewModel, IShippingMethodOverviewStepViewModel
    {
        public ShippingMethodOverviewStepViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
            IRepositoryFactory<IPaymentMethodRepository> paymentMethdRepositoryFactory,
            IRepositoryFactory<IOrderRepository> orderRepositoryFactory, IOrderEntityFactory entityFactory, ShippingMethod item)
			: base(appConfigRepositoryFactory, languagesVmFactory, repositoryFactory, paymentMethdRepositoryFactory, orderRepositoryFactory, entityFactory, item)
        {
        }
    }

	public class ShippingMethodSettingsStepViewModel : ShippingMethodViewModel, IShippingMethodSettingsStepViewModel, ISupportWizardPrepare
    {
        public ShippingMethodSettingsStepViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
            IRepositoryFactory<IPaymentMethodRepository> paymentMethdRepositoryFactory,
            IRepositoryFactory<IOrderRepository> orderRepositoryFactory, IOrderEntityFactory entityFactory, ShippingMethod item)
			: base(appConfigRepositoryFactory, languagesVmFactory, repositoryFactory, paymentMethdRepositoryFactory, orderRepositoryFactory, entityFactory, item)
        {
        }

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			UpdateOfPaymentItems();
			UpdateOfJurisdictionGroup();
		}

		#endregion
	}

	public class ShippingMethodParametersStepViewModel : ShippingMethodViewModel, IShippingMethodParametersStepViewModel
    {
        public ShippingMethodParametersStepViewModel(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			IViewModelsFactory<IGeneralLanguagesStepViewModel> languagesVmFactory,
			IRepositoryFactory<IShippingRepository> repositoryFactory,
            IRepositoryFactory<IPaymentMethodRepository> paymentMethdRepositoryFactory,
            IRepositoryFactory<IOrderRepository> orderRepositoryFactory, IOrderEntityFactory entityFactory, ShippingMethod item)
			: base(appConfigRepositoryFactory, languagesVmFactory, repositoryFactory, paymentMethdRepositoryFactory, orderRepositoryFactory, entityFactory, item)
        {
        }
	}

}
