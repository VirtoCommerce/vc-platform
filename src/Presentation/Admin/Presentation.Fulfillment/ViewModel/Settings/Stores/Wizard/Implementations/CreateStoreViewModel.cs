using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class CreateStoreViewModel : WizardContainerStepsViewModel, ICreateStoreViewModel
	{
		#region Dependencies
		
		private readonly IViewModelsFactory<IStoreOverviewStepViewModel> _overviewVmFactory;
		private readonly IViewModelsFactory<IStoreLocalizationStepViewModel> _localizationVmFactory;
		private readonly IViewModelsFactory<IStoreTaxesStepViewModel> _taxesVmFactory;
		private readonly IViewModelsFactory<IStorePaymentsStepViewModel> _paymentsVmFactory;
		private readonly IViewModelsFactory<IStoreNavigationStepViewModel> _navigationVmFactory;

		#endregion

		public CreateStoreViewModel(
			IViewModelsFactory<IStoreOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<IStoreLocalizationStepViewModel> localizationVmFactory,
			IViewModelsFactory<IStoreTaxesStepViewModel> taxesVmFactory,
			IViewModelsFactory<IStorePaymentsStepViewModel> paymentsVmFactory,
			IViewModelsFactory<IStoreNavigationStepViewModel> navigationVmFactory,
			Store item)
		{
			_overviewVmFactory = overviewVmFactory;
			_localizationVmFactory = localizationVmFactory;
			_taxesVmFactory = taxesVmFactory;
			_paymentsVmFactory = paymentsVmFactory;
			_navigationVmFactory = navigationVmFactory;
			CreateWizardSteps(item);
		}

		private void CreateWizardSteps(Store item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);

			var OverviewStepViewModel = _overviewVmFactory.GetViewModelInstance(itemParameter);
			var LocalizationStepViewModel = _localizationVmFactory.GetViewModelInstance(itemParameter);
			var TaxesStepViewModel = _taxesVmFactory.GetViewModelInstance(itemParameter);
			var PaymentsStepViewModel = _paymentsVmFactory.GetViewModelInstance(itemParameter);
			var NavigationStepViewModel = _navigationVmFactory.GetViewModelInstance(itemParameter);
			//var SeoStepViewModel = _seoVmFactory.GetViewModelInstance(itemParameter);

			RegisterStep(OverviewStepViewModel);
			RegisterStep(LocalizationStepViewModel);
			RegisterStep(TaxesStepViewModel);
			RegisterStep(PaymentsStepViewModel);
			RegisterStep(NavigationStepViewModel);
			//RegisterStep(SeoStepViewModel);
		}
	}
}
