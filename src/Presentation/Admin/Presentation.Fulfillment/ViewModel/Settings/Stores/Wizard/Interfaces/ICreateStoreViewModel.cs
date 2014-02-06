using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces
{
	public interface ICreateStoreViewModel : IViewModel
	{

	}

	public interface IStoreOverviewStepViewModel : IWizardStep
	{
	}
	
	public interface IStoreLocalizationStepViewModel : IWizardStep
	{
	}

	public interface IStorePaymentsStepViewModel : IWizardStep
	{
		List<StorePaymentsStepViewModel.StorePaymentGatewayViewModel> AvailableStorePaymentGateways { get; }
		List<StorePaymentsStepViewModel.StoreCardTypeViewModel> AvailableStoreCardTypes { get; }
		StoreSetting SettingEnableCVV { get; }
	}

	public interface IStoreSettingStepViewModel : IWizardStep
	{
	}

	public interface IStoreTaxesStepViewModel : IWizardStep
	{
		StoreTaxesStepViewModel.StoreTaxJurisdictionViewModel[] AvailableTaxJurisdictions { get; }
		StoreTaxesStepViewModel.StoreTaxCodeViewModel[] AvailableTaxCodes { get; }
	}

	public interface IStoreLinkedStoresStepViewModel : IWizardStep
	{
	}

	public interface IStoreNavigationStepViewModel : IWizardStep
	{
	}

}
